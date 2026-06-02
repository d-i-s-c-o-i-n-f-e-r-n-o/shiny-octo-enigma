using UnityEngine;

public class StabDelete : MonoBehaviour
{
    public Zombie Zombie;

    void Start()
    {
        Invoke("Delete_It", 0.5f);
    }
    void Delete_It()
    {
        Destroy(gameObject);
    }
}