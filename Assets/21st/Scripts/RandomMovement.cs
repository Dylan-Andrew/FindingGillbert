using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MovementState
{
    Moving,
    Stopping,
    Rotating
}


public class RandomMovementBezier : MonoBehaviour
{
    [SerializeField]
    private Vector2 areaSize = new Vector2(5, 5);
    [SerializeField]
    private float minSpeed = 1.0f;
    [SerializeField]
    private float maxSpeed = 5.0f;
    [SerializeField]
    private float rotationSpeed = 5.0f;
    private float speed;
    private Vector3 startPoint, controlPoint, targetPosition;
    private float t = 0f;
    MovementState currentState = MovementState.Moving;
    float stopDuration = 0.01f;
    float stopTimer = 0f;

    void Start()
    {
        SetNewBezierCurve();
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        switch (currentState)
        {
            case MovementState.Moving:
                MoveAlongBezierCurve();
                break;
            case MovementState.Stopping:
                StopAndRotate();
                break;
            case MovementState.Rotating:
                RotateTowardsTarget();
                break;
        }
    }

    void SetNewBezierCurve()
    {
        startPoint = transform.position;

        float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        float randomZ = Random.Range(-areaSize.y / 2, areaSize.y / 2);
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);

        controlPoint = (startPoint + targetPosition) / 2;
        controlPoint += new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));

        t = 0f;
        currentState = MovementState.Stopping;
        stopTimer = 0f;
    }

    void StopAndRotate()
    {
        stopTimer += Time.deltaTime;
        if (stopTimer >= stopDuration)
        {
            currentState = MovementState.Rotating;
        }
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = targetPosition - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                currentState = MovementState.Moving;
            }
        }
    }

    void MoveAlongBezierCurve()
    {
        t += Time.deltaTime * speed / Vector3.Distance(startPoint, targetPosition);

        if (t <= 1f)
        {
            Vector3 bezierPoint = Mathf.Pow(1 - t, 2) * startPoint +
                                  2 * (1 - t) * t * controlPoint +
                                  Mathf.Pow(t, 2) * targetPosition;

            Vector3 direction = bezierPoint - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = rotation;
            }

            transform.position = bezierPoint;
        }
        else
        {
            SetNewBezierCurve();
            speed = Random.Range(minSpeed, maxSpeed);
        }
    }
}
