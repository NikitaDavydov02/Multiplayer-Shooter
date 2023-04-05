using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float velocity;
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public GameObject owner;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = velocity * transform.right;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != owner)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Damage(damage);
            }
            Destroy(this.gameObject);
        }
        
    }
}
