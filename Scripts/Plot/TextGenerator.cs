using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

[RequireComponent(typeof(TMP_Text))]
public class TextGenerator : MonoBehaviour
{
    public TMP_Text TextArea;// поле вывода текста
    public LocalizedString[] text;// то что будет выводится
    public Image dialogBox;

    public bool skip = false;

    public float specialDelay = 0; //специальная сюжетная задержка
    public bool done = true;
    public bool saying = false;

    public byte csLength;
    public byte dialogEnding, character;
    [Header("Characters icons")]
    public UnityEngine.UI.Image iconImg;
    public Sprite[] icons;

    //[Header("Characters colors")]
    Color[] characterTextColor;

    void Awake()
    {
        characterTextColor = StaticHolder.characterTextColor;
        csLength = Convert.ToByte(text.Length);

        TextArea = GetComponent<TMP_Text>();
        Erase();
    }

    private void Start()
    {
        dialogBox = GetComponentInParent<Image>();
    }

    public void Generate(byte num, bool erase)
    {
        if (erase) Erase();

        string target = text[num].GetLocalizedString();
        //Debug.Log(target);
        if (target.Contains("_")) //Если нужно разделение на персонажей
        {
            dialogEnding = Convert.ToByte($"{target[0]}");           //первый символ - прерывание диалога
            character = Convert.ToByte($"{target[1]}{target[2]}");  //второй и третий символ - персонаж

            //Debug.Log(icons.Length + " " + character + " " + target[1] + " " + target[2]);
            if (icons.Length > 0) iconImg.sprite = icons[character];
            TextArea.color = characterTextColor[character];
            target = target[4..];
        }

        if (target.Contains("$"))
        {
            
        }
        
        if (done) StartCoroutine(TextAnimation(target, erase));
    }

    private IEnumerator TextAnimation(string str, bool erase)
    {
        //Debug.Log(num);
        done = false;
        //skip = false;
        int i = 0;

        bool specialChars = false;
        foreach (char ch in str)
        {
            if (ch != '_')
            {
                TextArea.text += ch.ToString();
                if (".,!?:;\"«»\'".Contains(ch))
                {
                    yield return new WaitForSeconds(0.3f + specialDelay);   //задержка знаков препинания
                    saying = false;
                }
                else if (ch == '\n')
                {
                    yield return new WaitForSeconds(0.6f + specialDelay);   //задержка абзацов
                    saying = false;
                }
                else if (ch == '<' || ch == '>')                            //быстрый вывод тэгов
                    specialChars = !specialChars;
                else
                {
                    if (!specialChars)
                    {
                        yield return new WaitForSeconds(0.05f + specialDelay);  //время для вывода след. буквы
                    }
                    //if (ch != ' ' && character != 0) AudioController.instance.PlaySound("voice", character-1);

                    saying = true;
                }
                i++;
                if (skip) { TextArea.text += str[i..]; break; }
            }
        }
        if (!erase) TextArea.text += "\n";
        saying = false;
        //skip = false;
        done = true;
    }

    public void Erase()
    {
        TextArea.text = "";
    }
}
