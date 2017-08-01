using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaseEnnemy : Human {

    int IDLE = 0;
    int PASSIVE = 1;
    int AGRESSIVE = 2;

    protected int AIState = 0;

    //0 = IDLE -- IL BOUGE PAS
    //1 = Passive 
    //2 = Agressive
    public int getState
    {
        get { return AIState; }
    }
    protected float AttackRange;
    protected float IdleRange;
    protected float PassiveRange;
    protected Vector3 PositionGoal;
    protected float LastAttack;
    protected float AttackCD;
    protected float AttackDuration;

    protected int BaseDamage;
    protected int DamagePerLevel;

    protected int BaseArmor;
    protected float ArmorPerLevel;

    protected int HealthPerLevel;

    void Awake()
    {
        Team = 1;
        GameObject DamageHit = new GameObject("HitPoints");
        DamageHit.transform.SetParent(GameObject.Find("Canvas").transform);
        HealthTextEnnemy Script = DamageHit.AddComponent<HealthTextEnnemy>();
        Script.Init(transform);
    }

    public override void UpdateStats()
    {

        Stats newStats = new Stats();

        Stats LevelStats = new Stats();

        LevelStats.Damage = BaseDamage + Level * DamagePerLevel;
        LevelStats.Armor = BaseArmor + Mathf.CeilToInt(Level * ArmorPerLevel);
        LevelStats.Health = BaseHealth + HealthPerLevel * Level;
        /*
        for (int i = 0; i < Equip.ItemLenght; i++)
        {
            newStats += Equip.GetItemInSlot(i)._Stats;
        }
        */
        _Stats = newStats + LevelStats;

        MaxHealth =_Stats.Health;
        Health = MaxHealth;
    }

    public override void CheckDeath(Entity Attacker)
    {
        if (Health <= 0)
        {
            Attacker.OnKill(this);
            MANAGER.RemoveEnemyToList(this);
            Destroy(gameObject);
        }
    }

    public override void Update()
    {


            if (AIState == IDLE)
        {
            IdleAction();
            return;
        }
        if (LastAttack + AttackDuration< Time.timeSinceLevelLoad)
        {
            transform.LookAt(MANAGER.GetPlayerPos);
            if (AIState == PASSIVE)
            {
                PassiveAction();
            }
            if (AIState == AGRESSIVE)
            {
                AgressiveAction();
            }
        }
    }

    public virtual void IdleAction()
    {
        float distance = Vector3.Distance(MANAGER.GetPlayerPos, transform.position);
        if (distance <= IdleRange)
        {
            AIState = PASSIVE;
            return;
        }
    }
    public virtual void PassiveAction()
    {
        float distance = Vector3.Distance(MANAGER.GetPlayerPos, transform.position);
        if (distance >= IdleRange)
        {
            AIState = IDLE;
            return;
        }
        if (distance < PassiveRange && MANAGER.CanGetAgressive(5)) {
            AIState = AGRESSIVE;
            return;
        }
        else if (distance < PassiveRange)
        {
            PositionGoal = (transform.position - MANAGER.GetPlayerPos).normalized * PassiveRange;
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(PositionGoal);
            //GetAwwaysFromPlayer
        }
        else if (GetComponent<NavMeshAgent>().isStopped)
        {
            PositionGoal = (Random.insideUnitSphere + MANAGER.GetPlayerPos).normalized* PassiveRange;
            PositionGoal.y = 0;
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(PositionGoal);
            //Move ARROUND HIM
        }
    }
    public virtual void AgressiveAction()
    {
        float distance = Vector3.Distance( MANAGER.GetPlayerPos,transform.position);
        if (distance >= PassiveRange)
        {
            AIState = PASSIVE;
            return;
        }

        if (distance<= AttackRange && (LastAttack + AttackCD < Time.timeSinceLevelLoad))
        {
            Attack(MANAGER.GetPlayerPos);
        }
        else
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(MANAGER.GetPlayerPos);
        }
    }

    protected IEnumerator DestObj(Transform gameObj, float delay)
    {
        Debug.Log("Destroy all !");
        yield return new WaitForSeconds(delay);
        Destroy(gameObj.gameObject);
    }

    public virtual void Attack(Vector3 position)
    {
        LastAttack = Time.timeSinceLevelLoad;
    }


}


public class HealthTextEnnemy : MonoBehaviour
{
    float CreationTime;
    Transform Character;
    public void Init(Transform pos)
    {
        Character = pos;
        Text text = gameObject.AddComponent<Text>();
        text.text = Character.GetComponent<BaseEnnemy>().GetHealth + "  /  " + Character.GetComponent<BaseEnnemy>().GetMaxHealth;
        
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.font = ArialFont;
        text.material = ArialFont.material;
        text.fontSize = 40;
        text.color = Color.black;
        text.alignment = TextAnchor.MiddleCenter;

        GetComponent<RectTransform>().sizeDelta = new Vector2(250, 50);

        Outline outline = gameObject.AddComponent<Outline>();
        outline.effectColor = Color.white;
        outline.effectDistance = new Vector2(1, 1);
        Vector3 ScreenPosition = Camera.main.WorldToScreenPoint(Character.position);
        GetComponent<RectTransform>().position = ScreenPosition;
        CreationTime = Time.timeSinceLevelLoad;
    }

    public void Update()
    {
        if (Character == null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        GetComponent<Text>().text = Character.GetComponent<BaseEnnemy>().GetHealth + "  /  " + Character.GetComponent<BaseEnnemy>().GetMaxHealth;
        Vector3 ScreenPosition = Camera.main.WorldToScreenPoint(Character.position);
        GetComponent<RectTransform>().position = ScreenPosition + Vector3.up * 100;
        
    }
}