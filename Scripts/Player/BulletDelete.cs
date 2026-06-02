using UnityEngine;

public class BulletDelete : MonoBehaviour
{
    public float speed;

    void Start()
    {
        Invoke(nameof(DeleteBullet), 0.5f);

        Animator anim = GetComponent<Animator>();
        FPlayer player = GameObject.FindGameObjectWithTag("Player").GetComponent<FPlayer>();

        bool[] view = player.view;
        string[] viewName = { "Up", "Down", "Left", "Right" };
        Vector2[] vel = { new(0, speed), new(0, -speed), new(-speed, 0), new(speed, 0) };

        for (int i = 0; i < view.Length; i++)
            if (view[i])
            {
                GetComponent<Rigidbody2D>().linearVelocity = vel[i];
                break;
            }

        for (int i = 0; i < view.Length; i++)
            anim.SetBool(viewName[i], view[i]);
    }
    void DeleteBullet()
    {
        Destroy(gameObject);
    }
}