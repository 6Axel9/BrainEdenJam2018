using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopKey : MonoBehaviour {

    public TextMesh RenderText;
    public KeyCode Key;
    private bool Hold;
    private bool Swap;

	// Use this for initialization
	void Start () {

        Hold = false;
        Swap = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        RenderText.text = "";

        Collider[] hits = Physics.OverlapSphere(transform.position, 2.0f);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Vector3 direction = hit.transform.position - transform.position;
                direction.y = 0;

                transform.forward = -direction.normalized;

                if(Input.GetKeyDown(Key) && !Hold && !Swap)
                {
                    Key = KeyCode.Mouse1;
                    Hold = true;
                }
                else if (Input.GetKeyDown(Key) && Hold)
                {
                    Key = KeyCode.E;
                    Hold = false;
                }
            }

            if (hit.CompareTag("Item") && hit.transform == gameObject.transform.parent && Hold && !Swap)
            {
                Vector3 direction = hit.transform.position - transform.position;
                direction.y = 0;

                transform.forward = -direction.normalized;

                Key = KeyCode.Q;
                Swap = true;

                RenderText.text = Key.ToString();

                if (Input.GetKeyDown(Key))
                {
                    Key = KeyCode.Mouse1;
                    Swap = false;
                }

                RenderText.text = Key.ToString();
            }   
            else if (!Hold && !Swap)
            {
                Key = KeyCode.E;
            }
        }
	}
}
