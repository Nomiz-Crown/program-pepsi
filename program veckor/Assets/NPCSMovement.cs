using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 startPosition;
    Vector2 targetPosition;
    bool isMoving = false;
    float speed = 2f; // Movement speed
    float waitTime = 5f; // Wait time between movements
    float moveDistance = 2f; // Move distance in Unity units

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            ChooseNewDirection();
            yield return new WaitUntil(() => isMoving == false);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void ChooseNewDirection()
    {
        int direction = Random.Range(1, 5); // 1-Up, 2-Right, 3-Left, 4-Down
        startPosition = transform.position;

        switch (direction)
        {
            case 1: // Up
                targetPosition = startPosition + Vector2.up * moveDistance;
                break;
            case 2: // Right
                targetPosition = startPosition + Vector2.right * moveDistance;
                break;
            case 3: // Left
                targetPosition = startPosition + Vector2.left * moveDistance;
                break;
            case 4: // Down
                targetPosition = startPosition + Vector2.down * moveDistance;
                break;
        }

        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        isMoving = true;

        while ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}