using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { private set; get; }

    [Header("Spawn Options")]
    [SerializeField]
    private GameObject[] spawners;
    [SerializeField]
    private GameObject enemyPrefab,
                       enemyBPrefab;
    [SerializeField]
    private float spawnerRadius = 10f;

    public int EnemiesCreated { set; get; }

    private void Awake() {
        Instance = this;
        StartCoroutine("Spawning");
    }

    private LinkedList<GameObject> CheckSpawners()
    {
        LinkedList<GameObject> activeSpawners = new LinkedList<GameObject>();
        foreach (GameObject spawn in spawners)
        {
            if (Vector3.Distance(spawn.transform.position, Player.Instance.transform.position) <= spawnerRadius)
            {
                activeSpawners.AddLast(spawn);
            }
        }
        return activeSpawners;
    }

    private IEnumerator Spawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            LinkedList<GameObject> tmp = CheckSpawners();

            foreach (GameObject spawn in tmp)
            {
                if (EnemiesCreated < GameManager.Instance.KillsToEnd)
                {
                    EnemiesCreated++;
                    //print("Creado el "+EnemiesCreated);
                    
                    if (Random.Range(0f, 1f) > 0.5f)
                    {
                        Instantiate(enemyPrefab, spawn.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(enemyBPrefab, spawn.transform.position, Quaternion.identity);
                    }
                    
                }
                
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (GameObject spawn in spawners)
        {
            float theta = 0;
            float x = spawnerRadius * Mathf.Cos(theta);
            float y = spawnerRadius * Mathf.Sin(theta);
            Vector3 pos = spawn.transform.position + new Vector3(x, 0, y);
            Vector3 newPos = pos;
            Vector3 lastPos = pos;
            for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
            {
                x = spawnerRadius * Mathf.Cos(theta);
                y = spawnerRadius * Mathf.Sin(theta);
                newPos = spawn.transform.position + new Vector3(x, 0, y);
                Gizmos.DrawLine(pos, newPos);
                pos = newPos;
            }
            Gizmos.DrawLine(pos, lastPos);


        }
    }
}
