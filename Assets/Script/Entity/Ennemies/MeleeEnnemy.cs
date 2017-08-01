using System.Collections;
using UnityEngine;

public class MeleeEnnemy : BaseEnnemy
{

    Material invinsible;


    public override void Attack(Vector3 position)
    {
        base.Attack(position);


        Mesh mesh = new Mesh();
        mesh.name = "AttackDectection";
        Vector3[] Vertice = new Vector3[4];

        Vertice[0] = transform.position + transform.right * 0.5f;
        Vertice[1] = transform.position - transform.right * 0.5f;
        Vertice[2] = Vertice[0] + transform.forward * AttackRange;
        Vertice[3] = Vertice[1] + transform.forward * AttackRange;

        mesh.vertices = Vertice;
        int[] triangle = { 0, 1, 2, 1, 3, 2 };
        mesh.triangles = triangle;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GameObject ADGO = new GameObject("Attack Detection");

        ADGO.AddComponent<MeshFilter>().sharedMesh = mesh; //FUCK THE POLICE !


        ADGO.AddComponent<MeshRenderer>().material = invinsible;
        MeshCollider colider = ADGO.AddComponent<MeshCollider>();



        colider.convex = true;
        colider.isTrigger = true;

        Transform AttackVisualisation = ADGO.transform;
        AttackVisualisation.parent = transform;
        StartCoroutine(DestObj(AttackVisualisation, AttackDuration));

        DamageTrigger Damager = AttackVisualisation.gameObject.AddComponent<DamageTrigger>();
        Damager.damageOnTouch = GetDamage;
        Damager.Attacker = this;
        Damager.Team = GetTeam;
    }
}
