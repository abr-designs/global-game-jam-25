using UnityEngine;

public class Bubble : MonoBehaviour
{

    private float _lifeTime = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = Vector3.one * Random.Range(.5f,1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        _lifeTime -= Time.deltaTime;
        if(_lifeTime < 0)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision) {
        
    }
}
