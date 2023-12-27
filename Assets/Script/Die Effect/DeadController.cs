using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadController : MonoBehaviour
{
    [SerializeField]
    private float dyingTime;
    private float dyingCounter = 0f;

    private GameObject playerGfx;
    private GameObject headGfx;
    private GameObject gunGfx;
    private GameObject playerVfx;

    // Start is called before the first frame update
    void Start()
    {
        playerGfx = GameObject.Find("PlayerGFX");
        headGfx = GameObject.Find("HeadGFX");
        gunGfx = GameObject.Find("Gun");
        playerVfx = GameObject.Find("Player_Effects");
    }

    // Update is called once per frame
    void Update()
    {
        dyingCounter += Time.deltaTime;

        if (dyingCounter >= dyingTime) ChangeScene();
        else if (dyingCounter >= dyingTime * 0.75f) playerVfx.SetActive(false);
        else if (dyingCounter >= dyingTime * 0.5f)
        {
            //Mengurangi intensitas Gambar Sprite dari Grafik Player
            playerGfx.GetComponent<SpriteRenderer>().enabled = false;
            headGfx.GetComponent<SpriteRenderer>().enabled = false;
            
        }

        gunGfx.SetActive(false);

    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("ScoreScene");
        
        //PauseHandler pause = FindObjectOfType<PauseHandler>();
        //Time.timeScale = pause.normalTimeSCale;
    }

}
