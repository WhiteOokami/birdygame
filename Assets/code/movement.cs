using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ////down
        //if (Input.GetKey("w") || Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    rb.velocity = new Vector3(rb.velocity.x, upSpeed, 0);
        //}

        //if (Input.GetKey("d") || Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    rb.velocity = new Vector3(horizontalSpeed, rb.velocity.y, 0);
        //}

        //if (Input.GetKey("a") || Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    rb.velocity = new Vector3(-horizontalSpeed, rb.velocity.y, 0);
        //}

        //if (Input.GetKey("s") || Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    rb.velocity = new Vector3(rb.velocity.x, -upSpeed, 0);
        //}


        ////up
        //if (Input.GetKeyUp("w") || Input.GetKeyUp(KeyCode.UpArrow))
        //{
        //    rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        //}

        //if (Input.GetKeyUp("d") || Input.GetKeyUp(KeyCode.RightArrow))
        //{
        //    rb.velocity = new Vector3(0, rb.velocity.y, 0);
        //}

        //if (Input.GetKeyUp("a") || Input.GetKeyUp(KeyCode.LeftArrow))
        //{
        //    rb.velocity = new Vector3(0, rb.velocity.y, 0);
        //}

        //if (Input.GetKeyUp("s") || Input.GetKeyUp(KeyCode.DownArrow))
        //{
        //    rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        //}

    }

    void FixedUpdate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Input.GetMouseButton(0))
        {
            //rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), turnSpeed * Time.deltaTime));
           // if the mouse button is on the bird, dont move
           if (Vector2.Distance(rb.position, mousePosition)>0.2)
                rb.MovePosition(rb.position + ((Vector2)transform.right * moveSpeed * Time.deltaTime));
        }
    }
}
