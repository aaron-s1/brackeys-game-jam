using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool mirrored;
    Rigidbody2D rigid;

    public bool canJump;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        if (mirrored)
            moveSpeed *= -1f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            transform.Translate(new Vector3(-moveSpeed, 0, 0) * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(new Vector3(moveSpeed, 0, 0) * Time.deltaTime);
            // speed *= -1f;
        
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
            rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            // transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
            canJump = true;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
            canJump = false;
    }

    // void CanJump() =>
    //     canJump = !canJump;
}
