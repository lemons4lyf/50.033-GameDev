using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    private bool isDead;
    public GameObject gameOverUI;
    public GameObject restartButton;
    public GameObject score;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameOver(){
        if(!isDead){
            isDead = true;
            gameOverUI.SetActive(true);
            score.SetActive(false);
            restartButton.SetActive(false);
        }
    }

}
