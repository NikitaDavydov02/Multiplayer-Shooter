using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

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
    public float bulletOffcet = 1;

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
        //if(!photonView.IsMine)
        if (MainManager.GameStatus!=GameStatus.Playing || !photonView.IsMine)
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
    [PunRPC]
    public void Damage(int damage)
    {
        if (photonView.IsMine)
        {
            Debug.Log("I took damage");
        }
        else
            Debug.Log("Another took damage");
        HP -= damage;
        if (HP < 0)
            HP = 0;
        if (HP == 0)
            Alive = false;
        Debug.Log(photonView.Owner.NickName + " HP" + HP);
    }
    public void Shot()
    {
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, this.gameObject.transform.position+transform.TransformDirection(bulletOffcet,0,0), this.transform.rotation) as GameObject;
        Debug.Log("Instantiate player id " + photonView.ViewID);
        bullet.GetComponent<Bullet>().SetOwnerID(photonView.ViewID);
        //bullet.GetComponent<Bullet>().photonView.RPC
        //bullet.GetComponent<Bullet>().owner = this.gameObject;
    }
    public void CoinIsCollected()
    {
        CoinsCollected++;
        photonView.RPC("CoinIsCollectedRPC", RpcTarget.All, CoinsCollected);
        OnCoinIsCollcted();
    }
    [PunRPC]
    public void CoinIsCollectedRPC(int coinsCollected)
    {
        CoinsCollected = coinsCollected;
    }
    public event EventHandler CoinIsCollcted;
    public void OnCoinIsCollcted()
    {
        EventHandler handler = CoinIsCollcted;
        if (handler != null)
            handler(this, new SpawnNewPlayerEventArgs(this));
    }
}
