using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Intro : MonoBehaviour
{
    public Color transitionColor;
    public Color rollColor;
    [Header("Game Objects")]
    public Animator cat;
    public RectTransform logo;
    public RectTransform headphones;

    void Start()
    {
        StartCoroutine(StartIntro());
        
        cat.speed = 0.9f;
    }

    IEnumerator StartIntro()
    {
        yield return StaticHolder.wait0[1];
        StartCoroutine(UI_Utility.ColorChange(cat.GetComponent<UnityEngine.UI.Image>(), Color.white, 50f));   //Появление кота


        yield return StaticHolder.wait[1];
        AudioController.instance.PlaySound("intro", 0);                                      //звук


        yield return StaticHolder.wait0[4];
        cat.SetBool("pop", true);                                           //анимация котика


        yield return new WaitForEndOfFrame();
        cat.SetBool("pop", false);                                          //сброс анимации
        
        StartCoroutine(UI_Utility.SmoothUImove(logo, new Vector2(340, -350), false, 50));
        StartCoroutine(UI_Utility.SmoothUImove(headphones, new Vector2(0, 200), false, 50));
        Vector3 zero3 = Vector3.zero;
        while (logo.localScale.x < 1f && headphones.localScale.x < 1f)
        {
            logo.localScale = Vector3.SmoothDamp(logo.localScale, Vector3.one, ref zero3, 0.25f);
            headphones.localScale = Vector3.SmoothDamp(headphones.localScale, Vector3.one, ref zero3, 0.25f);
            yield return new WaitForEndOfFrame();
        }
        GameManager gm = Camera.main.GetComponent<GameManager>();
        gm.transitionColor = transitionColor;
        gm.rollColor = rollColor;
        yield return StaticHolder.wait[3];
        gm.ToMainMenu(false);
    }
}
