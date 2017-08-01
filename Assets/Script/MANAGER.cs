using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;




public class MANAGER : MonoBehaviour {


    static public Player player;
    static public List<BaseEnnemy> Enemies = new List<BaseEnnemy>();
    static public MANAGER CurManger;

    static public Vector3 GetPlayerPos{
        get { return player.Model.transform.position; }
    }

    static public void AddEnemyToList(BaseEnnemy ennemy){
        Enemies.Add(ennemy);
    }

    static public void RemoveEnemyToList(BaseEnnemy ennemy)
    {
        Enemies.Remove(ennemy);
    }

    static public bool CanGetAgressive(int max){
        int amm = 0;
        if (Enemies.Count <= max){
            return true;
        }
        for (int i = 0; i < Enemies.Count; i++){
            if (Enemies[i].getState == 2)
                amm++;
            if (amm > max)
                return false;
        }
        return true;
    }

    public int MinEnnemyAmmounts = 3;
    public float NewEnnemyEveryMinute = 1;
    
    public Transform ennemy;
   // Round CurrentRound;

    private void Awake()
    {
        Enemies = new List<BaseEnnemy>();
        Debug.Log("Test");
    }


    private void Update()
    {
        if (player.GetHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
        if (MANAGER.Enemies.Count < MinEnnemyAmmounts + Mathf.CeilToInt(NewEnnemyEveryMinute * Time.timeSinceLevelLoad / 60))
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        float Xpos = Random.Range(3, 10);
        if (Random.Range(0, 10) >= 5)
        {
            Xpos *= -1;
        }
            float Ypos = Random.Range(7, 15);
        if (Random.Range(0, 10) >= 5)
        {
            Ypos *= -1;
        }
        Vector3 Position = new Vector3(Xpos, 0, Ypos) + player.transform.position ;
        Transform actualEnnemy = Instantiate(ennemy, new Vector3(Position.x,0, Position.z), Quaternion.identity);

        TESTENNEMY enScript = actualEnnemy.GetComponent<TESTENNEMY>();
        enScript.level = player.GetLevel+2;

        if (Random.Range(0,10) >= 9)
        {
            actualEnnemy.localScale = actualEnnemy.localScale * 2.5f;
            enScript.level = Mathf.CeilToInt(player.GetLevel * 1.5f + 3);
        }
        enScript.Init();
    }


    /*
    private void Update()
    {
        if (IsRoundFinished())
        {
            StartRound();
        }
    }

    bool IsRoundFinished()
    {
        return CurrentRound.IsRoundDone();
    }
    void StartRound(Round round)
    {
        CurrentRound = round;
        CurrentRound.SpawnEnemies();
    }

    public IEnumerator SpawnEnemy(float delay)
    {
        yield return new WaitForSeconds(delay);
        //pawn Ennemy
    }
    */
}
/*
class Round
{
    
    public EnnemyRound[] enemies;
    
    public Round(int Ammounts, int Level)
    {
        new 
    }

    public bool IsRoundDone()
    {
        for (int i = 0; i< enemies.Length; i++)
        {
            if (enemies[i].ammounts > 0)
                return false;
        }
        return true;
    }
    public void SpawnEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            for (int j = 0; j < enemies[i].ammounts; j++)
            {
                MANAGER.CurManger.SpawnEnemy(enemies[i].BaseTIme + enemies[i].Delay * j);
            }
        }
    }
    
}

class EnnemyRound
{
    public float Delay;
    public float BaseTIme;
    Transform Ennemy;
    public int ammounts;
}*/
