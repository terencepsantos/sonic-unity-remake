using UnityEngine;
using System.Collections;

public class CamPlayer : MonoBehaviour 
{
    public GameObject Target;


	void LateUpdate () 
    {
        transform.position = new Vector3(
            Target.transform.position.x,
            Target.transform.position.y,
            transform.position.z);
	}
}
