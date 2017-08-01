using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipable : Item
{
    public Stats _Stats;
    int Slot;
    protected override void Use()
    {
        Parent.removeItem(Parent.getItemSlot((Items)this));
        Parent.addItem((Items)Parent.Parent.Equip.GetItemInSlot(Slot));

    }
}