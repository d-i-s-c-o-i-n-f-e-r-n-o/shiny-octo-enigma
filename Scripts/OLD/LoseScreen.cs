using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    public bool defeated, moving;
    public int health;
    public GameObject loseScreen;
    bool vulnerable;

    public void Start()
    {
        defeated = false;
        moving = true;
        vulnerable = true;
        health = SaveManager.instance.activeSave.health;
    }
    void Update()
    {
        if (!moving)
        {
            defeated = true;
            Invoke("YouLose", 2f);
        }
    }

    void YouLose()
    {
        Time.timeScale = 0f;
        loseScreen.SetActive(true);
    } //Ёкран —мерти
    void Invulnerable()
    {
        vulnerable = true;
    } //ќтмена неу€звимости

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (vulnerable) health--;   //убывание сердечка если у€звим
            if (health == 0) moving = false; //чЄ сдох да лох
            else
            {
                vulnerable = false;
                Invoke("Invulnerable", 1.5f);
            }
        }
    } //ѕердечный сриступ при столкновении с врагом
}