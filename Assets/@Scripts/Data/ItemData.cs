using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable] // 아이템 데이터 클래스-메모리 변환
public class ItemData 
{
    public int TemplateID;
    public int ItemType;
    public string NameTextID;
    public string DescriptionTextID;
    public string IconImageID;
    public string PrefabNameID;
}

[Serializable] // 아이템 데이터 로더 클래스-JSON 변환
public class ItemDataLoader : IDataLoader<int, ItemData> // IDataLoader 인터페이스 구현
{
    public List<ItemData> Items = new List<ItemData>();

    public Dictionary<int, ItemData> MakeDict()
    {
        Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();
        foreach (var item in Items)
            dict.Add(item.TemplateID, item);
        return dict;
    }
    public bool Validate()
    {
        return Items != null && Items.Count > 0;
    }
}