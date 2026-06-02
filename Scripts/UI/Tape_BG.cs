using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Tape_BG : MonoBehaviour
{
    public int speed;
    public 

    short maxY = 925;
    Vector3 restartPos = new (0, -1115, 0);
    Color red = new (0.8901961f, 0.2509804f, 0.1764706f, 0.972549f);
    Color blue = new (0.4962264f, 0.6743304f, 1f, 0.972549f);
    RectTransform rect;
    bool toRed;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    void FixedUpdate()
    {
        if (rect.anchoredPosition.y < maxY)
            rect.anchoredPosition = Vector2.MoveTowards(
                rect.anchoredPosition,
                new Vector2(rect.anchoredPosition.x, maxY),
                speed/100f);
        else rect.anchoredPosition = restartPos;
    }
    public void ChangeColorToRed(bool red)
    {
        toRed = red;
        Invoke(nameof(Temp), 0.5f);
    }
    void Temp()
    {
        StartCoroutine(UI_Utility.ColorChange(gameObject.GetComponent<UnityEngine.UI.Image>(), toRed ? red : blue));
    }
}