using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class CoinSpawner : MonoBehaviour
{
    [HideInInspector]
    public static CoinSpawner Instance{get;private set;}
    public float spawnTime = 1.0f;
    void Awake(){
        Instance = this;
    }

    public void StartCoinSpawning(){
        StartCoroutine(SpawnCoin());
    }

    public void StopCoinSpawning(){
        StopAllCoroutines();
    }

    public static CoinSpawner GetCoinSpawnInstance(){
        return Instance;
    }

    public void MulCoinSpawnSpeed(float mul, float time){
        spawnTime *= mul;
        Invoke("resetSpawnSpeed",time);
    }

    public void resetSpawnSpeed(){
        spawnTime = 1.0f;
    }

    private IEnumerator SpawnCoin(){
        while(true){
            // select a random position
            Vector2 p = Random.insideUnitCircle * 80;
            Vector3 randomPosition = new Vector3(p.x,-0.2f,p.y);
            GameObject go = ObjectPoolerManager.Instance.CoinPooler.GetPooledObject();
            go.transform.position = randomPosition;
            go.SetActive(true);
            GameManager.Instance.Coins.Add(go);
            ((Coin)go.GetComponent(typeof(Coin))).startInvoke();
            yield return new WaitForSeconds(Constants.CoinSpawnSplitTime/spawnTime);
        }
    }
}
