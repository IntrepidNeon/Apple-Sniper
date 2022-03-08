using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public Mesh_explosion mesh_explosion;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("entityTimeout", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Destroy(this.gameObject);
            RigidbodyController.playerHealth -= 1;
            RigidbodyController.scoreStreak = 0;
        }
    }
    void entityTimeout()
    {
        Destroy(this.gameObject);
    }
    public void shootAndDestroy(Vector3 hitDirection)
    {
        this.mesh_explosion.setForceDirection(hitDirection);
        this.GetComponent<Collider>().enabled = false;
        StartCoroutine(this.mesh_explosion.split_mesh(false));
        Invoke("entityTimeout", 0.1f);
    }
}
