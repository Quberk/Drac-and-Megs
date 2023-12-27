using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallRocketSpawner : MonoBehaviour
{
    [Header("Rocket Launcher")]
    public GameObject[] projectile;
    public int maxRocket;
    private GameObject[] enemy;

    public float waitTime;
    private float waitCounter = 0f;

    private int num = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        waitCounter += Time.deltaTime;
        if (waitCounter >= waitTime)
        {
            enemy = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemy.Length; i++)
            {
                if (num == projectile.Length) num = 0;
                GameObject bullet = Instantiate(projectile[num], transform.position, Quaternion.identity);
                bullet.transform.localScale = new Vector3(0.4915f, 0.4915f, 0.4915f);
                bullet.GetComponent<Bullet>().target = enemy[i];
                num++; 
            }
            Destroy(gameObject);
        }
    }
}
