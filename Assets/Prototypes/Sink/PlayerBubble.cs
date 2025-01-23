using GameInput;
using Unity.Cinemachine;
using Unity.Collections;
using UnityEngine;

public class PlayerBubble : MonoBehaviour
{
    public float PlayerSpeed = 2f;

    public float MouseSensitivity = 100f;

    [SerializeField]
    private CinemachineCamera cinemachineCamera;
    [SerializeField]
    private Transform camTarget;

    private RideableBubble _currentAttachment;

    private Vector2 _currentMovement = Vector2.zero;

    private Vector2 _mouseDelta = Vector2.zero;
    private float xRotation = 0f;


    public float HopTime = 3f;
    private bool _isHopping = false;
    private Vector3 _hopTarget;
    private Vector3 _hopStart;
    private float _hopTimer = 0f;

    void OnEnable() {
        GameInputDelegator.OnMovementChanged += OnMovement;
        GameInputDelegator.OnMouseMove += OnMouseMove;
        GameInputDelegator.OnLeftClick += OnClick;
    }

    void OnDisable() {
        GameInputDelegator.OnMovementChanged -= OnMovement;
        GameInputDelegator.OnMouseMove -= OnMouseMove;
        GameInputDelegator.OnLeftClick -= OnClick;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Shoot ray straight down to find attachment
        var ray = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 100f) ) {
            _currentAttachment = hitInfo.collider.GetComponent<RideableBubble>();
            camTarget.position = _currentAttachment.transform.position;
        }

        // Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {

        if(_isHopping) {
            _hopTimer += Time.deltaTime;
            var t = _hopTimer / HopTime;
            transform.position = Vector3.Lerp(_hopStart,_hopTarget, t);
            camTarget.position = transform.position;

            if(_hopTimer >= HopTime) {
                Debug.Log("finished hopping");
                camTarget.position = _hopTarget;
                _isHopping = false;
            }
            return;
        }

         var rot = _mouseDelta * MouseSensitivity * Time.deltaTime;
         _mouseDelta = Vector2.zero;
         xRotation -= rot.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        // Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // cinemachineCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // transform.Rotate(transform.up, rot.x);




        // var dir = transform.forward * _currentMovement.y + transform.right * _currentMovement.x;
        // var vel = dir.normalized * PlayerSpeed;
        // var newPos = transform.position + (vel * Time.deltaTime);

        // NEW DIR USING CAMERA
        var dir = Camera.main.transform.up * _currentMovement.y + Camera.main.transform.right * _currentMovement.x;
        var vel = dir.normalized * PlayerSpeed;
        var newPos = transform.position + (vel*Time.deltaTime);

        // Snap object to surface
        var normal = (newPos - _currentAttachment.transform.position).normalized;
        var attachPos = _currentAttachment.transform.position + normal * _currentAttachment.Radius;

        transform.position = attachPos;        
        transform.forward = dir;
        transform.up = normal;
        

        // Adjust camera
        float bestDot = -100f;
        Vector3[] dirs = {Vector3.forward, -Vector3.forward, Vector3.up, -Vector3.up, Vector3.right, -Vector3.right };
        Vector3 closest = Vector3.zero;
        for(int i = 0; i < dirs.Length; i++) {
            var dot = Vector3.Dot(dirs[i], normal);
            if(bestDot < dot) {
                bestDot = dot;
                closest = dirs[i];
                // Debug.Log($"Closest - {closest} - {normal} - {dot}");
            }
        }

        camTarget.LookAt(camTarget.transform.position - closest);

    }

    void OnMovement(Vector2 input) {
        _currentMovement = input;
    }

    void OnMouseMove(Vector2 delta) {
        _mouseDelta = delta;
    }

    void OnClick(bool pressed) {
        if(pressed) {

            // Do check for target
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out var hitInfo)) {
                var bubble = hitInfo.collider.GetComponent<RideableBubble>();
                if(bubble) {
                    
                    var pos = hitInfo.point;
                    var normal = pos - bubble.transform.position;
                    
                    _currentAttachment = bubble;
                    _hopTarget = pos;
                    _hopStart = transform.position;
                    _hopTimer = 0f;
                    _isHopping = true;
                    // transform.position = pos;
                    transform.up = normal;

                }
            }

        }
    }



}
