using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float velocity;
    [SerializeField]
    private float angularVelocity;
    private Vector2 movmentDir;
    private Rigidbody2D rb;
    private PlayerInputActions inputActions;
    private Vector2 targetRotationDirection;
    private float forwardMovmentInput;
    public bool Alive { get; private set; } = true;
    public int HP { get; private set; }
    [SerializeField]
    public int maxHP;
    [SerializeField]
    private GameObject bulletPrefab;
    private Vector3 currentRotation;
    [SerializeField]
    private float rotationJoystickTreshhold = 1f;
    // Start is called before the first frame update
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HP = maxHP;
    }
    // Update is called once per frame
    void Update()
    {
        if (MainManager.GameIsFinished)
            return;
        currentRotation= new Vector3(transform.TransformDirection(Vector3.right).x, transform.TransformDirection(Vector3.right).y, 0);
        currentRotation.Normalize();
        Vector2 forwardInput = inputActions.MainActionMap.Move.ReadValue<Vector2>();
        //Debug.Log("Forward input: " + forwardInput);
        Vector2 rotationInput = inputActions.MainActionMap.Rotate.ReadValue<Vector2>();
        targetRotationDirection = rotationInput;
        targetRotationDirection.Normalize();
        if (rotationInput.magnitude < rotationJoystickTreshhold)
        {
            targetRotationDirection = currentRotation;
        }
        forwardMovmentInput = forwardInput.y;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shot();
        }

        movmentDir = transform.right;
        movmentDir.Normalize();
    }
    private void FixedUpdate()
    {
        //Vector3 currentDirection = new Vector3(transform.TransformDirection(Vector3.right).x, transform.TransformDirection(Vector3.right).y,0);
        Vector3 dot = Vector3.Cross(currentRotation, targetRotationDirection);
        //Debug.DrawLine(transform.position, transform.position + currentRotation, Color.red);
        //Debug.DrawLine(transform.position, transform.position + new Vector3(targetRotationDirection.x, targetRotationDirection.y, 0), Color.blue);
        //if dot<0 turn to the left
        //Debug.Log("Dot: " + dot);
        if (dot.z < 0)
        {
            rb.angularVelocity = -angularVelocity;
        }
        else if (dot.z > 0)
        {
            rb.angularVelocity = angularVelocity;
        }
        else
            rb.angularVelocity = 0;
        //float targetAngle = Mathf.Atan2(targetRotationDirection.y, targetRotationDirection.x)*Mathf.Rad2Deg;
        //if (targetRotationDirection.x < 0)
        //    targetAngle += 180;
        //Debug.Log("target angle:" + targetAngle);
        //float currentAngle = Mathf.Atan2(transform.TransformDirection(Vector3.right).y, transform.TransformDirection(Vector3.right).x) * Mathf.Rad2Deg;
        //if (transform.TransformDirection(Vector3.right).x < 0)
        //    currentAngle += 180;
        //Debug.Log("Movment velocity:" + movmentDir * forwardMovmentInput * velocity);
        rb.velocity = movmentDir* forwardMovmentInput * velocity;
    }
    public void Damage(int damage)
    {
        HP -= damage;
        if (HP < 0)
            HP = 0;
        if (HP == 0)
            Alive = false;
    }
    public void Shot()
    {
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        bullet.transform.position = this.gameObject.transform.position;
        bullet.transform.rotation = this.transform.rotation;
        bullet.GetComponent<Bullet>().owner = this.gameObject;
    }
    //private void OnCollisionEnter(Collision collision)
    //{
     //   Debug.Log("Collision");
    //}
}
