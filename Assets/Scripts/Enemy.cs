using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject bloodEffectPrefab;

    public GameObject gunSmoke;
    

    [SerializeField] private Transform firePoint;

    public float sightRadius;
    public float hearRadius;
    private Vector3 middlePoint;
    
    bool isShoot;

    public GameObject target;

    public LayerMask targetMask;
    public LayerMask obsructionMask;

    public bool canSeeTarget;

    private enum Facing { left, right };

    [SerializeField] private Facing facing;

    private void Start()
    {
        middlePoint = transform.position + new Vector3(0, 0.8f, 0);

        SetFacing();

        //Dagger.Instance.OnThrow += Dagger_OnThrow;
        //Dagger.Instance.OnHitGround += Dagger_OnHitGround;
        //Player.Instance.OnTeleport += Player_OnTeleport;
        StartCoroutine(SearchTarget());

        Assassin.OnTeleport += Assassin_OnTeleport;

    }

    private void Assassin_OnTeleport(object sender, System.EventArgs e)
    {
        HearAndShoot();
    }

    void HearAndShoot()
    {
        Transform assassin = FindObjectOfType<Assassin>().transform;

        if (Vector3.Distance(middlePoint, assassin.position) <= hearRadius)
        {
            if (!FacingCheck())
            {
                transform.Rotate(Vector3.up, 180f);
                if (facing == Facing.left)
                facing = Facing.right;
                else if (facing == Facing.right)
                facing = Facing.left;
            }

            Vector3 directionToTarget = (assassin.position - transform.position).normalized;

            float distanceToTarget = Vector3.Distance(middlePoint, assassin.position);


            if (!Physics.Raycast(middlePoint, directionToTarget, distanceToTarget, obsructionMask))
            {
                Shoot(assassin, directionToTarget);
            }
            
        }

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

        Collider[] rangeCheck = Physics.OverlapSphere(middlePoint, sightRadius, targetMask);

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
                            Shoot(target, directionToTarget);
                        }
                    }


                    
                }

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

    private void Shoot(Transform target, Vector3 directionToTarget)
    {
        float force = 10f;
        target.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        target.gameObject.GetComponent<Rigidbody>().AddForce(directionToTarget * force, ForceMode.Impulse);

        GameObject gunSmokeIns =  Instantiate(gunSmoke, firePoint.position, Quaternion.identity);
        Destroy(gunSmokeIns, 0.2f);
        //isShoot = true;
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
            Dagger.State daggerState = collision.gameObject.GetComponent<Dagger>().state;

            if (daggerState == Dagger.State.OnAir || daggerState == Dagger.State.BouncedOff)
            {
                ContactPoint contactPoint = collision.contacts[0];
                Instantiate(bloodEffectPrefab, contactPoint.point, transform.rotation);
                Destroy(gameObject, 1f);
            }

            
        }
    }

    private void OnDestroy()
    {
        //Dagger.Instance.OnThrow -= Dagger_OnThrow;
        //Dagger.Instance.OnHitGround -= Dagger_OnHitGround;
        //Player.Instance.OnTeleport -= Player_OnTeleport;
        Assassin.OnTeleport -= Assassin_OnTeleport;
        StopCoroutine(SearchTarget());
    }

}
