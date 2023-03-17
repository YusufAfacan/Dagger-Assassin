using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private GameObject bloodEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Dagger>())
        {
            ContactPoint contactPoint = collision.contacts[0];
            Instantiate(bloodEffect, contactPoint.point, transform.rotation);
            Destroy(gameObject, 1f);
            

        }
    }

}
