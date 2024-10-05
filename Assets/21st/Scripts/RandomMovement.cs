using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovementBezier : MonoBehaviour
{
    [SerializeField]
    private Vector2 areaSize = new Vector2(5, 5);
    [SerializeField]
    private float minSpeed = 0.2f;
    [SerializeField]
    private float maxSpeed = 1.0f;
    private float speed;
    private Vector3 startPoint, controlPoint, targetPosition;
    private float t = 0f;

    void Start()
    {
        SetNewBezierCurve();
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        MoveAlongBezierCurve();
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
    }

    void MoveAlongBezierCurve()
    {
        t += Time.deltaTime * speed / Vector3.Distance(startPoint, targetPosition);

        if (t <= 1f)
        {
            Vector3 bezierPoint = Mathf.Pow(1 - t, 2) * startPoint +
                                  2 * (1 - t) * t * controlPoint +
                                  Mathf.Pow(t, 2) * targetPosition;

            transform.position = bezierPoint;
        }
        else
        {
            SetNewBezierCurve();
            speed = Random.Range(minSpeed, maxSpeed);
        }
    }
}
