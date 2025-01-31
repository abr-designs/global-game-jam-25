using Unity.VisualScripting;
using UnityEngine;
using Utilities.Debugging;
using Utilities.Tweening;

// A platform that moves between two points

public class SpinningPlatform : MonoBehaviour
{
    // Speed in degrees per second
    public float RotationSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back, RotationSpeed * Time.deltaTime);

    }

}