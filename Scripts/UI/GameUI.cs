using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    FPlayer player;

    [Header("PhoneMovements")]
    public RectTransform Phone;
    public RectTransform Backpack;
    public Image PhoneHolographic;
    public Image Thought;

    [Header("PhoneApps")]
    public Map mapApp;
    public Diary diaryApp;
    public Quest questApp;
    public Profile profileApp;
    public GameObject pauseApp;
    public GameObject icons;

    [Header("Booleans")]
    public bool phone = false; //пауза
    public bool backpack = false; //рюкзак
    public bool canTogglePhone = true;
    public bool canToggleBackpack = true;
    public bool thought = false;

    PlayerInput input;

    [Header("UI positions")]
    Dictionary<string, Vector2[]> menuPos = new Dictionary<string, Vector2[]>()
    {
        { "phone", new Vector2[] { new(620, -1045), new(620, 10) } },
        { "backpack", new Vector2[] { new(-1740, -126.8f), new(-453.2f, -126.8f) } }
    };

    //--------------------------------------------
    private void Awake()
    {
        input = new PlayerInput();
        input.UI.Enable();

        input.UI.Exit.performed += context => PhoneActivation(context);
        input.UI.Journal.performed += context => PhoneActivation(context, "journal");
        input.UI.Map.performed += context => PhoneActivation(context, "map");
        input.UI.Person.performed += context => PhoneActivation(context, "person");
        input.UI.Diary.performed += context => PhoneActivation(context, "log");

        input.UI.Backpack.performed += context => BackpackActivation(context);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FPlayer>();

        TogglePhoneApps();
    }

    //------------Переключалка телефона----------------------
    void PhoneActivation(UnityEngine.InputSystem.InputAction.CallbackContext obj, string app = null)
    {
        //Debug.Log("Pressed");
        //переключалка телефона
        if (!player.isPlot && canTogglePhone) //тогда возможно тыкать кнопки
        {
            //Debug.Log("Entered");
            if (phone && (mapApp.isActive || diaryApp.isActive || questApp.isActive /*|| profileApp.isActive*/))
            {
                if (app != null) switch (app)
                {
                    case "map": TogglePhoneApps(isMap: true); break;
                    case "journal": TogglePhoneApps(isQuest: true); break;
                    case "person": TogglePhoneApps(isProfile: true); break;
                    case "log": TogglePhoneApps(isDiary: true); break;
                }
                else TogglePhoneApps(isPause: true);
            }
            else
            {
                canTogglePhone = false;
                StartCoroutine(UI_Utility.SmoothUImove(Phone, menuPos["phone"][Convert.ToInt32(!phone)], phone, onComplete: () => canTogglePhone = true));
                TogglePhoneApps();
                StartCoroutine(UI_Utility.FillImageFull(PhoneHolographic, phone, onComplete: () => TogglePhoneApps(isPause: phone)));
                AudioController.instance.PlaySound("ui", 0);
                phone = !phone;
            }
        }
    }
    void BackpackActivation(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Here");
        if (!player.isPlot && canToggleBackpack)
        {
            canToggleBackpack = false;
            StartCoroutine(UI_Utility.SmoothUImove(Backpack, menuPos["backpack"][Convert.ToInt32(!backpack)], backpack, onComplete: () => canToggleBackpack = true));
            backpack = !backpack;
        }
    }

    //Приложения
    public void StartMap()
    {
        TogglePhoneApps(isMap: true);
    }
    public void StartQuest()
    {
        TogglePhoneApps(isQuest: true);
    }
    public void StartDiary()
    {
        TogglePhoneApps(isDiary: true);
    }
    //public void StartProfile()
    //{
    //    TogglePhoneApps(false, false, false, true);
    //}

    void TogglePhoneApps(bool isMap = false, bool isQuest = false, bool isDiary = false, bool isPause = false, bool isProfile = false)
    {
        mapApp.gameObject.SetActive(isMap);
        mapApp.app.SetActive(isMap);
        mapApp.isActive = isMap;

        diaryApp.gameObject.SetActive(isDiary);
        diaryApp.note.gameObject.SetActive(isDiary);
        diaryApp.isActive = isDiary;

        questApp.gameObject.SetActive(isQuest);
        questApp.description.gameObject.SetActive(isQuest);
        questApp.isActive = isQuest;

        profileApp.gameObject.SetActive(isProfile);
        profileApp.app.SetActive(isProfile);
        profileApp.isActive = isProfile;

        bool isIcon = !(isMap || isQuest || isDiary);
        icons.SetActive(isIcon); //если никакое приложение не работает, то обои
        pauseApp.SetActive(isPause);
    }


    //-------------Мысля-----------------------------------
    public IEnumerator ThoughtAppear(byte num)
    {
        player.isPlot = thought = true;
        StartCoroutine(UI_Utility.FillImageFull(Thought, false, 2));
        TextGenerator txt = Thought.GetComponentInChildren<TextGenerator>();
        txt.iconImg.gameObject.SetActive(true);

        yield return StaticHolder.wait[1];
        
        player.isPlot = false;
        txt.Generate(num, true);
        while (!txt.done) yield return new WaitForEndOfFrame();

        yield return StaticHolder.wait[5];
        txt.iconImg.gameObject.SetActive(false);
        txt.Erase();
        yield return StartCoroutine(UI_Utility.FillImageFull(Thought, true, 2));
        thought = false;
    }
    //------------------------------------------
    // Вот здесь будут все иконки обрабатываться
    //------------------------------------------
    [Header("UI Bars")]
    public Image[] barLevel;
    public Image bulletIcon;
    public Image choosenWeapon;

    public GameObject staminaObj;
    public GameObject weaponObj;
    public GameObject weaponLoadObj;
    public TMPro.TMP_Text bulletText;

    [Header("Bar sprites")]
    public Sprite[] healthSprite0;
    public Sprite[] defenceSprite1;
    public Sprite[] backpackSprite2;
    public Sprite[] weaponSprite3;
    public Sprite[] staminaSprite4;
    public Sprite[] bulletIconSprite;

    public static void UpdateBar(Image bar, Sprite[] state, int currentValue, float maxValue)
    {
        //Debug.Log((int)(currentValue / maxValue * (state.Length - 1)));
        bar.sprite = state[(int)(currentValue / maxValue * (state.Length - 1))];
    }
}