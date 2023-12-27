using UnityEngine;

public class ParticleSystemIgnoreTime : MonoBehaviour
{
    private ParticleSystem myParticle;
    // Start is called before the first frame update
    void Start()
    {
        myParticle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        myParticle.Simulate(Time.unscaledDeltaTime, true, false);
    }
}
