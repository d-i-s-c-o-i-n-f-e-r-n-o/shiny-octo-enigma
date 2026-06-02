using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject SaveMenu;
    public GameObject SettingsDeleteMenu;
    [Header("Transition things")]
    public GameObject blackImage;
    public GameObject transition;
    public Color transitionColor;
    public Color rollColor;

    public LoseScreen loseScreen;

    public bool settingVisible = false, saveVisible = false, settingsDeleteVisible = false;
    

    //Vector3 MenuPos = new(0, 0, -10);
    //int lvl = 0;

    private void Awake()
    {
        if (SaveManager.instance != null) SaveManager.instance.Load();
    }
    void Start()
    {
        //Transition(true);
    }
    //-----База
    public void AppExit()
    {
        AudioController.instance.PlaySound("ui", 0);
        SaveManager.instance.activeSave.isGameOn = false;
        SaveManager.instance.Save();
        Application.Quit();
    }
    public void Transition()
    {
        Instantiate(transition, new Vector3(transform.position.x, transform.position.y, transform.position.z + 10), transform.rotation);
    }
    //-----Настройки
    public void Saves()
    {
        AudioController.instance.PlaySound("ui", 0);
        saveVisible = !saveVisible;
        SaveMenu.SetActive(saveVisible);
    }
    public void SettingsDelete()
    {
        AudioController.instance.PlaySound("ui", 0);
        settingsDeleteVisible = !settingsDeleteVisible;
        SettingsDeleteMenu.SetActive(settingsDeleteVisible);
    }

    public void DeleteSaves()
    {
        SaveManager.instance.DeleteSaveData();
        LoadMain();
    }
    public void DeleteSettings()
    {
        SaveManager.instance.DeleteSaveData();
        LoadMain();
    }

    //-----Загрузка разных сцен
    public void ToMainMenu(bool click)
    {
        if (click) AudioController.instance.PlaySound("ui", 0);
        Transition();
        Invoke("LoadMain", 0.5f);
    }
    public void ToLvlChoose_FromLvl()
    {
        LoadMain();
        //LvlVisible();
    }
    void LoadMain()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public IEnumerator ToCutscene2()
    {
        Transition();
        yield return StaticHolder.wait[1];
        SceneManager.LoadScene(3);
    }
    public void ToCutscene3()
    {
        SceneManager.LoadScene(2);
    }

    public void ToCredits()
    {
        StartCoroutine(ToCredit());
    }

    IEnumerator ToCredit()
    {
        Transition();
        AudioController.instance.PlaySound("ui", 0);
        yield return StaticHolder.wait0[5];
        SceneManager.LoadScene(5);
    }

    public IEnumerator LoadChapter(int chapter)
    {
        AudioController.instance.PlaySound("ui", 0);
        transitionColor = Color.black;
        rollColor = Color.red;
        Transition();
        int scene = 0;
        switch (chapter)
        {
            case 1: scene = 2; break;
            case 2: break;
            case 3: break;
        }
        yield return StaticHolder.wait[1];
        SceneManager.LoadScene(scene);
    }
}