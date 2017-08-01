using System.Collections;
using UnityEngine;
using System;

//----------------------------------------------------------------------------------------------------------------------------------------------
//-------------------------------------------------------- Item Stack BaseClass ----------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------------------


public class Items
{
    public Item Item;
    public int Quantity;
    public int MaxQuantity
    {
        get { return Item.MaxStack; }
    }
    public int State {
        get { return Item.State; }
    }

    public Items(Item item, int quantity = 1)
    {
        Item = item;
        Quantity = quantity;
    }




    //Just lot of override so having +/- >= / <= work with the quantities
    // equals , == and != work with item id to check if item are the same
    // == and != with item and number simply compare the ammounts :
    public override bool Equals(object obj)
    {
        if (obj is Items)
            return Quantity == ((Items)obj).Quantity;
        else
            return false;
    }
    public static implicit operator int(Items i)
    {
        return i.Quantity;
    }
    public static implicit operator Item(Items i)
    {
        return i.Item;
    }

    public override int GetHashCode()
    {
        return Quantity.GetHashCode() ^ Item.GetHashCode();
    }

    public static bool operator >(Items _is1, Items _is2)
    {
        return _is1.Quantity > _is2.Quantity;
    }
    public static bool operator >=(Items _is1, Items _is2)
    {
        return _is1.Quantity >= _is2.Quantity;
    }
    public static bool operator <=(Items _is1, Items _is2)
    {
        return _is1.Quantity <= _is2.Quantity;
    }
    public static bool operator <(Items _is1, Items _is2)
    {
        return _is1.Quantity < _is2.Quantity;
    }

    public static bool operator ==(Items _is1, Items _is2)
    {
        return _is1.Item.ID == _is2.Item.ID;
    }
    
    public static bool operator !=(Items _is1, Items _is2)
    {
        return _is1.Item.ID != _is2.Item.ID;
    }

    public static bool operator ==(Items _is, int _i)
    {
        return _is.Quantity == _i;
    }
    public static bool operator !=(Items _is, int _i)
    {
        return _is.Quantity != _i;
    }

    public static Items operator +(Items _is1, Items _is2)
    {
        if (_is1 != _is2)
            return _is1;
        Items result = _is1;
        result.Quantity = Mathf.Clamp( _is1.Quantity + _is2.Quantity, 0, _is1.MaxQuantity);
        return result;
    }
    public static Items operator +(Items _is, int _i)
    {
        Items result = _is;
        result.Quantity = Mathf.Clamp(_is.Quantity + _i, 0, _is.MaxQuantity);
        return result;
    }

    public static Items operator -(Items _is1, Items _is2)
    {
        if (_is1 != _is2)
        {
            return _is1;
        }
        Items result = _is1;
        result.Quantity = Mathf.Clamp(_is1.Quantity - _is2.Quantity, 0, _is1.MaxQuantity);
        return result;
    }
    public static Items operator -(Items _is, int _i)
    {
        Items result = _is;
        result.Quantity = Mathf.Clamp(_is.Quantity - _i, 0, _is.MaxQuantity);
        return result;
    }
}


//----------------------------------------------------------------------------------------------------------------------------------------------
//-------------------------------------------------------  Inventory BaseClass -----------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------------------

public class Inventory {

    Items[] ItemList;
    public Human Parent; 
    public Inventory(int _iSize)
    {
        ItemList = new Items[_iSize];
    }

    public int getFreeSlot()
    {
        for (int i = 0; i < ItemList.Length; i++)
        {
            if (ItemList[i] == null || ItemList[i] == default(Items))
            {
                return i;
            }
        }
        return -1;
    }

    public int getItemSlot(Items item) {
        //check if item exist in inventory
        for (int i = 0; i < ItemList.Length;i++)
        {
            if (ItemList[i] == item)
            {
                return i;
            }
        }
        //check for the first empty slot
        return -1;
    }
    //return true if the item is taken in inventory
    public bool addItem(Items item)
    {
        int pos = getItemSlot(item);
        if (pos == -1 
            && (pos = getFreeSlot()) == -1)
        {
            return false;
        }
        if (ItemList[pos] == null || ItemList[pos] == default(Items))
        {
            item.Item.Parent = this;
            ItemList[pos] = item;
            return true;
        }
        else
        {
            ItemList[pos] += item;
            return true;
        }
    }

    public void reduceItem(int slot, int ammounts)
    {
        ItemList[slot] -= ammounts;
        if (ItemList[slot] < 0)
        {
            removeItem(slot);
        }
    }

    public void removeItem (int slot)
    {
        ItemList[slot] = default(Items);
    }
}


