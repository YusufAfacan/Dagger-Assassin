using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    private Touch touch;
    private Vector3 firstTouchPos;
    private Vector3 lastTouchPos;

    [SerializeField] private float forceAmount;

    public static Dagger Instance;

    public event EventHandler OnHitGround;
    public event EventHandler OnThrow;

    public enum State {Equipped, OnAir, BouncedOff };
    public State state = State.Equipped;

    private Rigidbody rb;

    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Throw();


        switch (state)
        {
            case State.OnAir:
                Vector3 position = transform.position;
                position.z = 0;
                transform.position = position;
                transform.Rotate(0, 0, -360 * Time.deltaTime);
                break;
            case State.BouncedOff:
                Vector3 position2 = transform.position;
                position2.z = 0;
                transform.position = position2;
                break;
            
        }
    }

    private void Throw()
    {
        if (state == State.Equipped)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    firstTouchPos = Camera.main.ScreenToWorldPoint(touch.position);
                    firstTouchPos.z = 0;
                }

                else if (touch.phase == TouchPhase.Ended)
                {
                    lastTouchPos = Camera.main.ScreenToWorldPoint(touch.position);
                    lastTouchPos.z = 0;

                    Vector3 throwDirection = (lastTouchPos - firstTouchPos).normalized;
                    rb.isKinematic = false;
                    rb.useGravity = true;

                    rb.AddForce(throwDirection * forceAmount, ForceMode.Impulse);
                    state = State.OnAir;
                    OnThrow?.Invoke(this, EventArgs.Empty);

                }
            }
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() == true)
        {
            if(state == State.OnAir || state == State.BouncedOff)
            {
                StartCoroutine(HitEnemy());
            }
            
        }

        if (collision.gameObject.GetComponent<Ground>() == true)
        {
            if(state == State.OnAir)
            {
                state = State.BouncedOff;
                ChangeToCapsule();
            }
        }
    }

    private IEnumerator HitEnemy()
    {
        state = State.BouncedOff;
        rb.isKinematic = true;
        yield return new WaitForSeconds(1f);
        rb.isKinematic = false;
        ChangeToCapsule();

    }

    private void ChangeToCapsule()
    {
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<SphereCollider>().enabled = false;
    }

    public void ChangeToSphere()
    {
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>())
        {
            if (state == State.BouncedOff)
            {
                if (rb.velocity.magnitude <= 0.01f)
                {
                    rb.isKinematic = true;
                    
                    OnHitGround?.Invoke(this, EventArgs.Empty);
                }
            }

            
        }
    }
}
