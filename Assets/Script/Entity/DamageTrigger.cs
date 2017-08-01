using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTrigger : MonoBehaviour {

    public int damageOnTouch;
    public Human Attacker;
    public int Team; // 0 = player , 1 = Ennemy

	void OnTriggerEnter(Collider OtherCollider) {
        Entity TrigEntity = OtherCollider.GetComponent<Entity>();
        if (TrigEntity == null)
            return;
        if (Team != TrigEntity.GetTeam)
        {
            
            Attacker.OnHit(TrigEntity);
            int damagedone = TrigEntity.ReceiveAttack(damageOnTouch, Attacker);

            GameObject DamageHit = new GameObject("Damage Hit");
            DamageHit.transform.SetParent(GameObject.Find("Canvas").transform);
            DamageText Script = DamageHit.AddComponent<DamageText>();
            Script.Init(damagedone, OtherCollider.transform.position);


            //Create Hit
        }

    }

    private void Update()
    {
        
    }
}

public class DamageText : MonoBehaviour
{
    float CreationTime;
    Vector3 Pos;
    public void Init(int damagedone,Vector3 pos)
    {
        Text text = gameObject.AddComponent<Text>();
        text.text = "- " + damagedone;
        Pos = pos;
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.font = ArialFont;
        text.material = ArialFont.material;
        text.fontSize = Mathf.Clamp( Mathf.CeilToInt( 20 * Mathf.Log10(damagedone*5)),0,50);
        text.color = new Color(1, 0.5f, 0,1);

        Outline outline = gameObject.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(1, 1);
        Vector3 ScreenPosition = Camera.main.WorldToScreenPoint(Pos);
        GetComponent<RectTransform>().position = ScreenPosition;
        CreationTime = Time.timeSinceLevelLoad;
    }

    public void Update()
    {

        Vector3 ScreenPosition = Camera.main.WorldToScreenPoint(Pos);
        GetComponent<RectTransform>().position = ScreenPosition + Vector3.up*(Time.timeSinceLevelLoad-CreationTime)*100;


        if (CreationTime +0.5f <= Time.timeSinceLevelLoad)
        {

            if (CreationTime + 1.5f <= Time.timeSinceLevelLoad)
                Destroy(gameObject);

           GetComponent<Text>().color = new Color(1, 0.5f, 0, (CreationTime + 1.5f) - Time.timeSinceLevelLoad);
            GetComponent<Outline>().effectColor = new Color(0,0, 0, (CreationTime + 1.5f) - Time.timeSinceLevelLoad);
        }
    }
}

