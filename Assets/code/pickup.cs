using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{

    Rigidbody2D rb;
    FixedJoint2D joint;
    public int breakforce = 500;
    public GameObject theObject = null;
    public GameObject carryingObject = null;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }
    
    void pickupObject()
    {
        joint = gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
        joint.connectedBody = theObject.GetComponent<Rigidbody2D>();
        joint.breakForce = breakforce;
        carryingObject = theObject;
    }

    void releaseObject()
    {
        Destroy(joint);
        carryingObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("e") || Input.GetMouseButtonDown(1))
        {
            if (carryingObject != null)
                releaseObject();
            else if (theObject != null)
                pickupObject();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "grabableItem")
        {
            if (carryingObject == null)
                theObject = collision.collider.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (GameObject.ReferenceEquals(collision.collider, theObject))
            Debug.Log("hello");
        theObject = null;
    }
}
