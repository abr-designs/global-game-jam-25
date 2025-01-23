using UnityEngine;

public class RideableBubble : MonoBehaviour
{
    public float Radius = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.localScale = Vector3.one * Radius * 2f;
        
    }
}
