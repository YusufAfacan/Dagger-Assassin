using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject bloodEffectPrefab;

    [SerializeField] private GameObject bulletPrefab;

    public float radius;
    private Vector3 middlePoint;
    public Transform firePoint;
    bool isShoot;

    public GameObject target;

    public LayerMask targetMask;
    public LayerMask obsructionMask;

    public bool canSeeTarget;

    private enum Facing { left, right };

    private Facing facing;

    private void Start()
    {
        middlePoint = transform.position + new Vector3(0, 0.8f, 0);

        SetFacing();

        //Dagger.Instance.OnThrow += Dagger_OnThrow;
        //Dagger.Instance.OnHitGround += Dagger_OnHitGround;
        //Player.Instance.OnTeleport += Player_OnTeleport;
        StartCoroutine(SearchTarget());
    }



    private void SetFacing()
    {
        switch (transform.rotation.y)
        {
            case 270:
                facing = Facing.left;
                break;
            case 90:
                facing = Facing.right;
                break;
        }
    }

    //private void Dagger_OnThrow(object sender, System.EventArgs e)
    //{
    //    target = FindObjectOfType<Dagger>().gameObject;
    //    targetMask = LayerMask.GetMask("Dagger");

    //}

    IEnumerator SearchTarget()
    {
        target = FindObjectOfType<Dagger>().gameObject;

        float delay = 0.2f;

        WaitForSeconds delayTime = new(delay);

        while (true)
        {
            yield return delayTime;
            FieldOfViewCheck();
        }
    }

    bool AngleCheck(Vector3 directionToTarget)
    {

        float angle = Vector3.Angle(transform.forward, directionToTarget);

        Debug.Log(angle);

        if(angle < 75)
        {
            return true;
        }
        else
        {
            return false;
        }

        
    }


    void FieldOfViewCheck()
    {

        Collider[] rangeCheck = Physics.OverlapSphere(middlePoint, radius, targetMask);

        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            float distanceToTarget = Vector3.Distance(middlePoint, target.position);

            

            if (!Physics.Raycast(middlePoint, directionToTarget, distanceToTarget, obsructionMask))
            {
                canSeeTarget = true;

                if (FacingCheck())
                {
                    if(AngleCheck(directionToTarget))
                    {
                        if (target.gameObject.GetComponent<Dagger>())
                        {
                            float force = 10f;
                            target.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            target.gameObject.GetComponent<Rigidbody>().AddForce(directionToTarget * force, ForceMode.Impulse);
                            //isShoot = true;
                        }
                    }


                    
                }


                

                //if (FacingCheck())
                //{
                    

                //    //if (!isShoot)
                //    //{
                        
                        
                //    //}

                    


                //    //Shoot();
                //}
                else
                {
                    canSeeTarget = false;
                }

            }
            else
            {
                canSeeTarget = false;
            }

        }
        else if(canSeeTarget)
        {
            canSeeTarget = false;
        }

    }

    bool FacingCheck()
    {
        if (facing == Facing.left)
        {
            if (transform.position.x >= target.transform.position.x)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else if (facing == Facing.right)
        {
            if (transform.position.x <= target.transform.position.x)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void Shoot()
    {
        if (!isShoot)
        {



            //GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            //bullet.GetComponent<Bullet>().target = target.transform;
            //isShoot = true;
        }
        
    }

    //private void Dagger_OnHitGround(object sender, System.EventArgs e)
    //{
    //    StopCoroutine(SearchTarget());
    //}

    //private void Player_OnTeleport(object sender, System.EventArgs e)
    //{
    //    isShoot = false;
    //    target = FindObjectOfType<Player>().gameObject;
    //    targetMask = LayerMask.GetMask("Player");
    //    FieldOfViewCheck();
    //}


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Dagger>())
        {
            ContactPoint contactPoint = collision.contacts[0];
            Instantiate(bloodEffectPrefab, contactPoint.point, transform.rotation);
            Destroy(gameObject, 1f);
        }
    }

    private void OnDestroy()
    {
        //Dagger.Instance.OnThrow -= Dagger_OnThrow;
        //Dagger.Instance.OnHitGround -= Dagger_OnHitGround;
        //Player.Instance.OnTeleport -= Player_OnTeleport;
        StopCoroutine(SearchTarget());
    }

}
