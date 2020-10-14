using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float currHealth = 100f;

    private void Update()
    {
        if (currHealth <= 0.0f)
        {
            gameObject.GetComponent<ZombieAI>().isDead = true;
        }
    }

    public void takeDamage(float damage)
    {
        currHealth -= damage;
        Debug.Log(currHealth);
    }
}
