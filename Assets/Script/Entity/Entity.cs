using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//----------------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------- Entity BaseClass ------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------------------

public class Entity : MonoBehaviour {

    protected int Health;
    protected int Team;
    protected float Speed;

    public float GetSpeed
    {
        get { return Speed; }
    }

    public int GetHealth
    {
        get { return Health; }
    }
    public int GetTeam
    {
        get { return Team; }
    }

    protected int MaxHealth;
    public int GetMaxHealth
    {
        get { return MaxHealth; }
    }

    public virtual int ReceiveAttack(int damage, Entity Attacker)
    {
        Health = Mathf.Clamp(Health - damage, 0, MaxHealth);
        //OnReceiveAttack(damage);
        CheckDeath(Attacker);
        return damage;
    }
    public virtual void CheckDeath(Entity Attacker)
    {
        if (Health <= 0)
        {
            Attacker.OnKill(this);
            Destroy(gameObject);
        }
    }
    protected int BaseHealth;
    public virtual void OnKill(Entity Killed)
    {
    }
    public virtual void OnKill(Human Killed)
    {
    }
    public virtual void OnHit(Entity Victim)
    {
    }
    public virtual void OnHit(Human Victim)
    {
    }
    List<Modifier> Mod;
    /*
    //--------- Just modifier function call --------------
    
    public virtual void OnUpdate() {
        for (int i = 0; i< Mod.Count; i++)
        {
            Mod[i].OnUpdate();
        }
    }

    public virtual void OnDeath() {
        for (int i = 0; i < Mod.Count; i++)
        {
            Mod[i].OnDeath();
        }
    }

    public virtual void OnReceiveAttack(int damage) {
        for (int i = 0; i < Mod.Count; i++)
        {
            Mod[i].OnReceiveAttack(damage);
        }
    }

    public virtual void OnAttack(int damage) {
        for (int i = 0; i < Mod.Count; i++)
        {
            Mod[i].OnAttack(damage);
        }
    }

    public virtual void OnHeal(int heal) {
        for (int i = 0; i < Mod.Count; i++)
        {
            Mod[i].OnHeal(heal);
        }
    }
    */
}
