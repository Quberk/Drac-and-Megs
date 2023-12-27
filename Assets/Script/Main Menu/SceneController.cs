using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public GameObject firstGun;
    public GameObject secondGun;
    public GameObject thirdGun;

    public GameObject selected;

    [HideInInspector]
    public GameObject theGun;

    // Start is called before the first frame update
    void Start()
    {
        selected.GetComponent<Button>().Select();
        theGun = firstGun;
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void FirstWeapon()
    {
        theGun = firstGun;
    }

    public void SecondWeapon()
    {
        theGun = secondGun;
    }

    public void ThirdWeapon()
    {
        theGun = thirdGun;
    }
}
