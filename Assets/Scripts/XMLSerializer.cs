using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using UnityEngine;


public static class XMLSerializer
{

	public static void SaveSettings(Settings settings)
    {
        string path = Application.persistentDataPath + "/settings.dat";

        XmlSerializer serializer = new XmlSerializer(typeof(Settings));
        StreamWriter streamWriter = new StreamWriter(path);

        serializer.Serialize(streamWriter.BaseStream, settings);
        streamWriter.Close();

        Debug.Log("XMLSerializer: PlayerStats saved - " + path);
    }
	
    public static Settings LoadSettings()
    {
        string path = Application.persistentDataPath + "/settings.dat";

        if (File.Exists(path))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            StreamReader streamReader = new StreamReader(path);

            Settings settings = (Settings)serializer.Deserialize(streamReader.BaseStream);
            streamReader.Close();

            Debug.Log("XMLSerializer: PlayerStats loaded - " + path);

            return settings;
        }
        else
        {
            Debug.Log("Error! Nothing to read from found at " + path);
            return null;
        }     
    }
}
