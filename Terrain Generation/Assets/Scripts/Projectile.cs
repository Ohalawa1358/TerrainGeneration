using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float startingVelocity;

    public float bulletLifeTime = 2f;
    public float bulletDamage = 10f;
    private float startBulletLifeTimer;

    // Update is called once per frame
    private void Start()
    {
        startBulletLifeTimer = bulletLifeTime;
    }

    void Update()
    {
        transform.position += transform.forward * startingVelocity * Time.deltaTime;

        bulletLifeTime -= Time.deltaTime;
        if (bulletLifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().takeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }

}
