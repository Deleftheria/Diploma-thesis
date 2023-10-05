using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveNLoad : MonoBehaviour
{
    //the position that gets saved
    public GameObject player;
    public Vector3 savedLocation;

    public void Save()
    {
        //saving position to a file outside the game
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerInfo data = new PlayerInfo();
        data.xPos = player.transform.position.x;
        data.yPos = player.transform.position.y;
        data.zPos = player.transform.position.z;
        savedLocation = player.transform.position;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        //check if the saved file exists, then retrieve the saved position and set it to player
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            PlayerInfo data = (PlayerInfo)bf.Deserialize(file);
            file.Close();

            player.transform.position = new Vector3(data.xPos, data.yPos, data.zPos);
        }
        else
        {
            print("file does not exist");
        }
    }
}

//a class of what to save to the file
[Serializable]
public class PlayerInfo
{
    public float xPos;
    public float yPos;
    public float zPos;
}
