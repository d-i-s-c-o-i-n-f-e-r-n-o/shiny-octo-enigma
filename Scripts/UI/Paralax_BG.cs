using System.Collections;
using TMPro;
using UnityEngine;

public class Paralax_BG : MonoBehaviour
{
    public float speed;
    public Color red, blue;
    

    public bool done = true;
    private Vector2[] movePosition = new Vector2[2];
    void Start()
    {
        speed /= 2200.0f;
        Time.timeScale = 1f;
        movePosition[0] = new Vector2(Mathf.Sign(-speed) * 23.067f, transform.position.y); //от
        movePosition[1] = new Vector2(Mathf.Sign(speed) * 23.067f, transform.position.y); //до

        //Debug.Log($"{gameObject.name}: {movePosition[0]}, {movePosition[1]}");
        StartCoroutine(ParalaxAnimate());
    }

    IEnumerator ParalaxAnimate()
    {
        while (true)
        {
            Coroutine routine = StartCoroutine(UI_Utility.SimpleUImove(transform, movePosition[1], false, speed));
            yield return routine;
            StopCoroutine(routine);
            if (Mathf.Abs(transform.position.x) > 23.06f)
                transform.position = movePosition[0]; //тпшка
        }
    }

    public void ChangeColorToRed(bool toRed)
    {
        StartCoroutine(UI_Utility.ColorChange(GetComponent<SpriteRenderer>(), toRed ? red : blue, 0));
    }
}