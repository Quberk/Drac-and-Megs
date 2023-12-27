using UnityEngine;

public class TouchScreenBtn : MonoBehaviour
{
    private CharacterMovement characterMovement;

    // Start is called before the first frame update
    void Start()
    {
        characterMovement = FindObjectOfType<CharacterMovement>();    
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Clicked()
    {
        characterMovement.clickedBtn = true;
    }
}
