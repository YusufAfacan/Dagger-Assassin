using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : MonoBehaviour
{
    private Touch touch;

    
    
    
    
    [SerializeField] private float forceAmount;
    private Vector3 firstTouchPos;
    private Vector3 lastTouchPos;
    [SerializeField] private Rigidbody daggerRb;


    // Start is called before the first frame update
    void Start()
    {
        Dagger.Instance.OnHitEnemy += Dagger_OnHitEnemy;
        Dagger.Instance.OnHitGround += Dagger_OnHitGround;


    }

    private void Dagger_OnHitGround(object sender, System.EventArgs e)
    {
        Invoke(nameof(Teleport), 3f);
    }

    private void Dagger_OnHitEnemy(object sender, System.EventArgs e)
    {
        Invoke(nameof(Teleport), 3f);
       
    }

    private void Teleport()
    {
        transform.position = Dagger.Instance.transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        if (!Dagger.Instance.isThrowed)
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
                    daggerRb.useGravity = true;
                    daggerRb.AddForce(throwDirection * forceAmount, ForceMode.Impulse);
                    Dagger.Instance.isThrowed = true;

                }

            }
        }

    }

    //void CalculateAngle()
    //{
    //    touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    touchPos.z = 0;

    //    Vector3 dir = touchPos - transform.position;

    //    float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
    //    UpdateAngle((int)angle);

    //}

    //void UpdateAngle(int angle)
    //{
    //    curAngle = angle;
    //}

    //void ThrowDagger()
    //{
    //    GameObject throwedDagger = Instantiate(dagger, transform.position, transform.rotation);
    //    throwedDagger.GetComponent<Rigidbody>().AddForce(Vector3.right, ForceMode.Impulse);
    //}

}
