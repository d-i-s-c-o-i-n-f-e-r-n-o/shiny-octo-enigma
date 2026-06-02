using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Emotions : MonoBehaviour
{
    public bool testingAnimation = false;
    public int height = 0;
    public float between = 0;
    bool lookingUp = false;
    bool emotionTears = false;
    bool emotionEyesClosed = false;
    bool eyesClosed = false;
    bool canBlink = true;
    bool isPlayer = false;
    float[] counter = new float[] { 0f, 1f };

    public int characterNumber = 0;

    int[] blinkRate = { 5, 10 };
    public int blinkSpeed = 1;

    int[] tearRate = { 1, 3 };
    public float tearSpeed = 1;

    Dictionary<string, GameObject[]> elements = new Dictionary<string, GameObject[]>();

    //public GameObject[] brows = new GameObject[2];
    //public GameObject[] eyelashes = new GameObject[2];
    //public GameObject[] eyetears = new GameObject[2];
    //public GameObject[] eyes = new GameObject[2];
    //public GameObject[] pupils = new GameObject[2];
    //public GameObject[] blinks = new GameObject[2];
    //диапазон зрачков -0.2; 0.2, блики просто зеркалить
    public GameObject tearPrefab;

    float[] up_y = new float[2] { 0, 0 };
    Vector2 halfscreen = new Vector2(960, 540);
    SpriteRenderer spr;

    Vector2[] brow_pos = new Vector2[] //y + r
    {
        new(3.8f, 7.31f),   //x_left + x_right  0
        new(-1.16f, 18),    //грусть            1
        new(-0.26f, 25),    //испуг/изумление   2
        new(-0.8f, -8),     //недоумевание      3
        new(-0.2f, 0),      //дефолтное         4
        new(-0.4f, 0),      //нахмуренное       5
        new(-0.95f, -25)    //злость            6
    };
    Vector3 eyelashes_pos = new(4.059f, -1.79f, 7.0587f);
    Vector3 tears_pos = new(4.049f, -2.83f, 7.0599f);

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();

        FindEverything();
        Blank();
        

        if (testingAnimation) AllRenderer();
        isPlayer = gameObject.CompareTag("Player");

        //Sad();
        //InTears(true);
        //Crying();
        //Scared_Surprised();

        StartCoroutine(Blinking(blinkRate[0], blinkRate[1]));
    }

    void FindEverything()
    {
        foreach (string key in new string[] { "Brow", "Tear", "Eyelash", "Eye" })
        {
            elements.Add(key, new GameObject[] {
                transform.Find($"Emotions/{key}_Left").gameObject,
                transform.Find($"Emotions/{key}_Right").gameObject,
            });
        }

        elements.Add("Pupil", new GameObject[] {
            transform.Find($"Emotions/Eye_Left/Pupil_Left").gameObject,
            transform.Find($"Emotions/Eye_Right/Pupil_Right").gameObject
        });
        elements.Add("Blink", new GameObject[] {
            transform.Find($"Emotions/Eye_Left/Pupil_Left/Blink_Left").gameObject,
            transform.Find($"Emotions/Eye_Right/Pupil_Right/Blink_Right").gameObject
        });

        //foreach (GameObject[] g in elements.Values)
        //{
        //    Debug.Log(g[0].name + "    " + g[1].name);
        //}
    }


    private void FixedUpdate()
    {
        if (isPlayer) MovePupils();
        if (canBlink)
        {
            counter[0] += 0.01f;
            if (counter[0] > counter[1] && !eyesClosed)
            {
                counter = new float[] { 0f, UnityEngine.Random.Range(blinkRate[0] / blinkSpeed, blinkRate[1] / blinkSpeed) };
                //ToggleHelper(eyelashes, !lookingUp);
                ToggleHelper(elements["Eyelash"], !lookingUp);

                eyesClosed = true;
            }
            else if (counter[0] > 0.2f && eyesClosed)
            {
                ToggleHelper(elements["Eyelash"], emotionEyesClosed);
                eyesClosed = false;
            }
        }
    }
    void MovePupils()
    {
        Vector3 to = Mouse.current.position.ReadValue() - halfscreen;
        to = new Vector3(to.x / 4800, to.y / 2700);

        foreach (GameObject p in elements["Pupil"])
            p.transform.localPosition = to;
    }

    void ToggleHelper(GameObject[] obj, bool needed)
    {
        for (int i = 0; i < obj.Length; i++)
            obj[i].SetActive(needed);
    }

    //------------Моргание------------
    IEnumerator Blinking(int min_rate, int max_rate)
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(min_rate * 1f / blinkSpeed, max_rate * 1f / blinkSpeed));

            //Debug.Log(lookingUp);

            ToggleHelper(elements["Eyelash"], !lookingUp);
            yield return StaticHolder.wait0[2];

            ToggleHelper(elements["Eyelash"], emotionEyesClosed);
        }

    }
    public void EyesClosed(bool needed)
    {
        ToggleHelper(elements["Eyelash"], needed);
        emotionEyesClosed = needed;
    }

    //-----------Выравнивание для анимаций-----------
    void Expression_Up(float shift) =>          Exp_helper(new float[] { shift, shift, 1f, 1f });   // <- -> верхнее
    void Expression_Default(float shift) =>     Exp_helper(new float[] { shift, shift, 0f, 0f });   // <- -> дефолт
    void Expression_Down(float shift) =>        Exp_helper(new float[] { shift, shift, -1f, -1f }); // <- -> нижнее


    void Expression_Towards_Up(float shift) =>  Exp_helper(new float[] { shift - 1f, shift, 1f, 1f });  // V верхнее
    void Expression_Towards(float shift) =>     Exp_helper(new float[] { shift - 1f, shift, 0f, 0f });  // V дефолт
    void Expression_Towards_Down(float shift) =>Exp_helper(new float[] { shift - 1f, shift, -1f, -1f });// V нижнее
    
    void Expression_Active(int needed)
    {
        bool act = (needed == 1);
        lookingUp = !act;
        ToggleHelper(elements["Brow"], act);
        ToggleHelper(elements["Tear"], emotionTears && act);
        ToggleHelper(elements["Eye"], act);
    } //Переключалка

    void Exp_helper(float[] arg)
    {
        MoveBrows(arg);
        Move("Tear", tears_pos, arg);
        Move("Eyelash", eyelashes_pos, arg);
        Move("Eye", eyelashes_pos, arg);
    }

    //--------------Передвижения и повороты------------
    void RotateBrows(params float[] angle)
    {
        for (int i = 0; i < elements["Brow"].Length; i++)
            elements["Brow"][i].transform.localRotation = Quaternion.Euler(0, 0, angle[i]);
    }
    void MoveBrows(params float[] shift)
    {
        elements["Brow"][0].transform.localPosition = new Vector3(brow_pos[0][0] + shift[0] + between, shift[2] + up_y[0]);
        elements["Brow"][1].transform.localPosition = new Vector3(brow_pos[0][1] + shift[1] - between, shift[3] + up_y[1]);
    }

    void Move(string key, Vector3 pose, params float[] shift)
    {
        elements[key][0].transform.localPosition = new Vector3(pose.x + shift[0], pose.y + shift[2] + height);
        elements[key][1].transform.localPosition = new Vector3(pose.z + shift[1], pose.y + shift[3] + height);
    }

    //---------Выражения бровей-------------
    public void Blank()
    {
        between = 0;
        up_y[0] = up_y[1] = brow_pos[4][0] + height;
        RotateBrows(0, 0);
    }
    public void Sad()
    {
        between = 0;
        up_y[0] = up_y[1] = brow_pos[1][0] + height;
        RotateBrows(brow_pos[1][1], -brow_pos[1][1]);
    }
    public void Tired_Agression()
    {
        between = 0;
        up_y[0] = up_y[1] = brow_pos[1][0] + height;
        RotateBrows(0, 0);
    }
    public void Scared_Surprised()
    {
        between = 0;
        up_y[0] = up_y[1] = brow_pos[2][0] + height;
        RotateBrows(brow_pos[2][1], -brow_pos[2][1]);
    }
    public void Confused()
    {
        between = 0;
        up_y[0] = brow_pos[3][0] + height;
        up_y[1] = 0 + height;

        RotateBrows(0, brow_pos[3][1]);
    }
    public void Frown()
    {
        between = 0.3f;
        up_y[0] = up_y[1] = brow_pos[5][0] + height;
        RotateBrows(0, 0);
    }
    public void Anger()
    {
        between = 0.1f;
        up_y[0] = up_y[1] = brow_pos[6][0] + height;
        RotateBrows(brow_pos[6][1], -brow_pos[6][1]);
    }

    //--------------Слёзы-------------------
    public void InTears(bool needed)
    {
        ToggleHelper(elements["Tear"], needed);
        emotionTears = needed;
    }
    public void Crying()
    {
        StartCoroutine(ShedTears(tearRate[0], tearRate[1]));
    }
    public void StopCrying()
    {
        StopCoroutine(ShedTears(tearRate[0], tearRate[1]));
    }
    IEnumerator ShedTears(int minRate, int maxRate)
    {
        while (true)
        {
            if (!lookingUp)
            {
                int randomIndex = UnityEngine.Random.Range(0, elements["Tear"].Length);
                GameObject fallingTear = Instantiate(tearPrefab, elements["Tear"][randomIndex].transform.position, Quaternion.identity);
                fallingTear.GetComponent<Tear>().height = (height + 1) * 0.0625f;
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(minRate / tearSpeed, maxRate / tearSpeed));
        }
    }

    //------------Дебаг--------------------
    void AllRenderer()
    {
        //tearSpeed = 5f;
        InTears(true);
        Crying();
        InvokeRepeating(nameof(Blank), 0, 21f);
        InvokeRepeating(nameof(Tired_Agression), 3f, 21f);
        InvokeRepeating(nameof(Confused), 6f, 21f);
        InvokeRepeating(nameof(Scared_Surprised), 9f, 21f);
        InvokeRepeating(nameof(Sad), 12f, 21f);
        InvokeRepeating(nameof(Frown), 15f, 21f);
        InvokeRepeating(nameof(Anger), 18f, 21f);
    }
}
