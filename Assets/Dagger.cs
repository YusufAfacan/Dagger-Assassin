using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    public static Dagger Instance;

    public event EventHandler OnHitEnemy;
    public event EventHandler OnHitGround;

    private Rigidbody rb;

    public bool isThrowed;

    private void Awake()
    {
        Instance = this;

        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.z = 0;
        transform.position = position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() == true)
        {
            StartCoroutine(HitEnemy());
        }

        if (collision.gameObject.GetComponent<Ground>() == true)
        {
            StartCoroutine(HitGround());
        }


    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>())
        {
            if (rb.velocity.magnitude <= 0.25f)
            {
                if (isThrowed == true)
                {
                    isThrowed = false;
                    OnHitGround?.Invoke(this, EventArgs.Empty);

                }

            }
        }


    }


    IEnumerator HitEnemy()
    {
        rb.isKinematic = true;
        OnHitEnemy?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(1f);

        rb.isKinematic = false;

    }

    IEnumerator HitGround()
    {

        yield return new WaitForSeconds(1f);



    }
}
