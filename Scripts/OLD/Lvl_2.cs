using UnityEngine;

public class Lvl_2 : MonoBehaviour
{
    //    public AudioController AudioController;
    //    public GameObject Player;
    //    public GameObject ToggleWeapon;

    //    public GameUI GameUI;
    //    public PlayerOLD PlayerOLD;
    //    public Zombie Security;
    //    public Zombie Zombie;

    //    [Header("Bool variable")]
    //    public bool isPlot, isPlotOver = false;
    //    bool isSay2;
    //    bool isSay3;
    //    bool isSay4;
    //    bool isTeachToggleWeapon_Complete;
    //    bool isSecurityDead;
    //    public bool isLvl2;

    //    [Header("Prefab variable")]
    //    public GameObject PlayerIcon;
    //    public GameObject DialogueBox;
    //    public GameObject Say1;
    //    public GameObject Say2;
    //    public GameObject Say3;
    //    public GameObject Say4;
    //    public GameObject TeachToggleWeapon;

    //    void Awake()
    //    {
    //        if (isLvl2)
    //        {
    //            AudioController.pla;
    //            UI_Reset();
    //            isPlot = true;
    //            isSay2 = false;
    //            isSay3 = false;
    //            isSay4 = false;
    //            isTeachToggleWeapon_Complete = false;
    //            ToggleWeapon.SetActive(false);
    //        }
    //    }
    //    void Start()
    //    {
    //        if (isLvl2) Invoke("Say1_Start", 1f);
    //    } //Первая реплика через 1 секунду после начала игры
    //    void Update()
    //    {
    //        if (isLvl2)
    //        {
    //            if (isPlot && Input.GetButtonDown("Fire1") && !GameUI.pauseOpen && !GameUI.invOpen)
    //                Invoke("Dialog_Close", 0.1f);

    //            if (Zombie.moving && !isPlot && !isSay2)
    //                Invoke("Say2_Start", 0.1f);

    //            if (Security.isDead && !isTeachToggleWeapon_Complete && !isSay3)
    //                Invoke("Say3_Start", 0.1f);

    //            if (Security.isDead && isTeachToggleWeapon_Complete && !isSay4)
    //                Invoke("Say4_Start", 0.1f);
    //        }
    //    }

    //    void Dialog_Start()
    //    {
    //        isPlot = true;
    //        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    //        DialogueBox.SetActive(true);
    //        PlayerIcon.SetActive(true);
    //    }
    //    void Dialog_Close()
    //    {
    //        if (!GameUI.pauseOpen) UI_Reset(); //обучение после закрытия реплик писать сюда
    //        DialogueBox.SetActive(false);
    //        PlayerIcon.SetActive(false);
    //    } //Закрытие реплики

    //    void UI_Reset()
    //    {
    //        Time.timeScale = 1f;
    //        isPlot = false;
    //        Say1.SetActive(false);
    //        Say2.SetActive(false);
    //        Say3.SetActive(false);
    //        Say4.SetActive(false);
    //        TeachToggleWeapon.SetActive(false);
    //    } //Сброс переменных
    //    void Teach_Close()
    //    {
    //        isPlot = false;
    //        isTeachToggleWeapon_Complete = true;
    //        TeachToggleWeapon.SetActive(false);
    //        Time.timeScale = 1f;
    //        if (isTeachToggleWeapon_Complete)
    //        {
    //            SaveManager.instance.activeSave.hasGun = true;
    //            PlayerOLD.hasGun = true;
    //        }
    //    } //Закрытие обучения через время

    //    void Say1_Start()
    //    {
    //        Dialog_Start();
    //        Say1.SetActive(true);
    //    } //Первая реплика

    //    void Say2_Start()
    //    {
    //        isSay2 = true;
    //        Time.timeScale = 0.1f;
    //        Dialog_Start();
    //        Say2.SetActive(true);
    //    } //Вторая реплика

    //    void Say3_Start()
    //    {
    //        isSay3 = true;
    //        Dialog_Start();
    //        Say3.SetActive(true);
    //        Invoke("Dialog_Close", 4f);
    //        Invoke("TeachToggleWeapon_Start", 4f);
    //    } //Третья реплика

    //    void TeachToggleWeapon_Start()
    //    {
    //        Say3.SetActive(false);
    //        TeachToggleWeapon.SetActive(true);
    //        ToggleWeapon.SetActive(true);
    //        Invoke("Teach_Close", 4f);
    //    } //Обучение переключения оружия

    //    void Say4_Start()
    //    {
    //        isSay4 = true;
    //        Dialog_Start();
    //        Say4.SetActive(true);
    //    } //Четвёртая реплика
}