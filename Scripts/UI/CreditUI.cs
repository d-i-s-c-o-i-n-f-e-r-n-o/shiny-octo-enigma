using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.UIElements;

[RequireComponent(typeof(UI_Utility))]
[RequireComponent(typeof(GameManager))]
public class CreditUI : MonoBehaviour
{
    public RectTransform creditText;
    public RectTransform redMask;
    public RectTransform but;
    public AudioSource creditMusic;
    public SpriteRenderer[] lights;
    
    PlayerInput input;

    public RectTransform[] people;
    int[] peopleWidth = new int[] { 1200, 925, 925, 925, 925 };

    void Awake()
    {
        input = new PlayerInput();
        input.UI.Exit.performed += context => ForcerExit();
    }
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void Start()
    {
        
        StartCoroutine(Credits());
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape)) ForcerExit();
    //}

    IEnumerator Credits()
    {
        yield return StaticHolder.wait[3];
        //------------------Показываем участников
        for (int i = 0; i < people.Length; i++)
        {
            yield return StartCoroutine(UI_Utility.SmoothUImove(people[i], Vector2.zero));

            for (int j = 730; j < peopleWidth[i]; j += 20)
            {
                people[i].sizeDelta = new Vector2(j, people[i].sizeDelta.y);
                // (people[i].sizeDelta.x > 1150) break;
                yield return StaticHolder.waitFrame;
            }
            yield return StaticHolder.wait[5];

            yield return StartCoroutine(UI_Utility.SmoothUImove(people[i], new Vector2(0, -940)));
        }

        //StartCoroutine(UI_Utility.SimpleUImove(creditText, new(4, 19), false,
        //    0.05f));
        //    //0.05f));

        //------------------Переход в красный режим
        yield return StartCoroutine(UI_Utility.SimpleUImove(redMask, Vector2.zero, false, 1f));

        //---------------------Появляется кнопка принудительного выхода и загорается свет
        yield return StaticHolder.wait0[5];
        yield return StartCoroutine(UI_Utility.SmoothUImove(but, new(0, -430), false, 40));

        StartCoroutine(Light());
        but.GetComponentInChildren<UnityEngine.UI.Button>().interactable = true;

        //---------------------Ждём окончания музыки--------
        while (creditMusic.isPlaying) yield return StaticHolder.waitFrame;
        GetComponent<GameManager>().ToMainMenu(false);
    }
    public void ForcerExit()
    {
        GetComponent<GameManager>().ToMainMenu(true);
    }

    IEnumerator Light()
    {
        while (lights[^1].color != Color.yellow)
        {
            for (int i = 0; i < lights.Length; i++)
                lights[i].color = Color.Lerp(lights[i].color, Color.yellow, 0.03f);

            yield return StaticHolder.waitFrame;
        }
    }
}
