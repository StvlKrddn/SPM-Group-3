using System.Runtime.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static string SaveData = "SaveData";
    public static string TowerData = "TowerData";

    /// <summary> 
    /// Pass in a data structure and save it to a binary file in the default path.
    /// This method will OVERWRITE files with the same name
    /// </summary>
    /// <param name="dataStructure"> Any struct or class that contains data </param>
    /// <param name="fileName"> The name of the file, excluding file type, to save to </param>
    public static void WriteToFile(object dataStructure, string fileName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + fileName + ".dat", FileMode.OpenOrCreate);

        formatter.Serialize(file, dataStructure);
        file.Close();
    }

    /// <summary> Returns a data structure from a binary file </summary>
    /// <param name="fileName"> The name of the file, excluding file type, to load from </param>
    public static object ReadFromFile(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".dat";
        object dataStructure = null;
        if (FileExists(fileName))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);

            dataStructure = formatter.Deserialize(file);
            file.Close();
        }
        else
        {
            Debug.LogError("File: \"" + path + "\" does not exist or cannot be found!");
        }
        return dataStructure;
    }

    public static bool FileExists(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".dat";
        return File.Exists(path);
    }

    public static void DeleteFile(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".dat";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
