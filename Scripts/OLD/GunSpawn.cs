using UnityEngine;

public class GunSpawn : MonoBehaviour
{
    Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        body.linearVelocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            SaveManager.instance.activeSave.hasGun = true;
            SaveManager.instance.Save();
            Destroy(gameObject);
        }
    }
}