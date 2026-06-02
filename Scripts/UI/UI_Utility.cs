
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class UI_Utility
{
    public static float INACCURACY = 0.05f;

    /************************
     * ПЛАВНОЕ ПЕРЕДВИЖЕНИЕ *
    *************************/

    public static IEnumerator SmoothUImove(RectTransform gmObj, Vector2 to, bool isActive = false, float time = 100f, Action onComplete = null)
    {        
        Vector2 zero = Vector2.zero;

        if (!isActive) gmObj.gameObject.SetActive(true); //если он щас неактивный, то активируем

        while (Mathf.Abs(gmObj.anchoredPosition.x - to.x) > INACCURACY ||
            Mathf.Abs(gmObj.anchoredPosition.y - to.y) > INACCURACY)
        {
            gmObj.anchoredPosition = Vector2.SmoothDamp(gmObj.anchoredPosition, to, ref zero, 15f/time);
            yield return StaticHolder.waitFrame;
        }
        gmObj.anchoredPosition = to;
        
        if (isActive) gmObj.gameObject.SetActive(false); //если он щас активный, то деактивируем\
        onComplete?.Invoke();
    }
    public static IEnumerator SmoothUImove(Transform gmObj, Vector2 to, bool isActive = false, float time = 100f, Action onComplete = null)
    {
        Vector2 zero = Vector2.zero;

        if (!isActive) gmObj.gameObject.SetActive(true); //если он щас неактивный, то активируем

        while (Mathf.Abs(gmObj.position.x - to.x) > INACCURACY ||
            Mathf.Abs(gmObj.position.y - to.y) > INACCURACY)
        {
            gmObj.position = Vector2.SmoothDamp(gmObj.position, to, ref zero, 15f / time);
            yield return StaticHolder.waitFrame;
        }
        gmObj.position = to;
        
        if (isActive) gmObj.gameObject.SetActive(false); //если он щас активный, то деактивируем
        onComplete?.Invoke();
    }

    /************************
     * ПРОСТОЕ ПЕРЕДВИЖЕНИЕ *
    *************************/

    public static IEnumerator SimpleUImove(RectTransform gmObj, Vector2 to, bool isActive = false, float speed = 0, Action onComplete = null)
    {
        if (speed == 0) speed = 1f;
        
        Vector2 zero = Vector2.zero;

        if (!isActive) gmObj.gameObject.SetActive(true); //если он щас неактивный, то активируем

        while (Mathf.Abs(gmObj.anchoredPosition.x - to.x) > INACCURACY ||
            Mathf.Abs(gmObj.anchoredPosition.y - to.y) > INACCURACY)
        {
            gmObj.anchoredPosition = Vector2.MoveTowards(gmObj.anchoredPosition, to, speed);
            yield return StaticHolder.waitFrame;
        }
        gmObj.anchoredPosition = to;
        
        if (isActive) gmObj.gameObject.SetActive(false); //если он щас активный, то деактивируем
        onComplete?.Invoke();
    }
    public static IEnumerator SimpleUImove(Transform gmObj, Vector2 to, bool isActive = false, float speed = 0, Action onComplete = null)
    {
        if (speed == 0) speed = 1f;
        
        Vector2 zero = Vector2.zero;

        if (!isActive) gmObj.gameObject.SetActive(true); //если он щас неактивный, то активируем

        while (Mathf.Abs(gmObj.position.x - to.x) > INACCURACY ||
            Mathf.Abs(gmObj.position.y - to.y) > INACCURACY)
        {
            gmObj.position = Vector2.MoveTowards(gmObj.position, to, speed);
            yield return StaticHolder.waitFrame;
        }
        gmObj.position = to;
        
        if (isActive) gmObj.gameObject.SetActive(false); //если он щас активный, то деактивируем
        onComplete?.Invoke();
    }

    /***************************
     * ИЗМЕНЕНИЕ ЦВЕТА ОБЪЕКТА *
    ****************************/

    public static IEnumerator ColorChange(UnityEngine.UI.Image gmObj, Color to, float time = 10f, Action onComplete = null)
    {
        while (!AreColorsSimilar(gmObj.color, to, INACCURACY))
        {
            gmObj.color = Color.Lerp(gmObj.color, to, 1 / time);
            yield return StaticHolder.waitFrame;
        }
        gmObj.color = to;
        onComplete?.Invoke();
    }
    public static IEnumerator ColorChange(SpriteRenderer gmObj, Color to, float time = 10f, Action onComplete = null)
    {
        while (!AreColorsSimilar(gmObj.color, to, INACCURACY))
        {
            gmObj.color = Color.Lerp(gmObj.color, to, 1 / time);
            yield return StaticHolder.waitFrame;
        }
        gmObj.color = to;
        onComplete?.Invoke();
    }
    public static IEnumerator ColorChange(TMPro.TMP_Text gmObj, Color to, float time = 10f, Action onComplete = null)
    {
        while (!AreColorsSimilar(gmObj.color, to, INACCURACY))
        {
            gmObj.color = Color.Lerp(gmObj.color, to, 1 / time);
            yield return StaticHolder.waitFrame;
        }
        gmObj.color = to;
        onComplete?.Invoke();
    }
    public static IEnumerator ColorChange(Light2D gmObj, Color to, float time = 10f, Action onComplete = null)
    {
        while (!AreColorsSimilar(gmObj.color, to, INACCURACY))
        {
            gmObj.color = Color.Lerp(gmObj.color, to, 1 / time);
            yield return StaticHolder.waitFrame;
        }
        gmObj.color = to;
        onComplete?.Invoke();
    }
    private static bool AreColorsSimilar(Color a, Color b, float tolerance)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance &&
               Mathf.Abs(a.a - b.a) < tolerance; // Включаем альфа-канал, если нужно
    }

    /*************************
     * СВЯЗАННОЕ С КЛАВИШАМИ *
    **************************/

    public static void ChangeActiveActionkeysColor(bool hold)
    {
        Color color = hold ? Color.cyan : Color.yellow;
        GameObject[] keys = GameObject.FindGameObjectsWithTag("Key");
        ChangeActiveActionkeysColor(keys, color);
    }
    public static void ChangeActiveActionkeysColor(GameObject[] keys, bool hold)
    {
        Color color = hold ? Color.cyan : Color.yellow;
        ChangeActiveActionkeysColor(keys, color);
    }
    public static void ChangeActiveActionkeysColor(GameObject[] keys, Color color)
    {
        for (int i = 0; i < keys.Length; i++)
            if (keys[i].activeInHierarchy)
            {
                if (keys[i].TryGetComponent<UnityEngine.UI.Image>(out UnityEngine.UI.Image img))
                    img.color = color;
                else if (keys[i].TryGetComponent<SpriteRenderer>(out SpriteRenderer spr))
                    spr.color = color;
            }
    }

    /*********************************
     * ЗАПОЛНЕНИЕ ИЗОБРАЖЕНИЯ ЦВЕТОМ *
    **********************************/

    public static IEnumerator FillImageFull(UnityEngine.UI.Image img, bool status, float speed = 5, Action onComplete = null)
    {
        float step = status ? -speed / 100 : speed / 100;
        float filling = Convert.ToInt32(status) + step;

        while (filling > 0f && filling < 1f)
        {
            //Debug.Log(filling);
            img.fillAmount = filling;
            filling += step;
            yield return StaticHolder.waitFrame;
        }

        img.fillAmount = Convert.ToInt32(!status);
        onComplete?.Invoke();
    }
    public static IEnumerator FillImagePartially_Down(UnityEngine.UI.Image img, float from, float to, float speed = 5, Action onComplete = null)
    {
        float step = -speed /100;
        float filling = from + step;

        while (filling > to)
        {
            img.fillAmount = filling;
            filling += step;
            yield return StaticHolder.waitFrame;
        }

        img.fillAmount = to;
        onComplete?.Invoke();
    }
    public static IEnumerator FillImagePartially_Up(UnityEngine.UI.Image img, float from, float to, float speed = 5, Action onComplete = null)
    {
        float step = speed / 100;
        float filling = from + step;

        while (filling < to)
        {
            img.fillAmount = filling;
            filling += step;
            yield return StaticHolder.waitFrame;
        }

        img.fillAmount = to;
        onComplete?.Invoke();
    }

    /****************************
     * ПОВОРОТ ВСЯКОЙ ТАМ ХЕРНИ *
    *****************************/

    public static IEnumerator RotateGameobject(Transform transform, Quaternion to, float time = 0)
    {
        if (time == 0f) time = 1.0f;

        float elapsedTime = 0;
        Quaternion from = transform.rotation;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float curve = Mathf.Pow(elapsedTime / time, 2); // Неравномерное движение
            transform.rotation = Quaternion.Slerp(from, to, curve);
            yield return null;
        }

        transform.rotation = to; // Убедитесь, что объект завершил поворот точно

        yield return StaticHolder.waitFrame;
    }
    public static IEnumerator RotateGameobject(Transform transform, float toDegree, float time = 0)
    {
        if (time == 0f) time = 0.1f;

        float elapsedTime = 0;
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.Euler(0, 0, toDegree);

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float curve = Mathf.Pow(elapsedTime / time, 2); // Неравномерное движение
            transform.rotation = Quaternion.Slerp(from, to, curve);
            yield return null;
        }

        transform.rotation = to; // Убедитесь, что объект завершил поворот точно

        yield return StaticHolder.waitFrame;
    }

}
