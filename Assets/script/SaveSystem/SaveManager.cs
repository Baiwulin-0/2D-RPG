using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> allSaveables;

    [SerializeField] private string fileName = "unityalexdev.json";
    [SerializeField] private bool encryptData = true;

    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        Debug.Log(Application.persistentDataPath);
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encryptData);
        allSaveables = FindISaveables();

        yield return null;
        LoadGame();
    }

    private void LoadGame()
    {
        gameData = dataHandler.LoadData();

        if (gameData == null)
        {
            Debug.Log("没有保存的数据，创建一个！");
            gameData = new GameData();
            return;
        }

        foreach (var saveable in allSaveables)
        {
            saveable.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (var saveable in allSaveables)
        {
            saveable.SaveData(ref gameData);
        }

        dataHandler.SaveData(gameData);
    }

    public GameData GetGameData() => gameData;

    [ContextMenu("*** 删除保存数据 ***")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encryptData);
        dataHandler.Delete();

        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveable> FindISaveables()
    {
        return
            //FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include,FindObjectsSortMode.None) --已弃用
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include)
            .OfType<ISaveable>()
            .ToList();
    }
}
