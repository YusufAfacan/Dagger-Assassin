using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject bloodEffectPrefab;

    [SerializeField] private GameObject bulletPrefab;

    

    private void Start()
    {
        
    }

    


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() || other.gameObject.GetComponent<Dagger>())
        {
            

            Vector3 direction = other.transform.position - transform.position;

            Ray ray = new(transform.position, direction);

            Physics.Raycast(transform.position, other.transform.position, out RaycastHit hit, 100, 9);

            Debug.Log(hit.transform.name);

            if(hit.transform.gameObject.GetComponent<Player>() || hit.transform.gameObject.GetComponent<Dagger>())

            transform.LookAt(hit.point);


            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, hit.point, 100);
            

        }
    }









    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Dagger>())
        {
            ContactPoint contactPoint = collision.contacts[0];
            Instantiate(bloodEffectPrefab, contactPoint.point, transform.rotation);
            Destroy(gameObject, 1f);
        }
    }
}
