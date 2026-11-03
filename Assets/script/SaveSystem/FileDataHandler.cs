using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string fullPath;
    private bool encrpyData;
    private string codeWord = "unityalexdev.com";

    public FileDataHandler(string dataDirPathh, string dataFileName,bool encryptdata)
    {
        fullPath = Path.Combine(dataDirPathh, dataFileName);
        this.encrpyData = encryptdata;
    }

    public void SaveData(GameData gameData)
    {
        try
        {
            //1.创建目录（不存在时）
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //2.数据转换json
            string dataToSave = JsonUtility.ToJson(gameData, true);

            if (encrpyData)
            {
                dataToSave = EncryptDecrypt(dataToSave);
            }

            //3.打开/创建文件
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                //4.写进文件
                using (StreamWriter write = new StreamWriter(stream))
                {
                    write.Write(dataToSave);
                }
            }
        }

        catch (Exception e)
        {
            Debug.Log("保存失败，路径：" + fullPath + "\n" + e);
        }
    }

    public GameData LoadData()
    {
        GameData loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (encrpyData)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }

            catch (Exception e)
            {
                Debug.LogError("读取失败，路径：" + fullPath + "\n" + e);
            }
        }
        return loadData;
    }

    public void Delete()
    {
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    private string EncryptDecrypt(string data)
    {
        string modifedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifedData += (char)(data[i] ^ codeWord[i % codeWord.Length]);
        }

        return modifedData;
    }

}
