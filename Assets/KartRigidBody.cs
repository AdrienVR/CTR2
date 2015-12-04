
using System;
using UnityEngine;

[Serializable]
public class KartRigidBody
{
    public float MinSpeedToHurt;
    public PlayerController PlayerController;

    [HideInInspector]
    public KartTransformer KartTransformer;

    [HideInInspector]
    public Transform transform;
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 rotationEuler;

    public static int WallLayer = 1 << LayerMask.NameToLayer("Ground");

    // Update is called once per frame
    public void Update ()
    {
        Vector3 direction = position - transform.position;

        if (direction.Equals(Vector3.zero))
        {
            return;
        }

        RaycastHit hitDirection;
        if (Vector3.Dot(direction, transform.forward) > 0)
        {
            if (Physics.Raycast(KartTransformer.FrontLeftWheel.position + Vector3.up * 3, direction, out hitDirection, direction.magnitude, WallLayer))
            {
            }
            if (Physics.Raycast(KartTransformer.FrontRightWheel.position + Vector3.up * 3, direction, out hitDirection, direction.magnitude, WallLayer))
            {
            }
        }
        else
        {
            if (Physics.Raycast(KartTransformer.BottomLeftWheel.position + Vector3.up * 3, direction, out hitDirection, direction.magnitude, WallLayer))
            {
            }
            if (Physics.Raycast(KartTransformer.BottomRightWheel.position + Vector3.up * 3, direction, out hitDirection, direction.magnitude, WallLayer))
            {
            }
        }
	}
    
    private void CheckHurt()
    {

    }

}
