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

    public override void CalculateLayoutInputVertical()
    {
        base.CalculateLayoutInputHorizontal();

        float sqrRt = Mathf.Sqrt(rectChildren.Count);
        rows = Mathf.CeilToInt(sqrRt);
        columns = Mathf.CeilToInt(sqrRt);

        if (fitType == FitType.Width || fitType == FitType.FixedColumns)
        {
            rows = Mathf.CeilToInt((float)rectChildren.Count / columns);
        }
        if (fitType == FitType.Height || fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt((float)rectChildren.Count / rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth - padding.left - padding.right - (spacing.x * (columns - 1))) / columns;
        float cellHeight = (parentHeight - padding.top - padding.bottom - (spacing.y * (rows - 1))) / rows;

        cellSize = new Vector2(cellWidth, cellHeight);
        //cellSize = Vector2.one * 200f;

        float totalGridWidth = columns * cellSize.x + (columns - 1) * spacing.x;
        float totalGridHeight = rows * cellSize.y + (rows - 1) * spacing.y;

        float startX = padding.left + (parentWidth - padding.left - padding.right - totalGridWidth) / 2f;
        float startY = padding.top + (parentHeight - padding.top - padding.bottom - totalGridHeight) / 2f;


        int cellCount = rectChildren.Count;

        for (int i = 0; i < cellCount; i++)
        {
            int row = i / columns;
            int column = i % columns;

            if (row >= rows)
                break;

            RectTransform item = rectChildren[i];
            //float xPos = padding.left + (cellSize.x + spacing.x) * column;
            //float yPos = padding.top + (cellSize.y + spacing.y) * row;

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
}
