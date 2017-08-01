using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;





public class UIHealthManager
{
    GameObject Health;
    GameObject HealthTop;
    GameObject XP;
    GameObject MaskHealth;
    GameObject MaskXp;

    GameObject HealthBG;

    GameObject HPText;
    GameObject XPText;

    float MaxSize = 1767;
    float HealthSize;


    public UIHealthManager(GameObject[] uIEllements)
    {
        MaskHealth = uIEllements[0];
        Health = uIEllements[1];
        HealthTop = uIEllements[2];
        MaskXp = uIEllements[3];
        XP = uIEllements[4];
        HealthBG = uIEllements[5];
        HPText = uIEllements[6];
        XPText = uIEllements[7];
    }

    public void ChangeHealthSize(int maxHealth)
    {
        HealthSize = Mathf.Clamp(Mathf.Log10(maxHealth)*300, 450, MaxSize);

        HealthBG.GetComponent<RectTransform>().sizeDelta = new Vector2(HealthSize, HealthBG.GetComponent<RectTransform>().sizeDelta.y);
        MaskHealth.GetComponent<RectTransform>().sizeDelta = new Vector2(HealthSize, MaskHealth.GetComponent<RectTransform>().sizeDelta.y);
        Health.GetComponent<RectTransform>().sizeDelta = new Vector2(HealthSize, Health.GetComponent<RectTransform>().sizeDelta.y);


    }

    public void UpdateHealth(float health, int maxHealth){
        health = Mathf.Clamp(health, 0, maxHealth);
        float ratio = 1-(health / maxHealth);
        
        float offset = ratio * HealthSize;

        Time.timeScale = Mathf.Clamp(Mathf.Pow(health / maxHealth,0.5f) + 0.35f, 0.4f, 1);

        Color BarColor = new Color(Mathf.Clamp((ratio * 5) - 1, 0, 1),Mathf.Clamp(1.5f-ratio*2, 0, 1), 0,1);

        Health.GetComponent<Image>().color = BarColor;
        HealthTop.GetComponent<Image>().color = BarColor;
        MaskHealth.GetComponent<RectTransform>().position = new Vector3(28.5f-offset, MaskHealth.GetComponent<RectTransform>().position.y, MaskHealth.GetComponent<RectTransform>().position.z);
        Health.GetComponent<RectTransform>().position = new Vector3(28.5f, Health.GetComponent<RectTransform>().position.y, Health.GetComponent<RectTransform>().position.z);
        HealthTop.GetComponent<Image>().color = BarColor;
        HealthTop.GetComponent<RectTransform>().position = new Vector3(28.5f, HealthTop.GetComponent<RectTransform>().position.y, HealthTop.GetComponent<RectTransform>().position.z);
        Health.GetComponent<RectTransform>().position = HealthTop.GetComponent<RectTransform>().position;
        HPText.GetComponent<Text>().text = Mathf.CeilToInt(health) + " / " + maxHealth + " HP";
    }

    public void UpdateXP(float xp, int maxXp)
    {
        xp = Mathf.Clamp(xp, 0, maxXp);
        float ratio = 1 - (xp / maxXp);

        float offset = ratio * 1377;
        MaskXp.GetComponent<RectTransform>().position = new Vector3(28.5f - offset, MaskXp.GetComponent<RectTransform>().position.y, MaskXp.GetComponent<RectTransform>().position.z);
        XP.GetComponent<RectTransform>().position = new Vector3(28.5f, XP.GetComponent<RectTransform>().position.y, XP.GetComponent<RectTransform>().position.z);

        XP.GetComponent<RectTransform>().position = new Vector3(28.5f, XP.GetComponent<RectTransform>().position.y, XP.GetComponent<RectTransform>().position.z);
        XPText.GetComponent<Text>().text = Mathf.CeilToInt(xp) + " / " + maxXp + " XP";
    }

}



public class Player : Human
{

    public GameObject[] UIEllements;
    //0 = Health mask
    //1 = health
    //2 = healthtop
    //3 = xp mask
    //4 = xp
    //5 = HealthBG
    //6 hp text
    //7 xp text

    public float HP= 50;
    public int MaxHp = 50;

    UIHealthManager UIManager;

    new protected float Health;
    public Material TrailMat;
    public Transform prefab;
    public Vector3 GetDirection
    {
        get { return Controler.lastDirVector.normalized; }
    }

