using System.Collections;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerIntro : MonoBehaviour
{
    Rigidbody2D body;
    public Animator[] anim = new Animator[2];

    public float speed;   //скорости
    PlayerInput input;

    void Awake()
    {
        input = new PlayerInput();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void Start()
    {
        speed = StaticHolder.walkSpeed + SaveManager.instance.activeSave.speedLvl;
        body = GetComponent<Rigidbody2D>();
        anim[0].speed = anim[1].speed = 0.5f;
    }

    void Update()
    {
        PlayerHandler();
    }

    void PlayerHandler() //Управление
    {
        Vector2 movementInput = input.Cutscene.MoveRight.ReadValue<Vector2>();
        bool viewLeft = movementInput.x < -0.1f;
        //---------Движение-------------
        body.linearVelocity = viewLeft ? Vector2.zero : movementInput * speed;
        //---------Анимация------------
        for (int i = 0; i < anim.Length; i++) {
            anim[i].SetFloat("Speed", movementInput == Vector2.zero ? 0f : 1f);
            anim[i].SetBool("Left", viewLeft);
            anim[i].SetBool("Right", !viewLeft);
        }
    }
}