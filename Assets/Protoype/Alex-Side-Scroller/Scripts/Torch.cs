using Protoype.Alex_Side_Scroller;
using UnityEngine;

public class Torch : MonoBehaviour, ICanBeCaptured, ICanInterface
{
    enum STATE
    {
        NONE,
        IDLE,
        MOVE,
        CAPTURED
    }

    public bool IsCaptured { get; private set; }
    public float MaxIdleTime => maxIdleTime;
    [SerializeField] private float maxIdleTime;

    private Rigidbody2D m_rigidbody2D;
    private Collider2D m_collider2D;

    //Unity Functions
    //============================================================================================================//

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_collider2D = GetComponent<Collider2D>();
    }

    //============================================================================================================//

    public GameObject Capture()
    {
        IsCaptured = true;
        Destroy(gameObject);

        return null;
    }
}
