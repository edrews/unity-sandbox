using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour
{
    void OnCollisionEnter (Collision col)
    {
	Debug.Log(col.gameObject.name);
    }
}
