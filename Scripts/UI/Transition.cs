using System.Collections;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public GameObject roll;
    public float animDelta = 0.1f;
    public float freezingTime = 0.75f;
    public float time = 0;

    public static Transition instance = null;

    void Awake()
    {
        // Одиночка
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Остаётся между сценами
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(Transitioning());
    }

    IEnumerator Transitioning()
    {
        gameObject.SetActive(true);
        AudioController.instance.PlaySound("ui", 1);
        StartCoroutine(Rotating(animDelta));

        GameManager gm = Camera.main.GetComponent<GameManager>();
        SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer rspr = roll.GetComponent<SpriteRenderer>();

        {
            rspr.color = Color.clear;
            spr.color = Color.clear;
            StartCoroutine(UI_Utility.ColorChange(spr, gm.transitionColor, time));
            yield return StartCoroutine(UI_Utility.ColorChange(rspr, gm.rollColor, time));
            yield return new WaitForSeconds(freezingTime);
        }
        {
            rspr.color = gm.rollColor;
            spr.color = gm.transitionColor;
            StartCoroutine(UI_Utility.ColorChange(spr, Color.clear, time));
            yield return StartCoroutine(UI_Utility.ColorChange(rspr, Color.clear, time));
            yield return StaticHolder.wait[1];
        }

        Destroy(gameObject);
    }

    IEnumerator Rotating(float delta)
    {
        int angle = 0;
        while (true)
        {
            roll.transform.localRotation = Quaternion.Euler(0, 0, angle);
            angle = (angle + 90) % 360;
            yield return new WaitForSeconds(delta);
        }
    }
}