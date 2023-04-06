using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float velocity;
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public PhotonView photonView { get; private set; }
    public int ownerID = 0;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.GameStatus == GameStatus.Finished)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        rb.velocity = velocity * transform.right;
    }
   public void SetOwnerID(int id)
    {
        if (photonView == null)
            photonView = GetComponent<PhotonView>();
        ownerID = id;
        photonView.RPC("SetOwnerIDRPC", RpcTarget.Others, id);
    }
    [PunRPC]
    public void SetOwnerIDRPC(int id)
    {
        if (photonView == null)
            photonView = GetComponent<PhotonView>();
        Debug.Log("Receive bullet RPC" + id);
        ownerID = id;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag!="Coin")
        {
            PhotonView pv = other.gameObject.GetComponent<PhotonView>();
            if(pv!=null && pv.ViewID != ownerID)
            {
                Player player = other.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    pv.RPC("Damage", RpcTarget.All, damage);

                }
                PhotonNetwork.Destroy(this.gameObject);
            }
            
        }
        
    }
}
