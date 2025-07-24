using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirectoryPath = "";
    private string dataFileName = "";
    private string settingsDataFileName = "";

    private bool encryptData = false;
    private string codeWord = "hubris";

    public FileDataHandler(string dataDirectoryPath, string dataFileName, bool encryptData, string settingsDataFileName)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
        this.encryptData = encryptData;
        this.settingsDataFileName = settingsDataFileName;
    }

    public void SaveSettings(SettingsData data)
    {
        string fullPath = Path.Combine(dataDirectoryPath, settingsDataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                    writer.Write(dataToStore);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error on trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public SettingsData LoadSettings()
    {
        string fullPath = Path.Combine(dataDirectoryPath, settingsDataFileName);

        SettingsData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                        dataToLoad = reader.ReadToEnd();
                }

                loadedData = JsonUtility.FromJson<SettingsData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error on trying to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (encryptData)
                dataToStore = EncryptDecrypt(dataToStore);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                    writer.Write(dataToStore);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error on trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);

        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                        dataToLoad = reader.ReadToEnd();
                }

                if (encryptData)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error on trying to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    public void DeleteSettings()
    {
        string fullPath = Path.Combine(dataDirectoryPath, settingsDataFileName);

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
            modifiedData += (char)(data[i] ^ codeWord[i % codeWord.Length]);

        return modifiedData;
    }
}
