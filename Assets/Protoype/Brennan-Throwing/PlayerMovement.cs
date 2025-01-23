using GameInput;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    public float PlayerSpeed = 1f;

    private Vector2 _input = Vector2.zero;

    void OnEnable() {
        GameInputDelegator.OnMovementChanged += OnMovement;
    }

    void OnDisable() {
        GameInputDelegator.OnMovementChanged -= OnMovement;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var moveDelta = new Vector3(_input.x, 0, 0);

        transform.position += moveDelta * PlayerSpeed * Time.deltaTime;
        transform.right = moveDelta.normalized;
                
    }

    void OnMovement(Vector2 input) {
        _input = input;
    }

}
