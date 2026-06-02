using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class KeyDown : MonoBehaviour
{
    UnityEngine.UI.Image img;
    SpriteRenderer spr;

    public Sprite[] sprites;
    bool isImg;
    public bool isSettings = false;

    public TMPro.TMP_Text[] Text;

    void Start()
    {
        //ui = Camera.main.GetComponent<UI_Utility>();
        isImg = TryGetComponent<UnityEngine.UI.Image>(out img);
        if (!isImg) TryGetComponent<SpriteRenderer>(out spr);

        if (isSettings) for (int i = 0; i < Text.Length; i++) //установка текста
        {
            Text[i].text = SaveManager.instance.activeSettings.keys[i].ToString(); //установка текста
        }

        //Debug.Log(needKey.device);
    }

    //public void OnButtonPressed(InputAction.CallbackContext context)
    //{
    //    if (context.performed) // Если кнопка нажата
    //    {
    //        isHolding = false;
    //        if (tap) StopCoroutine(nameof(KeyPressed));
    //        tap = true;

    //        StartCoroutine(KeyPressed());
    //    }
    //}

    public void OnButtonHeld(InputAction.CallbackContext context)
    {
        if (context.started) // Если кнопка удерживается
        {
            KeyPressed();
        }
        else if (context.canceled) // Если кнопка больше не удерживается
        {
            StopCoroutine(nameof(KeyReleased));
            StartCoroutine(KeyReleased());
        }
    }

    private void KeyPressed()
    {
        if (isImg) img.sprite = sprites[1]; // Изменение на зажатый спрайт
        else spr.sprite = sprites[1]; // Изменение на зажатый спрайт
    }
    public IEnumerator KeyReleased()
    {
        if (isImg)
        {
            img.sprite = sprites[2]; // Изменение на промежуточный
            yield return StaticHolder.wait00[5]; // Задержка
            img.sprite = sprites[0]; // Возврат к отпущенному
            if (!img.gameObject.activeInHierarchy) img.sprite = sprites[0];
        }
        else
        {
            spr.sprite = sprites[2]; // Изменение на промежуточный
            yield return StaticHolder.wait00[5]; // Задержка
            spr.sprite = sprites[0]; // Возврат к отпущенному
            if (!spr.gameObject.activeInHierarchy) spr.sprite = sprites[0];
        }
    }

    public void SwitchButton(int button)
    {
        AudioController.instance.PlaySound("ui", 0);
        //SaveManager.instance.activeSettings.keys[button] = GetComponent<KeyDown>().needKey;
        SaveManager.instance.Save();
        Text[button].text = SaveManager.instance.activeSettings.keys[button].ToString(); //установка текста
    }
}
