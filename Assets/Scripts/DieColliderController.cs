using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieColliderController : MonoBehaviour
{
    public static event Action PlayerDied;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            PlayerDied?.Invoke();
        }

        Destroy(collision.gameObject);
    }
}
