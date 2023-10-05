using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class GameUtility
{
    public const float ResolutionDelayTime = 1;
    public const string SavePrefValue = "Level_Value";

    public const string FileName = "Q";
    public static string FileDir
    {
        get
        {
            return Application.dataPath + "/";
        }
    }
}

[System.Serializable()]
public class Data
{
    public Question[] questions = new Question[0];
    
    public Data() { }

    public static void Write(Data data, string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Data));
        using (Stream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, data);
        }
    }

    public static Data Fetch(string filepath)
    {
        return Fetch(out bool result, filepath);
    }

    public static Data Fetch(out bool result, string filepath)
    {
        if (!File.Exists(filepath))
        {
            result = false;
            return new Data();
        }

        XmlSerializer deserializer = new XmlSerializer(typeof(Data));
        using (Stream stream = new FileStream(filepath, FileMode.Open))
        {
            var data = (Data)deserializer.Deserialize(stream);

            result = true;
            return data;
        }
    }
}
