using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    
    bool move = true;
    bool setting = false;
    bool isGamepad = false;

    [Header("UI to move")]
    public RectTransform tape;
    public RectTransform startButton;
    public RectTransform header;
    public RectTransform settings;
    public GameObject mainElements;

    public RectTransform backButton;
    public RectTransform chapterButton;
    public GameObject saveElements;
    public GameObject mouseSettings;

    public RectTransform[] switchRect;

    readonly Dictionary<string, Vector2[]> uiPos = new() //[0] - menu; [1] - chapter
    {
        //{ "tape", new Vector2[] { new (35, 0), new (954, 70) } },
        { "tape", new Vector2[] { new (35, 0), new (35, 1700) } },
        { "start", new Vector2[] { new (-70, 85), new (440, 85) } },
        { "back", new Vector2[] { new (-440, 85), new (400, 85) } },
        { "chapter", new Vector2[] { new (680, 85), new (-70, 85) } },
        { "header", new Vector2[] { new (-50, -80), new (-50, 425) } },
        { "setting", new Vector2[] { new (0, 0), new (-1980, 0) } },
        { "control0", new Vector2[] { Vector2.zero, new (0, -35) } },
        { "control1", new Vector2[] { new(80, 0), new (80, 35) } }
    };

    [Header("SettingsApps")]
    public GameObject[] settingsApps;

    [Header("SelectChapter")]
    public UnityEngine.UI.Button[] chapter = new UnityEngine.UI.Button[3];
    public TextGenerator chapterName;
    private int currentChapter = 0;

    [Header("ControlKeys")]
    public InputActionAsset input;
    public TMPro.TMP_Text[] keysText;

    [Header("Lights")]
    public Light2D[] lights;
    readonly Dictionary<string, Color[]> lightColor = new()
    {
        { "global", new Color[] { new(0f, 1f, 0.7137256f, 1f), new(0f, 0.7040162f, 1f, 1f) } },
        { "glory",  new Color[] { new(1f, 0.03753387f, 0f, 0.1019608f), new(1f, 0f, 0f, 0.03529412f) } },
        { "beam",   new Color[] { new(1f, 0.198849f, 0f, 1f), new(1f, 0.4324183f, 0f, 0.193f) } },
        { "sunset", new Color[] { new(1f, 0.151578f, 0f, 1f), new(1f, 0.04958333f, 0f, 0.7450981f) } },
    };

    void Start()
    {
        InputSystemSetter();
        
        for (int i = SaveManager.instance.activeSave.chapterDone.Length - 1; i >= 0; i--) //ищем какая глава текущая
            if (!SaveManager.instance.activeSave.chapterDone[i]) continue;
    }
    private void Update()
    {
        if (Mouse.current.leftButton.isPressed)
            mouseSettings.transform.localScale = new Vector3(-1, 1, 0);
        else mouseSettings.transform.localScale = new Vector3(1, 1, 0);
    }

    void InputSystemSetter()
    {
        InputSystemSetManager.inputActions = input;
        input = null;
        InputSystemSetManager.SetAllBindingsText(isGamepad ? "Gamepad" : "Keyboard", keysText);
    }

    /************************
     * Передвигалки менюшек *
    *************************/
    void ToggleElements(bool isMain)
    {
        saveElements.SetActive(!isMain);
        mainElements.SetActive(isMain);
    }
    public void ToChapterChoose()
    {
        if (move)
            StartCoroutine(MoveUI_toChapterChoose());
    }
    IEnumerator MoveUI_toChapterChoose()
    {
        move = false;
        //------------------------------------------------------------------
        AudioController.instance.PlaySound("ui", 0);
        startButton.GetComponentInChildren<Button>().interactable = false;

        ChangeLightsColor(1);
        StartCoroutine(UI_Utility.SmoothUImove(header, uiPos["header"][1]));
        StartCoroutine(UI_Utility.SmoothUImove(startButton, uiPos["start"][1]));

        yield return StartCoroutine(UI_Utility.SmoothUImove(tape, new(uiPos["tape"][1].x, -uiPos["tape"][1].y)));

        ToggleElements(false);
        StartCoroutine(UI_Utility.SmoothUImove(backButton, uiPos["back"][1]));
        tape.anchoredPosition = uiPos["tape"][1];

        yield return StartCoroutine(UI_Utility.SmoothUImove(tape, uiPos["tape"][0]));
        backButton.GetComponentInChildren<Button>().interactable = true;
        //------------------------------------------------------------------
        move = true;
    }
    public void ToMain()
    {
        if (move)
            StartCoroutine(MoveUI_toMain());
    }
    IEnumerator MoveUI_toMain()
    {
        move = false;
        //------------------------------------------------------------------
        AudioController.instance.PlaySound("ui", 0);
        backButton.GetComponentInChildren<Button>().interactable = false;
        chapterButton.GetComponentInChildren<Button>().interactable = false;

        StartCoroutine(UI_Utility.SmoothUImove(header, uiPos["header"][0]));
        StartCoroutine(UI_Utility.SmoothUImove(backButton, uiPos["back"][0]));
        StartCoroutine(UI_Utility.SmoothUImove(chapterButton, uiPos["chapter"][0]));
        ChangeLightsColor(0);

        yield return StartCoroutine(UI_Utility.SmoothUImove(tape, new(uiPos["tape"][1].x, -uiPos["tape"][1].y)));

        chapterName.TextArea.text = " ";
        chapterButton.gameObject.SetActive(false);
        ToggleElements(true);

        for (int i = 0; i < chapter.Length; i++)
            chapter[i].interactable = true; //включаем кнопочки

        StartCoroutine(UI_Utility.SmoothUImove(startButton, uiPos["start"][0]));
        tape.anchoredPosition = uiPos["tape"][1];
        

        yield return StartCoroutine(UI_Utility.SmoothUImove(tape, uiPos["tape"][0]));
        startButton.GetComponentInChildren<Button>().interactable = true;
        //------------------------------------------------------------------
        move = true;
    }

    /*********************************
     * Всё что связано с настройками *
    **********************************/

    public void OpenSettings()
    {
        if (move)
            StartCoroutine(MoveUI_OpenSettings());
    }
    IEnumerator MoveUI_OpenSettings()
    {
        move = false;
        //------------------------------------------------------------------
        AudioController.instance.PlaySound("ui", 0);
        startButton.GetComponentInChildren<Button>().interactable = false;

        setting = !setting;
        if (setting)
        {
            ShowControl();
            StartCoroutine(UI_Utility.SmoothUImove(settings, uiPos["setting"][0], false, 0)); //открыть настройки
            StartCoroutine(UI_Utility.SmoothUImove(tape, new(35, -1700), false, 0));
        }
        else
        {
            StartCoroutine(UI_Utility.SmoothUImove(settings, uiPos["setting"][1], true, 0)); //закрыть настройки
            StartCoroutine(UI_Utility.SmoothUImove(tape, uiPos["tape"][0], false, 0));
        }

        yield return StaticHolder.wait[1]; //пауза

        startButton.GetComponentInChildren<Button>().interactable = true;
        move = true;
    }
    public void ShowControl()
    {
        ToggleApps(new bool[] { true, false, false, false });
    }
    public void ShowSound()
    {
        ToggleApps(new bool[] { false, true, false, false });
    }
    public void ShowReset()
    {
        ToggleApps(new bool[] { false, false, true, false });
    }
    void ToggleApps(bool[] toggles)
    {
        for (int i = 0; i < settingsApps.Length; i++)
            settingsApps[i].SetActive(toggles[i]);
    }

    /****************************************
     * Меню выбора Главы и все составляющие *
    *****************************************/

    public void SelectChapter(int num)
    {
        if (move && chapterName.done)
            StartCoroutine(MoveUI_ContinueButton(num-1));
    }
    IEnumerator MoveUI_ContinueButton(int num)
    {
        move = chapterButton.GetComponentInChildren<Button>().interactable = false;
        //---------------------------------------------
        AudioController.instance.PlaySound("ui", 0);

        for (int i = 0; i < chapter.Length; i++)
            chapter[i].interactable = true; //включаем кнопочки

        chapter[num].interactable = false; //выбираем нужную кнопочку
        chapterName.Generate(Convert.ToByte(num+1), true); //печатаем имя главы
        //-------------------------------------------------
        chapterButton.gameObject.SetActive(true);
        
        yield return StartCoroutine(UI_Utility.SmoothUImove(chapterButton, uiPos["chapter"][1]));
        //---------------------------------------------------
        if (num == currentChapter) chapterButton.GetComponentInChildren<Button>().interactable = true;
        //---------------------------------------------------
        move = true;
    }
    public void StartChapter()
    {
        if (move)
        {
            AudioController.instance.PlaySound("ui", 0);
            StartCoroutine(Camera.main.GetComponent<GameManager>().LoadChapter(currentChapter + 1));
        }
    }

    public void ChangeLightsColor(int index)
    {
        int i = 0;
        foreach (string name in lightColor.Keys.ToList())
        {
            StartCoroutine(UI_Utility.ColorChange(lights[i++], lightColor[name][index], 100));
        }
    }


    /*******************************************************
     * ВСЁ ЧТО СВЯЗАНО С ПЕРЕНАЗНАЧЕНИЕМ КЛАВИШ УПРАВЛЕНИЯ *
    ********************************************************/

    bool needBinding = false;
    
    void OnGUI()
    {
        if (needBinding)
        {
            Debug.Log("WAITING FOR KEY");
            string scheme = null;
            
            if (InputSystemSetManager.CheckKeyboard(out ButtonControl key))
                scheme = "Keyboard";
            else if (InputSystemSetManager.CheckMouse(out key))
                scheme = "Mouse";
            else if (InputSystemSetManager.CheckGamepad(out key))
                scheme = "Gamepad";

            if (scheme != null)
            {
                needBinding = InputSystemSetManager.ApplyBinding(scheme, key.path);
                InputSystemSetManager.SetAllBindingsText(isGamepad? "Gamepad" : "Keyboard", keysText);
                //Если значение isGamepad != scheme тогда можно сделать выскакивание предупреждения, что вы не то устройство перебиндили
            }
        }
    }
    public void SetCompositeBinding(string path)
    {
        needBinding = InputSystemSetManager.SetComposite(path);
    }
    public void SetButtonBinding(string path)
    {
        needBinding = InputSystemSetManager.SetButton(path);
    }

    public void ControlSwitch()
    {
        StartCoroutine(ControlSW());
    }
    IEnumerator ControlSW()
    {
        Button but = switchRect[0].GetComponentInParent<Button>();
        but.interactable = false;
        isGamepad = !isGamepad;
        InputSystemSetManager.SetAllBindingsText(isGamepad ? "Gamepad" : "Keyboard", keysText);
        int j = Convert.ToInt32(isGamepad);
        //Debug.Log(but.gameObject.name);
        //------------------------------------
        for (int i = 0; i < switchRect.Length; i++)
            StartCoroutine(UI_Utility.SmoothUImove(switchRect[i], uiPos[$"control{i}"][j], false, 0));

        yield return StaticHolder.wait[2];

        for (int i = 0; i < switchRect.Length; i++)
            switchRect[i].anchoredPosition = uiPos[$"control{i}"][j];
        //--------------------------------------
        but.interactable = true;
    }
}



