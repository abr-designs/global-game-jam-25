using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Utilities.Debugging;
using Utilities.Tweening;

// A platform that moves between two points

public class MovingPlatform : MonoBehaviour
{
    public Transform Point1;
    public Transform Point2;

    public float MoveTime = 1f;

    private Transform currentTarget;
    private AnimationCurve _curve;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        transform.position = Point1.position;
        currentTarget = Point2;
        StartCoroutine(MoveCoroutine(currentTarget, MoveTime));
    }

    void OnMoveDone()
    {
        currentTarget = currentTarget == Point1 ? Point2 : Point1;
        StartCoroutine(MoveCoroutine(currentTarget, MoveTime));

    }

    private IEnumerator MoveCoroutine(Transform target, float duration)
    {
        float elapsedTime = 0f;

        Vector3 start = transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            t = Mathf.SmoothStep(0f, 1f, t); // ease-in-out
            transform.position = Vector3.Lerp(start, target.position, t);
            yield return null;
        }

        transform.position = target.position;
        OnMoveDone();

    }

    // Update is called once per frame
    void Update()
    {
        Draw.Circle(Point1.position, Color.magenta, 1f);
        Draw.Circle(Point2.position, Color.magenta, 1f);

    }

}
