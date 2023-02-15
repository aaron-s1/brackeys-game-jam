using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    public bool touchingExit;
    public bool playerCanMove = true;
    public GameObject doorNeededToTouch;
    public MovePlayer otherPlayer = null;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool mirrored;
    Rigidbody2D rigid;

    Vector2 moveDirection;

    public bool canJump;


///
    int currentLevel;
///
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        if (mirrored)
            moveSpeed *= -1f;

        currentLevel = SceneManager.GetActiveScene().buildIndex + 1;
    }


    void Update()
    {
        if (!playerCanMove)
            return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        moveDirection = new Vector2(moveHorizontal, moveVertical);

        Move();
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.A))
            rigid.velocity = moveDirection * moveSpeed;// * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            rigid.velocity = moveDirection * moveSpeed;// * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
            rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
            canJump = true;
    }


    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject == doorNeededToTouch)
        {
            touchingExit = true;
    
            if (otherPlayer == null)
                StartCoroutine("MoveToNextLevel");
            
            else if (otherPlayer.touchingExit)
                StartCoroutine("MoveToNextLevel");
        }
    }


    IEnumerator MoveToNextLevel()
    {
        playerCanMove = false;
        Debug.Log("MoveToNextLevel");

        // var nextLevel = SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
    }


    IEnumerator WaitBeforeNextSceneLoad()
    {
        yield break;

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(currentLevel++);

        transform.position = new Vector3(10.9f, 0.83f, -1.04f);
        yield return new WaitForSeconds(5f);        
        playerCanMove = true;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
            canJump = false;
        if (other.gameObject.tag == "Door")
            touchingExit = false;
        // if (other.gameObject.tag == "Barrier")
            // direction = -direction;
    }

    

    // void CanJump() =>
    //     canJump = !canJump;
}
