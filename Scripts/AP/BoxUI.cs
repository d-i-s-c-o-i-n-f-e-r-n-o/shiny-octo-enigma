using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class BoxUI : MonoBehaviour
{
    [Header("For Icon constructing")]
    public Sprite[] allIcons;
    public Dictionary<string, Item> items = new();
    public Dictionary<string, Weapon> weapon = new();

    [Header("Container items")]
    public int volume = 4;
    public List <ItemHandler> box = new List <ItemHandler>();
    public List <ItemHandler> possible_contents = new List <ItemHandler>(); //list rather than array

    [Header("OtherThings")]
    public bool isActive = false; //inactive box. Ok potentially useful
    public GameObject app;

    private void Awake()
    {
        if(box.Count>volume)
            volume = box.Count;
        //random fill of excess slots
        else{
            for(int i = box.Count-1; i<volume; i++){
                box.Add(possible_contents[Random.Range(0, possible_contents.Count-1)]); //uses prefabs from list
            }
        }
        //������������� �������
        //items.Add("canned_meat", new Item("�������� �������", "������� ������� �������, ���� �������� � �������. ����� ��������� ������ ��� ����, ��� ����� �������.", 1, 4, 25, allIcons[0]));
        //������������� ������� �������
        //weapon.Add("common_knife", new Weapon("������� ���", "�����-�� ��� �� ����-�� �����.", 1, 1, 50, allIcons[2], 3, 0, 0));
    }

    public void UpdateItems() //���������� ���� ������
    {
        foreach (var handler in box)
            handler.UpdateIcon();

    }

    //Box DOES NOT take weight of item into account only ammount
    public bool AddItem(Item itemToAdd) //well player can dump something in box (of use with player base)
    {
        //NOTE: contents of PECULIAR containers have to be serialized in savegames
        if (itemToAdd == null) return false; //������ ���������

        // Find the last empty slot (start from the end)
        for (int i = box.Count - 1; i >= 0; i--) //��� � ����� ������ �������� ���
        {
            if (box[i] == null || box[i].itemSelf == null || box[i].itemSelf.Quantity[0] == 0)
            {
                box[i].UpdateItem(itemToAdd);
                UpdateItems();
                return true;
            }
        }
        //�� ��� �������� ������� �� ���������, ��
        Debug.LogWarning("Cannot add item: container is full!");
        return false;
    }

    // Deletes an item by reference
    public void DeleteItem(Item itemToDelete) //������� ����� ��� �� ������ ������ ���������
    {
        for (byte i = 0; i < box.Count; i++)
        {
            if (box[i] != null && box[i].itemSelf == itemToDelete)
            {
                box[i].itemSelf = null;
                return;
            }
        }
        Debug.LogWarning("Item not found in box!");
    }
}