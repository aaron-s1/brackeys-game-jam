using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    public bool touchingExit;

    [SerializeField] GameObject doorNeededToTouch;
    [SerializeField] MovePlayer otherPlayer = null;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool mirroredPlayer;

    Vector2 moveDirection;
    Animator animator;
    Rigidbody2D rigid;
    public Camera CM_Camera;

    // Coroutine lockAnimator;

    [HideInInspector] bool canJump;
    [HideInInspector] bool playerCanMove = true;

    int currentLevel;

    
    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();

        if (mirroredPlayer)
            moveSpeed *= -1f;

        currentLevel = SceneManager.GetActiveScene().buildIndex + 1;
    }


    void Update()
    {
        if (!playerCanMove)
            return;

        float moveHorizontal = Input.GetAxis("Horizontal");

        moveDirection = new Vector2(moveHorizontal, 1f);

        Move();    // now sustains mid-air momentum
    }

    // void OldMove()
    // {
    //     if (Input.GetKey(KeyCode.A))
    //         rigid.velocity = moveDirection * moveSpeed;// * Time.deltaTime;
    //     if (Input.GetKey(KeyCode.D))
    //         rigid.velocity = moveDirection * moveSpeed;// * Time.deltaTime;

    //     if (Input.GetKeyDown(KeyCode.Space) && canJump)
    //         rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    // }

    SpriteRenderer renderer;

    void Move()
    {
        AssignDirectionalMovement();
        SetMovingAnimation();
        SetRotation();
        Jump();
    }



    void AssignDirectionalMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        moveDirection = new Vector2(moveHorizontal, 1f);

        float velocityY = rigid.velocity.y;
        Vector2 newVelocity = moveDirection * moveSpeed;
        newVelocity.y = velocityY;

        rigid.velocity = newVelocity;
    }


    void SetMovingAnimation()
    {   
        // if (!animator.GetBool("jump") && lockAnimator == null)
        // if (lockAnimator == null)
        // animator.SetBool("jump", false);
        if (Mathf.Abs(rigid.velocity.x) >= 0.1f)
            animator.SetBool("walk", true);

        // This will cause Animator to transition to default Idle state.
        else
            animator.SetBool("walk", false);
    }


    void SetRotation()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!mirroredPlayer)
                transform.eulerAngles = new Vector2(transform.eulerAngles.x, 0);
            else if (mirroredPlayer)
                transform.eulerAngles = new Vector2(transform.eulerAngles.x, 180);
        }

        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (!mirroredPlayer)
                transform.eulerAngles = new Vector2(transform.eulerAngles.x, 180f);
            if (mirroredPlayer)
                transform.eulerAngles = new Vector2(transform.eulerAngles.x, 0);
        }
    }


    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            // animator.SetBool("jump", true);
            // lockAnimator = StartCoroutine("LockAnimator");
        }
    }


    public float testLock;


    #region COLLISIONS

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
            canJump = true;
    }


    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
            canJump = false;
        if (other.gameObject.tag == "Door")
            touchingExit = false;
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == doorNeededToTouch)
        {
            touchingExit = true;
    
            if (otherPlayer == null && moveToNextLevel == null)
                moveToNextLevel = StartCoroutine("MoveToNextLevel");
            
            else if (otherPlayer.touchingExit && moveToNextLevel == null)
                moveToNextLevel = StartCoroutine("MoveToNextLevel");
        }
    }
    
    #endregion


    Coroutine moveToNextLevel;


    IEnumerator MoveToNextLevel()
    {
        moveToNextLevel = null;        
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




    IEnumerator LockAnimator()
    {
        yield return new WaitForEndOfFrame();
        // lockAnimator = null;
    }    
}
