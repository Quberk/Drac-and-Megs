using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManaging : MonoBehaviour
{
    public GameObject coffin;
    public GameObject batDieEffect;
    public GameObject dracEffectObject;

    [Header("Enemy Formation")]
    public GameObject[] enemyFormation;
    [HideInInspector]
    public bool specialFormation = false;

    public float[] waveTime;

    private float waveCounter = 0;
    [HideInInspector]
    public int waveNum = 0;

    public float[] spawnBtwTime;
    private float spawnBtwCounter = 0;

    [HideInInspector]
    public bool rageMode = false;

    private bool alreadySpawn = false;

    private int waveTransitionNum;

    private bool alreadyDead = false;

    [Header("Score")]
    public int score = 0;
    public float scoreTime;
    private float scoreCounter = 0f;
    private Text scoreUI;

    [Header("Obstacle")]
    public GameObject[] obstacle;
    public float[] obstacleBtwTime;
    private float obstacleBtwCounter;
    private bool obstacleSpawn = false;

    [Header("Smasher Enemy")]
    public GameObject smasherEnemy;
    private float timeBtwSpawn;
    private float counterBtwSpawn = 0f;
    private float coolDownTime;
    private float coolDownCounter = 0f;
    private int maxSpawn;
    private int spawnCounter = 0;
    private bool reset = false;

    [Header("SmasherSpam")]
    public float spamTime;
    public float spamSpawnTime;
    private float spamSpawnTimer;
    private float spamCounter = 0f;
    private float spamSpawnCounter = 0f;

    [Header("Wind of Fear")]
    public GameObject windOfFear;
    public GameObject bombObstacle;
    public float bombSpamDuration;
    private float bombSpamCounter = 0f;
    private float bombSpawnCounter = 0f;
    private bool alreadySpawnWindofFear = false;
    [HideInInspector]
    public bool bombSpamReady = false;

    [Header("Blockade Beast")]
    public GameObject blockadeBeast;
    public float blockadeDurationTime;
    public float btwSpawnBlockadeTime;
    private float btwSpawnBlockadeCounter = 0;
    private float blockadeDurationCounter = 0;

    [Header("ShootingEnemy")]
    public GameObject shootingWizard;
    private bool alreadyInstantiate = false;
    private int manyTimes = 0;

    [Header("Witch King")]
    [SerializeField]
    private GameObject witchKing;
    private bool alreadyInstantiateWitch = false;

    [Header("Special Item")]
    public GameObject WeaponBox;
    public float maxXPos;
    private GameObject weponBoxes;
    //private float snitchCounter = 0f;
    private float snitchCoolDown = 60f;

    [Header("Wind")]
    public GameObject[] windEffect;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public float windTime;
    private float windCounter = 0;

    [Header("Speed Controller")]
    private float universalSpeed = 6f;

    private PlayerStat playerHealth;
    private ScorePass scorePassing;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerStat>();
        scorePassing = FindObjectOfType<ScorePass>();
        scoreUI = GameObject.Find("Score_UI").GetComponent<Text>();
        spamSpawnTimer = spamSpawnTime;

        //Smasher Enemy
        maxSpawn = 1;
        coolDownTime = Mathf.Abs(1 - waveTime.Length) * 3f;
        timeBtwSpawn = Mathf.Abs(1 - waveTime.Length) * 1f;
        coolDownCounter = 0f;
        counterBtwSpawn = 0f;
        spawnCounter = 0;

        SpeedController(universalSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        ScoreAdder();
        scoreUI.text = score.ToString();

        //Wave hanya akan dilanjutkan apabila Player tidak sedang Rage Mode
        if (playerHealth.rageUsed == false && reset == false) waveCounter += Time.deltaTime;


        //Wave terdiri dari Obstacle Phase kemudian dilanjutkan dengan Enemy Combo kemudian dilanjutkan dengna transisi Wave
        if (waveCounter >= waveTime[waveNum])
        {
            waveCounter = 0f;
            waveNum++;

            //Mengupdate Universal Speed
            universalSpeed += 0.5f;

            reset = true;
            if (waveNum == waveTime.Length) waveNum = waveTime.Length - 1;

            //Jika Saat transisi Wave ada Gun Box maka akan dihancurkan
            if (FindObjectOfType<SnitchMovement>()) Destroy(FindObjectOfType<SnitchMovement>().gameObject);

            //Memilih Transisi Wave
            int angka = Random.Range(0, 800);
            if (angka < 100)
            {
                waveTransitionNum = 1;
            }

            else if (angka < 200)
            {
                waveTransitionNum = 2;
            }

            else if (angka < 300)
            {
                waveTransitionNum = 3;
            }

            else if (angka < 400)
            {
                waveTransitionNum = 4;
            }

            else if (angka < 500)
            {
                waveTransitionNum = 5;
            }
                
            else
            {
                waveTransitionNum = 6;
            }

            SmasherEnemy(waveNum);
        }

        else if (reset == true) NonActivateObs(); //Menonaktifkan Obstacle ketika memanggil Weapon Box

        else if (waveCounter >= waveTime[waveNum] / 2)
        {
            if (playerHealth.rageUsed == false) SpawnEnemyFormation(waveNum); //Spawn Enemy Biasa
            SmasherEnemy(waveNum); //Spawn Smasher Enemy
        }

        else
        {
            //SnitchController();
            ObstacleSpawner(waveNum); //Spawn Obstacle
        }

        //Jika Player mendapatkan sebuah Rage
        if (playerHealth.rageUsed == true)
        {
            SpawnEnemyFormation(waveNum);
            SpeedController(11f);
        }

        //Selama Player belum Mati maka Update terus Speed
        else if (playerHealth.healthCounter > 0f) SpeedController(universalSpeed);

        else {
            universalSpeed -= Time.deltaTime;
            SpeedController(universalSpeed); }



        //Pergantian Wave
        SmasherSpam(waveNum); //Memasuki tahap Spam Enemy Smasher
        ShootingEnemy(waveNum); //Memasuki tahap ShootingWizard
        WindOfFear(waveNum); //Memasuki tahap Wind of Fear
        BlockadeBeast(waveNum); //Memasuki tahap Blockade Beast
        WitchKing(waveNum);
        SnitchController();

        //Jika PLayer Mati maka akan dialihkan ke Scene Score
        if (playerHealth.healthCounter <= 0 && alreadyDead == false) {
            Vector3 pos = new Vector3(playerHealth.gameObject.transform.position.x - 0.21f, playerHealth.gameObject.transform.position.y + 1.41f, 0f);
            Instantiate(coffin, pos, Quaternion.identity);
            GameObject batEffect = Instantiate(batDieEffect, pos, Quaternion.identity);
            batEffect.transform.SetParent(dracEffectObject.transform);
            batEffect.transform.localPosition = new Vector3(-0.5f, 15.7f, 0f);
            batEffect.transform.localScale = new Vector3(1f, 1f, 1f);
            if (GameObject.FindGameObjectWithTag("BlindEffect")) Destroy(GameObject.FindGameObjectWithTag("BlindEffect"));
            //Time.timeScale = 0f;
            scorePassing.score = score;
            alreadyDead = true;
        }

        WindSpawner();
        
    }

    public void SpeedController(float obstacleSpeed)
    {
        float enemySpeed = 0f;
        enemySpeed = obstacleSpeed - 3f;

        if (obstacleSpeed <= 0) obstacleSpeed = 0f;

        float backGroundSpeed1 = obstacleSpeed * 0.9f;
        float backGroundSpeed2 = backGroundSpeed1 / 2f;
        float backGroundSpeed3 = backGroundSpeed2 / 2f;


        //Obs Speed
        GameObject[] obstacle = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacles in obstacle)
        {
            if (obstacles.GetComponent<BlockadeBeast>()) obstacles.GetComponent<BlockadeBeast>().speed = obstacleSpeed;
            else if (obstacles.GetComponent<BombController>()) obstacles.GetComponent<BombController>().speed = obstacleSpeed;
            else if (obstacles.GetComponent<ObstacleController>()) obstacles.GetComponent<ObstacleController>().speedMove = obstacleSpeed;
            else if (obstacles.GetComponent<ObstaclesCombo>()) obstacles.GetComponent<ObstaclesCombo>().speedMove = obstacleSpeed;
        }

        EnemyMovement[] enemyMovement = FindObjectsOfType<EnemyMovement>();
        EnemyFormation[] enemyFormations = FindObjectsOfType<EnemyFormation>();
        foreach (EnemyMovement enemyMovements in enemyMovement)
        {
            enemyMovements.speed = enemySpeed;
        }

        foreach (EnemyFormation enemyFormations1 in enemyFormations)
        {
            if (enemyFormations1.isCart == true) enemyFormations1.speed = enemySpeed;
        }


        //BackGround1 Speed
        GameObject[] background1 = GameObject.FindGameObjectsWithTag("Background1");
        foreach (GameObject background1s in background1)
        {
            background1s.GetComponent<ParallaxEffect>().parallexEffect = backGroundSpeed1;
        }

        //BackGround2 Speed
        GameObject[] background2 = GameObject.FindGameObjectsWithTag("Background2");
        foreach (GameObject background2s in background2)
        {
            background2s.GetComponent<ParallaxEffect>().parallexEffect = backGroundSpeed2;
        }

        //BackGround3 Speed
        GameObject[] background3 = GameObject.FindGameObjectsWithTag("Background3");
        foreach (GameObject background3s in background3)
        {
            background3s.GetComponent<ParallaxEffect>().parallexEffect = backGroundSpeed3;
        }


    }

    //Fungsi menambah Skor berdasarkan seberapa lama Player bertahan
    void ScoreAdder()
    {
        scoreCounter += Time.deltaTime;
        if (scoreCounter >= scoreTime)
        {
            int w = (int)waveCounter;
            score += w;
            scoreCounter = 0f;
        }
        
    }

    public void SpawnEnemyFormation(int i)
    {
        spawnBtwCounter += Time.deltaTime;

        //Spawn Enemy Jika sudah waktunya (Bukan saat Rage Mode)
        if (spawnBtwCounter >= spawnBtwTime[i] && rageMode == false && specialFormation == false)
        {
            alreadySpawn = false;
            int randomNumber = Random.Range(0, enemyFormation.Length * 100);

            for (int j = 1; j <= enemyFormation.Length; j++)
            {
                if (randomNumber <= j * 100f && alreadySpawn == false)
                {
                    Instantiate(enemyFormation[j - 1], transform.position, Quaternion.identity);
                    alreadySpawn = true;
                    spawnBtwCounter = 0f;
                }
            }
        }

        //Spawn Enemy saat (Rage Mode)
        else if (spawnBtwCounter >= 0.6f && rageMode == true && specialFormation == false)
        {
            alreadySpawn = false;
            int randomNumber = Random.Range(0, enemyFormation.Length * 100);

            for (int j = 1; j <= enemyFormation.Length; j++)
            {
                if (randomNumber <= j * 100f && alreadySpawn == false)
                {
                    Instantiate(enemyFormation[j - 1], transform.position, Quaternion.identity);
                    alreadySpawn = true;
                    spawnBtwCounter = 0f;
                }
            }
        }

    }

    //Fungsi untuk mensummon Obstacle
    void ObstacleSpawner(int wave)
    {
        obstacleBtwCounter += Time.deltaTime;
 
        if (obstacleBtwCounter >= obstacleBtwTime[wave])
        {
            obstacleSpawn = false;
            
            if (wave < 3)
            {
                int randomNumber = Random.Range(0, 500);
                for (int j = 1; j <= 5; j++)
                {
                    if (randomNumber <= j * 100f && obstacleSpawn == false)
                    {
                        Instantiate(obstacle[j - 1], transform.position, Quaternion.identity);

                        obstacleSpawn = true;
                        obstacleBtwCounter = 0f;
                    }
                }
            }

            else
            {
                int randomNumber = Random.Range(0, obstacle.Length * 100);
                for (int j = 1; j <= obstacle.Length; j++)
                {
                    if (randomNumber <= j * 100f && obstacleSpawn == false)
                    {
                        Instantiate(obstacle[j - 1], transform.position, Quaternion.identity);

                        obstacleSpawn = true;
                        obstacleBtwCounter = 0f;
                    }
                }
            }

        }
    }

    void WindSpawner()
    {
        windCounter += Time.deltaTime;

        float windTimer = windTime - (waveNum * 0.1f);

        if (windCounter >= windTimer)
        {
            float xPos = Random.Range(xMin, xMax);
            float yPos = Random.Range(yMin, yMax);
            Vector3 pos = new Vector3(xPos, yPos, 0f);

            int randomNum = Random.Range(1, 3);

            switch (randomNum)
            {
                case 1:
                    Instantiate(windEffect[0], pos, Quaternion.Euler(-180f, 90f, -90));
                    break;
                case 2:
                    Instantiate(windEffect[1], pos, Quaternion.Euler(-180f, 90f, -90));
                    break;
                case 3:
                    Instantiate(windEffect[2], pos, Quaternion.Euler(-180f, 90f, -90));
                    break;
            }
            
            windCounter = 0f;
        }
    }


    //Fungsi untuk memanggil Smash Enemy
    void SmasherEnemy(int wave)
    {
        //Reset Jika Wave telah berganti
        if (reset == true)
        {
            maxSpawn = 1;
            coolDownTime = Mathf.Abs(wave - waveTime.Length) * 3f;
            timeBtwSpawn = Mathf.Abs(wave - waveTime.Length) * 1f;
            coolDownCounter = 0f;
            counterBtwSpawn = 0f;
            spawnCounter = 0;

        }

        else if (reset == false)
        {
            coolDownCounter += Time.deltaTime;
            if (coolDownCounter >= coolDownTime)
            {
                counterBtwSpawn += Time.deltaTime;
                if (counterBtwSpawn >= timeBtwSpawn && spawnCounter < maxSpawn)
                {
                    Instantiate(smasherEnemy, transform.position, Quaternion.identity);
                    counterBtwSpawn = 0f;
                    spawnCounter++;
                }

                else if (spawnCounter == maxSpawn)
                {
                    coolDownCounter = 0f;
                    spawnCounter = 0;
                }
            }
        }
    }

    //Fungsi-fungsi untuk Transisi Wave
    void SmasherSpam(int wave)
    {
        if (reset == true && waveTransitionNum == 1)
        {
            //NonActivateObs();

            spamCounter += Time.deltaTime;
            if (spamCounter >= spamTime)
            {
                reset = false;
                spamCounter = 0f;
                spamSpawnTimer = spamSpawnTime - (wave * 0.1f);
            }

            spamSpawnCounter += Time.deltaTime;
            if (spamSpawnCounter >= spamSpawnTimer)
            {
                Instantiate(smasherEnemy, transform.position, Quaternion.identity);
                spamSpawnCounter = 0f; 
            }

            //Jika Rage Mode sedang aktif
            if (playerHealth.rageUsed == true)
            {
                reset = false;
                waveCounter = waveTime[wave - 1] - 3f;
                waveNum -= 1;
                spamCounter = 0f;
                spamSpawnCounter = 0f;
            }

        }
    }

    void ShootingEnemy(int wave)
    {
        if (reset == true && waveTransitionNum == 2)
        {
            //NonActivateObs();
            if (alreadyInstantiate == false && manyTimes < 3)
            {
                alreadyInstantiate = true;
                manyTimes++;
                if (wave <= 1)
                {
                    int number = Random.Range(1, 5);

                    GameObject enemy = Instantiate(shootingWizard, transform.position, Quaternion.identity);
                    enemy.GetComponent<ShootingWizard>().myNum = number;
                }

                else if (wave <= 3)
                {
                    int rand = Random.Range(0, 300);
                    int number1 = 0;
                    int number2 = 0;
                    //Skenario 1
                    if (rand <= 100)
                    {
                        number1 = 1;
                        number2 = 2;
                    }
                    //Skenario 2
                    else if (rand <= 200)
                    {
                        number1 = 1;
                        number2 = 6;
                    }
                    //Skenario 3
                    else if (rand <= 300)
                    {
                        number1 = 5;
                        number2 = 6;
                    }

                    GameObject enemy1 = Instantiate(shootingWizard, transform.position, Quaternion.identity);
                    enemy1.GetComponent<ShootingWizard>().myNum = number1;

                    GameObject enemy2 = Instantiate(shootingWizard, transform.position, Quaternion.identity);
                    enemy2.GetComponent<ShootingWizard>().myNum = number2;
                }

                //Jika Wave Lebih dari 2
                else
                {

                    int number = Random.Range(1, 5);

                    for (int i = 0; i < 6; i++)
                    {
                        if (i + 1 != number && i + 1 != number + 1)
                        {
                            GameObject enemy = Instantiate(shootingWizard, transform.position, Quaternion.identity);
                            enemy.GetComponent<ShootingWizard>().myNum = i + 1;
                        }

                    }
                }

                
            }

            else
            {
                GameObject[] enemy = GameObject.FindGameObjectsWithTag("ShootingWizard");
                if (enemy.Length == 0 && manyTimes < 3)
                {
                    alreadyInstantiate = false;
                }

                else if (enemy.Length == 0 && manyTimes == 3)
                {
                    reset = false;
                    alreadyInstantiate = false;
                    manyTimes = 0;
                }
            }

            //Jika Rage Mode sedang aktif
            if (playerHealth.rageUsed == true)
            {
                reset = false;
                waveCounter = waveTime[wave - 1] - 3f;
                waveNum -= 1;
                alreadyInstantiate = false;

                GameObject[] enemy = GameObject.FindGameObjectsWithTag("ShootingWizard");
                foreach (GameObject enemies in enemy)
                {
                    //Membuat Puke Enemy selesai menyerang dan pergi
                    enemies.GetComponent<ShootingWizard>().lifeCounter = enemies.GetComponent<ShootingWizard>().lifeTime;
                }
                
            }


        }
        
    }

    void WindOfFear(int wave)
    {
        if (reset == true && waveTransitionNum == 3)
        {
            //NonActivateObs();

            if (bombSpamReady == true)
            {
                AudioSource music = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
                music.volume -= 0.5f;


                bombSpamCounter += Time.deltaTime;
                //Jika waktu telah selesai
                if (bombSpamCounter >= bombSpamDuration)
                {
                    GameObject blindEffect = GameObject.FindGameObjectWithTag("BlindEffect");
                    Destroy(blindEffect);

                    bombSpamReady = false;
                    alreadySpawnWindofFear = false;
                    bombSpamCounter = 0f;
                    bombSpawnCounter = 0f;
                    reset = false;

                    music.volume += 0.5f;
                }

                bombSpawnCounter += Time.deltaTime;
                if (bombSpawnCounter >= obstacleBtwTime[wave] * 2f)
                {
                    GameObject bomb = Instantiate(bombObstacle, transform.position, Quaternion.identity);
                    bomb.GetComponent<BombController>().obstacle = false;
                    bombSpawnCounter = 0f;
                }
            }

            else if (alreadySpawnWindofFear == false)
            {
                Instantiate(windOfFear, transform.position, Quaternion.identity);
                alreadySpawnWindofFear = true;
            }

            //Jika Rage Mode sedang aktif
            if (playerHealth.rageUsed == true)
            {
                reset = false;
                waveCounter = waveTime[wave] - 3f;
                waveNum -= 1;

                bombSpamReady = false;
                alreadySpawnWindofFear = false;
                bombSpamCounter = 0f;
                bombSpawnCounter = 0f;

                GameObject[] enemy1 = GameObject.FindGameObjectsWithTag("BlindEffect");
                foreach (GameObject enemies in enemy1)
                {
                    Destroy(enemies);
                }

                GameObject[] enemy = GameObject.FindGameObjectsWithTag("WindOfFear");
                foreach (GameObject enemies in enemy)
                {
                    //Membuat Puke Enemy selesai menyerang dan pergi
                    enemies.GetComponent<WindFear>().Die();
                }

            }
        }
    }

    void BlockadeBeast(int wave)
    {
        if (reset == true && waveTransitionNum == 4)
        {
            //NonActivateObs();

            blockadeDurationCounter += Time.deltaTime;
            if (blockadeDurationCounter >= blockadeDurationTime + (wave * 2))
            {
                reset = false;
                blockadeDurationCounter = 0f;
                btwSpawnBlockadeCounter = 0f;
            }

            btwSpawnBlockadeCounter += Time.deltaTime;
            if (btwSpawnBlockadeCounter >= btwSpawnBlockadeTime)
            {
                GameObject enemy = Instantiate(blockadeBeast, transform.position, Quaternion.identity);
                enemy.GetComponent<BlockadeBeast>().speed += (wave * 0.5f);
                btwSpawnBlockadeCounter = 0f;
            }

            //Jika Player menggunakan Rage
            if (playerHealth.rageUsed == true)
            {
                reset = false;
                blockadeDurationCounter = 0f;
                btwSpawnBlockadeCounter = 0f;
            }
        }
    }

    void WitchKing(int wave)
    {
        if (reset == true && waveTransitionNum == 5)
        {
            if (alreadyInstantiateWitch == false)
            {
                alreadyInstantiateWitch = true;
                GameObject witch = Instantiate(witchKing, transform.position, Quaternion.identity);
                witch.GetComponent<WitchKing>().setWaveNum(wave);
            }

            if (GameObject.FindGameObjectsWithTag("WitchKing").Length == 0) reset = false;
        }


    }


    void SnitchController()
    {
        if (reset == true && waveTransitionNum == 6 /*&& playerHealth.rageUsed == false*/)
        {
            if (playerHealth.rageUsed == true)
            {
                reset = false;
                weponBoxes = null;
            }
            //Terpanggil apabila ENemy sudah tidak ada
            else if (!GameObject.FindGameObjectWithTag("Snitch") && FindObjectsOfType<EnemyMovement>().Length == 0) weponBoxes = Instantiate(WeaponBox, transform.position, Quaternion.identity);

            if (weponBoxes != null)
            {
                if (weponBoxes.transform.position.x <= maxXPos)
                {
                    reset = false;
                    Destroy(GameObject.FindGameObjectWithTag("Snitch"));
                }
            }

        }
    }

    //Fungsi untuk menonaktifkan Obstacle Normal
    void NonActivateObs()
    {
        //Menonaktifkan Obstacle
        GameObject[] obstacle = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacles in obstacle)
        {
            if (obstacles.GetComponent<ObstacleController>()) obstacles.GetComponent<ObstacleController>().Deactivate();
            else if (obstacles.GetComponent<BombController>()) obstacles.GetComponent<BombController>().Deactivate();
        }
    }
}
