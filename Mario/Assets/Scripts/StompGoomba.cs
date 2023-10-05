using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompGoomba : MonoBehaviour
{
   public Sprite flatSprite; 
   GameManager gameManager;

   void Start()
    {

        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

    }

   private void OnCollisionEnter2D(Collision2D collision){
    if(collision.gameObject.CompareTag("Player")){
        PlayerMovement mario = collision.gameObject.GetComponent<PlayerMovement>();
        if(collision.transform.DotTest(transform, Vector2.down)){
            Debug.Log("squish");
            Squish();
            gameManager.IncreaseScore(1);
        } else{
            mario.Hit();
        }

    }
   }

    private void Squish(){
    GetComponent<Collider2D>().enabled = false;
    GetComponent<EnemyMovement>().enabled = false;
    GetComponent<SpriteRenderer>().sprite = flatSprite;
    Destroy(gameObject, 1.0f);

   }

}
