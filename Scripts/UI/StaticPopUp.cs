using UnityEngine;
using UnityEngine.EventSystems;

public class StaticPopUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject popUp;
    private void Start()
    {
        popUp.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        popUp.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        popUp.SetActive(false);
    }
}