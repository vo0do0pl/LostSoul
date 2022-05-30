using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialProjectile : MonoBehaviour
{
    [SerializeField] float speed = 200f;
    [SerializeField] float angularVelocity = 25f;
    [SerializeField] float targetMargin = 0.25f;
    [SerializeField] GameObject explosion;

    public Rigidbody2D myRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody.angularVelocity = angularVelocity;
    }

    public void SetTargetAndLaunch(Vector3 playerPosition)
    {
        playerPosition.y += targetMargin;
        Vector3 direction = playerPosition - transform.position;
        myRigidBody.AddForce(direction * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Instantiate(explosion, collision.GetContact(0).point, Quaternion.identity);
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
