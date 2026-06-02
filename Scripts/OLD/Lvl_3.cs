using UnityEngine;

public class Lvl_3 : MonoBehaviour
{
    //public AudioController AudioController;
    //public Zombie KeyZombie1, KeyZombie2;
    //public GameObject Player;
    //public GameUI GameUI;

    //[Header("Bool variable")]
    //public bool isPlot, isPlotOver = false;
    //bool isSay1 = false, isSay2 = false, isSay3 = false, isSay4 = false,
    //    isSay5 = false, isSay6 = false, isSay7 = false, isSay8 = false;
    //public bool isLvl3;

    //[Header("Prefab variable")]
    //public GameObject PlayerIcon;
    //public GameObject DialogueBox;
    //public GameObject Say1, Say2, Say3, Say4, Say5, Say6, Say7, Say8;

    //void Awake()
    //{
    //    if (isLvl3)
    //    {
    //        AudioController.TapeStart_Click();
    //        UI_Reset();
    //        isPlot = true;
    //    }
    //}
    //void Start()
    //{
    //    if (isLvl3) Invoke("Say1_Start", 1f);
    //} //Первая реплика через 1 секунду после начала игры
    //void Update()
    //{
    //    if (isLvl3)
    //    {
    //        if (isPlot && Input.GetButtonDown("Fire1") && !GameUI.pauseOpen)
    //            Invoke("Dialog_Close", 0.1f);

    //        if (KeyZombie1.moving && !isSay5)
    //            Say5_Start();
    //        if (KeyZombie2.moving && !isSay8)
    //            Say8_Start();
    //    }
    //}

    //void Dialog_Close()
    //{
    //    if (!GameUI.pauseOpen)
    //    {
    //        UI_Reset(); //запуск реплик друг за другом
    //        if (isSay1 && !isSay2) Invoke("Say2_Start", 0.2f);
    //        if (isSay2 && !isSay3) Invoke("Say3_Start", 0.2f);
    //        if (isSay3 && !isSay4) Invoke("Say4_Start", 0.2f);
    //        if (isSay5 && !isSay6) Invoke("Say6_Start", 0.2f);
    //        if (isSay6 && !isSay7) Invoke("Say7_Start", 0.2f);
    //    }
    //} //Закрытие реплики
    //void UI_Reset()
    //{
    //    Time.timeScale = 1f;
    //    isPlot = false;

    //    Say1.SetActive(false);
    //    Say2.SetActive(false);
    //    Say3.SetActive(false);
    //    Say4.SetActive(false);
    //    Say5.SetActive(false);
    //    Say6.SetActive(false);
    //    Say7.SetActive(false);
    //    Say8.SetActive(false);
    //    DialogueBox.SetActive(false);
    //    PlayerIcon.SetActive(false);
    //} //Сброс переменных
    //void Dialog_Start()
    //{
    //    Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    //    isPlot = true;
    //    DialogueBox.SetActive(true);
    //    PlayerIcon.SetActive(true);
    //}

    //void Say1_Start()
    //{
    //    Dialog_Start();
    //    isSay1 = true;
    //    Say1.SetActive(true);
    //} //Первая реплика

    //void Say2_Start()
    //{
    //    Dialog_Start();
    //    isSay2 = true;
    //    Say2.SetActive(true);
    //} //Вторая реплика

    //void Say3_Start()
    //{
    //    Dialog_Start();
    //    isSay3 = true;
    //    Say3.SetActive(true);
    //} //Третья реплика

    //void Say4_Start()
    //{
    //    Time.timeScale = 0.05f;
    //    Dialog_Start();
    //    isSay4 = true;
    //    Say4.SetActive(true);
    //} //Четвёртая реплика

    //void Say5_Start()
    //{
    //    Time.timeScale = 0.05f;
    //    Dialog_Start();
    //    isSay5 = true;
    //    Say5.SetActive(true);
    //} //Пятая реплика

    //void Say6_Start()
    //{
    //    Time.timeScale = 0.05f;
    //    Dialog_Start();
    //    isSay6 = true;
    //    Say6.SetActive(true);
    //} //Шестая реплика

    //void Say7_Start()
    //{
    //    Time.timeScale = 0.05f;
    //    Dialog_Start();
    //    isSay7 = true;
    //    Say7.SetActive(true);
    //} //Седьмая реплика
    //void Say8_Start()
    //{
    //    Time.timeScale = 0.05f;
    //    Dialog_Start();
    //    isSay8 = true;
    //    Say8.SetActive(true);
    //} //Восьмая реплика
}