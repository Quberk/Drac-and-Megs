using UnityEngine;

public class FollowingTarget : MonoBehaviour
{
    [SerializeField]
    private string targetTag;
    private GameObject followingTarget;

    [SerializeField]
    private float xOffset;
    [SerializeField]
    private float yOffset;
    [SerializeField]
    private float zOffset;


    // Start is called before the first frame update
    void Start()
    {
        followingTarget = GameObject.FindGameObjectWithTag(targetTag);            
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(followingTarget.transform.position.x + xOffset,
                                           followingTarget.transform.position.y + yOffset,
                                           followingTarget.transform.position.z + zOffset);
    }
}
