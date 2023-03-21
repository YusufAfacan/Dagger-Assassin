
using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    public enum State {Equipped, OnAir, BouncedOff, OnGround }
    public State state = State.Equipped;

    public event EventHandler OnStaticize;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public SphereCollider sphereCol;
    [HideInInspector] public CapsuleCollider capsuleCol;

    [HideInInspector] public Vector3 Pos { get { return transform.position; } }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }


    private void Start()
    {
        sphereCol = GetComponent<SphereCollider>();
        capsuleCol = GetComponent<CapsuleCollider>();
        DeactivateRb();
    }

    private void Update()
    {
        RotateOnAir();
    }

    private void RotateOnAir()
    {
        if (state == State.OnAir)
        {
            transform.Rotate(0, 0, -180 * Time.deltaTime);

        }
    }

    public void Throw(Vector3 force)
    {
        state = State.OnAir;
        rb.AddForce(force, ForceMode.Impulse);
    }

    public void ActivateRb()
    {
        rb.isKinematic = false;
    }

    public void DeactivateRb()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Wall>())
        {
            if (state == State.OnAir)
            {
                Bounce();
            }
        }

        if (collision.gameObject.GetComponent<Enemy>())
        {
            if (state == State.OnAir || state == State.BouncedOff)
            {
                StartCoroutine(HitEnemy());
            }
        }

    }

    void Bounce()
    {
        state = State.BouncedOff;
        ChangeToCapsuleCollider();
    }

    IEnumerator HitEnemy()
    {
        rb.isKinematic = true;
        state = State.BouncedOff;
        ChangeToCapsuleCollider();
        yield return new WaitForSeconds(1f);
        rb.isKinematic = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<Wall>())
        {
            Staticize();
        }
    }


    void Staticize()
    {
        if (state == State.BouncedOff)
        {
            if (rb.velocity.magnitude <= 0.01f)
            {
                DeactivateRb();
                ChangeToSphereCollider();
                state = State.OnGround;
                
                OnStaticize?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void ChangeToCapsuleCollider()
    {
        capsuleCol.enabled = true;
        sphereCol.enabled = false;
    }

    private void ChangeToSphereCollider()
    {
        sphereCol.enabled = true;
        capsuleCol.enabled = false;
        
    }

}