using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Consumable : Item
{

    protected override void Use()
    {
        Parent.reduceItem(Parent.getItemSlot((Items)this), 1);
    }
}
