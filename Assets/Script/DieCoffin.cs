using UnityEngine;
using UnityEngine.SceneManagement;

public class DieCoffin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("ScoreScene");
        PauseHandler pause = FindObjectOfType<PauseHandler>();
        Time.timeScale = pause.normalTimeSCale;
    }
}
