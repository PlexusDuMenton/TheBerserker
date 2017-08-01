using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class TESTENNEMY : BaseEnnemy
{
    public int health = 1;
    public int level = 2;
    public float speed = 2;

    public float attackCD;
    public float attackDuration;

    public int baseDamage = 1;
    public int damagePerLevel = 2;

    public int baseArmor = 0;
    public float armorPerLevel = 1;

    public int healthPerLevel = 2;

    public float attackRange = 1.5f;
    public float idleRange = 15;
    public float passiveRange = 8;

    public Transform attackPrefab;

    public void Init()
    {
        MANAGER.AddEnemyToList(this);
        AttackRange = attackRange;
        Speed = speed;
        IdleRange = idleRange;
        AttackCD = attackCD;
        AttackDuration = attackDuration;
        PassiveRange = passiveRange;
        BaseHealth = health;
        Level = level;
        Team = 1;

        BaseDamage = baseDamage;
        DamagePerLevel = damagePerLevel;

        BaseArmor = baseArmor;
        ArmorPerLevel = armorPerLevel;

        HealthPerLevel = healthPerLevel;

        UpdateStats();

        GameObject DamageHit = new GameObject("HitPoints");
        DamageHit.transform.SetParent(GameObject.Find("Canvas").transform);
        HealthTextEnnemy Script = DamageHit.AddComponent<HealthTextEnnemy>();
        Script.Init(transform);

        GetComponent<NavMeshAgent>().speed = GetSpeed;
    }

    public override void Attack(Vector3 position)
    {
        base.Attack(position);
        Transform AttackVisualisation = Object.Instantiate(attackPrefab, transform.position + transform.forward, transform.rotation);
        AttackVisualisation.parent = transform;
        StartCoroutine(DestObj(AttackVisualisation, AttackDuration));

        DamageTrigger Damager = AttackVisualisation.gameObject.AddComponent<DamageTrigger>();
        Damager.damageOnTouch = GetDamage;
        Damager.Attacker = this;
        Damager.Team = GetTeam;
        //PATERNE D'ATTACK
    }
}
