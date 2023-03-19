using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject bloodEffectPrefab;

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
