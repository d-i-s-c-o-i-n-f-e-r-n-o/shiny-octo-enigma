using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    int language = 0;
    private void Awake()
    {
        LocalizationSettings.InitializationOperation.WaitForCompletion();
    }

    private void Start()
    {
        language = SaveManager.instance.activeSettings.language;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[language];
    }

    public void ChangeLanguage()
    {
        StartCoroutine(Language());
    }
    IEnumerator Language()
    {
        Button but = GetComponent<Button>();
        but.interactable = false;

        language = (language + 1) % 2;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[language];
        SaveManager.instance.activeSettings.language = language;
        yield return StaticHolder.wait0[5];

        but.interactable = true;
    }
}
