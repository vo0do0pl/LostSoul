using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCloudController : MonoBehaviour
{
    
    [SerializeField] float maxSpeed = 5;
    [SerializeField] float minSpeed = 1;

    public Rigidbody2D myRigidBody;
    public SpriteRenderer mySpriteRenderer;

    bool isCloudStatic = true;
    Vector2 currentDirection = new Vector2();
    float currentSpeed;

    public static event Action PlatformCloudVanished;

    void Start()
    {
        if(mySpriteRenderer != null)   
        {
            if(Convert.ToBoolean(UnityEngine.Random.Range(0, 2)))
            {
                mySpriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
            }
        }

        isCloudStatic = !Convert.ToBoolean(UnityEngine.Random.Range(0, 3));
        Initialize();
    }

    private void Initialize()
    {
        if (isCloudStatic)
        {
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }

        currentDirection = Convert.ToBoolean(UnityEngine.Random.Range(0, 2)) ? Vector2.left : Vector2.right;
        currentSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);

        myRigidBody.MovePosition(currentDirection * currentSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCloudStatic) return;

        if(collision.gameObject.layer == 10)
        {
            ChangeDirection();
        }
    }

    private void OnDestroy()
    {
        PlatformCloudVanished?.Invoke();
    }
    private void ChangeDirection()
    {
        currentDirection = currentDirection == Vector2.left ? Vector2.right : Vector2.left;
    }

    private void FixedUpdate()
    {
        if (isCloudStatic) return;

        myRigidBody.MovePosition(transform.position + new Vector3(currentDirection.x, 0, 0) * Time.deltaTime * currentSpeed);
    }

    private void OnBecameVisible()
    {
        PlatformCloudVanished?.Invoke();
        Destroy(gameObject);
    }

    public Vector2 GetCurrentVelocity()
    {
        return currentDirection * currentSpeed;
    }
}
