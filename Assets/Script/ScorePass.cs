using UnityEngine;

public class ScorePass : MonoBehaviour
{
    public float score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
