using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using static UnityEditor.Progress;

/* ------- СТРУКТУРА ФАЙЛА
 * Действующие классы:
 * - ItemManager    Подгрузка предметов
 * - ItemParser     Извлечение из JSON      static
 * 
 * Классы объектов: (^наследование)
 * - Item       Базовый предмет
 * - Weapon     ^Item + урон
 * - Knife      ^Weapon
 * - Gun        ^Weapon + обойма
 * 
 * ------- ИСПОЛЬЗОВАНИЕ
 * Скрипт ItemManager прикреплляется в 1 экземпляре на камеру и в Awake кэширует все игровые предметы
 * 
 * Чтобы создать предметы, ножи или огнестрел:
 *      Item item = Item.Create("name", count);
 *      Knife item = Knife.Create("name");
 *      Gun gun = Gun.Create("name);
 *  
 *  Скрипт по идее всё сам должен сделать, если парсер работает (не затестчено)
 *  ИИ не использую :D
 */

public class ItemManager : MonoBehaviour
{
    // Это кэшированные словари предметов извлечённые из JSON
    public static Dictionary<string, Item> items = null;
    public static Dictionary<string, Gun> guns = null;
    public static Dictionary<string, Knife> knives = null;
    static bool needInit = true;
    public void Awake()
    {
        if (needInit) //первый запуск
        {
            items = new Dictionary<string, Item>();
            List<Item> itemParsed = ItemParser.LoadItems();
            //Debug.Log(itemParsed[0].Name.TableEntryReference.Key); // это имя
            //Debug.Log(itemParsed[0].ToString());
            if (itemParsed != null || itemParsed.Count > 0)
                foreach(Item item in itemParsed)
                {
                    items.Add(item.Name.TableEntryReference.Key, item);
                    Debug.Log(item.ToString());
                }
                    

            guns = new Dictionary<string, Gun>();
            List<Gun> gunParsed = ItemParser.LoadGuns();
            if (gunParsed != null || gunParsed.Count > 0)
                foreach (Gun gun in gunParsed)
                {
                    guns.Add(gun.Name.TableEntryReference.Key, gun);
                    Debug.Log(gun.ToString());
                }
                    

            knives = new Dictionary<string, Knife>();
            List<Knife> knifeParsed = ItemParser.LoadKnives();
            if (knifeParsed != null || knifeParsed.Count > 0)
                foreach (Knife knife in knifeParsed)
                {
                    knives.Add(knife.Name.TableEntryReference.Key, knife);
                    Debug.Log(knife.ToString());
                }
                    

            //foreach (var k in items.Keys) Debug.Log($"Создан предмет с ключом: {k}");
            //foreach (var k in guns.Keys) Debug.Log($"Создан пистолет с ключом: {k}");
            //foreach (var k in knives.Keys) Debug.Log($"Создан нож с ключом: {k}");

            needInit = false;
        }
    }
}

//Все предметы будут храниться в json файле потому что их много.
internal static class ItemParser
{
    //Промежуточный класс потому что оно не поддреживает byte/short поля
    [System.Serializable]
    public class ItemInfo
    {
        public string name;
        public int maxQuantity;
        public int weightPerUnit;
        public bool locked;

        internal ItemInfo (string name = "", int maxQuantity = 0, int weightPerUnit = 0, bool locked = false)
        {
            this.name = name;
            this.maxQuantity = maxQuantity;
            this.weightPerUnit = weightPerUnit;
            this.locked = locked;
        }
        internal Item Convert()
        {
            return new Item(
                name:name,
                maxQuantity: (byte) Math.Min(Math.Max(maxQuantity, 0), 128),
                weightPerUnit: (byte)Math.Min(Math.Max(weightPerUnit, 0), 128),
                locked: locked
                );
        }
    }
    [System.Serializable]
    public class KnifeInfo : ItemInfo
    {
        public int damage;

        internal KnifeInfo(string name = "", int maxQuantity = 0, int weightPerUnit = 0, bool locked = false,
            int damage = 1)
            : base(name, maxQuantity, weightPerUnit, locked)
        {
            this.damage = damage;
        }

        internal new Knife Convert()
        {
            return new Knife(
                name: name,
                maxQuantity: (byte)Math.Min(Math.Max(maxQuantity, 0), 128),
                weightPerUnit: (byte)Math.Min(Math.Max(weightPerUnit, 0), 128),
                locked: locked,
                damage: (byte)Math.Min(Math.Max(damage, 0), 128)
                );
        }
    }
    [System.Serializable]
    public class GunInfo: ItemInfo
    {
        public int ammoId;
        public int maxAmmo;
        public int damage;

