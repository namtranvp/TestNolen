using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }

    [SerializeField] private FitType fitType;
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private Vector2 spacing;

    protected override void OnEnable()
    {
        base.OnEnable();
        GameController.OnSetRowColumn += SetRowColumn;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameController.OnSetRowColumn -= SetRowColumn;
    }

    public override void CalculateLayoutInputVertical()
    {
        base.CalculateLayoutInputHorizontal();

        int cellCount = rectChildren.Count;
        float sqrRt = Mathf.Sqrt(rectChildren.Count);

        if (fitType == FitType.Width)
        {
            columns = Mathf.CeilToInt(sqrRt);
            rows = Mathf.CeilToInt((float)rectChildren.Count / columns);
        }
        else if (fitType == FitType.Height)
        {
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt((float)rectChildren.Count / rows);
        }
        else if (fitType == FitType.Uniform)
        {
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }
        else if (fitType == FitType.FixedColumns)
        {
            columns = Mathf.Max(1, columns);
            rows = Mathf.CeilToInt((float)cellCount / columns);
        }
        else if (fitType == FitType.FixedRows)
        {
            rows = Mathf.Max(1, rows);
            columns = Mathf.CeilToInt((float)cellCount / rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth - padding.left - padding.right - (spacing.x * (columns - 1))) / columns;
        float cellHeight = (parentHeight - padding.top - padding.bottom - (spacing.y * (rows - 1))) / rows;

        //cellSize = new Vector2(cellWidth, cellHeight);
        float cellSizeSquare = Mathf.Min(cellWidth, cellHeight);
        cellSize = new Vector2(cellSizeSquare, cellSizeSquare);

        float totalGridWidth = columns * cellSize.x + (columns - 1) * spacing.x;
        float totalGridHeight = rows * cellSize.y + (rows - 1) * spacing.y;

        float startX = padding.left + (parentWidth - padding.left - padding.right - totalGridWidth) / 2f;
        float startY = padding.top + (parentHeight - padding.top - padding.bottom - totalGridHeight) / 2f;



        for (int i = 0; i < cellCount; i++)
        {
            int row = i / columns;
            int column = i % columns;

            if (row >= rows)
                break;

            RectTransform item = rectChildren[i];

            float xPos = startX + (cellSize.x + spacing.x) * column;
            float yPos = startY + (cellSize.y + spacing.y) * row;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }

    }

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {

    }

    private void SetRowColumn(Vector2Int rc)
    {
        rows = rc.y;
        columns = rc.x;
    }
}
