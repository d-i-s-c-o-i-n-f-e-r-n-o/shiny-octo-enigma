using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Door : MonoBehaviour
{
    Vector3[] doorPos;
    Transform player;
    public bool isActive = true;
    public bool isVertical = false;
    bool position = false;

    void Start()
    {
        GameObject playerCheck = GameObject.FindGameObjectWithTag("Player");
        if (playerCheck == null)
        {
            enabled = false;
            return;
        }
        else player = playerCheck.transform;

        if (isVertical)
            doorPos = new Vector3[2] {
                new(transform.position.x, transform.position.y+1.5f),
                new(transform.position.x, transform.position.y-0.1f)
            };
        else
            doorPos = new Vector3[2] {
                new(transform.position.x+1f, transform.position.y+0.5f),
                new(transform.position.x-1f, transform.position.y+0.5f)
            };
    }
    private void Update()
    {
        if (isVertical) position = player.position.y > transform.position.y;
        else            position = player.position.x > transform.position.x;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("NPC"))
        {
            if (isActive)
            {
                col.gameObject.transform.position = doorPos[Convert.ToInt32(position)];
                AudioController.instance.PlaySound("door", 0);
            }
            else AudioController.instance.PlaySound("door", 1);
        }
    }
}