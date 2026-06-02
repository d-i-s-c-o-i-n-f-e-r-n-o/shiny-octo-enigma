using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Note : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject note;
    KeyControl interact;

    void Start()
    {
        interact = SaveManager.instance.activeSettings.keys[0];
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) //если игрок подошёл к ней, то активируем клавишу и ждём
        {
            buttons[interact.keyCode == Key.E ? 0 : 1].SetActive(true);
            if (interact.wasPressedThisFrame) note.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        note.SetActive(false);
        StopAllCoroutines();
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].SetActive(false);
    }
}
