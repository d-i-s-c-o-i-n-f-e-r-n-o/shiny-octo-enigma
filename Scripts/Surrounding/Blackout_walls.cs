using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Blackout_walls : MonoBehaviour
{
    public Vector2[] borders;
    public Color color;
    GameObject player;
    Tilemap tl;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) this.enabled = false;
        tl = GetComponent<Tilemap>();
        if (color == Color.clear) color = Color.white;
    }

    void Update()
    {
        bool isPlayerInside = player.activeInHierarchy &&
            (player.transform.position.x > borders[0].x && player.transform.position.x < borders[1].x) &&
            (player.transform.position.y > borders[0].y && player.transform.position.y < borders[1].y);
        if (isPlayerInside)
            tl.color = Color.clear;
        else tl.color = color;
    }
}