    public void Awake()
    {
        UIManager = new UIHealthManager(UIEllements);
        Xp = 0;
        Level = 1;
        UpdateStats();
        Health = MaxHealth;

        
        XpNextLevel = 100;
        Team = 0;
        Controler = new PlayerControler(this);
        Speed = ActualSpeed;
        UIManager.UpdateXP(Xp, XpNextLevel);

        MANAGER.player = this;

    }
    new public float GetHealth
    {
        get { return Health; }
    }

    public override int ReceiveAttack(int damage, Entity Attacker)
    {
        damage = Mathf.Clamp(damage - _Stats.Armor, 1, damage);
        Health = Mathf.Clamp(Health - damage, 0, MaxHealth);
        CheckDeath(Attacker);
        return damage;
    }

    public override void UpdateStats()
    {

        Stats newStats = new Stats();

        Stats LevelStats = new Stats();

        LevelStats.Damage = 2 + Level * 2;
        LevelStats.Armor = 0 + Mathf.CeilToInt(Level * 1.5f);
        LevelStats.Health = 60 + 40 * Level;
        /*
        for (int i = 0; i < Equip.ItemLenght; i++)
        {
            newStats += Equip.GetItemInSlot(i)._Stats;
        }
        */
        _Stats = newStats + LevelStats;

        MaxHealth = BaseHealth + _Stats.Health;
        UIManager.ChangeHealthSize(MaxHealth);
    }


    //ON KILL ENNEMY
    public override void OnKill(Human Killed)
    {
            GainExp(Killed.GetLevel * 10);
            Health = Mathf.Clamp(Health + Killed.GetLevel*20, 0, MaxHealth);
    }
    public override void OnHit(Human Killed)
    {
        Health = Mathf.Clamp(Health +MaxHealth*0.01f, 0, MaxHealth);
    }
    public override void OnHit(Entity Killed)
    {
        Health = Mathf.Clamp(Health + MaxHealth * 0.01f, 0, MaxHealth);
    }

    public void DestroyObject(Transform gameObj, float delay)
    {

        StartCoroutine(DestObj(gameObj,delay));
    }
    public IEnumerator DestObj(Transform gameObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObj.gameObject);
    }

    public int ActualSpeed = 5;
    public int DashMultiplierSpeed = 10;
    public float DashCD = 0.5f;
    public float AttackCD = 0.5f;
    public float AttackDuration = 0.3f;
    public float HpLosePerSecond = 2;

    public override void CheckDeath(Entity Attacker)
    {
        if (Health <= 0)
        {
            Attacker.OnKill(this);
            //GAMEOVER
        }
    }

    public GameObject Model;
    private int Xp;
    private PlayerControler Controler;
    public int GetXp
    {
        get { return Xp; }
    }

    private int XpNextLevel;

    public int GetXpNextLevel
    {
        get { return XpNextLevel; }
    }
    public void OnLevelUp()
    {
        XpNextLevel = 100 + Mathf.CeilToInt(50 *Mathf.Pow(Level,1.5f));
        float HealthRatio = MaxHealth / Health;

        //ImproveStats
        Level++;
        UpdateStats();
        Health = HealthRatio * MaxHealth;

        
        //Player Effect
    }

    public void GainExp(int expToGain)
    {
        Xp += expToGain;
        if (Xp >= XpNextLevel)
        {
            Xp -= XpNextLevel;
            OnLevelUp();
        }
        UIManager.UpdateXP(Xp, XpNextLevel);
    }

    public void UseDash()
    {
        Health -= 3 + MaxHealth*0.02f;
        GetComponent<AudioSource>().Play();
    }

    public override void Update()
    {
        Controler.Update();

        Health -= (HpLosePerSecond * Time.smoothDeltaTime);
        //Health = HP;
        //MaxHealth = MaxHp; 
        UIManager.UpdateHealth(Health, MaxHealth);
        
        //UIManager.ChangeHealthSize(MaxHealth);
    }

}

public class PlayerControler
{
    public PlayerControler(Player owner)
    {
        Owner = owner;
    }

    public Player Owner;
    public bool Dash;
    public Vector3 DashGoal;
    public float LastDashUse = -999;
    public float AttackLastUse = -999;

    public Vector3 lastDirVector = Vector3.zero;

    public Transform Trail;
   
