using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool OnGround { get; private set; }
    public float Friction { get; private set; }

    private Controller _controller;
    private Vector2 _normal;
    private PhysicsMaterial2D _material;

    private void Awake()
    {
        _controller = GetComponent<Controller>();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnGround = false;
        Friction = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
        EvaluatePlatform(collision);
        
        if (OnGround)
        {
            EventManager.TriggerEvent("onLand");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
        EvaluatePlatform(collision);
    }

    private IEnumerator EnableCollider(Collider2D contactCollider)
    {
        yield return new WaitForSeconds(0.2f);
        contactCollider.enabled = true;
    }

    private void EvaluatePlatform(Collision2D collision)
    {
        bool isDown = _controller.input.RetrieveDownInput();

        if (isDown)
        {
            print("down");
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.collider.gameObject.CompareTag("Platform"))
                {
                    contact.collider.enabled = false;
                    StartCoroutine(EnableCollider(contact.collider));
                }
            }
        }
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            _normal = collision.GetContact(i).normal;
            OnGround |= _normal.y >= 0.9f;
        }
    }

    private void RetrieveFriction(Collision2D collision)
    {
        _material = collision.rigidbody == null ? collision.collider.sharedMaterial : collision.rigidbody.sharedMaterial;

        Friction = 0;

        if(_material != null)
        {
            Friction = _material.friction;
        }
    }
}
