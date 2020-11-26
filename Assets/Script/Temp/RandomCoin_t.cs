// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class RandomCoin : MonoBehaviour
// {
//     public GameObject Coin;

//     public void StartCoinSpawning(){
//         StartCoroutine(SpawnCoins());
//     }

//     public void StopCoinSpawning(){
//         StopAllCoroutines();
//     }

//     private IEnumerator SpawnCoins(){
//         while(true){
//             float X = Random.Range(100,Screen.width-100);
//             float Y = Random.Range(100,Screen.height-100);
//             Vector3 randomPosition = Camera.main.ScreenToWorldPoint(new Vector3(X,Y,0));
//             //create and drop a coin
//             GameObject coin = Instantiate(Coin,new Vector3(randomPosition.x,randomPosition.y,transform.position.z),Quaternion.identity) as GameObject;
//             yield return new WaitForSeconds(Random.Range(GameManager.Instance.MinSpawnTime,GameManager.Instance.MaxSpawnTime));
//         }
//     }
// }
