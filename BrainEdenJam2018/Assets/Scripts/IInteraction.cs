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
    private bool Holding;
    private bool Flip;

    public void Hold(float speed)
    {
        if (!Item)
        {
            Ray location = new Ray(transform.position, transform.forward);
            RaycastHit[] hits = Physics.SphereCastAll(location, 1.0f);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Item"))
                {
                    Item = hit.collider.gameObject;

                    Holding = true;
                }
            }

        }
        else
        {
            var body = Item.GetComponent<Rigidbody>();

            Vector3 offset = transform.forward * 2.0f;
            Vector3 direction = transform.TransformPoint(offset) - Item.transform.position;

            if (direction.magnitude > 0.25f)
                body.AddForce(direction.normalized * speed);
        }
    }

    public void Throw(float force)
    {
        if (Item)
        {
            var body = Item.GetComponent<Rigidbody>();
            body.AddForce(transform.forward * force, ForceMode.Impulse);
            Item = null;

            Holding = false;
        }
    }

    public void Swap(float force)
    {
        if (Item)
        {
            Ray location = new Ray(transform.position, transform.forward);
            RaycastHit[] hits = Physics.SphereCastAll(location, 1.0f);

            var oldBody = Item.GetComponent<Rigidbody>();

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Item") &&
                    Item != hit.collider.gameObject)
                {
                    if (!Flip)
                    {
                        oldBody.AddForce(transform.right * force, ForceMode.Impulse);
                    }
                    else
                    {
                        oldBody.AddForce(-transform.right * force, ForceMode.Impulse);
                    }

                    Item = hit.collider.gameObject;
                    Flip = !Flip;
                }
            }
        }
    }

    public void Pickup(float speed)
    {
        Ray location = new Ray(transform.position, transform.forward);
        Collider[] hits = Physics.OverlapSphere(transform.position, 10.0f);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject.CompareTag("Pickup"))
            {
                var pickup = hit.gameObject;
                var body = pickup.GetComponent<Rigidbody>();

                Vector3 direction = transform.position - pickup.transform.position;

                if (direction.magnitude > 0.25f)
                    body.AddForce(direction.normalized * speed);
            }
        }

    }
}