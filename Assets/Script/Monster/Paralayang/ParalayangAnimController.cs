using UnityEngine;

public class ParalayangAnimController : MonoBehaviour
{

    private ParalayangEating paralayangEating;
    // Start is called before the first frame update
    void Start()
    {
        paralayangEating = FindObjectOfType<ParalayangEating>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FirstPos()
    {
        paralayangEating.enemyPosition = paralayangEating.enemyPos[0].transform.position;
    }

    public void SecondPos()
    {
        paralayangEating.enemyPosition = paralayangEating.enemyPos[1].transform.position;
    }

    public void ThirdPos()
    {
        paralayangEating.enemyPosition = paralayangEating.enemyPos[2].transform.position;
        paralayangEating.finishedAttacking = true;
    }
}
