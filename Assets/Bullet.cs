using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Dagger>())
        {
            float force = 10f;
            Vector3 dir = collision.contacts[0].point - target.position;
            dir = -dir.normalized;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
            
        }

        Destroy(gameObject);
    }

    
}
