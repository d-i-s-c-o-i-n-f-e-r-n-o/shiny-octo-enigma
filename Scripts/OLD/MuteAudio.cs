using UnityEngine;

public class MuteAudio : MonoBehaviour
{
    public GameObject ButtonMuted, ButtonUnmuted;

    public void MutedSpawn()
    {
        Instantiate(ButtonMuted, transform.position, Quaternion.identity);
    }
    public void UnmutedSpawn()
    {
        Instantiate(ButtonUnmuted, transform.position, Quaternion.identity);
    }
}
