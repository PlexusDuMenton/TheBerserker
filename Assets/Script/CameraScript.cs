using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Player Folowed;
    public float Speed = 0.1f;
    public float ZSpeed = 0.1f;
    public float DistanceFront =2;
    public float DistanceMove = 3;

    public float BaseHeight = 10;
    public float MaxBonusHeight = 10;

    // Update is called once per frame
    void Update () {
        Vector3 PositionGoal = Folowed.transform.position + DistanceFront * Folowed.Model.transform.forward + DistanceMove*Folowed.GetDirection;

        Vector3 NextFramePosition = Vector3.Lerp(transform.position, PositionGoal, Speed*Time.smoothDeltaTime);
        float height = BaseHeight + Mathf.Clamp(Vector3.Distance(new Vector3(PositionGoal.x,0, PositionGoal.z), new Vector3(transform.position.x,0, transform.position.z))*3,0,MaxBonusHeight);

        float finalheight = Mathf.Lerp(transform.position.y, height, ZSpeed * Time.smoothDeltaTime);
        transform.position = new Vector3(NextFramePosition.x, finalheight, NextFramePosition.z);
	}
}
