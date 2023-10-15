using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] spawnPoints;
    public GameObject alien;
    public int maxAliensOnScreen;
    public int totalAliens;
    public float minSpawnTime;
    public float maxSpawnTime;
    public int aliensPerSpawn;
    private int aliensOnScreen = 0;
    private float generatedSpawnTime = 0;
    private float currentSpawnTime = 0;
    public GameObject upgradePrefab;
    public Gun gun;
    public float upgradeMaxTimeSpawn = 7.5f;
    private bool spawnedUpgrade = false;
    private float actualUpgradeTime = 0;
    private float currentUpgradeTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        actualUpgradeTime = Random.Range(upgradeMaxTimeSpawn - 3.0f,
     upgradeMaxTimeSpawn);
        actualUpgradeTime = Mathf.Abs(actualUpgradeTime);

    }

    // Update is called once per frame
    void Update()
    {
        currentUpgradeTime += Time.deltaTime;
        if (currentUpgradeTime > actualUpgradeTime)
        {
            // 1
            if (!spawnedUpgrade)
            {
                // 2
                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                // 3
                GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
                Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
                upgradeScript.gun = gun;
                upgrade.transform.position = spawnLocation.transform.position;
                // 4
                //currentSpwnTime passed since last update call
                currentSpawnTime += Time.deltaTime;
                spawnedUpgrade = true;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpAppear);
            }
        }

        //condition to generate a new wave of aliens
        if (currentSpawnTime > generatedSpawnTime)
        {
            //resets the timer after a spawn occurs
            currentSpawnTime = 0;

            //spawn-time randomizer
            generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);

            //ensures number of aliens is between limits
            if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens)
            {
                //this list keeps track of where you have already spawned aliens
                List<int> previousSpawnLocations = new List<int>();


                //limits number of aliens to number of spawnpoints
                if (aliensPerSpawn > spawnPoints.Length)
                {
                    aliensPerSpawn = spawnPoints.Length - 1;

                }

                //preventative code to make sure you do not spawn more aliens than configured
                aliensPerSpawn = (aliensPerSpawn > totalAliens) ?
                    aliensPerSpawn - totalAliens : aliensPerSpawn;

                //this code loops once for each spawned alien
                for (int i = 0; i < aliensPerSpawn; i++)
                {
                    if (aliensOnScreen < maxAliensOnScreen)
                    {
                        //keeps track of number of aliens spawned
                        aliensOnScreen += 1;
                        // 1
                        int spawnPoint = -1;
                        // 2
                        while (spawnPoint == -1)
                        {
                            // 3
                            // create random index of list(array) between 0 and spawnpoint
                            int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                            // 4
                            //check to see if random spawnpoint has not  already been used
                            if (!previousSpawnLocations.Contains(randomNumber))
                            {
                                //add this random number to the list
                                previousSpawnLocations.Add(randomNumber);
                                //use this random number for the spawn location index
                                spawnPoint = randomNumber;
                            }
                        }

                        //actual point on arena to spawn next alien
                        GameObject spawnLocation = spawnPoints[spawnPoint];
                        //code to actually create a new alien from a prefab
                        GameObject newAlien = Instantiate(alien) as GameObject;

                        //position the new alien to the random unused spawned point
                        newAlien.transform.position = spawnLocation.transform.position;
                        Alien alienScript = newAlien.GetComponent<Alien>();
                        alienScript.target = player.transform;

                        Vector3 targetRotation = new Vector3(player.transform.position.x,
                                                    newAlien.transform.position.y, player.transform.position.z);
                                                    newAlien.transform.LookAt(targetRotation);
                    }
                }


            }
        }
    }
}
