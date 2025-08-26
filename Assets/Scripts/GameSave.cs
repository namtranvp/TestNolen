using System.IO;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public int numberOfCards;
    public int score;
}
public class GameSave : MonoBehaviour
{
    public static void SaveGameToFile(GameSaveData data)
    {

        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/savegame.json";
        File.WriteAllText(path, json);
        Debug.Log("Game saved to: " + path);
    }
    public static GameSaveData LoadGameFromFile()
    {
        string path = Application.persistentDataPath + "/savegame.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
            Debug.Log("Game loaded from: " + path);
            return data;
        }
        else
        {
            Debug.Log("No save file found!");
            return null;
        }
    }
}
