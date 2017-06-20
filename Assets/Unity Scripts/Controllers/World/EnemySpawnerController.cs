using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    //5 seconds
    public float timer_spawn = 5f;

    GameObject enemy1_prefab;
    
    
    // Use this for initialization
	void Start ()
    {
        enemy1_prefab = Resources.Load("World/Enemy") as GameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer_spawn -= Time.deltaTime;

        if (timer_spawn <= 0)
        {
            SpawnEnemy();
            timer_spawn = Random.Range(1, 5);
        }
        
    }

    
    void SpawnEnemy()
    {
        GameObject enemy = ObjectPoolManager.CreatePooled(enemy1_prefab, Vector3.zero, Quaternion.Euler(0, 0, 0));

        enemy.transform.position = new Vector2(Random.Range(-75,75), Random.Range(50,50));
        
        enemy.transform.SetParent(this.transform);//set as parent
    }


    

}
