using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    
    public bool isActive = false;
    /*public */Vector2[] mapPos = {
        new (717, 130), //0: ostvine
        new (472, 270), //1: watercall
        new (117, 674), //2: vivenstone
        new (-387, 840),//3: oldmine
        new (787, 800), //4: oldshell
        new (770, -446),//5: lagholm
        new (0, 10),    //6: whole
    };
    public UnityEngine.UI.Button[] mapBut = new UnityEngine.UI.Button[7];
    float INACCURACY = 0.05f;
    float time = 0.3f;
    public GameObject app;
    public RectTransform map;
    public TextGenerator description;
    public Image darkBG;
    //0.55f

    public void MoveToTown(int num)
    {
        if (map.anchoredPosition != mapPos[num])
        {
            description.Generate(Convert.ToByte(num), true);

            if (num == 6)
            {
                StartCoroutine(SmoothMapMove(mapPos[num], Vector3.one, time));
                StartCoroutine(UI_Utility.FillImagePartially_Up(darkBG, darkBG.fillAmount, 1f, 3));
            }
            else
            {
                StartCoroutine(SmoothMapMove(mapPos[num], Vector3.one * 5, time));
                StartCoroutine(UI_Utility.FillImagePartially_Down(darkBG, darkBG.fillAmount, 0.5f, 3));
            }
        }
    }

    public void Click(bool set)
    {
        for (int i = 0; i < 7; i++) mapBut[i].interactable = set;
    }

    private IEnumerator SmoothMapMove(Vector2 to, Vector3 scale, float time)
    {
        Vector2 zero2 = Vector2.zero;
        Vector3 zero3 = Vector3.zero;

        Click(false);

        while (Vector2.Distance(map.anchoredPosition, to) > INACCURACY)
        {
            //Debug.Log(Vector2.Distance(map.anchoredPosition, to));
            map.localScale = Vector3.SmoothDamp(map.localScale, scale, ref zero3, time);
            map.anchoredPosition = Vector2.SmoothDamp(map.anchoredPosition, to, ref zero2, time);
            yield return new WaitForEndOfFrame();
        }
        map.anchoredPosition = to;
        map.localScale = scale;
        while (!description.done) yield return new WaitForEndOfFrame();
        Click(true);
    }
}