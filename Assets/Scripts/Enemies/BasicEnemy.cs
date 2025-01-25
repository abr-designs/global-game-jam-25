using GGJ.BubbleFall;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using Utilities.Debugging;

public class BasicEnemy : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float fallMultiplier = 2f;


    private Rigidbody2D _rb;
    private Collider2D _coll;
    private Actor _actor;

    private int _moveDir;

    // Collision check
    private bool _isGrounded;
    private RaycastHit2D _leftGrounded;
    private RaycastHit2D _leftWallHit;

    private RaycastHit2D _rightGrounded;
    private RaycastHit2D _rightWallHit;


    // Move vars

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        _actor = GetComponent<Actor>();

        // Start with random direction
        _moveDir = Random.Range(0, 2) * 2 - 1; // Returns -1 or 1
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        CollisionChecks();
        Move();
    }

    private void Move()
    {
        // We are inert when captured
        if (_actor.IsCaptured) return;

        // If not grounded we apply fall gravity
        if (!_isGrounded)
        {
            _rb.linearVelocityY += Physics2D.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
        else
        {
            _rb.linearVelocityY = 0f;
        }

        // Only move left/right if we are grounded
        if (_isGrounded)
        {
            if (_leftWallHit || !_leftGrounded)
                _moveDir = 1;
            else if (_rightWallHit || !_rightGrounded)
                _moveDir = -1;
            else if (_rb.linearVelocityX == 0f)
            {
                // Special case -- we are probably stuck on another actor
                _moveDir *= -1;
            }

            _rb.linearVelocityX = _moveDir * moveSpeed;
        }
        else
        {
            _rb.linearVelocityX = 0f;
        }

    }

    private void CollisionChecks()
    {
        var leftSide = new Vector2(_coll.bounds.min.x, _coll.bounds.center.y);
        var rightSide = new Vector2(_coll.bounds.max.x, _coll.bounds.center.y);

        float colliderExtent = 0.1f;

        // cast down
        _leftGrounded = Physics2D.Raycast(leftSide, Vector2.down, _coll.bounds.extents.y + colliderExtent, groundLayerMask);
        _rightGrounded = Physics2D.Raycast(rightSide, Vector2.down, _coll.bounds.extents.y + colliderExtent, groundLayerMask);

        _isGrounded = _leftGrounded || _rightGrounded;

        // check walls
        var boxCastSize = new Vector2(colliderExtent, _coll.bounds.size.y * 0.8f);
        _leftWallHit = Physics2D.BoxCast(leftSide, boxCastSize, 0f, Vector2.left, colliderExtent, groundLayerMask);
        _rightWallHit = Physics2D.BoxCast(rightSide, boxCastSize, 0f, Vector2.right, colliderExtent, groundLayerMask);
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        var vel = _rb == null ? Vector2.zero : _rb.linearVelocity;
        Draw.Label(transform.position, $"Grounded: {_isGrounded} \n Velocity: {vel} \n MoveDir: {_moveDir} \n LeftGround: {_leftGrounded} RightGround: {_rightGrounded}");

    }

#endif


}
