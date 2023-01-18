using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string directory = Application.persistentDataPath + "/Data/";
    public static readonly string saveDataPath = Application.persistentDataPath + "/Data/gamedata.dat";

    public static void SaveData(GameData gameData)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(saveDataPath, FileMode.Create);

        formatter.Serialize(stream, gameData);

        stream.Close();
    }

    public static GameData LoadData()
    {
        if (File.Exists(saveDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(saveDataPath, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;

            stream.Close();

            return data;
        }
        else
        {
            return null;

        }

    }

    public static void DeleteData()
    {
        if (File.Exists(saveDataPath))
        {
            File.Delete(saveDataPath);
            Debug.Log("Data deleted");
        }
        else
        {
            Debug.Log("No data to delete");
        }
    }
}
