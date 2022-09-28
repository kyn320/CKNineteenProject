using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIItemSlotImageData", menuName = "UI/ImageData/ItemSlot", order = 0)]
public class UIItemSlotImageData : ScriptableObject
{
    [SerializeField]
    private SerializableDictionary<ItemType, Sprite> itemTypeSpriteDic;
    public SerializableDictionary<ItemType, Sprite> ItemTypeSpriteDic { get { return itemTypeSpriteDic; } }

    [SerializeField]
    private Sprite equippedTagSprite;

    public Sprite EquippedTagSprite { get { return equippedTagSprite; } }



}