public static class InputSystemSetManager
{
    public static InputActionAsset inputActions;
    static string composite;
    static InputAction actionNeedsBinding;

    static string TrimPath(string path) => string.Join("/", path.Trim('/').Split('/').Skip(1));
    public static bool CheckKeyboard(out ButtonControl pressed)
    {
        pressed = null;
        if (Keyboard.current == null) return false;

        pressed = Keyboard.current.allControls.OfType<ButtonControl>().LastOrDefault(b => b.wasPressedThisFrame);
        return pressed != null;
    }
    public static bool CheckGamepad(out ButtonControl pressed)
    {
        pressed = null;
        if (Gamepad.current == null) return false;

        pressed = Gamepad.current.allControls.OfType<ButtonControl>().LastOrDefault(b => b.wasPressedThisFrame);
        return pressed != null;
    }
    public static bool CheckMouse(out ButtonControl pressed)
    {
        pressed = null;
        if (Mouse.current == null) return false;

        pressed = Mouse.current.allControls.OfType<ButtonControl>().LastOrDefault(b => b.wasPressedThisFrame);
        return pressed != null;
    }
    public static bool ApplyBinding(string scheme, string path)
    {
        var binds = actionNeedsBinding.bindings;
        for (int i = 0; i < binds.Count; i++)
            Debug.Log($"idx {i}: path={binds[i].path} name={binds[i].name} groups={binds[i].groups} isComposite={binds[i].isComposite} isPartOfComposite={binds[i].isPartOfComposite}");


        string newPath = $"<{scheme}>/{TrimPath(path)}";
        if (actionNeedsBinding.type == InputActionType.Value) //Если он композитный
        {
            int index = actionNeedsBinding.bindings.ToList().FindIndex(b => b.isPartOfComposite && b.name == composite);
            Debug.Log($"I'm overriding composite one: {newPath} to index {index}");
            if (index != -1) actionNeedsBinding.ApplyBindingOverride(index, newPath);
        }
        else //Если кнопочка
        {
            int index = actionNeedsBinding.bindings.ToList().FindIndex(b => b.groups.Contains(scheme));
            Debug.Log($"I'm overriding button one: {newPath} to index {index}");
            if (index != -1) actionNeedsBinding.ApplyBindingOverride(index, newPath);
        }

        actionNeedsBinding = null;
        return false;
    }

