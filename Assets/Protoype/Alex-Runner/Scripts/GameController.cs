using System;
using UnityEngine;
using Utilities.Debugging;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    [SerializeField, Min(0f)]private float levelRadius;
    [SerializeField, Min(0f)]private float playerZDepth;

    [SerializeField, Header("Controller")] 
    private bool useClickController;

    private Vector3 m_playerPlanePos;
    private Vector3 m_worldMousePos;
    
    private Camera m_mainCamera;

    private float playerRadius;


    //Unity Functions
    //============================================================================================================//
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        m_playerPlanePos = Vector3.forward * playerZDepth;
        m_mainCamera = Camera.main;

        playerTransform.position = m_playerPlanePos + Vector3.down * levelRadius;
        playerRadius = playerTransform.localScale.x/2f;
    }

    // Update is called once per frame
    private void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = playerZDepth;
        
        m_worldMousePos = m_mainCamera.ScreenToWorldPoint(mousePos);
        
        Draw.Circle(m_worldMousePos, Color.magenta, 0.1f);

        if (useClickController)
            ClickController();
        else
            MouseFollow();
    }

    private void ClickController()
    {
        var dir = Vector3.ProjectOnPlane(m_worldMousePos, Vector3.forward);
        Draw.Arrow(m_playerPlanePos, -dir.normalized * levelRadius, Color.green);
        Draw.Arrow(m_playerPlanePos + (dir.normalized * levelRadius), -dir.normalized * levelRadius, Color.red);
        
        Draw.Circle(m_playerPlanePos - dir.normalized * levelRadius, Color.red, playerRadius);
        
        if(Input.GetKeyDown(KeyCode.Mouse0))
            playerTransform.position = m_playerPlanePos - dir.normalized * levelRadius;
    }

    private void MouseFollow()
    {
        var dir = Vector3.ProjectOnPlane(m_worldMousePos, Vector3.forward).normalized;
        Draw.Arrow(m_playerPlanePos, dir * levelRadius, Color.green);
        Draw.Circle(m_playerPlanePos + dir * levelRadius, Color.red, playerRadius);

        playerTransform.position = m_playerPlanePos + dir * levelRadius;
    }
    

    //Unity Editor Functions
    //============================================================================================================//

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Draw.Circle(Vector3.forward * playerZDepth, Color.yellow, levelRadius);
    }

#endif
    
}
