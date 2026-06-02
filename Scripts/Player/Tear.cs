using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear : MonoBehaviour
{
    public float height;

    void Start()
    {
        float y = transform.position.y - 1f - height;
        StartCoroutine(TearFalling(y));
    }

    IEnumerator TearFalling(float y)
    {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.sortingOrder = 3;
        //Debug.Log("Needed y: " + y);
        while (transform.position.y > y)
        {
            //Debug.Log("Current y: " + transform.localPosition.y);
            yield return new WaitForEndOfFrame();
        }

        spr.sortingOrder = 0;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        transform.localScale = new Vector3(transform.localScale.x, 0.09f);

        yield return StaticHolder.wait[3];

        Destroy(gameObject);
    }
}
