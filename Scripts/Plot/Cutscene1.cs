using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class Cutscene1 : MonoBehaviour
{
    
    PlayerInput input;

    [Header("Cutscene Properties")]
    public GameObject cutsceneGameObject;
    public bool Cutscene1Done;
    public bool cutsceneEnabled = false; //������ ��� �������, ��� �������� ����� 1 �������
    public bool isPlot;

    [Header("Text Properties")]
    public TextGenerator dialog;
    public bool auto = false;
    public byte csLength;
    public byte line = 0;

    [Header("Animators")]
    public Animator di;
    public Animator stivia;
    public Animator john;

    [Header("John")]
    Rigidbody2D johnBody;
    bool viewRight = true, viewUp = false;  //аним для джона
    public bool johnWalking = false;
    public int position = -1;
    public Vector3[] pos = new Vector3[6];
    public float speed;

    void Awake()
    {
        input = new PlayerInput();
        input.Cutscene.Next.performed += context => TryPerformDialog();
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    void Start()
    {
        if (SaveManager.instance.activeSave.cutscene2Done)
        {
            cutsceneGameObject.SetActive(false);
            this.enabled = false;
            return;
        }

        csLength = dialog.csLength;
        stivia.speed = 1f;
        di.speed = 1f;

        TurnCharacterRight(di, true);
        TurnCharacterRight(stivia, true);

        johnBody = john.GetComponent<Rigidbody2D>();
        StartCoroutine(StartCutscene());
    }

    //-------Ходить---------
    IEnumerator HandlerJohnMovement()
    {
        while (johnWalking)
        {
            bool walking = true;
            bool isAutoCutscene = false;
            Vector2 direction = Vector2.zero;

            if (position == 3 || position == 7) //остановка на диалог
            {
                johnWalking = false;
                auto = true;
                johnBody.linearVelocity = Vector2.zero;
                john.SetFloat("speed", 0f);
                break;
            }

            switch (position) //поворот в зависимости от позиции
            {
                case -1: walking = false; johnBody.linearVelocity = direction; break;     //начало движения после первой реплики
                case 0: case 2: case 4: case 6: viewRight = true; break;   //идти направо
                case 1: case 5: viewUp = true; break;                      //идти вверх
            }

            johnWalking = walking;
            auto = isAutoCutscene;

            john.SetBool("viewUp", viewUp);
            john.SetBool("viewRight", viewRight);
            viewUp = false; viewRight = false;

            //johnBody.velocity = direction * (walking ? speed : 0);
            while (Vector2.Distance(john.transform.position, pos[position]) > 0.01f)
            {
                //Debug.Log("Джон на позиции " + position + ": " + pos[position]);
                john.speed = 0.5f;
                john.SetFloat("speed", 1f);
                john.transform.position = Vector2.MoveTowards(john.transform.position, pos[position], (walking ? speed : 0) * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
                
            position++;
        }
        //Debug.Log("Всё");
    }

    //-----------------------------------

    IEnumerator StartCutscene()
    {
        yield return StaticHolder.wait[2];
        Dialog_Reset();
        //StartCoroutine(ui.SmoothUImove(blackImage, new Vector2(-850, 0), false, 30f));
        //while (!ui.moveDone) yield return new WaitForEndOfFrame();
        //--------------------------------------------
        cutsceneEnabled = true;
        Dialog_Start(line);
    }

    void TalkingAnimation()
    {
        float talkingSpeed = dialog.saying ? 10f : 1f;
        switch (dialog.character)
        {
            case 1: //Di
                di.speed = talkingSpeed;
                break;
            case 2: //John
                if (!johnWalking) john.speed = talkingSpeed;
                break;
            case 3: //Stivia
                stivia.speed = talkingSpeed;
                break;
        }
    }

    void Update()
    {
        TalkingAnimation();

        if (auto) TryPerformDialog();

        CheckFollowingCamera();
        CheckCusceneEnding();
    }

    void TryPerformDialog()
    {
        //Debug.Log(line);
        if (cutsceneEnabled && dialog.done && //если катсцена доступна, предыдущая реплика нарисована
            line < csLength && !johnWalking) //Если джон пришёл на позицию и ещё остались реплики
        {
            auto = dialog.skip = false;

            //---------------Вызов Реплики--------------------
            if (isPlot && Convert.ToBoolean(dialog.dialogEnding)) Dialog_Reset(); //Закрытие реплик вида 1ХХ_
            else Cutscene1Done = Dialog_Start(line); //Вызов следующей по списку реплики (если закончились, катсцена заканчивается
            //-----------------------------------------------------

            if (!isPlot) CheckJohnMovement();
        }
        else if (!dialog.done && !auto) dialog.skip = true;
    }


    void CheckJohnMovement()
    {
        if (position == -1 || position == 3)
        {
            position++;
            johnWalking = true;
            StartCoroutine(HandlerJohnMovement());
        }
    }
    void CheckCusceneEnding()
    {
        if (Cutscene1Done && dialog.done)
        {
            Cutscene1Done = false;
            SaveManager.instance.activeSave.cutscene1Done = true;
            SaveManager.instance.Save();
            StartCoroutine(GetComponent<GameManager>().ToCutscene2());
        }
    }
    void CheckFollowingCamera()
    {
        if (position > 3)
            gameObject.transform.position = new Vector3(john.transform.position.x, john.transform.position.y, -10);
    }
     
    bool Dialog_Start(byte i)
    {
        isPlot = true;
        dialog.iconImg.gameObject.SetActive(true);
        dialog.dialogBox.color = new Color(0,0,0,0.7f);

        dialog.Generate(i, true);

        CheckDialogEvent();

        line++;
        return (line == csLength);
    }

    void CheckDialogEvent()
    {
        switch (line)
        {
            case 3:
                TurnCharacterRight(stivia, false);
                break;
            case 5:
                stivia.GetComponent<Emotions>().Anger();
                break;
            case 6:
                john.GetComponent<Emotions>().Tired_Agression();
                break;
            case 11:
                john.GetComponent<Emotions>().Blank();
                break;
            case 13:
                stivia.GetComponent<Emotions>().Blank();
                TurnCharacterRight(stivia, true);
                break;
            case 18:
                di.GetComponent<Emotions>().Tired_Agression();
                TurnCharacterRight(di, false);
                break;
        }
    }

    void TurnCharacterRight(Animator character, bool toRight)
    {
        character.SetBool("viewRight", toRight);
        character.SetBool("viewLeft", !toRight);
    }


    void Dialog_Reset()
    {
        isPlot = false;
        dialog.TextArea.text = string.Empty;
        dialog.iconImg.gameObject.SetActive(false);
        dialog.dialogBox.color = Color.clear;
    } //Сброс диалога
}