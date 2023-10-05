using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{   
    private Vector3[] scoreTextPosition = {
        new Vector3(285, 194, 0),
        new Vector3(3, -30, 0)
        };
    private Vector3[] resetButtonPosition = {
        new Vector3(-369, 189, 0),
        new Vector3(0, -138, 0)
    };

    public GameObject gameOverUI;
    public Transform resetButton;
    public GameObject scoreText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart(){

         // hide gameover panel
        gameOverUI.SetActive(false);
        scoreText.transform.localPosition = scoreTextPosition[0];
        resetButton.localPosition = resetButtonPosition[0];

    }

    public void SetScore(int score){
    
        Debug.Log("Score Updated");
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();

    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[1];
        resetButton.localPosition = resetButtonPosition[1];
    }

}
