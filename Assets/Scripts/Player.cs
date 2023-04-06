using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    public bool Alive { get; private set; } = true;
    public int HP { get; private set; }

    public int CoinsCollected { get; private set; }
    [SerializeField]
    public int maxHP;
    [SerializeField]
    private float velocity;
    [SerializeField]
    private float angularVelocity;
    private Rigidbody2D rb;
    private PlayerInputActions inputActions;
    private Vector2 targetRotationDirection;
    private Vector2 movmentDir;

    [SerializeField]
    private GameObject bulletPrefab;
    private Vector3 currentRotation;
    [SerializeField]
    private float rotationJoystickTreshhold = 1f;
    private PhotonView photonView;
    // Start is called before the first frame update
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();
        inputActions.MainActionMap.Shoot.started += Shoot_started;
        
    }

    private void Shoot_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(photonView.IsMine)
            Shot();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        HP = maxHP;
        CoinsCollected = 0;
    }
    // Update is called once per frame
    void Update()
    {
        //if (MainManager.GameStatus!=GameStatus.Playing || !photonView.IsMine)
        if(!photonView.IsMine)
            return;
        currentRotation= new Vector3(transform.TransformDirection(Vector3.right).x, transform.TransformDirection(Vector3.right).y, 0);
        currentRotation.Normalize();
        movmentDir = inputActions.MainActionMap.Move.ReadValue<Vector2>();

        //Debug.Log("Forward input: " + forwardInput);
        Vector2 rotationInput = inputActions.MainActionMap.Rotate.ReadValue<Vector2>();
        targetRotationDirection = rotationInput;
        targetRotationDirection.Normalize();
        if (rotationInput.magnitude < rotationJoystickTreshhold)
        {
            targetRotationDirection = currentRotation;
        }
    }
    private void FixedUpdate()
    {
        Vector3 dot = Vector3.Cross(currentRotation, targetRotationDirection);
        
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
        rb.velocity = movmentDir* velocity;
    }
    //[PunRPC]
    public void Damage(int damage)
    {
        Debug.Log("Damage");
        HP -= damage;
        if (HP < 0)
            HP = 0;
        if (HP == 0)
            Alive = false;
    }
    public void Shot()
    {
        //Debug.Log("Shot");
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, this.gameObject.transform.position, this.transform.rotation) as GameObject;
        //bullet.transform.position = this.gameObject.transform.position;
        //bullet.transform.rotation = this.transform.rotation;
        bullet.GetComponent<Bullet>().owner = this.gameObject;
    }
    public void CoinIsCollected()
    {
        CoinsCollected++;
    }
}
