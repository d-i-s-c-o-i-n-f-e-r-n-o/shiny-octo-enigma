using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer[] mix;
    public Slider[] slider;
    public GameObject[] but_on;
    public GameObject[] but_off;
    public TMPro.TMP_Text[] volumeText;
    float[] volume = new float[3];

    void Start()
    {
        //Debug.Log(mix.Length);
        for (int i = 0; i < mix.Length; i++)
        {
            if (mix[i] != null) mix[i].SetFloat("volume", SaveManager.instance.activeSettings.volume[i] * 50 - 50);
            if (slider[i] != null) slider[i].value = SaveManager.instance.activeSettings.volume[i];
            if (but_off[i] != null) but_off[i].SetActive(slider[i].value <= 0);
            if (but_on[i] != null) but_on[i].SetActive(!but_off[i].activeSelf);
        }
    }

    //-----Функции сохранения изменённой громкости в статикхолдер
    public void OnChangeVolume(int num)
    {
        SaveManager.instance.activeSettings.volume[num] = volume[num] = slider[num].value;
        volumeText[num].text = Convert.ToByte(slider[num].value * 100).ToString()/* + "%"*/;
        mix[num].SetFloat("volume", SaveManager.instance.activeSettings.volume[num] * 50 - 50);
        SaveManager.instance.Save();
    }

    //-----Переключалка звука
    public void VolumeToggle(int num)
    {
        AudioController.instance.PlaySound("ui", 0);
        if (slider[num].interactable) //выключить
        {
            Switch_Button(but_off[num], but_on[num]);
            slider[num].interactable = false;
            mix[num].SetFloat("volume", -50);
        }
        else //включить
        {
            Switch_Button(but_on[num], but_off[num]);
            slider[num].interactable = true;
            OnChangeVolume(num);
        }
    }

    //-----Добавление/убавление звука
    public void AddToVolume(int button)
    {
        AudioController.instance.PlaySound("ui", 0);
        if (button > 0) //прибавление звука
        {
            button = Mathf.Abs(button) - 1; //кнопка
            if (slider[button].value < 1) slider[button].value += 0.01f;
        }
        else //убавление звука
        {
            button = Mathf.Abs(button) - 1; //кнопка
            if (slider[button].value > 0) slider[button].value -= 0.01f;
        }
    }
    //-----Вспомогательные функции
    public void Switch_Button(GameObject butOn, GameObject butOff)
    {
        butOn.SetActive(true);
        butOff.SetActive(false);
        //UIclick_sound.Play();
    }
}
