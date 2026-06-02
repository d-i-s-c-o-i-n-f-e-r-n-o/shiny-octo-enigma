using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Localization;


public class DisplayInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoPanel;

    public bool resizable = false;

    float screenDivider = 1;
    RectTransform rect;
    Vector2 canvasSize = new();

    void Start()
    {
        rect = infoPanel.GetComponent<RectTransform>();

        canvasSize = GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.size;
        screenDivider = Screen.width / canvasSize.x;
        //Debug.Log(Screen.width + " x " + Screen.height);
        //Debug.Log(canvasSize);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Info(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Info(false);
    }

    void Update()
    {

        if (infoPanel.activeSelf) rect.localPosition = Mouse.current.position.value / screenDivider - canvasSize / 2;
        //Debug.Log(Input.mousePosition + " : " + screenDevider + " = " + Input.mousePosition / screenDevider);
    }


    void Info(bool info)
    {
        infoPanel.SetActive(info);

        if (resizable)
        {
            TMPro.TMP_Text buttonText = infoPanel.GetComponentInChildren<TMPro.TMP_Text>(true);
            Vector2 size = new Vector2(25 * buttonText.text.Length, 40);
            buttonText.GetComponent<RectTransform>().sizeDelta = rect.sizeDelta = size;
        }
    }
}
