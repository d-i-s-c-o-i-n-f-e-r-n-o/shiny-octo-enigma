using UnityEngine;

public class IntersceneStaying : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}