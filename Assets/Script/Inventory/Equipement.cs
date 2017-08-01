using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipement
{
    Equipable[] _Item;
    Entity Parent;
    public int ItemLenght
    {
        get { return _Item.Length; }
    }

    public Equipable GetItemInSlot(int slot)
    {
        return _Item[slot];
    }

    public void addItem(Equipable items,int slot)
    {
        _Item[slot] = items;
    }
    public void removeItem(int slot)
    {
        _Item[slot] = default(Equipable);
    }
    public Equipement(int size)
    {
        _Item = new Equipable[size];
    }
}
