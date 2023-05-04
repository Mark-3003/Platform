using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public Transform[] points;
    public float speed;

    private int currentPoint;
    private int nextPoint;

    private float pointDistance;
    private float timer;
    private void Awake()
    {
        currentPoint = 0;
        nextPoint = 1;
        transform.position = points[currentPoint].position;
        pointDistance = CalculateLerpSpeed();
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime * speed;

        if (timer > pointDistance)
            ContinuePoint();

        transform.position = Vector2.Lerp(points[currentPoint].position, points[nextPoint].position, timer / pointDistance);
    }
    void ContinuePoint()
    {
        timer = 0;

        currentPoint++;
        if(currentPoint == points.Length)
        {
            currentPoint = 0;
        }

        nextPoint = currentPoint + 1;
        if (nextPoint == points.Length)
        {
            nextPoint = 0;
        }
        Debug.Log("CurrentPoint = " + currentPoint + ". NextPoint = " + nextPoint);
        pointDistance = CalculateLerpSpeed();
    }
    float CalculateLerpSpeed()
    {
        float _pointDistance = Mathf.Sqrt(Square(points[nextPoint].position.x - points[currentPoint].position.x) + Square(points[nextPoint].position.y - points[currentPoint].position.y));
        return _pointDistance;
    }
    float Square(float _num)
    {
        return _num * _num;
    }
}
