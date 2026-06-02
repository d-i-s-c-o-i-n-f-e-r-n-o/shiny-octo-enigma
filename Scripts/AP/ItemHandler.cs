using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
//Саня пж не заруинь кодировку, я только на русском пишу и по памяти уже не восстановлю

// Этот класс для отображения слотов инвентаря
// Все остальные классы в файлике ItemManager.cs, лежит рядом
public class ItemHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int index;
    string animBool;
    public BackpackUI bpUI;
    public Animator bp;
    public Item itemSelf;
    public UnityEngine.UI.Image itemImage;

    [Header("Pop Up")]
    public TMPro.TMP_Text nameText;
    public TMPro.TMP_Text descText;
    TMPro.TMP_Text countText;
    public TMPro.TMP_Text weightText;
    public UnityEngine.UI.Image icon;

    [Header("Weight Pop UP")]
    public RectTransform totalWeight;
    TMPro.TMP_Text stackWeight;

    private void Start()
    {
        stackWeight = totalWeight.GetComponentInChildren<TMPro.TMP_Text>(true);
        countText = GetComponentInChildren<TMPro.TMP_Text>(true);
        switch (index)
        {
            case 0: animBool = "isOpened"; break;
            case 1: animBool = "isZip"; break;
            case 2: animBool = "isSides"; break;
        }
        //itemSelf = Item.Create("test")[0];

        //popUp.SetActive(false);

        if (itemSelf == null || itemSelf.Icon == null)
        {
            itemImage.enabled = false;
        }
        else
        {
            itemImage.sprite = itemSelf.Icon;
            itemImage.enabled = true;
            if (countText != null) countText.text = itemSelf.Quantity[0] > 1 ? itemSelf.Quantity[0].ToString() : "";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bp.SetBool(animBool, true);

        if (itemSelf != null)
        {
            UpdateInformation(itemSelf);
            totalWeight.anchoredPosition = GetComponent<RectTransform>().anchoredPosition /*+ new Vector2(0, 35)*/;
            totalWeight.gameObject.SetActive(true);
            stackWeight.text = itemSelf.CalculateWeight().ToString();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bp.SetBool(animBool, false);
        totalWeight.gameObject.SetActive(false);
    }
    
    public void UpdateInformation(Item newItem = null)
    {
        if (newItem != null)
        {
            nameText.text = newItem.Name.GetLocalizedString();
            descText.text = newItem.Description.GetLocalizedString();
            countText.text = newItem.Quantity[0] > 1 ? newItem.Quantity[0].ToString() : "";
            weightText.text = newItem.WeightPerUnit.ToString();
            icon.sprite = newItem.Icon;
            icon.enabled = newItem.Icon != null;
            bpUI.currentItem = newItem;
        }
        else
        {
            nameText.text = descText.text = countText.text = weightText.text = string.Empty;
            icon.sprite = null;
        }
    }

    //--------------Обновление интерфейса

    public void UpdateItem(Item newItem) // Записывает предмет в слот
    {
        itemSelf = newItem;
        UpdateIcon();
    }

    public void UpdateIcon() //Обнолвляет иконку
    {
        if (itemSelf != null && itemSelf.Icon != null)
        {
            itemImage.sprite = itemSelf.Icon;
        }
        itemImage.enabled = itemSelf.Icon != null;
    }
}