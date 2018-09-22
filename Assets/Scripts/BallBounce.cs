using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{

    void OnCollisionEnter(Collision col)
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        Vector3 temp = Vector3.Cross(col.contacts[0].normal, rb.velocity);
        Vector3 tangent = Vector3.Cross(col.contacts[0].normal, temp);

        Vector3 tangent_component = Vector3.Project(rb.velocity, tangent);
        Vector3 normal_component = Vector3.Project(rb.velocity, col.contacts[0].normal);

        rb.velocity = tangent_component + normal_component;
    }
}
