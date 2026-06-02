using UnityEngine;

public class PlayerOLD : MonoBehaviour
{
    //Rigidbody2D body;
    //float axisX, axisY;
    //public float speed;
    //public Animator anim;
    //int arrowCount, bulletCount, healCount, health;

    //[Header("Animator variable")]
    //int choosen_weapon;                                 //переключалка дл€ оружи€
    //public bool hasKnife, hasGun, hasCrossbow;           //какое оружие имеет при себе
    //public bool setStab, setGun, setCrossbow;           //какое оружие установлено
    //public bool viewRight, viewLeft, viewUp, viewDown;  //куда смотришь
    //public bool underAttack, isPlot, isPlotOver;
    //bool attack;
    //[Header("Prefabs")]
    //public GameObject bullet, stab, arrow;                  //хитбоксы пули, ножа и стрелы
    //public GameObject gun_icon, stab_icon, crossbow_icon;   //иконки пистолета, ножа и арбалета
    //[Header("Game")]
    //public GameObject loseScreen;
    //public LoseScreen LoseScreen;
    //public Lvl_1 Lvl1; //а надо ли мне это всЄ?...
    //public Lvl_2 Lvl2;
    //public Lvl_3 Lvl3;
    //public Lvl_4 Lvl4; //а надо ли мне это всЄ ещЄ раз???...
    //public Lvl_5 Lvl5;
    //public Lvl_6 Lvl6;

    //public void Start()
    //{
    //    Time.timeScale = 1f;
    //    viewUp = viewDown = viewLeft = false;
    //    viewRight = true;

    //    hasKnife = SaveManager.instance.activeSave.hasKnife;
    //    hasGun = SaveManager.instance.activeSave.hasGun;
    //    hasCrossbow = SaveManager.instance.activeSave.hasCrossbow;
    //    setStab = setGun = setCrossbow = false;
    //    attack = false;
    //    choosen_weapon = 0;

    //    arrowCount = SaveManager.instance.activeSave.arrowCount;
    //    bulletCount = SaveManager.instance.activeSave.bulletCount;
    //    healCount = SaveManager.instance.activeSave.healCount;
    //    health = SaveManager.instance.activeSave.health;

    //    loseScreen.SetActive(false);
    //    body = GetComponent<Rigidbody2D>();
    //}

    //void Update()
    //{
    //    float time = 0; //ну    Єбаный      таймер      на      хилку   заебал  сука    ајјјјјјј
    //    axisX = Input.GetAxis("Horizontal");
    //    axisY = Input.GetAxis("Vertical");
    //    Input.GetKey(KeyCode.Escape);

    //    Animation();
    //    PlayerAttack();
    //    DeathCheck();
    //    if (Input.GetButtonDown("Fire2")) ChangeWeapon();

    //    while (Input.GetKey(KeyCode.C) && healCount > 0)
    //    {  //хилка после маслины
    //        Debug.Log("Ёй бл€");
    //        body.velocity = Vector2.zero;
    //        time += Time.deltaTime;
    //        if (time > 3)
    //        {
    //            time = 0;
    //            healCount--; health++;
    //            break;
    //        }
    //    }
    //}

    //void FixedUpdate()
    //{
    //    isPlot = ((Lvl1.isLvl1 && Lvl1.isPlot) || (Lvl2.isLvl2 && Lvl2.isPlot) || (Lvl3.isLvl3 && Lvl3.isPlot) || (Lvl4.isLvl4 && Lvl4.isPlot) || (Lvl5.isLvl5 && Lvl5.isPlot) || (Lvl6.isLvl6 && Lvl6.isPlot));
    //    isPlotOver = ((Lvl2.isLvl2 && Lvl2.isPlotOver) || (Lvl3.isLvl3 && Lvl3.isPlotOver) || (Lvl4.isLvl4 && Lvl4.isPlotOver) || (Lvl5.isLvl5 && Lvl5.isPlotOver) || (Lvl6.isLvl6 && isPlotOver));
    //    anim.SetBool("SetCrossbow", setCrossbow);
    //    anim.SetBool("SetGun", setGun);
    //    anim.SetBool("SetStab", setStab);
    //    PlayerMovement();
    //}

