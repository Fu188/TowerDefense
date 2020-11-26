// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using System.Linq;
// using System;
// using Assets.Scripts;

// public class GameManager : MonoBehaviour
// {
//     [HideInInspector]
//     public static GameManager Instance{get;private set;}
//     void Awake(){
//         Instance = this;
//     }
//     public List<GameObject> Enemies;
//     public GameObject EnemyPrefab;
//     public GameObject PathPrefab;
//     public GameObject TowerPrefab;
//     public Transform[] Waypoints;
//     private GameObject PathPiecesParent;
//     private GameObject WaypointsParent;
//     private LevelStuffFromXML levelStuffFromXML;
//     public RandomCoin CoinSpawner;
//     [HideInInspector]
//     public int MoneyAvailable{get;private set;}
//     [HideInInspector]
//     public float MinSpawnTime;
//     [HideInInspector]
//     public float MaxSpawnTime;
//     public int Lives = 10;
//     private int currentRoundIndex = 0;
//     [HideInInspector]
//     public GameState CurrentGameState;
//     public SpriteRenderer PeopleGeneratorSprite;
//     [HideInInspector]
//     public bool FinalRoundFinished;
//     public Text infoText;
//     private object lockerObject = new object();

//     // Start is called before the first frame update
//     void Start()
//     {
//         IgnoreLayerCollisions();
//         Enemies = new List<GameObject>();
//         PathPiecesParent = GameObject.Find("PathPieces");
//         WaypointsParent = GameObject.Find("Waypoints");
//         levelStuffFromXML = Utilities.ReadXMLFile();
//         CreateLevelFromXML();
//         CurrentGameState = GameState.Start;
//         FinalRoundFinished = false;
//     }

//     private void CreateLevelFromXML(){
//         foreach (var position in levelStuffFromXML.Paths){
//             GameObject go = Instantiate(PathPrefab,position,Quaternion.identity) as GameObject;
//             go.GetComponent<SpriteRenderer>().sortingLayerName = "Path";
//             go.transform.parent = PathPiecesParent.transform;
//         }

//         for(int i=0;i<levelStuffFromXML.Waypoints.Count;i++){
//             GameObject go = new GameObject();
//             go.transform.position = levelStuffFromXML.Waypoints[i];
//             go.transform.parent = WaypointsParent.transform;
//             go.tag = "Waypoints";
//             go.name = "Waypoints"+i.ToString();
//         }

//         GameObject tower = Instantiate(TowerPrefab,levelStuffFromXML.Tower,Quaternion.identity) as GameObject;
//         tower.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
//         Waypoints = GameObject.FindGameObjectsWithTag("Waypoints").OrderBy(x=>x.name).Select(x=>x.transform).ToArray();
//         MoneyAvailable = levelStuffFromXML.InitialMoney;
//         MinSpawnTime = levelStuffFromXML.MinSpawnTime;
//         MaxSpawnTime = levelStuffFromXML.MaxSpawnTime;
//     }

//     // make the arrow collide only with enemies
//     private void IgnoreLayerCollisions(){
//         int peopleLayerID = LayerMask.NameToLayer("People");
//         int enemyLayerID = LayerMask.NameToLayer("Enemy");
//         int arrowLayerID = LayerMask.NameToLayer("Arrow");
//         int PeopleGeneratorLayerID = LayerMask.NameToLayer("PeopleGenerator");
//         int backgroundLayerID = LayerMask.NameToLayer("Background");
//         int pathLayerID = LayerMask.NameToLayer("Path");
//         int towerLayerID = LayerMask.NameToLayer("Tower");
//         int coinLayerID = LayerMask.NameToLayer("Coin");
//         Physics2D.IgnoreLayerCollision(peopleLayerID,enemyLayerID);
//         Physics2D.IgnoreLayerCollision(arrowLayerID,PeopleGeneratorLayerID);
//         Physics2D.IgnoreLayerCollision(arrowLayerID,backgroundLayerID);
//         Physics2D.IgnoreLayerCollision(arrowLayerID,pathLayerID);
//         Physics2D.IgnoreLayerCollision(arrowLayerID,peopleLayerID);
//         Physics2D.IgnoreLayerCollision(arrowLayerID,towerLayerID);
//         Physics2D.IgnoreLayerCollision(arrowLayerID,coinLayerID);
//     }

