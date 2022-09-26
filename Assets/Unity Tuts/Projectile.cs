using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [HideInInspector]
    new public Rigidbody rigidbody;
    public float damageRadius = 1;

    private void Reset()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
}
