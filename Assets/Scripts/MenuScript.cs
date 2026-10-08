using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public TMP_Text highScoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float getHighscore = PlayerPrefs.GetFloat("Highscore");
        highScoreText.text = "Highscore: " + getHighscore.ToString() + "$";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
