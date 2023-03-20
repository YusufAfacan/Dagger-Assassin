using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    

    private Rigidbody rb;

    public event EventHandler OnTeleport;


    public static Player Instance;

    private void Awake()
    {
        Instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        Dagger.Instance.OnHitGround += Dagger_OnHitGround;
    }

    void Update()
    {
        
    }

    

    private void Dagger_OnHitGround(object sender, System.EventArgs e)
    {
        Invoke(nameof(Teleport), 1f);
    }

    private void Teleport()
    {
        transform.position = Dagger.Instance.transform.position;
        rb.useGravity = true;

        Dagger.Instance.transform.SetPositionAndRotation(transform.position + new Vector3(0.5f, 1.5f, 0), Quaternion.identity);

        Dagger.Instance.state = Dagger.State.Equipped;
        Dagger.Instance.ChangeToSphere();
        OnTeleport?.Invoke(this, EventArgs.Empty);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>())
        {
            rb.useGravity = false;
        }
    }

}
