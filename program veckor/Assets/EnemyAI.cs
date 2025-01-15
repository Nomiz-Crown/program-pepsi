using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Spelarens transform
    public float chaseSpeed = 3f;
    public float detectionRange = 5f;
    public float stopDistance = 1f;
    public float raycastDistance = 2f; // Distans f�r raycast f�r att uppt�cka hinder
    public LayerMask obstacleLayer; // Lager f�r hinder (h�r ignorerar vi spelaren)

    // Waypoints f�r patrullering
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private bool isPatrolling = false;

    private bool isChasing = false;
    private Movement playerMovement; // Referens till spelarens r�relseskript

    private bool isWaiting = false; // Flagga f�r att kontrollera om fienden v�ntar
    private float waitTime = 5f; // Tiden fienden ska v�nta innan den b�rjar patrullera

    void Start()
    {
        // H�mta spelarens Movement-komponent
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
            isPatrolling = false; // Stopp patrullering n�r fienden b�rjar jaga
            Debug.Log("Fienden har b�rjat jaga spelaren!");
            // Uppdatera spelarens �ngestsystem
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
            // Sluta p�verka spelarens �ngestsystem
            if (playerMovement != null)
            {
                playerMovement.isBeingChased = false;
            }

            // B�rja v�nta i 5 sekunder och kolla om fienden fortfarande ska jaga
            if (!isWaiting)
            {
                StartCoroutine(WaitAndCheckChaseStatus());
            }
        }
    }

    IEnumerator WaitAndCheckChaseStatus()
    {
        isWaiting = true;

        // V�nta i 5 sekunder
        yield return new WaitForSeconds(waitTime);

        // Kolla om spelaren fortfarande �r inom r�ckh�ll
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > detectionRange)
        {
            // Om inte, b�rja patrullera fr�n waypoint 0
            Debug.Log("Spelaren �r inte l�ngre inom r�ckh�ll, b�rjar patrullera.");
            transform.position = waypoints[0].position; // Flytta tillbaka till waypoint 0
            StartPatrol();
        }
        else
        {
            // Om spelaren fortfarande �r i r�ckh�ll, b�rja jaga igen
            Debug.Log("Fienden b�rjar jaga spelaren igen.");
            StartChase();
        }

        isWaiting = false;
    }

    void ChasePlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > stopDistance)
        {
            // Kontrollera f�r hinder framf�r fienden med raycast, ignorerar spelaren
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
                // Fienden ska nu flytta bort fr�n hindret
                Vector2 avoidDirection = Vector2.Perpendicular(hit.normal); // Vinkelr�tt riktning f�r att undvika hindret
                Vector3 newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)avoidDirection, chaseSpeed * Time.deltaTime);
                newPosition.z = 2; // Set Z to 2
                transform.position = newPosition;
            }
        }
    }

    void StartPatrol()
    {
        isPatrolling = true;
        currentWaypointIndex = 0; // B�rja vid den f�rsta waypointen
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return; // Kontrollera om det finns n�gra waypoints

        // R�ra sig mot nuvarande waypoint
        Vector2 targetPosition = waypoints[currentWaypointIndex].position;

        // Kontrollera f�r hinder med raycast i alla fyra riktningar (upp, ner, v�nster, h�ger)
        RaycastHit2D frontHit = Physics2D.Raycast(transform.position, (targetPosition - (Vector2)transform.position).normalized, raycastDistance, obstacleLayer);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0, 2), (targetPosition - (Vector2)transform.position).normalized, raycastDistance, obstacleLayer); // V�nster
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0, 2), (targetPosition - (Vector2)transform.position).normalized, raycastDistance, obstacleLayer); // H�ger
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
            // Om hinder uppt�cks, v�lj mellan v�nster, h�ger, upp eller ner
            if (leftHit.collider == null) // Om v�nster inte �r blockerad
            {
                Vector2 avoidDirection = Vector2.left; // Flytta �t v�nster
                Vector3 newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)avoidDirection, chaseSpeed * Time.deltaTime);
                newPosition.z = 2; // Set Z to 2
                transform.position = newPosition;
            }
            else if (rightHit.collider == null) // Om h�ger inte �r blockerad
            {
                Vector2 avoidDirection = Vector2.right; // Flytta �t h�ger
                Vector3 newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)avoidDirection, chaseSpeed * Time.deltaTime);
                newPosition.z = 2; // Set Z to 2
                transform.position = newPosition;
            }
            else if (upHit.collider == null) // Om upp inte �r blockerad
            {
                Vector2 avoidDirection = Vector2.up; // Flytta upp�t
                Vector3 newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)avoidDirection, chaseSpeed * Time.deltaTime);
                newPosition.z = 2; // Set Z to 2
                transform.position = newPosition;
            }
            else if (downHit.collider == null) // Om ner inte �r blockerad
            {
                Vector2 avoidDirection = Vector2.down; // Flytta ned�t
                Vector3 newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)avoidDirection, chaseSpeed * Time.deltaTime);
                newPosition.z = 2; // Set Z to 2
                transform.position = newPosition;
            }
            else
            {
                // Om alla riktningar �r blockerade, f�rs�k att flytta bort fr�n hindret (undvikande)
                Vector2 avoidDirection = Vector2.Perpendicular(frontHit.normal); // Vinkelr�tt riktning f�r att undvika hindret
                Vector3 newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)avoidDirection, chaseSpeed * Time.deltaTime);
                newPosition.z = 2; // Set Z to 2
                transform.position = newPosition;
            }
        }

        // Om AI'n har n�tt waypointen, g� vidare till n�sta
        if (Vector2.Distance(transform.position, targetPosition) < 0.2f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length) // Om vi har n�tt slutet p� listan, b�rja om
            {
                currentWaypointIndex = 0;
            }
        }
    }
}