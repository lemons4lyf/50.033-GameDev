using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    public float maxSpeed = 20;
    public float upSpeed = 10;
    private Rigidbody2D marioBody;
    private bool onGroundState = true;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    public TextMeshProUGUI scoreText;
    public GameObject enemies;

    public GameManagerScript gameManager;
    public Animator marioAnimator;


    // Start is called before the first frame update
    void Start()
    {
         // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        marioAnimator.SetBool("onGround", onGroundState);
        
    }

    // Update is called once per frame
    void Update()
    {
         // toggle state
        if (Input.GetKeyDown("a") && faceRightState){
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.1f)
                marioAnimator.SetTrigger("onSkid");
      }

        if (Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.velocity.x < -0.1f)
                marioAnimator.SetTrigger("onSkid");
      }
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));


      
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
      if (col.gameObject.CompareTag("Ground") && !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    void  FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // check if it doesn't go beyond maxSpeed
            if (marioBody.velocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
        }

        // stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            // stop
            marioBody.velocity = Vector2.zero;
        }
        // other instructions
        if (Input.GetKeyDown("space") && onGroundState){
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
        }     

    void OnTriggerEnter2D(Collider2D other)
     {
      if (other.gameObject.CompareTag("Enemy"))
      {
          Debug.Log("Collided with goomba!");
          Time.timeScale = 0.0f;
          gameManager.gameOver();
      }
     }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    } 

    // for audio
    public AudioSource marioAudio;

        void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        scoreText.text = "Score: 0";
        // reset Goomba

        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }

    }

}