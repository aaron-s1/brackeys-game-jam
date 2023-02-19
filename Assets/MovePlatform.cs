using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    // public float lerpTime = 10f;
    Vector3 startPos;
    public float yShift;

    Vector3 endPos;

    // private float currentTime = 0f;
    // private bool isLerping = true;
    private bool isGoingBack;

    public float speed;

    Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        endPos = new Vector3 (startPos.x, startPos.y + yShift, startPos.z);
    }

    void Update()
    {
        // endPos = new Vector3 (transform.position.x, 4.5f, transform.position.z);

        if (Vector3.Distance(transform.position, endPos) >= 0.1f && !isGoingBack)
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
        else
            isGoingBack = true;


        if (Vector3.Distance(transform.position, startPos) >= 0.1f && isGoingBack)
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
        else
            isGoingBack = false;






        // if (isLerping)
        // {
        //     currentTime += Time.deltaTime;

        //     float t = currentTime / lerpTime;
        //     // rigidbody.MovePosition(Vector3.Lerp(startPos, endPos, Mathf.PingPong(t, 1f)));
        //     transform.position = Vector3.Lerp(transform.position, endPos, 1f);


        //     if (transform.position == endPos)
        //     {
        //         currentTime = 0f;
        //         endPos = startPos;
        //     }

        //     else if (transform.position == startPos)
        //     {
        //         currentTime = 0f;
        //         endPos = new Vector3 (startPos.x, startPos.y + yShift, startPos.z);
        //     }

            // if (transform.position == startPos)
            // {
                
            // }

            // if (t >= 1f)
            // {
            //     isLerping = false;
            //     currentTime = 0f;
                
            // }
        // }
    }
}
