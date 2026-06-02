using JetBrains.Annotations;
using System;
using System.Collections;
using System.Globalization;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(GameManager))]
public class Cutscene2 : MonoBehaviour
{
    [Header("Scripts")]
    public TextGenerator blackIntro;
    public TextGenerator diary;
    //public TextGenerator monolog;
    TextGenerator currentTextField;

    public Transform[] layers;

    
    PlayerInput input;
    Animator player;
    public Animator crow;
    public Animator[] crows;
    //RectTransform but;

    [Header("Objects")]
    public RectTransform phone;
    public UnityEngine.UI.Image blackBG;
    public GameObject title;


    [Header("Bool variable")]
    public bool isPlot;
    public bool cutsceneEnabled = false; //помощь для отладки, вкл катсцену после 1 секунды
    public bool auto = false;
    public bool Cutscene2Done;
    bool blackFlag = true;
    bool crowFlag = true;
    bool canSkip = true;
    bool erase = true;

    [Header("Stops")]
    public byte blackLine;
    public byte crowLine;
    public byte csLength;
    public byte line = 0;
    public byte shakingRate = 0;

    void Awake()
    {
        input = new PlayerInput();
        input.Cutscene.Next.performed += OnNextLine;
    }

    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }
    void OnNextLine(InputAction.CallbackContext context)
    {
        TryNextLine();
    }
    void OnCrowStart(InputAction.CallbackContext context)
    {
        StartCoroutine(CrowFalling());
    }
    void OnPhoneStart(InputAction.CallbackContext context)
    {
        StartCoroutine(SwitchToPhone());
    }

    void Start()
    {
        csLength = blackIntro.csLength;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        player.speed = 0.3f;
        //but = GameObject.FindGameObjectWithTag("Cutscene").GetComponent<RectTransform>();
        //Debug.Log(but + " " + but.gameObject.name);

        currentTextField = blackIntro;

        Dialog_Reset();
        StartCoroutine(StartCutscene());
    }

    IEnumerator StartCutscene()
    {
        yield return StaticHolder.wait[1];
        cutsceneEnabled = true;
        Dialog_Start(line);
        //----------Паралакс--------------
        StartCoroutine(Paralax(0.003f));
    }
    IEnumerator Paralax(float speed = 0.045f)
    {
        Vector2[] pos = new Vector2[] { new(-45, 0), new(65, 0) };
        while (!player.GetBool("Stop"))
        {
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].localPosition = Vector3.MoveTowards(layers[i].localPosition, pos[1], speed);
                if (layers[i].localPosition.x > 59.99f) layers[i].localPosition = pos[0];
            }
            yield return StaticHolder.waitFrame;
        }
    }

    IEnumerator SwitchToPhone()
    {
        input.Cutscene.Next.performed -= OnPhoneStart;
        yield return StaticHolder.wait[1];

        //-------------Чёрный экран убирается----------------
        Color a = blackIntro.GetComponent<TMPro.TMP_Text>().color;
        a.a = 0f;
        StartCoroutine(UI_Utility.ColorChange(blackIntro.GetComponent<TMPro.TMP_Text>(), a, 30,
            onComplete: () => blackIntro.GetComponent<TMPro.TMP_Text>().text = null));
        
        yield return StartCoroutine(UI_Utility.ColorChange(blackBG, Color.clear, 50,
            onComplete: () => blackBG.raycastTarget = false));

        //-------------Появляется телефон----------------
        StartCoroutine(UI_Utility.SmoothUImove(phone, new Vector2(-160, -120), false, 15f));

        //-------------Рисуем запись----------------
        while (!diary.done) yield return StaticHolder.waitFrame;
        diary.skip = erase = auto = false;
        diary.specialDelay = 0.03f;
        yield return StaticHolder.wait[1];
        //--------------------------------------------------------------
        Dialog_Start(line);
        while (!diary.done) yield return StaticHolder.waitFrame;
        //--------------------------------------------------------------
        cutsceneEnabled = canSkip /*= auto */= true;
    }
    IEnumerator MoveCrows(int[] which, float to_x, float speed = 0.045f, Action onComplete = null)
    {
        while (crows[which[^1]].transform.localPosition.x < to_x)
        {
            for (int i = 0; i < which.Length; i++)
            {
                Vector3 to = new Vector3(12, crows[which[i]].transform.localPosition.y);
                crows[which[i]].transform.localPosition = Vector3.MoveTowards(crows[which[i]].transform.localPosition, to, speed);
            }
            yield return StaticHolder.waitFrame;
        }
        onComplete?.Invoke();
    }

    IEnumerator CrowFalling()
    {
        input.Cutscene.Next.performed -= OnCrowStart;
        input.Cutscene.Next.performed -= OnNextLine;
        yield return StaticHolder.wait[1];

        //--------------Убираем дневник----------------
        StartCoroutine(UI_Utility.SmoothUImove(phone, new Vector2(-1500, -80), false, 15f));
        Vector3 to = new (0, 0, -10);
        while (Camera.main.transform.position != to)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, to, 0.04f);
            yield return StaticHolder.waitFrame;
        }
        //--------------Трясём камеру----------------
        InvokeRepeating(nameof(CameraShake), 0, 0.05f);
        //--------------Игрок останавливается------------
        player.SetBool("Stop", true);
        StopCoroutine(nameof(Paralax));

        float crow_speed = 0.07f;
        Vector2[] crow_fall = new Vector2[]
        {
            new (3.75f, 0.25f),
            new (-1.2f, 0.75f),
            new (2.75f, -1f),
            new (-3f, -1.33f),
            new (-5f, -1.6f),
            new (-1.2f, 0.75f)
        };

        StartCoroutine(UI_Utility.SimpleUImove(crows[0].gameObject.transform, crow_fall[0], false, crow_speed));
        yield return StaticHolder.wait[1];
        player.speed = 1.3f;
        player.SetBool("LookAround", true);
        StartCoroutine(UI_Utility.SimpleUImove(crows[1].gameObject.transform, crow_fall[1], false, crow_speed));
        StartCoroutine(UI_Utility.SimpleUImove(crows[2].gameObject.transform, crow_fall[2], false, crow_speed));

        yield return StaticHolder.WaitThis(StaticHolder.wait[1], StaticHolder.wait0[2]);
        player.SetBool("LookAround", false);
        yield return StaticHolder.wait[2];
        //--------------Игрок бегит------------
        player.SetBool("Stop", false);
        StartCoroutine(Paralax());

        //---------Да ёбаные вороны-----------
        yield return StartCoroutine(MoveCrows(new int[] { 0, 1, 2 }, 5f));
        StartCoroutine(UI_Utility.SimpleUImove(crows[3].gameObject.transform, crow_fall[3], false, crow_speed));
        yield return StartCoroutine(MoveCrows(new int[] { 0, 1, 2 }, 10f));
        yield return StartCoroutine(MoveCrows(new int[] { 0, 1, 2, 3 }, 4f));
        StartCoroutine(UI_Utility.SimpleUImove(crows[4].gameObject.transform, crow_fall[4], false, crow_speed));
        StartCoroutine(UI_Utility.SimpleUImove(crows[5].gameObject.transform, crow_fall[5], false, crow_speed));
        yield return StartCoroutine(MoveCrows(new int[] { 3 }, 10f));
        yield return StartCoroutine(MoveCrows(new int[] { 5, 4 }, 10f));
        //--------------Камеру перестаёт трясти------------
        CancelInvoke();
        //--------------Останавливается и оборачивается----------
        yield return StaticHolder.wait[1];
        player.speed = 1f;
        player.SetBool("Stop", true);
        
        yield return StaticHolder.wait[1];
        player.SetBool("LookAround", true);
        //--------------Варона атакует------------------
        yield return StartCoroutine(UI_Utility.SimpleUImove(crow.gameObject.transform, new Vector2(player.transform.position.x, player.transform.position.y+0.75f), false, crow_speed));
        while (!player.GetBool("Death")) yield return StaticHolder.waitFrame;
        yield return StartCoroutine(UI_Utility.SimpleUImove(crow.gameObject.transform, new Vector2(-0.25f, -0.8f), false, 0.04f));
        crow.gameObject.transform.position = new Vector3(-0.25f, -0.8f, 0);
        //---------------плохо в глазах------------------
        blackBG.gameObject.SetActive(true);
        blackBG.gameObject.transform.localPosition = Vector3.zero;

        yield return StaticHolder.wait0[2];
        yield return StartCoroutine(UI_Utility.ColorChange(blackBG, new Color(0.46f, 0, 0))); //красный
        yield return StartCoroutine(UI_Utility.ColorChange(blackBG, Color.white, 15f)); //белый
        yield return StartCoroutine(UI_Utility.ColorChange(blackBG, Color.black, 100f)); //чёрный
        //----------------Тексты--------------------
        Color a = blackIntro.GetComponent<TMPro.TMP_Text>().color;
        a.a = 1f;
        yield return StartCoroutine(UI_Utility.ColorChange(blackIntro.GetComponent<TMPro.TMP_Text>(), a, 50));
        //Debug.Log($"Done! {currentTextField.name}");
        input.Cutscene.Next.performed += OnNextLine;
        cutsceneEnabled = auto = true;
        //--------------------------------------------------
        //Cutscene2Done = true;
    }
    void CameraShake()
    {
        float shake = 0.4f;
        Camera.main.transform.position = new Vector3(UnityEngine.Random.Range(0, shake), UnityEngine.Random.Range(0, shake), -10);
    }

    private void CheckRemoveBlackScreen()
    {
        if (line == blackLine && blackFlag && currentTextField.done) //Убирает чёрный экран
        {
            blackFlag = cutsceneEnabled = false;
            currentTextField = diary;
            input.Cutscene.Next.performed += OnPhoneStart;
        }
    }

    private void CheckTheFallOfTheCrow()
    {
        if (line == crowLine && crowFlag && currentTextField.done) //Падение вороны
        {
            crowFlag = cutsceneEnabled = false;
            //currentTextField = monolog;
            currentTextField = blackIntro;
            input.Cutscene.Next.performed += OnCrowStart;
        }
    }

    private void TryNextLine()
    {
        if (cutsceneEnabled && currentTextField.done && line < csLength)   //если текст уже отрисовался и если не все реплики ещё проиграны
        {
            //Debug.Log(line);
            currentTextField.skip = auto = false;
            if (isPlot && Convert.ToBoolean(currentTextField.dialogEnding))
                Dialog_Reset(); //если идёт сюжет и первая 1, то закрываем
            else Cutscene2Done = Dialog_Start(line);           //иначе начинаем реплику
        }
        //-----------------------------------------------------
        else if (canSkip && !currentTextField.done) currentTextField.skip = true;
    }


    void Update()
    {
        if (auto) TryNextLine();
        CheckRemoveBlackScreen();
        CheckTheFallOfTheCrow();

        //-----------------------------------------------------
        if (Cutscene2Done)
        {
            Cutscene2Done = false;
            StartCoroutine(Titleling());
            //Спавним лого

            
        }
    }

    bool Dialog_Start(byte i)
    {
        isPlot = true;
        currentTextField.Generate(i, erase);
        line++;
        return line == csLength;
    }
    void Dialog_Reset()
    {
        isPlot = false;
        //Работает и без закомментированного.
        //if (currentTextField == dialog)
        //{
        //    dialog.TextArea.text = "";
        //    dialog.TextArea.text += "";
        //}
        //else if (currentTextField == skip1)
        //{
        //    skip1.TextArea.text = "";
        //    skip1.TextArea.text += "";
        //}
    } //Сброс переменных

    IEnumerator Titleling()
    {
        canSkip = false;
        while (!currentTextField.done) yield return StaticHolder.waitFrame;
        yield return StartCoroutine(UI_Utility.ColorChange(currentTextField.TextArea, Color.clear, 300));

        //-----------Заставка появляется---------
        title.SetActive(true);
        yield return colorswitch(1f);
        yield return StaticHolder.wait[3];
        //-----------Заставка исчезает---------
        yield return colorswitch(0f);
        //Debug.Log("Cutscene Done!");

        //------------Переход на другую сцену--------
        SaveManager.instance.activeSave.cutscene2Done = true;
        SaveManager.instance.Save();
        GetComponent<GameManager>().ToCutscene3();

        IEnumerator colorswitch(float a)
        {
            foreach (TMPro.TMP_Text st in title.GetComponentsInChildren<TMPro.TMP_Text>())
            {
                Color needed = st.color;
                needed.a = a;
                yield return StartCoroutine(UI_Utility.ColorChange(st, needed, 300));
                //Debug.Log(st.name);
            }
        }
    }
}