        internal GunInfo(string name = "", int maxQuantity = 0, int weightPerUnit = 0, bool locked = false,
            int ammoId = 0, int maxAmmo = 0, int damage = 1)
            : base(name, maxQuantity, weightPerUnit, locked)
        {
            this.ammoId = ammoId;
            this.maxAmmo = maxAmmo;
            this.damage = damage;
        }

        internal new Gun Convert()
        {
            return new Gun(
                name: name,
                maxQuantity: (byte)Math.Min(Math.Max(maxQuantity, 0), 128),
                weightPerUnit: (byte)Math.Min(Math.Max(weightPerUnit, 0), 128),
                locked: locked,
                ammoId: (byte)Math.Min(Math.Max(ammoId, 0), 128),
                maxAmmo: (byte)Math.Min(Math.Max(maxAmmo, 0), 128),
                damage: (byte)Math.Min(Math.Max(damage, 0), 128)
                );
        }
    }
    [System.Serializable]
    private class ItemArrayWrapper { public ItemInfo[] items; }
    [System.Serializable]
    private class GunArrayWrapper { public GunInfo[] guns; }
    [System.Serializable]
    private class KnifeArrayWrapper { public KnifeInfo[] knives; }

    // Загрузить список Item из Resources/items.json
    public static List<Item> LoadItems()
    {
        TextAsset file = Resources.Load<TextAsset>("Items");
        if (file == null)
        {
            Debug.LogError("Items JSON not found at Resources/Items.json");
            return null;
        }
        //Debug.Log(file.text);
        ItemArrayWrapper wrapper = JsonUtility.FromJson<ItemArrayWrapper>(file.text);
        if (wrapper == null || wrapper.items == null)
        {
            Debug.LogError("Failed to parse items JSON or no items present.");
            return new List<Item>();
        }

        // Заполняем
        List<Item> result = new(wrapper.items.Length);
        foreach (var wrappedItem in wrapper.items)
            result.Add(wrappedItem.Convert());
        return result;
        //items;
    }
    
    public static List<Gun> LoadGuns()
    {
        TextAsset file = Resources.Load<TextAsset>("Guns");
        if (file == null)
        {
            Debug.LogError("Items JSON not found at Resources/Guns.json");
            return null;
        }
        // Оборачиваем
        GunArrayWrapper wrapper = JsonUtility.FromJson<GunArrayWrapper>(file.text);
        if (wrapper == null || wrapper.guns == null)
        {
            Debug.LogError("Failed to parse guns JSON or no guns present.");
            return new List<Gun>();
        }

        // Заполняем
        List<Gun> result = new(wrapper.guns.Length);
        foreach (var wrappedItem in wrapper.guns)
            result.Add(wrappedItem.Convert());

        return result;
    }

    
    public static List<Knife> LoadKnives()
    {
        TextAsset file = Resources.Load<TextAsset>("Knives");
        if (file == null)
        {
            Debug.LogError("Items JSON not found at Resources/Knives.json");
            return null;
        }
        // Оборачиваем
        KnifeArrayWrapper wrapper = JsonUtility.FromJson<KnifeArrayWrapper>(file.text);
        if (wrapper == null || wrapper.knives == null)
        {
            Debug.LogError("Failed to parse knives JSON or no knives present.");
            return new List<Knife>();
        }

        // Заполняем
        List<Knife> result = new(wrapper.knives.Length);
        foreach (var wrappedItem in wrapper.knives)
            result.Add(wrappedItem.Convert());

        return result;
    }
}

public class Item
{
    public LocalizedString Name { get; set; } // Переводится
    public LocalizedString Description { get; set; } // Переводится
    public byte[] Quantity { get; set; } // 0: текущее количество, 1: максимальное в стаке
    public byte WeightPerUnit { get; set; }
    public Sprite Icon { get; set; } // Подгружается
    public bool Locked { get; set; } // for odd high-tech item or quest item of dubious purpose (prevents using that as equipable or consumable)


