using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "New item", order = 1)]

public class Item : ScriptableObject
{
    public Sprite sprite;
    [TextArea(2, 5)]
    public string desc;

}