    //void PlayerMovement()
    //{ //”правление
    //    if (isPlot) { body.velocity = Vector2.zero; return; }              //если сюжет идЄт, то сто€ть.
    //    if (attack) { body.velocity = Vector2.zero; return; }
    //    if (underAttack) { body.velocity = new Vector2(body.velocity.x, body.velocity.y); return; }
    //    if (LoseScreen.moving)
    //    {   //иначе движение
    //        if (Input.GetButton("Horizontal") && Input.GetButton("Vertical")) body.velocity = new Vector2(axisX * speed - 1, axisY * speed - 1);
    //        else if (Input.GetButton("Horizontal")) body.velocity = new Vector2(axisX * speed, 0);
    //        else if (Input.GetButton("Vertical")) body.velocity = new Vector2(0, axisY * speed);
    //        else body.velocity = Vector2.zero;
    //        return;
    //    }
    //    body.velocity = Vector2.zero;
    //}

    //void Animation()
    //{
    //    if (body.velocity.x != 0 || body.velocity.y != 0)
    //    { //если движение
    //        viewUp = viewDown = viewLeft = viewRight = false;   //сброс анимации
    //        anim.SetFloat("Speed", 1f); //движение
    //        if (body.velocity.x < 0) viewLeft = true;  //налево
    //        if (body.velocity.x > 0) viewRight = true; //направо
    //        if (body.velocity.y > 0) viewUp = true;                  //вверх
    //        if (body.velocity.y < 0) viewDown = true;                //вниз

    //        anim.SetBool("Right", viewRight);
    //        anim.SetBool("Left", viewLeft);
    //        anim.SetBool("Up", viewUp);
    //        anim.SetBool("Down", viewDown);
    //        return;
    //    }
    //    anim.SetFloat("Speed", 0f);
    //} //јнимации

    //void Attack_Reset()
    //{
    //    attack = false;
    //    anim.SetBool("Attack", attack);
    //} //ќтмена атаки
    //void PlayerAttack()
    //{
    //    if (Input.GetButtonDown("Fire1") && hasKnife && !isPlot/* && !attack*/)
    //    {
    //        attack = true;

    //        anim.SetFloat("Speed", 0f);
    //        anim.SetBool("Attack", attack);
    //        Invoke("Attack_Reset", 0.4f);

    //        if (choosen_weapon == 0 || setStab)     //спавнит хитбокс ножика
    //            Instantiate(stab, new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), transform.rotation);
    //        else if (setGun && bulletCount > 0)     //спавнит пулю вместе с еЄ хитбоксом
    //            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z), transform.rotation);
    //        else if (setCrossbow && arrowCount > 0) //спавнит стрелу вместе с еЄ хитбоксом
    //            Instantiate(arrow, new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z), transform.rotation);
    //    }
    //} //—трельба из всего, что можно и нельз€

    //void ChangeWeapon_Reset()
    //{
    //    setStab = setGun = setCrossbow = false;
    //    anim.SetBool("SetCrossbow", setCrossbow);
    //    anim.SetBool("SetGun", setGun);
    //    anim.SetBool("SetStab", setStab);
    //} //—брос смены оружи€
    //void ChangeWeapon()
    //{
    //    choosen_weapon++; //переключалка
    //    int hasWeapon = 0;
    //    if (hasKnife) hasWeapon++;
    //    if (hasGun) hasWeapon++;
    //    if (hasCrossbow) hasWeapon++;

    //    if (hasKnife && choosen_weapon % hasWeapon == 0)
    //    {
    //        ChangeWeapon_Reset();
    //        setStab = true;
    //        stab_icon.SetActive(setStab);
    //        anim.SetBool("SetStab", setStab);
    //    } //выбор ножа
    //    if (hasGun && choosen_weapon % hasWeapon == 1)
    //    {
    //        ChangeWeapon_Reset();
    //        setGun = true;
    //        gun_icon.SetActive(setGun);
    //        anim.SetBool("SetGun", setGun);
    //    } //выбор пистолета
    //    if (hasCrossbow && choosen_weapon % hasWeapon == 2)
    //    {
    //        ChangeWeapon_Reset();
    //        setCrossbow = true;
    //        crossbow_icon.SetActive(setCrossbow);
    //        anim.SetBool("SetCrossbow", setCrossbow);
    //    } //выбор пистолета
    //    stab_icon.SetActive(setStab);
    //    gun_icon.SetActive(setGun);
    //    crossbow_icon.SetActive(setCrossbow);
    //} //—мена оружи€

    //void DeathCheck()
    //{
    //    if (LoseScreen.defeated || health < 1) anim.SetBool("Dead", true);
    //} //проверка, подох ли терентий
}