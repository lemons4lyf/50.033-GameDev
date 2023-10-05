using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    public float maxSpeed = 20;
    public float upSpeed = 10;
    private Rigidbody2D marioBody;
    private bool onGroundState = true;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    public GameObject enemies;

    public Animator marioAnimator;

    public AudioSource marioAudio;

    public AudioSource marioDeathAudio;

    public float deathImpulse = 15;

    // state
    [System.NonSerialized]
    public bool alive = true;

    public Transform gameCamera;

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    private bool moving = false;
    private bool jumpedState = false;

    GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
         // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        marioAnimator.SetBool("onGround", onGroundState);  
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();      
    }

    // Update is called once per frame
    void Update()
    {
        
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
       
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.velocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    void gameOverScene(){

        Debug.Log("GameOverScene");
        gameManager.GameOver();
        
    }

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }


    void OnCollisionEnter2D(Collision2D col)
    {
      if (col.gameObject.CompareTag("Ground") && !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }

      if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);  
    }
    }

    void FixedUpdate()
    {   
        if(alive && moving){

            Move(faceRightState == true ? 1 : -1);

        }


    }

    void Move(int value){

        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.velocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
        }


    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }
       
    public void Jump(){

        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    } 
        
    
    public void Hit(){

          Debug.Log("Collided with goomba!");
           // play death animation
            marioAnimator.Play("mario-die");
            marioAudio.PlayOneShot(marioDeathAudio.clip);
            alive = false;

    }

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    public void GameRestart()
    {
       // reset position
        marioBody.transform.position = new Vector3(-4.14f, -3.87f, 0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;
        
        
    }
}