    //constructor for special items (above variant will be phased out? idk)
    // ! Правильная мысль, я тоже так думаю
    // Теперь string name - это ключ по которому подгружаются название, описание и картинка
    // Отдельный конструктор если name LocalizedString
    internal Item(string name, byte maxQuantity, byte weightPerUnit, byte currentQuantity = 1, bool locked = false)
    {
        Name = new LocalizedString { TableReference = "Items", TableEntryReference = name };
        Description = new LocalizedString { TableReference = "Items", TableEntryReference = $"{name}_desc" };
        Quantity = new byte[2] { currentQuantity, maxQuantity };
        WeightPerUnit = weightPerUnit;
        Icon = Resources.Load<Sprite>($"Icons/{name}");
        Locked = locked;
    }
    internal Item(LocalizedString name, byte maxQuantity, byte weightPerUnit, byte currentQuantity = 1, bool locked = false)
    {
        Name = name;
        Description = new LocalizedString { TableReference = name.TableReference, TableEntryReference = $"{name.TableEntryReference.Key}_desc" };
        Quantity = new byte[2] { currentQuantity, maxQuantity };
        WeightPerUnit = weightPerUnit;
        Icon = Resources.Load<Sprite>($"Icons/{name.TableEntryReference.Key}");
        Locked = locked;
    }

    //---------- УНИКАЛЬНОЕ
    internal short CalculateWeight() // Подсчёт веса стака
    {
        return (short)(Quantity[0] * WeightPerUnit);
    }
    public override string ToString()
    {
        return $"{Name.GetLocalizedString()}: {Description.GetLocalizedString()}\n" +
            $"{Quantity[0]}/{Quantity[1]}, weight {WeightPerUnit}\n" +
            $"Has Icon: {Icon != null}, Locked: {Locked}";
    }

    //---------- ИЗОЛЯЦИЯ СОЗДАНИЯ И ВСЁ СЛУЖЕБНОЕ
    public static List<Item> Create(string itemName, byte count = 1)
    {
        if (ItemManager.items.TryGetValue(itemName, out Item foundItem))
        {
            Item item = foundItem.Clone();      //нашли такой предмет
            return item.UpdateCount(count);     //но его может заспавниться несколько стаков
        }
        else return null; //Не нашли
    }
    public Item Clone()
    {
        return new Item(
            name: Name.TableEntryReference,
            currentQuantity: Quantity[0],
            maxQuantity: Quantity[1],
            weightPerUnit: WeightPerUnit,
            locked: Locked
        );
    }
    internal List<Item> UpdateCount(byte count)
    {
        List<Item> items = new() { this };

        while (count > Quantity[1] && UpdateCount(count, out Item excessItem))
        {
            items.Add(excessItem);
            count = excessItem.Quantity[0];
        }

        return items;
    }
    // Обновление количества + false если предмет не уничтожен (кол-во не 0) + создание лишнего стака если в один не влезает
    // Вспомогательное
    private bool UpdateCount(byte count, out Item excessItem)
    {
        excessItem = null;
        if (count <= Quantity[1])
        {
            // ЛУЧШИЙ ПРОСТО
            //Debug.Log($"Attempting to update count: {count}. Current quantity: {Quantity[0]}/{Quantity[1]}.");
            Quantity[0] = count;
            return count != 0;
        }

        Quantity[0] = Quantity[1]; // В изначальном стаке максимум предметов
        // Создание лишнего стака предмета (не защищено от тройного стака)
        excessItem = new Item(
            name: Name,
            currentQuantity: (byte)(count - Quantity[1]),
            maxQuantity: Quantity[1],
            weightPerUnit: WeightPerUnit);
        return true;
    }
}


public abstract class Weapon : Item
{
    public byte Damage { get; private set; } // Урон

    // В конструкторах тоже уже тяжко пошло, пора указывать параметры словами
    public Weapon(string Name, byte MaxQuantity, byte WeightPerUnit,
        bool Locked = false,
        byte CurrentQuantity = 1,
        byte Damage = 1
        )
        : base(name: Name,
            maxQuantity: MaxQuantity,
            weightPerUnit: WeightPerUnit,
            currentQuantity: CurrentQuantity,
            locked: Locked)
    {
        this.Damage = Damage;
    }
    public Weapon(LocalizedString Name, byte MaxQuantity, byte WeightPerUnit,
        bool Locked = false,
        byte CurrentQuantity = 1,
        byte Damage = 1
        )
        : base(name: Name,
            maxQuantity: MaxQuantity,
            weightPerUnit: WeightPerUnit,
            currentQuantity: CurrentQuantity,
            locked: Locked)
    {
        this.Damage = Damage;
    }

