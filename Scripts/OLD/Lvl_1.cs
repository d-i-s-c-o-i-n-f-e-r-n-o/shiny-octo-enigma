using UnityEngine;

public class Lvl_1 : MonoBehaviour
{
    //[Header("Scripts variable")]
    //public AudioController AudioController;
    //public SaveManager SaveManager;
    //public Zombie Zombie1;
    //public Zombie Zombie2;
    //public Zombie Zombie3;
    //public PlayerOLD PlayerOLD;
    //public GameUI GameUI;

    //[Header("Bool variable")]
    //public bool isPlot;
    //bool isSay1, isSay3, isSay4;
    //bool isTeachFight;
    //bool isZombie1_Dead, isZombie2_Dead, isZombie3_Dead;
    //public bool isLvl1;
    //public bool isLvl1Complete;

    //[Header("Prefab variable")]
    //public GameObject Player;
    //public GameObject PlayerIcon, DialogueBox;
    //public GameObject Say1, Say2, Say3, Say4;
    //public GameObject TeachMove, TeachFight;

    //void Start()
    //{
    //    //isLvl1Complete = SaveManager.instance.activeSave.isLvl1Complete;
    //    if (isLvl1 && !isLvl1Complete)
    //    {
    //        AudioController.TapeStart_Click();
    //        UI_Reset();
    //        isPlot = true;
    //        isTeachFight = false;
    //    }
    //    if (isLvl1 && !isLvl1Complete) Invoke("Say1_Start", 1f);
    //} //Первая реплика через 1 секунду после начала игры
    //void Update()
    //{
    //    if (isLvl1 && !isLvl1Complete)
    //    {
    //        if (isPlot && Input.GetButtonDown("Fire1") && !GameUI.pauseOpen && !GameUI.invOpen && !isTeachFight)
    //            Invoke("Dialog_Close", 0.1f); //закрыть диалоговое окно

    //        if (Zombie1.moving && !isPlot && !isTeachFight && !isSay3)
    //            Invoke("Say3_Start", 0.1f); //запуск реплики когда впервые увидел зомби

    //        if (Zombie1.isDead && Zombie2.isDead && Zombie3.isDead && !isSay4)
    //            Invoke("Say4_Start", 3f); //если все зомби мертвы, то Say4 один раз
    //    }
    //}

    //void Toggle_Talk(bool ist)
    //{
    //    PlayerIcon.SetActive(ist);
    //    DialogueBox.SetActive(ist);
    //}
    //void Dialog_Start()
    //{
    //    isPlot = true;
    //    Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    //    isTeachFight = false;
    //}
    //void Teach_Close()
    //{
    //    isPlot = false;
    //    TeachMove.SetActive(false);
    //    TeachFight.SetActive(false);
    //    Time.timeScale = 1f;
    //    if (isTeachFight)
    //    {
    //        SaveManager.instance.activeSave.hasKnife = true;
    //        PlayerOLD.hasKnife = true;
    //        PlayerOLD.setStab = true;
    //    }
    //    isTeachFight = false;
    //} //Закрытие обучения через время
    //void Dialog_Close()
    //{
    //    if (!GameUI.pauseOpen && !GameUI.invOpen && isPlot)
    //    {
    //        if (isSay1) Invoke("TeachMove_Start", 0.1f);
    //        UI_Reset();
    //    }
    //} //Закрытие реплики
    //void UI_Reset()
    //{
    //    isPlot = isSay1 = false;
    //    Toggle_Talk(false);
    //    Say1.SetActive(false);
    //    Say2.SetActive(false);
    //    Say3.SetActive(false);
    //    Say4.SetActive(false);
    //    TeachMove.SetActive(false);
    //    TeachFight.SetActive(false);
    //} //Сброс переменных

    //void Say1_Start()
    //{
    //    Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    //    isPlot = true;
    //    isTeachFight = false;
    //    isSay1 = true;

    //    Toggle_Talk(true);
    //    Say1.SetActive(true);
    //} //Первая реплика

    //void TeachMove_Start()
    //{
    //    isPlot = false;
    //    TeachMove.SetActive(true);
    //    Invoke("Teach_Close", 4.2f);
    //    Invoke("Say2_Start", 4.2f);
    //} //Обучение движения

    //void Say2_Start()
    //{
    //    Dialog_Start();
    //    Toggle_Talk(true);
    //    Say2.SetActive(true);
    //} //Вторая реплика

    //void Say3_Start()
    //{
    //    Time.timeScale = 0.05f;
    //    isSay3 = true;
    //    isPlot = true;

    //    Dialog_Start();
    //    Toggle_Talk(true);
    //    Say3.SetActive(true);

    //    isTeachFight = true;
    //    Invoke("TeachFight_Start", 0.1f);
    //} //Третья реплика

    //void TeachFight_Start()
    //{
    //    Toggle_Talk(false);
    //    Say3.SetActive(false);

    //    TeachFight.SetActive(true);
    //    Invoke("Teach_Close", 0.15f);
    //} //Обучение сражения

    //void Say4_Start()
    //{
    //    isSay4 = true;
    //    Dialog_Start();
    //    Toggle_Talk(true);
    //    Say4.SetActive(true);
    //} //Четвёртая реплика
}