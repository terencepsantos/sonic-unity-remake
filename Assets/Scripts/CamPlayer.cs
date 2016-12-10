using UnityEngine;
using System.Collections;

public class CamPlayer : MonoBehaviour 
{
    public GameObject Target;


	void LateUpdate () 
    {
        if (Target != null)
        {
            transform.position = new Vector3(
                Target.transform.position.x,
                Target.transform.position.y + 3,
                transform.position.z);
        }
    }

}
