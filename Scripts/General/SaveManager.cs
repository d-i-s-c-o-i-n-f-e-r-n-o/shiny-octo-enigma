using Newtonsoft.Json.Bson;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public SaveData activeSave;
    public SaveSettings activeSettings;
    public bool hasLoaded;

    string dataPath;

    void Awake()
    {
        instance = this;
        dataPath = Application.persistentDataPath;
        //Debug.Log(dataPath);
        Load();
    }

    public void Save()
    {
        File.WriteAllText(dataPath + "/Progress.json", JsonUtility.ToJson(activeSave)); //сохранить файл
        File.WriteAllText(dataPath + "/Settings.json", JsonUtility.ToJson(activeSettings)); //сохранить настройки
    }

    public void Load()
    {
        // Загрузить сохранение
        string saveDataPath = dataPath + "/Progress.json";
        if (File.Exists(saveDataPath))
        {
            string loadDataJson = File.ReadAllText(saveDataPath);
            activeSave = JsonUtility.FromJson<SaveData>(loadDataJson);
        }
        else Save();

        // Загрузить настройки
        string saveSettingsPath = dataPath + "/Settings.json";
        if (File.Exists(saveSettingsPath))
        {
            string loadSettingsJson = File.ReadAllText(saveSettingsPath);
            activeSettings = JsonUtility.FromJson<SaveSettings>(loadSettingsJson);
        }
        else Save();
        
        hasLoaded = true;
    }

    public void DeleteSaveData()
    {
        //Удалить прогресс
        string saveDataPath = dataPath + "/" + activeSave + ".json";
        if (File.Exists(saveDataPath))
            File.Delete(saveDataPath);

        Save();
    }

    public void DeleteSaveSettings()
    {
        //Удалить настройки
        string saveDataPath = dataPath + "/" + activeSettings + ".json";
        if (File.Exists(saveDataPath))
            File.Delete(saveDataPath);

        Save();
    }
}

public struct CheckPoint
{
    public int sceneIdx;
    public string sceneName;
    public Vector3 playerPosition;

    public CheckPoint(int sceneIdx, string sceneName, Vector3 playerPosition)
    {
        this.sceneIdx = sceneIdx;
        this.sceneName = sceneName;
        this.playerPosition = playerPosition;
    }
}

[System.Serializable]
public class SaveData
{
    [Header("Plot")]
    public bool isGameOn = false;
    public bool cutscene1Done = false;
    public bool cutscene2Done = false;
    public bool cutscene3Done = false;
    public bool[] chapterDone = { false, false, false };
    [Header("Weapons")]
    public bool hasKnife = true;
    public bool hasGun = true;
    [Header("Position")]
    CheckPoint player;
    [Header("Levels")]
    public byte healthLvl = 0;
    public byte staminaLvl = 0;
    public byte backpackLvl = 0;
    public byte speedLvl = 0;
    public short cooldownLvl = -1;
    [Header("Values")]
    public byte health = StaticHolder.healthBase;
    public int[] bulletCount = null;

    public Item[] inventory = new Item[15];
    public Weapon[] weapon = new Weapon[3];
}

[System.Serializable]
public class SaveSettings
{
    public float[] volume = new float[3] { 0.6f, 0.75f, 1f };
    public int language = 0;

    public KeyControl[] keys =
    {
        new() { keyCode = Key.E },          //interact  [0]
        new() { keyCode = Key.LeftShift }   //run       [1]
    };
}

