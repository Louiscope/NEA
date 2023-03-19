using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS IS PARTLY TUTORIAL -- NOT ALL MY CODE
public class Gun : MonoBehaviour
{
    // Sets range, camera location, firerate and counter for next shot
    public float range = 100f;
    public Camera cam;
    public float FireRate = 5f;
    public float nextShot = 0f;
    
    // Recieves inputs and fires -- nextshot set to recip of firerate added to time (5 shots per second is 1/5 seconds per shot)
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextShot)
        {
            nextShot = Time.time + 1f / FireRate;
            fire();
        }
    }
    // Fires a ray to find the object being looked at, if true and object is enemy, deal damage
    void fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            EnemyAiTutorial Enemy = hit.transform.GetComponent<EnemyAiTutorial>();
            if (Enemy != null)
            {
                Enemy.TakeDamage(2);
            }
        }
    }
}
