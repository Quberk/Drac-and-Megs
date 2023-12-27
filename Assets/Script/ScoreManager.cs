using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public float score;
    public Text myScore;

    public Text highScore;

    private ScorePass scorePass;

    // Start is called before the first frame update
    void Start()
    {
        scorePass = FindObjectOfType<ScorePass>();
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        score = scorePass.score;

        string score1 = score.ToString();
        myScore.text = score1;

        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", (int)score); 
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

}
