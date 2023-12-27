using UnityEngine;

public class ShieldAnimController : MonoBehaviour
{

    [SerializeField]
    private WitchKing witchKing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IsAttack()
    {
        witchKing.IsAttacking();
    }
}
