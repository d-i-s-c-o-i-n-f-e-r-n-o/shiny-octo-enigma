using UnityEngine;

public class NoteShowUp : MonoBehaviour
{
    public GameObject note1, note2, note3, note4, note5;
    public GameObject bg1, bg2, bg3;
    public GameObject close;
    public GameUI game;

    public void Start()
    {
        note1.SetActive(false);
        note2.SetActive(false);
        note3.SetActive(false);
        note4.SetActive(false);
        note5.SetActive(false);

        bg1.SetActive(false);
        bg2.SetActive(false);
        bg3.SetActive(false);

        close.SetActive(false);
    }
    public void ShowNote1()
    {
        Start();
        note1.SetActive(true);
        bg1.SetActive(true); //изменить
        close.SetActive(true);
    }
    public void ShowNote2()
    {
        Start();
        note2.SetActive(true);
        bg1.SetActive(true); //изменить
        close.SetActive(true);
    }
    public void ShowNote3()
    {
        Start();
        note3.SetActive(true);
        bg1.SetActive(true); //изменить
        close.SetActive(true);
    }
    public void ShowNote4()
    {
        Start();
        note4.SetActive(true);
        bg3.SetActive(true);
        close.SetActive(true);
    }
    public void ShowNote5()
    {
        Start();
        note5.SetActive(true);
        bg1.SetActive(true); //изменить
        close.SetActive(true);
    }
}
