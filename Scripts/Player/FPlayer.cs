using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class FPlayer : MonoBehaviour
{
    Rigidbody2D body;
    Animator anim;
    GameUI gameUI;

    //-----Оруженька
    public Knife knife;
    public Gun gun;
    int choosen_weapon = -1;
    int[] bulletCount = new int[] { 0, 0, 0 };

    public float speed, runSpeed, walkSpeed;   //скорости

    //[] - тип боеприпаса
    
    //--------Всякие полосочки где 0 - текущее, 1 - максимальное
    public byte[] health = new byte[2], stamina = new byte[2], cooldown = null;
    public short[] backpack = new short[2];

    [Header("Animator variable")]
    static string[] viewName = new string[4] { "Up", "Down", "Left", "Right" };
    public bool[] view = new bool[] { false, false, false, true};  //куда смотришь [up, down, left, right]

    public bool underAttack, isPlot;
    bool canRun = true;
    public bool attack = false;
    [Header("Prefabs")]
    public GameObject bullet, stab;                          //хитбоксы пули, ножа
    [Header("Game")]
    public GameObject loseScreen;
    public LoseScreen LoseScreen;
    [Header("Keys")]
    PlayerInput input;


    void Awake()
    {
        input = new PlayerInput();
        input.Player.Attack.performed += context => PlayerAttack();
        input.Player.ChangeWeapon.performed += context => ChangeWeapon();
        input.Player.Reload.performed += context => Reload();

        input.Player.Sprint.performed += context => PlayerRun();
        input.Player.Sprint.canceled += context => PlayerWalk();
        input.Enable();
        //Debug.Log("Awake setter works");
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    void SetPlayerBars()
    {
        if (SaveManager.instance.activeSave.bulletCount.Length == 0)
        {
            SaveManager.instance.activeSave.bulletCount = new int[3] { 0, 0, 0 };
            SaveManager.instance.Save();
        }

        //Debug.Log(SaveManager.instance.activeSave.bulletCount.Length);
        for (int i = 0; i < SaveManager.instance.activeSave.bulletCount.Length; i++)
            bulletCount[i] = SaveManager.instance.activeSave.bulletCount[i];

        stamina[1] = (byte)(StaticHolder.staminaBase * (SaveManager.instance.activeSave.staminaLvl + 1));
        stamina[0] = (byte)(stamina[1] / 2);
        //Debug.Log(stamina[0] + "/" + stamina[1]);

        health[0] = SaveManager.instance.activeSave.health;
        health[1] = (byte)(StaticHolder.healthBase + SaveManager.instance.activeSave.healthLvl);

        if (SaveManager.instance.activeSave.cooldownLvl != -1)
            cooldown[1] = (byte)(StaticHolder.cooldownBase - 15 * SaveManager.instance.activeSave.cooldownLvl);
    }
    void GetPlayerComponents()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gameUI = GetComponentInChildren<GameUI>();
    }
    void SetWeapons()
    {
        //Debug.Log(SaveManager.instance.activeSave.hasGun + " " + SaveManager.instance.activeSave.hasKnife);
        //weapon[0] = SaveManager.instance.activeSave.weapon[0];

        //if (true /*SaveManager.instance.activeSave.hasGun*/) weapon[0] = GetComponentInChildren<BackpackUI>(true).weapon["police_pistol"];
        //Debug.Log(weapon[0].name + " " + weapon[0].ammo_id);
        //if (true /*SaveManager.instance.activeSave.hasKnife*/) weapon[1] = GetComponentInChildren<BackpackUI>(true).weapon["common_knife"];

        //if (weapon[choosen_weapon] != null)
        //{
        //    //Debug.Log(weapon[1].name + " " + weapon[1].ammo_id);
        //    //Debug.Log(weapon[choosen_weapon].ammo_id + " " + choosen_weapon);
        //    weapon[choosen_weapon].UpdateLoadBar(gameUI);
        //}
        //else
        //{
        //    gameUI.weaponLoadObj.SetActive(false);
        //}
    }

    public void Start()
    {
        GetPlayerComponents();
        SetPlayerBars();
        SetWeapons();
        StartCoroutine(Running());
        //----------------------------------

        walkSpeed = StaticHolder.walkSpeed + SaveManager.instance.activeSave.speedLvl;
        runSpeed = StaticHolder.runSpeed + SaveManager.instance.activeSave.speedLvl;
        speed = walkSpeed;

        loseScreen.SetActive(false);
        //Debug.Log(gameUI);

        gun = Gun.Create("police_gun")[0];
        knife = Knife.Create("kitchen_knife")[0];
        gun.CurrentAmmo = 4;
        bulletCount[0] += 20;
        ChangeWeapon();        

        //Debug.Log(gun.ToString());
    }
    IEnumerator Running()
    {
        while (true)
        {

            //Debug.Log($"{canRun}  {stamina[0]}/{stamina[1]} Порог: {stamina[1] / 3}");
            if (!canRun) 
            {
                yield return StaticHolder.wait0[2];
                stamina[0] += 2;
            }

            if (stamina[0] <= 2 || body.linearVelocity == Vector2.zero)
            {
                PlayerWalk();               //speed = walkSpeed;
                canRun = false;             //если мы добегались, то не можем бежать
            }
            else if (stamina[0] >= stamina[1] / 3) canRun = true;//если не добегались, то можем бежать

            // Рассход-востановление стамины
            if (speed == runSpeed && canRun) stamina[0] -= 2;  //если мы бежим, то стамина убывает на 2
            else if (stamina[0] < stamina[1]) stamina[0]++;       //если не бежим, то восстанавливаем на 1

            stamina[0] = (byte)Mathf.Clamp(stamina[0], 0, stamina[1]);
            gameUI.staminaObj.SetActive(stamina[0] != stamina[1]);

            GameUI.UpdateBar(gameUI.barLevel[4], gameUI.staminaSprite4, stamina[0], stamina[1]);
            yield return StaticHolder.wait0[1];
        }
    }

    void Update()
    {
        DeathCheck();
        PlayerMovement();
    }

    void FixedUpdate()
    {
        Animation();
    }

    void PlayerMovement()
    { //Управление
        //Debug.Log("CAN YOU CAN YOU A DRUG PATH ETCHED IN THE SURFACE AS EVIDENCE I LEFT THERE ON PUTPOSE");
        if (isPlot || gameUI.phone || attack) //если нужно обездвижить игрока
        {
            body.linearVelocity = Vector2.zero;
            return;
        }

        if (underAttack)    //откид от атаки - надо поработать над этим
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y);
            return;
        }

        if (LoseScreen.moving) //иначе движение
        {
            Vector2 movementInput = input.Player.Move.ReadValue<Vector2>();
            body.linearVelocity = movementInput * speed;
            return;
        }
        body.linearVelocity = Vector2.zero;
    }

    void PlayerRun()
    {
        //------------Бег-----------------
        if (canRun && anim.GetFloat("Speed") > 0.5f) //run
        {
            gameUI.staminaObj.SetActive(true);
            speed = runSpeed;
        }
        else speed = walkSpeed;             //walk
    }
    void PlayerWalk()
    {
        speed = walkSpeed;
    }

    void Animation()
    {
        Vector2 vel = body.linearVelocity;
        anim.SetFloat("Speed", vel == Vector2.zero ? 0f : 1f);

        if (vel != Vector2.zero) //в движении меняем стороны
            view = new bool[] { vel.y > 0.1f, vel.y < -0.1f, vel.x < -0.1f, vel.x > 0.1f }; //сброс анимации (вверх, вниз, навлево, направо)
        if ((view[0] || view[1]) && (view[2] || view[3])) //если встал разрешаем конфликт сторон
            view[0] = view[1] = false; //поворачиваем игрока вправо или влево
        for (int i = 0; i < viewName.Length; i++) anim.SetBool(viewName[i], view[i]);

        if (attack) anim.speed = 1f;                        //атака
        else if (speed == runSpeed) anim.speed = 1.2f;      //бег
        else anim.speed = 0.5f;                             //ходьба
    } //Анимации

    void PlayerAttack()
    {
        if (!isPlot && (gun != null || knife != null) && !attack)
        {
            StartCoroutine(AttackCoroutine());
        }
    }
    IEnumerator AttackCoroutine()
    {
        attack = true;
        PlayerWalk();
        anim.SetBool("Attack", true);
        anim.SetFloat("Speed", 0f);

        float shift_y = view[0] ? 0.688f : view[1] ? -0.68f : -0.125f;
        float shift_x = view[2] ? -0.563f : view[3] ? 0.625f : 0f;
        Vector3 pos = new (transform.position.x + shift_x, transform.position.y + shift_y);


        Debug.Log($"{choosen_weapon}, {choosen_weapon == 1 && knife != null}");

        if (choosen_weapon == 1 && knife != null) //спавнит невидимый нож
        {
            Instantiate(stab, new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), transform.rotation);
            knife.UpdateLoadBar(gameUI);
            yield return StaticHolder.wait0[5];
        }
        else if (choosen_weapon == 0 && gun != null && gun.CurrentAmmo > 0) //спавнит пулю
        {
            Instantiate(bullet, pos, transform.rotation);
            gun.CurrentAmmo--;
            gun.UpdateLoadBar(gameUI, (byte)bulletCount[gun.AmmoId]);
            yield return StaticHolder.wait0[5];
        }
        else Debug.Log("no bullets");

        attack = false;
        anim.SetBool("Attack", false);
    }

    void ChangeWeapon()
    {
        int weapon_count = (knife == null ? 0 : 1) + (gun == null ? 0 : 1);
        //Debug.Log(weapon_count);
        if (weapon_count > 0)
        {
            if (!gameUI.weaponObj.activeSelf) gameUI.weaponObj.SetActive(true);
            choosen_weapon++; //переключалка
            choosen_weapon %= weapon_count;
            switch (choosen_weapon)
            {
                case 0: gun.UpdateLoadBar(gameUI, (byte)bulletCount[gun.AmmoId]); break;
                case 1: knife.UpdateLoadBar(gameUI); break;
            }
            //Debug.Log($"Weaponset: gun - {gun != null && choosen_weapon == 0}, knife - {knife != null && choosen_weapon == 1}");
            anim.SetBool("SetGun", gun != null && choosen_weapon == 0);
            anim.SetBool("SetStab", knife != null && choosen_weapon == 1);
        }
        else
        {
            gameUI.weaponObj.SetActive(false);
            gameUI.weaponLoadObj.SetActive(false);
        }
    } //Смена оружия

    void Reload()
    {
        if (choosen_weapon == 0)
        {
            int type = gun.AmmoId;
            bulletCount[type] = gun.Reload((short)bulletCount[type]);
            gun.UpdateLoadBar(gameUI, (byte)bulletCount[gun.AmmoId]);
            //SaveManager.instance.activeSave.bulletCount[type] = bulletCount[type];
            //SaveManager.instance.Save();
        }
    }

    void DeathCheck()
    {
        if (LoseScreen.defeated || health[0] < (byte)1) anim.SetBool("Dead", true);
    } //проверка, подох ли терентий

    //// So Update Loop... Move it in player script? idk
    //private void Update()
    //{
    //    // Pickup from ground: If touching a ground item and E is pressed
    //    if (currentGroundItem != null && Input.GetKeyDown(KeyCode.E))
    //    {
    //        if (AddItem(currentGroundItem))
    //        {
    //            groundItems.Remove(currentGroundItem);
    //            currentGroundItem = null;
    //        }
    //    }
    //}

    //// OnTriggerEnter/Stay from Player.cs or whatever else
    //public void OnTouchGroundItem(ItemHandler groundItem)
    //{
    //    currentGroundItem = groundItem;
    //}

    //// same for boxes
    //public void OnTouchBox(bool touching)
    //{
    //    isTouchingBox = touching;
    //}
}