using System;
using System.Collections;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    public bool isActive;
    public UnityEngine.UI.Button[] questBut = new UnityEngine.UI.Button[3];
    public float time = 0.2f;
    public TextGenerator description;

    public void ShowQuest(int num)
    {
        StartCoroutine(WaitForText(num));
    }
    public void Click(bool set)
    {
        for (int i = 0; i < 3; i++) questBut[i].interactable = set;
    }
    private IEnumerator WaitForText(int num)
    {
        Click(false);
        description.Generate(Convert.ToByte(num), true);
        while (!description.done) yield return new WaitForEndOfFrame();
        Click(true);
    }
}
