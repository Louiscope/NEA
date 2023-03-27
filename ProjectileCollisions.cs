using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisions : MonoBehaviour
{
    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
        
    }
}
