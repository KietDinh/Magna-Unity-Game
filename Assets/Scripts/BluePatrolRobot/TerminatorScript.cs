using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminatorScript : MonoBehaviour
{
    [SerializeField] GameObject projectile;

    public float fireRate = 5f;
    float nextFire;

    // Start is called before the first frame update
    void Start()
    {
        nextFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfTimeToFire();
    }

    void CheckIfTimeToFire()
    {
        if(Time.time > nextFire)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
        }
    }
}
