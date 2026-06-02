using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GameUI))]
public class Cutscene3 : MonoBehaviour
{
    GameUI gameUI;
    PlayerInput input;
    public GameObject cutsceneGameObject;
    public TextGenerator blackTxt;
    public TMPro.TMP_Text getUp;
    public GameObject[] getUpKeys;
    public UnityEngine.UI.Image blackImage;
    public Animator player;

    void Awake()
    {
        input = new PlayerInput();
    }

    void SetBinding(string name, KeyDown key)
    {
        //input.FindAction(name).performed += context => key.OnButtonPressed(context);
        input.FindAction(name).started += context => key.OnButtonHeld(context);
        input.FindAction(name).canceled += context => key.OnButtonHeld(context);
        input.FindAction(name).Enable();
    }
    void ResetBinding(string name, KeyDown key)
    {
        //input.FindAction(name).performed -= context => key.OnButtonPressed(context);
        input.FindAction(name).started -= context => key.OnButtonHeld(context);
        input.FindAction(name).canceled -= context => key.OnButtonHeld(context);
        input.FindAction(name).Disable();
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
        if (!SaveManager.instance.activeSave.cutscene2Done)
        {
            cutsceneGameObject.SetActive(false);
            this.enabled = false;
            return;
        }
        
        gameUI = GetComponent<GameUI>();
        gameUI.canTogglePhone = false;
        StartCoroutine(TheStart());
    }

    IEnumerator TheStart()
    {
        //--------------------Начало--------------------------
        foreach (string bl in new string[] { "Right", "Dead", "Lying" })
            player.SetBool(bl, true);

        FPlayer pl = player.GetComponent<FPlayer>();
        pl.isPlot = true;
        pl.enabled = false;

        yield return StartCoroutine(UI_Utility.ColorChange(blackImage, Color.clear, 0));
        yield return StaticHolder.wait[2];
        input.Player.Move.performed += OnTryingToMove; //Ждём когда игрок попробует сдвинуться
    }

    private void OnTryingToMove(InputAction.CallbackContext context)
    {
        input.Player.Move.performed -= OnTryingToMove;
        StartCoroutine(StandUp());
    }
    IEnumerator StandUp()
    {
        //------------Встаёт с кровати и падает---------------]
        player.SetBool("Right", false);
        player.SetBool("Lying", false);
        player.SetBool("Up", true);

        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.15f);

        yield return StartCoroutine(UI_Utility.ColorChange(blackImage, new Color(0, 0, 0, 0.85f)));
        //-----------Пытается встать------------
        blackTxt.Generate(0, true); //______________________________________________________________________!
        yield return StaticHolder.wait0[6];
        player.SetBool("Lying", true);
        while (!blackTxt.done) yield return StaticHolder.waitFrame;
        input.Player.Move.performed += OnStandingUp;
        input.Cutscene.Next.performed += OnStandingUp;
        //Debug.Log("Waiting for key");
    }

    void OnStandingUp(InputAction.CallbackContext context)
    {
        //Debug.Log("KeyPressed!");
        input.Player.Move.performed -= OnStandingUp;
        input.Cutscene.Next.performed -= OnStandingUp;
        StartCoroutine(Dizzy());
    }

    void ChangeButtonsToPress(bool isFirst)
    {
        for (int i = 0; i < 4; i++) getUpKeys[i].SetActive(isFirst);
        for (int i = 1; i < 3; i++) getUpKeys[^i].SetActive(!isFirst);
        UI_Utility.ChangeActiveActionkeysColor(isFirst);
    }

    IEnumerator Dizzy()
    {
        //blackTxt.Generate(1, false); //______________________________________________________________________!
        getUp.gameObject.SetActive(true);

        getUp.text = "0%";
        while (!blackTxt.done) yield return StaticHolder.waitFrame;
        //----------Вставай---------------
        int up = 0;
        ChangeButtonsToPress(true);
        for (int i = 0; i < 4; i++) SetBinding(i < 2 ? "Q": "E", getUpKeys[i].GetComponent<KeyDown>());
        while (up < 100)
        {
            bool isQPressed = input.Cutscene.Q.ReadValue<float>() > 0;
            bool isEPressed = input.Cutscene.E.ReadValue<float>() > 0;
            
            if (isQPressed && isEPressed)
            {
                up++;
                getUp.text = up + "%";
                yield return StaticHolder.wait00[5];
            }
            else if (up > 0)
            {
                up--;
                getUp.text = up + "%";
                yield return StaticHolder.wait0[5];
            }
            yield return StaticHolder.waitFrame;
        }
        for (int i = 0; i < 4; i++) ResetBinding(i < 2 ? "Q" : "E", getUpKeys[i].GetComponent<KeyDown>());
        ChangeButtonsToPress(false);

        for (int i = 4; i < 6; i++) SetBinding("W", getUpKeys[i].GetComponent<KeyDown>());
        while (!(input.Cutscene.W.ReadValue<float>() > 0)) yield return StaticHolder.waitFrame;
        for (int i = 4; i < 6; i++) ResetBinding("W", getUpKeys[i].GetComponent<KeyDown>());

        Destroy(getUp.gameObject);
        Destroy(blackTxt.gameObject);

        //----------------------------------------
        yield return StartCoroutine(UI_Utility.ColorChange(blackImage, Color.black));

        player.SetBool("Lying", false);
        player.SetBool("Dead", false);

        yield return StartCoroutine(UI_Utility.ColorChange(blackImage, Color.clear));
        blackImage.gameObject.SetActive(false);
        //-------------Встал и мысль--------------------

        FPlayer pl = player.GetComponent<FPlayer>();
        pl.enabled = true;
        yield return StartCoroutine(gameUI.ThoughtAppear(0)); //_________________________________________________________!

        //--------------Теперь можно ходить, то есть мы ждём когда он выйдет в коридор-------------

        while (!(player.transform.position.x <= 3.3f) && !(player.transform.position.y <= -1.5f)) yield return StaticHolder.waitFrame;
        StartCoroutine(gameUI.ThoughtAppear(1)); //_________________________________________________________!

        //---------------------------------------------------------------------------------------
    }
}
