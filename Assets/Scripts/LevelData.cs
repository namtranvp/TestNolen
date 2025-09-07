using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField] Level[] levels;
    public Level GetLevel(int index)
    {
        if (index < 0 || index >= levels.Length)
            index = 0;
        return levels[index];
    }
}

[System.Serializable]
public class Level
{
    [Range(5, 30)]
    public float time = 5f;
    public Vector2Int cardGrid = new Vector2Int(2, 2);
}