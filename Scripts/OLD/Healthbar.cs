using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public Sprite[] barStates;
    UnityEngine.UI.Image img;

    private void Start()
    {
        img = GetComponent<UnityEngine.UI.Image>();
    }

    public void UpdateBar(int state)
    {
        img.sprite = barStates[state];
    }
}
