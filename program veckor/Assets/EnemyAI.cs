using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Spelarens transform
    public float chaseSpeed = 3f;
    public float detectionRange = 5f;
    public float stopDistance = 1f;
    public float raycastDistance = 2f; // Distans för raycast för att upptäcka hinder
    public LayerMask obstacleLayer; // Lager för hinder (här ignorerar vi spelaren)

    // Waypoints för patrullering
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private bool isPatrolling = false;

    private bool isChasing = false;
    private Movement playerMovement; // Referens till spelarens rörelseskript

    private bool isWaiting = false; // Flagga för att kontrollera om fienden väntar
    private float waitTime = 5f; // Tiden fienden ska vänta innan den börjar patrullera

    void Start()
    {
        // Hämta spelarens Movement-komponent
        playerMovement = player.GetComponent<Movement>();

        if (waypoints.Length > 0)
        {
            isPatrolling = true; // Starta patrullering om det finns waypoints
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            StartChase();
        }
        else
        {
            StopChase();
            if (!isPatrolling)
            {
                StartPatrol();
            }
        }

        if (isChasing)
        {
            ChasePlayer(distanceToPlayer);
        }

        if (isPatrolling)
        {
            Patrol();
        }
    }

    void StartChase()
    {
        if (!isChasing)
        {
            isChasing = true;
            isPatrolling = false; // Stopp patrullering när fienden börjar jaga
            Debug.Log("Fienden har börjat jaga spelaren!");
            // Uppdatera spelarens ångestsystem
            if (playerMovement != null)
            {
                playerMovement.isBeingChased = true;
            }
        }
    }

    void StopChase()
    {
        if (isChasing)
        {
            isChasing = false;
            Debug.Log("Fienden slutade jaga spelaren.");
            // Sluta påverka spelarens ångestsystem
            if (playerMovement != null)
            {
                playerMovement.isBeingChased = false;
            }

            // Börja vänta i 5 sekunder och kolla om fienden fortfarande ska jaga
            if (!isWaiting)
            {
                StartCoroutine(WaitAndCheckChaseStatus());
            }
        }
    }

    IEnumerator WaitAndCheckChaseStatus()
    {
        isWaiting = true;

        // Vänta i 5 sekunder
        yield return new WaitForSeconds(waitTime);

        // Kolla om spelaren fortfarande är inom räckhåll
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > detectionRange)
        {
            // Om inte, börja patrullera från waypoint 0
            Debug.Log("Spelaren är inte längre inom räckhåll, börjar patrullera.");
            transform.position = waypoints[0].position; // Flytta tillbaka till waypoint 0
            StartPatrol();
        }
        else
        {
            // Om spelaren fortfarande är i räckhåll, börja jaga igen
            Debug.Log("Fienden börjar jaga spelaren igen.");
            StartChase();
        }

        isWaiting = false;
    }

    void ChasePlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > stopDistance)
        {
            // Kontrollera för hinder framför fienden med raycast, ignorerar spelaren
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.position - transform.position).normalized, raycastDistance, obstacleLayer);

            if (hit.collider == null) // Om inget hinder detekteras
            {
                Vector2 direction = (player.position - transform.position).normalized;
                Vector3 newPosition = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
                newPosition.z = 0; // Set Z to 2
                transform.position = newPosition;
            }
            else
            {
                // Fienden ska nu flytta bort från hindret
                Vector2 avoidDirection = Vector2.Perpendicular(hit.normal); // Vinkelrätt riktning för att undvika hindret
                Vector3 newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)avoidDirection, chaseSpeed * Time.deltaTime);
                newPosition.z = 2; // Set Z to 2
                transform.position = newPosition;
            }
        }
    }

    void StartPatrol()
    {
        isPatrolling = true;
        currentWaypointIndex = 0; // Börja vid den första waypointen
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return; // Kontrollera om det finns några waypoints

        // Röra sig mot nuvarande waypoint
        Vector2 targetPosition = waypoints[currentWaypointIndex].position;

        // Kontrollera för hinder med raycast i alla fyra riktningar (upp, ner, vänster, höger)
        RaycastHit2D frontHit = Physics2D.Raycast(transform.position, (targetPosition - (Vector2)transform.position).normalized, raycastDistance, obstacleLayer);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0, 2), (targetPosition - (Vector2)transform.position).normalized, raycastDistance, obstacleLayer); // Vänster
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0, 2), (targetPosition - (Vector2)transform.position).normalized, raycastDistance, obstacleLayer); // Höger
        RaycastHit2D upHit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 2), (targetPosition - (Vector2)transform.position).normalized, raycastDistance, obstacleLayer); // Upp
        RaycastHit2D downHit = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f, 2), (targetPosition - (Vector2)transform.position).normalized, raycastDistance, obstacleLayer); // Ner

        // Om inget hinder detekteras rakt fram
        if (frontHit.collider == null)
        {
            Vector3 newPosition = Vector2.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);
            newPosition.z = 2; // Set Z to 2
            transform.position = newPosition;
        }
        else
        {
            // Om hinder upptäcks, välj mellan vänster, höger, upp eller ner
            if (leftHit.collider == null) // Om vänster inte är blockerad
            {
                Vector2 avoidDirection = Vector2.left; // Flytta åt vänster
                Vector3 newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)avoidDirection, chaseSpeed * Time.deltaTime);
                newPosition.z = 2; // Set Z to 2
                transform.position = newPosition;
            }
            else if (rightHit.collider == null) // Om höger inte är blockerad
            {
                Vector2 avoidDirection = Vector2.right; // Flytta åt höger
                Vector3 newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)avoidDirection, chaseSpeed * Time.deltaTime);
                newPosition.z = 2; // Set Z to 2
                transform.position = newPosition;
            }
            else if (upHit.collider == null) // Om upp inte är blockerad
            {
                Vector2 avoidDirection = Vector2.up; // Flytta uppåt
                Vector3 newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)avoidDirection, chaseSpeed * Time.deltaTime);
                newPosition.z = 2; // Set Z to 2
                transform.position = newPosition;
            }
            else if (downHit.collider == null) // Om ner inte är blockerad
            {
                Vector2 avoidDirection = Vector2.down; // Flytta nedåt
                Vector3 newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)avoidDirection, chaseSpeed * Time.deltaTime);
                newPosition.z = 2; // Set Z to 2
                transform.position = newPosition;
            }
            else
            {
                // Om alla riktningar är blockerade, försök att flytta bort från hindret (undvikande)
                Vector2 avoidDirection = Vector2.Perpendicular(frontHit.normal); // Vinkelrätt riktning för att undvika hindret
                Vector3 newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)avoidDirection, chaseSpeed * Time.deltaTime);
                newPosition.z = 2; // Set Z to 2
                transform.position = newPosition;
            }
        }

        // Om AI'n har nått waypointen, gå vidare till nästa
        if (Vector2.Distance(transform.position, targetPosition) < 0.2f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length) // Om vi har nått slutet på listan, börja om
            {
                currentWaypointIndex = 0;
            }
        }
    }
}