//     IEnumerator NextRound(){
//         // give the player 2 secs to do stuff
//         yield return new WaitForSeconds(2f);
//         // get a reference to the next round details
//         Round currentRound = levelStuffFromXML.Rounds[currentRoundIndex];
//         for(int i=0;i<currentRound.NoOfEnemies_low;i++){
//             GameObject enemy = Instantiate(EnemyPrefab,Waypoints[0].position,Quaternion.identity) as GameObject;
//             Enemy enemyComponent = enemy.GetComponent<Enemy>();
//             enemyComponent.Speed += Mathf.Clamp(currentRoundIndex,1f,5f);
//             enemyComponent.EnemyKilled += OnEnemyKilled;
//             Enemies.Add(enemy);
//             yield return new WaitForSeconds(1f/(currentRoundIndex==0?1:currentRoundIndex));
//         }
//     }

//     void OnEnemyKilled(object sender,EventArgs e){
//         bool startNewRound = false;
//         lock(lockerObject){
//             if(Enemies.Where(x=>x!=null).Count()==0 && CurrentGameState==GameState.Playing){
//                 startNewRound = true;
//             }
//         }
//         if(startNewRound){
//             CheckAndStartNewRound();
//         }
//     }

//     private void CheckAndStartNewRound(){
//         if(currentRoundIndex<levelStuffFromXML.Rounds.Count-1){
//             currentRoundIndex++;
//             StartCoroutine(NextRound());
//         }else{
//             FinalRoundFinished = true;
//         }
//     }

//     void Update(){
//         switch (CurrentGameState)
//         {
//             case GameState.Start:
//                 if(Input.GetMouseButtonUp(0)){
//                     CurrentGameState = GameState.Playing;
//                     StartCoroutine(NextRound());
//                     CoinSpawner.StartCoinSpawning();
//                 }
//                 break;
//             case GameState.Playing:
//                 if(Lives==0){
//                     // no more rounds
//                     StopCoroutine(NextRound());
//                     DestroyExistingEnemiesAndCoins();
//                     CoinSpawner.StopCoinSpawning();
//                     CurrentGameState = GameState.Lost;
//                 }else if(FinalRoundFinished && Enemies.Where(x=>x!=null).Count()==0){
//                     DestroyExistingEnemiesAndCoins();
//                     CoinSpawner.StopCoinSpawning();
//                     CurrentGameState = GameState.Won;
//                 }
//                 break;
//             case GameState.Won:
//                 if(Input.GetMouseButtonUp(0)){
//                     // restart
//                     Application.LoadLevel(Application.loadedLevel);
//                 }
//                 break;
//             case GameState.Lost:
//                 if(Input.GetMouseButtonUp(0)){
//                     // restart
//                     Application.LoadLevel(Application.loadedLevel);
//                 }
//                 break;
//             default:
//                 break;
//         }
//     }

//     private void DestroyExistingEnemiesAndCoins(){
//         // get all the enemies
//         foreach(var item in Enemies){
//             if(item!=null){
//                 Destroy(item.gameObject);
//             }
//         }
//         var coins = GameObject.FindGameObjectsWithTag("Coin");
//         // get all the coins
//         foreach(var item in coins){
//             Destroy(item);
//         }
//     }

//     public void AlterMoneyAvailable(int money){
//         MoneyAvailable += money;
//         if (MoneyAvailable < Constants.PeopleCost){
//             Color temp = PeopleGeneratorSprite.color;
//             temp.a = 0.3f;
//             PeopleGeneratorSprite.color = temp;
//         }else{
//             Color temp = PeopleGeneratorSprite.color;
//             temp.a = 1.0f;
//             PeopleGeneratorSprite.color = temp;
//         }
//     }

//     void OnGUI(){
//         Utilities.AutoResize(800,480);
//         switch (CurrentGameState){
//             case GameState.Start:
//                 infoText.text = "Tap to start";
//                 break;
//             case GameState.Playing:
//                 infoText.text = "Money: "+MoneyAvailable.ToString() + "\n" + "Life: " + Lives.ToString()+"\n"+string.Format("round {0} of {1}",currentRoundIndex+1,levelStuffFromXML.Rounds.Count);
//                 break;
//             case GameState.Won:
//                 infoText.text = "Won! Tap to restart!";
//                 break;
//             case GameState.Lost:
//                 infoText.text = "Lost! Tap to restart!";
//                 break;
//             default:
//                 break;
//         }
//     }
// }
