using UnityEngine;

public class Rain : MonoBehaviour
{
    public float speed;
    public Vector2 restartPos;
    public Vector2 endPos;
    RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    void FixedUpdate()
    {
        if (rect.anchoredPosition.y > endPos.y)
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, endPos, speed);
        else rect.anchoredPosition = -endPos;
    }
}