using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{
    
    public bool keyCrow;
    bool flag = true;
    void Start()
    {
        StartCoroutine(CrowFalling());
    }

    IEnumerator CrowFalling()
    {
        int rot = 180;
        while(flag)
        {
            rot += UnityEngine.Random.Range(0, 180);
            transform.rotation = Quaternion.Euler(0, 0, rot);
            yield return StaticHolder.wait[1];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
        {
            flag = false;
            StopCoroutine(CrowFalling());
            if (transform.localScale.x < 0) 
                transform.rotation = Quaternion.Euler(0, 0, -170);
            else transform.rotation = Quaternion.Euler(0, 0, -189);
            GetComponent<Animator>().SetBool("Fell", true);
            if (keyCrow)
            {
                Animator player = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
                player.SetBool("Death", true);
            }
        }
    }
}
