using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteraction
{
    void Hold(float speed);
    void Throw(float force);
    void Swap(float force);
    void Pickup(float speed);   
}

public partial class Humanoid
{
    private GameObject Item;
    public Transform Hand;
    public Vector3 Offset;
    private bool Holding;
    private bool Flip;

    public void Hold(float speed)
    {
        Vector3 direction = Vector3.zero;
        Vector3 bulletSpawn = Hand.position + Hand.TransformDirection(Offset);

        if (!Item)
        {

            Collider[] hits = Physics.OverlapSphere(bulletSpawn, 0.5f);

            foreach (Collider hit in hits)
            {
                if (hit.gameObject.CompareTag("Item"))
                {
                    Item = hit.gameObject;

                    Holding = true;

                }
            }

        }
        else
        {
            var body = Item.GetComponent<Rigidbody>();

            direction = bulletSpawn - Item.transform.position;

            if (direction.magnitude > 0.2f)
                body.AddForce(direction.normalized * speed);
        }
    }

    public void Throw(float force)
    {
        if (Item)
        {
            var body = Item.GetComponent<Rigidbody>();
            body.AddForce(-Hand.up * force, ForceMode.Impulse);
            Item = null;

            Holding = false;
        }
    }

    public void Swap(float force)
    {
        Vector3 direction = Vector3.zero;
        Vector3 bulletSpawn = Hand.position + Hand.TransformDirection(Offset);

        if (Item)
        {
            Collider[] hits = Physics.OverlapSphere(bulletSpawn, 1.0f);

            Rigidbody oldBody = Item.GetComponent<Rigidbody>();

            bool found = false;

            foreach (Collider hit in hits)
            {
                if (hit.gameObject.CompareTag("Item") &&
                    Item != hit.gameObject && oldBody && !found)
                {
                    if (!Flip)
                    {
                        oldBody.AddForce(Hand.right * force, ForceMode.Impulse);
                    }
                    else
                    {
                        oldBody.AddForce(-Hand.right * force, ForceMode.Impulse);
                    }
                    Item = hit.gameObject;
                    Flip = !Flip;
                    found = true;
                }
            }
            found = false;
        }
    }

    public void Pickup(float speed)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 2.0f);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject.CompareTag("Pickup"))
            {
                var pickup = hit.gameObject;
                var body = pickup.GetComponent<Rigidbody>();

                Vector3 direction = Hand.position - pickup.transform.position;

                if (direction.magnitude > 0.25f)
                    body.AddForce(direction.normalized * speed);
            }
        }

    }
}