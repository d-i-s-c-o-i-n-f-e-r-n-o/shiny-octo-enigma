using System.Collections.Generic;
using UnityEngine;

public class TreeSetter : MonoBehaviour
{
    Animator anim;
    public string season = "summer";
    public float speed = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        Randomize();
    }

    void Randomize()
    {
        //---------------Цвет
        Dictionary<string, Color> seasons = new()
        {
            { "summer", new Color(UnityEngine.Random.Range(0f, 0.6f), UnityEngine.Random.Range(0.8f, 1f), UnityEngine.Random.Range(0f, 0.8f))  },
            { "latesummer", new Color(UnityEngine.Random.Range(0.5f, 1f), UnityEngine.Random.Range(0.8f, 1f), UnityEngine.Random.Range(0f, 0.3f))  }
        };
        GetComponent<SpriteRenderer>().color = seasons[season];

        //---------------Размер
        float scale = UnityEngine.Random.Range(0.95f, 1.2f);
        gameObject.transform.localScale = new Vector3(scale, scale, 0);

        //---------------Отразить
        if (UnityEngine.Random.Range(0, 6) == 1) //12.5% т.к. 1/6
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        //---------------Скорость (пропорционально размеру)
        anim.speed = 0.5f - scale/3 + speed;

        //---------------Выравнивание (пропорционально размеру)
        gameObject.transform.position = new Vector3(
            gameObject.transform.position.x,
            gameObject.transform.position.y + scale * scale - 1f
            );

        //---------------Начало с рандомного кадра (не трогай пока работает)
        var animController = anim.runtimeAnimatorController;
        if (animController.animationClips.Length > 0)
        {
            AnimationClip clip = animController.animationClips[0]; //выбираем анимацию
            float clipLength = clip.length; //всего её длина
            float randomStartTime = UnityEngine.Random.Range(0f, clipLength); //рандомный старт от начала до конца
            anim.Play(clip.name, 0, randomStartTime / clipLength); //Плейбэк тайм какой-то ёмаё
        }
    }
}