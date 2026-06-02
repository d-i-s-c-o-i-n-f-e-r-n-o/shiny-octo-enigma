using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Diary : MonoBehaviour
{
    public bool isActive;

    public UnityEngine.UI.Button[] noteBut = new UnityEngine.UI.Button[3];
    public float time = 0.2f;
    public TextGenerator note;

    public void ShowNote(int num)
    {
        StartCoroutine(WaitForText(num));
    }
    public void Click(bool set)
    {
        for (int i = 0; i < 3; i++) noteBut[i].interactable = set;
    }
    private IEnumerator WaitForText(int num)
    {
        Click(false);
        note.Generate(Convert.ToByte(num), true);
        while (!note.done) yield return new WaitForEndOfFrame();
        Click(true);
    }
}