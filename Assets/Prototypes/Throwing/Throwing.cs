using System;
using GameInput;
using Unity.VisualScripting;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    public enum THROW_TYPE {
        Fixed,
        Charged
    }

    public THROW_TYPE ThrowType = THROW_TYPE.Fixed;

    // Line renderer to draw the trajectory trail
    [SerializeField]
    private LineRenderer lineRendererPrefab;
    private LineRenderer _throwIndicator;

    // Plane the character exists on -- use this for mouse cursor intersections
    private Plane _xyPlane = new Plane(Vector3.forward, Vector3.zero);


    [SerializeField]
    private GameObject reticle;
    public int LinePoints = 20;
    public float LineTimeStep = 0.1f;
    public float ThrowSpeed = 1f;
    public float BubbleGravity = -9.8f;

    public float ThrowCooldown = 0.1f;
    private float _throwingTimer = 0f;

    // How many seconds to ramp to full charge
    public float ThrowChargeSpeed = 0.5f;
    private float _chargeStrength = 0;

    private bool _isLeftButtonDown = false;


    private Vector3 _currentThrowVector = Vector3.zero;

    [SerializeField]
    private GameObject bubblePrefab;

    void OnEnable() 
    {
        GameInputDelegator.OnLeftClick += OnClick;
    }
    void OnDisable() 
    {
        GameInputDelegator.OnLeftClick -= OnClick;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(_throwIndicator == null)
            _throwIndicator = Instantiate<LineRenderer>(lineRendererPrefab);
    }

    // Update is called once per frame
    void Update()
    {

        if(_throwingTimer > 0)
            _throwingTimer -= Time.deltaTime;
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(_xyPlane.Raycast(ray, out float distance )) {
            var hitPoint = ray.GetPoint(distance);
            if(reticle)
            {
                reticle.transform.position = hitPoint;
            }

            _throwIndicator.positionCount = LinePoints;
            var direction = hitPoint - transform.position;
            _currentThrowVector = direction.normalized;

            float _throwSpeed = ThrowType == THROW_TYPE.Fixed ? ThrowSpeed : ThrowSpeed * _chargeStrength;

            Vector3 vel = _currentThrowVector * _throwSpeed;
            Vector3 pos = transform.position;
            _throwIndicator.SetPosition(0, pos);
            for(int i=1; i<LinePoints; i++) {
                vel = vel + (Vector3.down * BubbleGravity * LineTimeStep);
                pos += vel * LineTimeStep;
                _throwIndicator.SetPosition(i,pos);
            }
        }
        
        if(ThrowType == THROW_TYPE.Fixed)
        {
    
            if(_isLeftButtonDown && _throwingTimer <= 0)
            {
                // Shoot projectile
                var bubble = Instantiate(bubblePrefab);
                bubble.transform.position = transform.position;
                bubble.GetComponent<Rigidbody>().linearVelocity = _currentThrowVector * ThrowSpeed;

                _throwingTimer = ThrowCooldown;
            }


        } else if (ThrowType == THROW_TYPE.Charged) {
            
            if(_isLeftButtonDown && _throwingTimer <= 0)
            {   
                _chargeStrength = Mathf.Clamp01(_chargeStrength + Time.deltaTime * ThrowChargeSpeed);
                
            } else if(!_isLeftButtonDown && _chargeStrength > 0) {
                // Shoot projectile
                var bubble = Instantiate(bubblePrefab);
                bubble.transform.position = transform.position;
                bubble.GetComponent<Rigidbody>().linearVelocity = _currentThrowVector * ThrowSpeed * _chargeStrength;

                _throwingTimer = ThrowCooldown;
                _chargeStrength = 0;
            }
        }

        

    }

    void OnClick(bool pressed) {

        _isLeftButtonDown = pressed;


    }
}
