using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderCloudController : MonoBehaviour
{

    public static event Action<float> BorderCloudVanished;

    private void OnBecameInvisible()
    {
        BorderCloudVanished?.Invoke(gameObject.transform.position.x);
        Destroy(gameObject);
    }
}
