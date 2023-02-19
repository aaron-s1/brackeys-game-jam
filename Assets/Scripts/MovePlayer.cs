using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    public bool touchingExit;

    [SerializeField] MovePlayer otherPlayer = null;
    [SerializeField] GameObject doorNeededToTouch;
    [SerializeField] GameObject arrowPointingTowardsExit;
    [SerializeField] GameObject checkmarkWhenAtExitDoor;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool mirroredPlayer;

    [SerializeField] float waitTimeBeforeNextLevelLoad = 1f;

    Vector2 moveDirection;
    Animator animator;
    Rigidbody2D rigid;

    // Coroutine lockAnimator;

    public bool canJump;
    public bool acceptingPlayerInput = true;

    int currentLevel;

    
    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        
        GetComponentInChildren<CinemachineVirtualCamera>().LookAt = doorNeededToTouch.transform;
        

        if (mirroredPlayer)
            moveSpeed *= -1f;

        currentLevel = SceneManager.GetActiveScene().buildIndex + 1;
    }


    void Update()
    {
        if (!acceptingPlayerInput)
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
        if (!acceptingPlayerInput)
            return;

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


    // bool alreadyJumping;

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump) //&& !alreadyJumping)
        {
            rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            // alreadyJumping = true;
            // animator.SetBool("jump", true);
            // lockAnimator = StartCoroutine("LockAnimator");
        }
    }


    public float testLock;


    #region COLLISIONS

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            // alreadyJumping = false;
            canJump = true;            
        }
    }


    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Platform")
            canJump = false;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground") //&& !alreadyJumping)
        {
            canJump = true;            
        }        
    }



    void OnTriggerStay2D(Collider2D other)
    {


        if (other.gameObject == doorNeededToTouch)
        {
            if (!touchingExit)
                touchingExit = true;

            SetActiveObjState(arrowPointingTowardsExit, false);
            SetActiveObjState(checkmarkWhenAtExitDoor, true);
    
            // StartCoroutine("WaitBeforeNextSceneLoad");
            
            if (otherPlayer == null)
                moveToNextLevel = StartCoroutine("MoveToNextLevel");
            
            else if (otherPlayer.touchingExit)
                moveToNextLevel = StartCoroutine("MoveToNextLevel");
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == doorNeededToTouch) {
            touchingExit = false;
            SetActiveObjState(checkmarkWhenAtExitDoor, false);
            SetActiveObjState(arrowPointingTowardsExit, true);
        }        
    }

    #endregion


    void SetActiveObjState(GameObject obj = null, bool state = false)
    {
        if (obj.activeInHierarchy != state)
            obj.SetActive(state);
    }


    Coroutine moveToNextLevel;


    IEnumerator MoveToNextLevel()
    {        
        moveToNextLevel = null;
        acceptingPlayerInput = false;
        rigid.velocity = new Vector2(0,0);
        animator.SetBool("walk", false);
        animator.SetBool("jump", false);

        yield return new WaitForSeconds(waitTimeBeforeNextLevelLoad);

        SceneManager.LoadScene(currentLevel++);
        // var nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
    }


    // IEnumerator WaitBeforeNextSceneLoad()
    // {
    //     // yield break;

    //     // yield return new WaitForSeconds(2f);

    //     // transform.position = new Vector3(10.9f, 0.83f, -1.04f);
    //     // yield return new WaitForSeconds(5f);        
    //     // playerCanMove = true;
    // }




    IEnumerator LockAnimator()
    {
        yield return new WaitForEndOfFrame();
        // lockAnimator = null;
    }    
}
