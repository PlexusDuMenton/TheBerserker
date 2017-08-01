using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Entity
{
    public Inventory Inv;
    protected Stats _Stats;
    protected int Level;

    public override int ReceiveAttack(int damage, Entity Attacker)
    {
        damage = Mathf.Clamp(damage - _Stats.Armor, 1, damage);
        Health = Mathf.Clamp(Health - damage, 0, MaxHealth);
        //OnReceiveAttack(damage);
        CheckDeath(Attacker);
        return damage;
    }

    public int GetLevel
    {
        get { return Level; }
    }
    public Equipement Equip;

    public override void CheckDeath(Entity Attacker)
    {
        if (Health <= 0)
        {
            Attacker.OnKill(this);
            Destroy(gameObject);
        }
    }

    public virtual void UpdateStats()
    {

        Stats newStats = new Stats();

        for (int i = 0; i < Equip.ItemLenght; i++)
        {
            newStats += Equip.GetItemInSlot(i)._Stats;
        }

        _Stats = newStats;

        MaxHealth = BaseHealth + _Stats.Health;
    }

    public int GetDamage
    {
        get { return _Stats.Damage; }
    }
    public int GetArmor
    {
        get
        { return _Stats.Armor; }
    }
    public virtual void Update()
    {
        //ia...
    }
}