    public void Update()
    {
       

        


        //Mouse Pos

        Vector3 TargetPos = Utility.GetMouseWorldPosition(Owner.transform.position);
        if (TargetPos != Vector3.zero)
        {
            Quaternion TargetRotation = Quaternion.LookRotation(TargetPos - Owner.Model.transform.position);
            Owner.Model.transform.rotation = Quaternion.Slerp(Owner.Model.transform.rotation, TargetRotation, 100 * Time.deltaTime);
        }

        if (!Dash) {
            //Base Movement
            bool Attacking = AttackLastUse + Owner.AttackDuration > Time.timeSinceLevelLoad;
            float XMovement = Input.GetAxis("Horizontal");
            float YMovement = Input.GetAxis("Vertical");
            float SpeedModifier = 1;
            if (Attacking)
            {
                SpeedModifier *= 0.2f;
            }
            

            //Dash

            if (Input.GetButtonDown("Fire2") & LastDashUse + Owner.DashCD <= Time.timeSinceLevelLoad & !Attacking)
            {
                DashGoal = TargetPos;
                Dash = true;
                //================Create Trail========================//

                Owner.UseDash();


                Mesh mesh = new Mesh();
                Transform ChildTransform = Owner.Model.transform;
                mesh.name = "Trail_Mesh";
                Vector3[] Vertice = new Vector3[4];

                Vertice[0] = ChildTransform.position + ChildTransform.right * 0.5f;
                Vertice[1] = ChildTransform.position - ChildTransform.right * 0.5f;
                Vertice[2] = Vertice[0] + ChildTransform.forward*0.1f;
                Vertice[3] = Vertice[1] + ChildTransform.forward * 0.1f;

                mesh.vertices = Vertice;
                int[] triangle = { 0, 1, 2, 1, 3, 2 };
                mesh.triangles = triangle;
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();

                GameObject TrailGO = new GameObject("Trail");
                
                MeshFilter TrailMesh = TrailGO.AddComponent<MeshFilter>();
                TrailMesh.sharedMesh = mesh;
                MeshRenderer renderer = TrailGO.AddComponent<MeshRenderer>();
                MeshCollider colider = TrailGO.AddComponent<MeshCollider>();

                

                colider.convex = true;
                colider.isTrigger = true;
                
                renderer.material = Owner.TrailMat;
                Trail = TrailGO.transform;

                return;
            }

            Vector3 Movement = new Vector3(XMovement, 0, YMovement).normalized * Time.smoothDeltaTime * Owner.GetSpeed * SpeedModifier;
            Owner.transform.position += Movement;
            lastDirVector = Movement;

            if (Input.GetButtonDown("Fire1") & AttackLastUse + Owner.AttackCD < Time.timeSinceLevelLoad)
            {
                Transform AttackVisualisation = Object.Instantiate(Owner.prefab, Owner.transform.position+ Owner.Model.transform.forward, Owner.Model.transform.rotation);
                AttackVisualisation.parent = Owner.Model.transform;
                Owner.DestroyObject(AttackVisualisation, Owner.AttackDuration);
                AttackLastUse = Time.timeSinceLevelLoad;

                DamageTrigger Damager = AttackVisualisation.gameObject.AddComponent<DamageTrigger>();
                Damager.damageOnTouch = Owner.GetDamage;
                Damager.Attacker = Owner;
                Damager.Team = Owner.GetTeam;
            }
        }
        else
        {
            Vector3 DashDirection = DashGoal - Owner.transform.position;
            float CurSpeed = Time.smoothDeltaTime * (Owner.GetSpeed * Owner.DashMultiplierSpeed);
            if (DashDirection.magnitude <= CurSpeed) { 
                CurSpeed = DashDirection.magnitude;
                Dash = false;
                LastDashUse = Time.timeSinceLevelLoad;
                //DestroyTrail
                Owner.DestroyObject(Trail, 0.5f);
                DamageTrigger Damager = Trail.gameObject.AddComponent<DamageTrigger>();
                Damager.damageOnTouch = Mathf.CeilToInt(Owner.GetDamage*1.5f);
                Damager.Attacker = Owner;
                Damager.Team = Owner.GetTeam;
                //Check All Colision With Trail

            }
            DashDirection = DashDirection.normalized;
            Vector3 Movement = DashDirection * CurSpeed;
            Owner.transform.position += Movement;
            Mesh mesh = Trail.GetComponent<MeshFilter>().mesh;
            Transform ChildTransform = Owner.Model.transform;
            //Update Trail Position
            Vector3[] Vertice = mesh.vertices;
            Vertice[2] = ChildTransform.position + ChildTransform.right *0.5f;
            Vertice[3] = ChildTransform.position - ChildTransform.right*0.5f;
            mesh.vertices = Vertice;
            mesh.RecalculateBounds();
            Trail.GetComponent<MeshCollider>().sharedMesh = mesh;
        }

    }

}