    public Weapon(Item item, byte Damage = 1)
        : base (name: item.Name.TableEntryReference,
            maxQuantity: item.Quantity[1],
            weightPerUnit: item.WeightPerUnit,
            currentQuantity: item.Quantity[0],
            locked: item.Locked)
    {
        this.Damage = Damage;
    }
}

public class Knife : Weapon
{
    // Автоматических констуктор, думаю ничего не надо лишнего
    internal Knife(string name, byte maxQuantity, byte weightPerUnit, byte currentQuantity = 1, bool locked = false, byte damage = 1)
        : base(Name: name, MaxQuantity: maxQuantity, WeightPerUnit: weightPerUnit, CurrentQuantity: currentQuantity, Damage: damage, Locked: locked) { }
    internal Knife(LocalizedString name, byte maxQuantity, byte weightPerUnit, byte currentQuantity = 1, bool locked = false, byte damage = 1)
        : base(Name: name, MaxQuantity: maxQuantity, WeightPerUnit: weightPerUnit, CurrentQuantity: currentQuantity, Damage: damage, Locked: locked) { }
    internal Knife(Weapon weapon) : base(weapon) { }

    public override string ToString()
    {
        return $"{Name.GetLocalizedString()}: {Description.GetLocalizedString()}\n" +
            $"{Quantity[0]}/{Quantity[1]}, weight {WeightPerUnit}, " +
            $"Has Icon: {Icon != null}, Locked: {Locked}, " +
            $"Damage {Damage}";
    }

    //---------- УНИКАЛЬНОЕ
    public void UpdateLoadBar(GameUI gm) // Обновление интерфейса
    {
        gm.weaponLoadObj.SetActive(false);
        gm.choosenWeapon.sprite = Icon;
    }

    //---------- ИЗОЛЯЦИЯ СОЗДАНИЯ И ВСЁ СЛУЖЕБНОЕ
    public static new List<Knife> Create(string itemName, byte count = 1)
    {
        if (ItemManager.knives.TryGetValue(itemName, out Knife foundItem))
        {
            Knife knife = foundItem.Clone();      //нашли такой предмет
            return knife.UpdateCount(count);     //но его может заспавниться несколько стаков
        }
        else return null; //Не нашли
    }
    public new Knife Clone()
    {
        return new Knife(
            name: Name,
            maxQuantity: Quantity[1],
            weightPerUnit: WeightPerUnit,
            currentQuantity: Quantity[0],
            locked: Locked,
            damage: Damage
        );
    }
    internal new List<Knife> UpdateCount(byte count)
    {
        List<Knife> items = new() { this };

        while (count > Quantity[1] && UpdateCount(count, out Knife excessItem))
        {
            items.Add(excessItem);
            count = excessItem.Quantity[0];
        }

        return items;
    }
    private bool UpdateCount(byte count, out Knife excessItem)
    {
        excessItem = null;
        if (count <= Quantity[1])
        {
            // ЛУЧШИЙ ПРОСТО
            //Debug.Log($"Attempting to update count: {count}. Current quantity: {Quantity[0]}/{Quantity[1]}.");
            Quantity[0] = count;
            return count != 0;
        }

        Quantity[0] = Quantity[1]; // В изначальном стаке максимум предметов
        // Создание лишнего стака предмета (не защищено от тройного стака)
        excessItem = this.Clone();
        excessItem.Quantity[0] = (byte)(count - Quantity[1]);
        return true;
    }
}

public class Gun : Weapon
{
    public byte AmmoId { get; set; } // 0 - пистолет, 1 - винтовка, 2 - дробовик
    public byte MaxAmmo { get; set; }
    public byte CurrentAmmo { get; set; }

