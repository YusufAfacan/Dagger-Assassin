using UnityEngine;

public class Assassin : MonoBehaviour
{

    Camera cam;

    public Dagger dagger;
    public Trajectory trajectory;

    [SerializeField] float throwForce;

    bool isDragging = false;

    Vector3 startPoint;
    Vector3 endPoint;
    Vector3 direction;
    Vector3 force;
    

    //---------------------------------------
    void Start()
    {
        cam = Camera.main;
        dagger.OnStaticize += Dagger_OnStaticize;
    }

    private void Dagger_OnStaticize(object sender, System.EventArgs e)
    {
        Teleport();
    }

    private void Teleport()
    {
        transform.position = dagger.Pos;
        EquipDagger();
    }

    private void EquipDagger()
    {
        dagger.transform.SetPositionAndRotation(transform.position + new Vector3(0.35f, 0.84f, 0), Quaternion.identity);
        dagger.state = Dagger.State.Equipped;
    }

    void Update()
    {
        if (dagger.state == Dagger.State.Equipped)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                OnDragStart();
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                OnDragEnd();
            }

            if (isDragging)
            {
                OnDrag();
            }
        }

    }

    //-Drag--------------------------------------
    void OnDragStart()
    {
        dagger.DeactivateRb();
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        startPoint.z = 0;
        trajectory.Show();
    }

    void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        endPoint.z = 0;
        direction = (startPoint - endPoint).normalized;
        force = direction * throwForce;
        trajectory.UpdateDots(dagger.Pos, force);
    }

    void OnDragEnd()
    {
        //push the ball
        dagger.ActivateRb();

        dagger.Throw(force);

        trajectory.Hide();
    }

}