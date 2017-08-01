using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//----------------------------------------------------------------------------------------------------------------------------------------------
//------------------------------------------------------------ Item BaseClass ------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------------------

public class Item {
    public Inventory Parent;
    public int ID;
    public int MaxStack;
    public string Name;
    public int State;
    public int Value;


    public static implicit operator Items(Item i)
    {
        return new Items(i);
    }

    protected virtual void Use(){}
}

//could change it to class for performance reason , have to make test
public struct Stats
{
    public int Damage;
    public int Armor;
    public int Health;
   

    public static Stats operator +(Stats stat1, Stats stat2)
    {
        Stats newStats = new Stats();
        newStats.Damage = stat1.Damage + stat2.Damage;
        newStats.Armor = stat1.Armor + stat2.Armor;
        newStats.Health = stat1.Health + stat2.Health;
        //more stats
        return newStats;
    }
}

//-------------------------------------------------------------------------------------------------------------------------------------------
//-------------------------------------------------------- Modifier System ----------------------------------------------------------------
//-------------------------------------------------------------------------------------------------------------------------------------------

public class Modifier
{
    Entity Caster;
    Entity Target;
    public Modifier(Entity caster, Entity target)
    {
        Caster = caster;
        Target = target;
        OnApply();
    }
    
    public virtual void OnApply() {}

    public virtual void OnRemove() {}

    public virtual void OnUpdate() {}

    public virtual void OnDeath() {}

    public virtual void OnReceiveAttack(int damage) {}

    public virtual void OnAttack(int damage) {}

    public virtual void OnHeal(int heal) {}

}