    internal Gun(string name, byte maxQuantity, byte weightPerUnit, byte ammoId, byte maxAmmo,
        byte currentAmmo = 0,
        byte currentQuantity = 1,
        bool locked = false,
        byte damage = 1
        )
        : base(Name: name,
            MaxQuantity: maxQuantity,
            WeightPerUnit: weightPerUnit,
            CurrentQuantity: currentQuantity,
            Damage: damage,
            Locked: locked)
    {
        AmmoId = ammoId;
        MaxAmmo = maxAmmo;
        CurrentAmmo = currentAmmo;
    }
    internal Gun(LocalizedString name, byte maxQuantity, byte weightPerUnit, byte ammoId, byte maxAmmo,
        byte currentAmmo = 0,
        byte currentQuantity = 1,
        bool locked = false,
        byte damage = 1
        )
        : base(Name: name,
            MaxQuantity: maxQuantity,
            WeightPerUnit: weightPerUnit,
            CurrentQuantity: currentQuantity,
            Damage: damage,
            Locked: locked)
    {
        AmmoId = ammoId;
        MaxAmmo = maxAmmo;
        CurrentAmmo = currentAmmo;
    }

    internal Gun(Weapon weapon, byte ammoId, byte maxAmmo, byte currentAmmo = 0)
        : base(item: weapon)
    {
        AmmoId = ammoId;
        MaxAmmo = maxAmmo;
        CurrentAmmo = currentAmmo;
    }
    public override string ToString()
    {
        return $"{Name.GetLocalizedString()}: {Description.GetLocalizedString()}\n" +
            $"{Quantity[0]}/{Quantity[1]}, weight {WeightPerUnit}, " +
            $"Has Icon: {Icon != null}, Locked: {Locked}\n" +
            $"Damage {Damage}, " +
            $"Ammo: {CurrentAmmo}/{MaxAmmo}, Ammo ID: {AmmoId}";
    }

    //---------- УНИКАЛЬНОЕ
    internal short Reload(short bulletCount)
    {
        if (bulletCount <= 0) return 0;

        if (bulletCount > MaxAmmo) // Если патронов больше чем помещается в магазин
        {
            CurrentAmmo = MaxAmmo;
            return (short)(bulletCount - MaxAmmo);
        }
        else // Оставшиеся патроны в магазин
        {
            CurrentAmmo = (byte)bulletCount;
            return (short)0;
        }
    }
    public void UpdateLoadBar(GameUI gm, byte AmmoInStock) // Обновление интерфейса
    {
        gm.weaponLoadObj.SetActive(true);
        GameUI.UpdateBar(gm.barLevel[3], gm.weaponSprite3, CurrentAmmo, MaxAmmo);
        gm.bulletText.text = $"{CurrentAmmo}/{AmmoInStock}"; // Proper representation of ammo
        //Debug.Log(gm.bulletIconSprite[AmmoId]);
        gm.bulletIcon.sprite = gm.bulletIconSprite[AmmoId];
        //Debug.Log(Name.GetLocalizedString() + " " + Icon);
        gm.choosenWeapon.sprite = Icon;
    }

    //---------- ИЗОЛЯЦИЯ СОЗДАНИЯ И ВСЁ СЛУЖЕБНОЕ
    // New скрывает наследование от Item
    public static new List<Gun> Create(string itemName, byte count = 1)
    {
        if (ItemManager.guns.TryGetValue(itemName, out Gun foundItem))
        {
            Gun gun = foundItem.Clone();      //нашли такой предмет
            return gun.UpdateCount(count);     //но его может заспавниться несколько стаков
        }
        else return null; //Не нашли
    }
    public new Gun Clone()
    {
        return new Gun(
            name: Name.TableEntryReference,
            maxQuantity: Quantity[1],
            weightPerUnit: WeightPerUnit,
            ammoId: AmmoId,
            maxAmmo: MaxAmmo,
            currentAmmo: CurrentAmmo,
            currentQuantity: Quantity[0],
            locked: Locked,
            damage: Damage
        );
    }
    internal new List<Gun> UpdateCount(byte count)
    {
        List<Gun> items = new() { this };

        while (count > Quantity[1] && UpdateCount(count, out Gun excessItem))
        {
            items.Add(excessItem);
            count = excessItem.Quantity[0];
        }

        return items;
    }
    private bool UpdateCount(byte count, out Gun excessItem)
    {
        excessItem = null;
        if (count <= Quantity[1])
        {
            //Debug.Log($"Attempting to update count: {count}. Current quantity: {Quantity[0]}/{Quantity[1]}.");
            Quantity[0] = count;
            return count != 0;
        }

        Quantity[0] = Quantity[1]; // В изначальном стаке максимум предметов
        // Создание лишнего стака предмета (не защищено от тройного стака)
        excessItem = this.Clone();
        excessItem.Quantity[0] = (byte)(count - Quantity[1]);
        return true;
    }
}