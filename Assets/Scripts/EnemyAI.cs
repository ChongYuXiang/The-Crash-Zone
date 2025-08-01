using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float damageAmount = 15f;
    [SerializeField] private float detectionRadius = 25f;
    [SerializeField] private float knockbackForce = 10f;

    private PlayersCarController targetCar;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        FindClosestPlayerInRange();

        if (targetCar != null)
        {
            Vector3 direction = (targetCar.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        }
    }

    void FindClosestPlayerInRange()
    {
        PlayersCarController[] allPlayers = FindObjectsOfType<PlayersCarController>();
        float shortestDistance = detectionRadius;
        PlayersCarController nearest = null;

        foreach (var player in allPlayers)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= shortestDistance)
            {
                shortestDistance = distance;
                nearest = player;
            }
        }

        targetCar = nearest;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayersCarController car = collision.gameObject.GetComponentInParent<PlayersCarController>();
        if (car != null)
        {
            car.health -= damageAmount;
            car.SendMessage("CheckHealth");
            //AudioManager.instance.PlaySFX("EnemyHit"); sound effect

            // Apply knockback to the car
            Rigidbody carRb = car.GetComponent<Rigidbody>();
            if (carRb != null)
            {
                Vector3 knockbackDir = (car.transform.position - transform.position).normalized;
                carRb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse); 
            }

            // Destroy enemy on hit (optional)
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
