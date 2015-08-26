Пока только мысли.

есть категории предметов
```csharp

public enum EThingCategory
{
[ThingCategory("Шлемы", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)] HELMETS,
[ThingCategory("Ожерелья", '\'', ConsoleKey.Oem7, EKeyModifiers.NONE)] NECKLACES,
[ThingCategory("Одежда", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)] WEAR,
[ThingCategory("Броня", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)] ARMOR,
[ThingCategory("Пояса", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)] GIRGLE,
[ThingCategory("Плащи", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)] CLOACK,
[ThingCategory("Наручи", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)] BRACERS,
[ThingCategory("Перчатки", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)] GAUNTLETS,
[ThingCategory("Обувь", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)] BOOTS,
[ThingCategory("Оружие", '(', ConsoleKey.D9, EKeyModifiers.SHIFT)] WEAPON,
[ThingCategory("Метательное оружие", '}', ConsoleKey.Oem6, EKeyModifiers.SHIFT)] MISSILE_WEAPON,
[ThingCategory("Кольца", '=', ConsoleKey.OemPlus, EKeyModifiers.NONE)] RINGS,
[ThingCategory("Пища", '%', ConsoleKey.D5, EKeyModifiers.SHIFT)] FOOD,
[ThingCategory("Снадобья", '!', ConsoleKey.D1, EKeyModifiers.SHIFT)] POTION,
[ThingCategory("Снаряды", '/', ConsoleKey.Oem2, EKeyModifiers.NONE)] MISSILES,
[ThingCategory("Инструменты", ']', ConsoleKey.Oem6, EKeyModifiers.NONE)] TOOLS,
[ThingCategory("Жезлы", '\\', ConsoleKey.Oem5, EKeyModifiers.NONE)] WANDS,
[ThingCategory("Книги", '"', ConsoleKey.D2, EKeyModifiers.SHIFT)] BOOKS,
[ThingCategory("Свитки", '?', ConsoleKey.Oem2, EKeyModifiers.SHIFT)] SCROLLS,
[ThingCategory("Мебель", '#', ConsoleKey.D3, EKeyModifiers.SHIFT)] FURNITURE,
[ThingCategory("Окружение", 'v', ConsoleKey.NoName, EKeyModifiers.NONE)] LANDSCAPE
}```

описатель вещи должен иметь категорию.

Также описатель должен содержать инфу о материале, который в свою очередь также сможет привнести свои доп категории, например светящийся материал делает любой предмет, из него изготовленный, в осветитель.

Также видимо нужна иерархия предметов, то есть чилд имеет все свойства родителя, но может добавить свои или перекрыть родительские.

Также чилд может добавить свою категорию, под которую данный предмет подпадает.
Например: труп - труп и еда, пузырек - пузырек и метательный снаряд, камень - метательный снаряд и оружие ближнего боя, кость тролля также является оружием.

Категория подразумевает действия, которые с данным предметом можно производить. А действия в свою очередь должны декларировать параметры предмета, необходимые для расчета эффекта действия.

Счетчик - сколько в мире таких объектов может существовать одновременно.

Уровень предмета. От этого зависит вероятность встречи объекта на местности или в луте.