    static string GetBindingsAsString(string scheme, InputAction action)
    {
        var bindings = action.bindings.ToList();
        string result = "";

        foreach (var binding in bindings)
        {
            if (binding.isComposite) continue;
            if (binding.groups.Contains(scheme))
                result += binding.effectivePath.Split('/').Last() + "\n";
        }
        return result;
    }

    public static void SetAllBindingsText(string scheme, params TMPro.TMP_Text[] keysText)
    {
        InputActionMap map = inputActions.FindActionMap("Player");
        string[] set = new string[]
        {
            GetBindingsAsString(scheme, map.FindAction("Move")),

            GetBindingsAsString(scheme, map.FindAction("Attack"))
            + GetBindingsAsString(scheme, map.FindAction("ChangeWeapon"))
            + GetBindingsAsString(scheme, map.FindAction("Reload")),

            GetBindingsAsString(scheme, map.FindAction("Use"))
            + GetBindingsAsString(scheme, map.FindAction("Interact"))
            + GetBindingsAsString(scheme, map.FindAction("Pause"))
            + GetBindingsAsString(scheme, map.FindAction("Sprint"))
        };

        for (int i = 0; i < set.Length && i < keysText.Length; i++) keysText[i].text = set[i];
    }

    //path состоит из двух наименований, он пишется так: Действие/названиеБиндинга
    public static bool SetComposite(string path)
    {
        Debug.Log($"I'm here {nameof(SetComposite)}");
        string[] searchPath = path.Split('/');

        actionNeedsBinding = inputActions.FindActionMap("Player").FindAction(searchPath.First());
        InputBinding bind = actionNeedsBinding.bindings.ToList().LastOrDefault(b => b.name == searchPath.Last());
        if (bind != null)
        {
            composite = TrimPath(bind.groups.Contains("Keyboard") ? path : path.ToLower());
            Debug.Log(composite);
            return true;
        }
        return false;
    }

    //path состоит только из названия действия, пишется: Действие
    public static bool SetButton(string path)
    {
        Debug.Log($"I'm here {nameof(SetButton)}");
        actionNeedsBinding = inputActions.FindActionMap("Player").FindAction(path);
        composite = null;
        return true;
    }
}