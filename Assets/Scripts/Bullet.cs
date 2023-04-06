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
    public GameObject owner;
    //private PhotonView photonView;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //photonView = GetComponent<PhotonView>();
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != owner && other.gameObject.tag!="Coin")
        {
            Debug.Log("Bullet hited");
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.GetComponent<PhotonView>().RPC("Damage", RpcTarget.All, damage);
                ////player.Damage(damage);
                
            }
            PhotonNetwork.Destroy(this.gameObject);
        }
        
    }
}
