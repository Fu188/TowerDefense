using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Assets.Scripts;
using UnityEngine.UI;
using Photon.Pun;
public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    public List<GameObject> Enemies;
    public List<GameObject> Coins;
    public List<GameObject> Peoples;

    public List<GameObject> KkPeoplePrefabs;

    public GameObject peoplePrefab_0;
    public GameObject peoplePrefab_1;
    public GameObject peoplePrefab_2;
    public GameObject peoplePrefab_3;
    public GameObject peoplePrefab_4;
    public GameObject peoplePrefab_5;
    public GameObject peoplePrefab_6;
    public GameObject peoplePrefab_7;
    public GameObject peoplePrefab_8;
    public GameObject peoplePrefab_9;
    public GameObject peoplePrefab_10;
    public GameObject peoplePrefab_11;
    public GameObject peoplePrefab_12;
    public static Transform peopleTransform;
    public GameObject EnemyPrefab1;
    public GameObject EnemyPrefab2;
    public GameObject EnemyPrefab3;
    public GameObject EnemyPrefab4;
    public GameObject EnemyPrefab5;
    public GameObject EnemyPrefab6;
    public GameObject keyPrefab;
    public GameObject EnemyGeneratorPrefab;
    public GameObject PathPrefab;
    public GameObject pathList;
    public GameObject enemyList;
    public GameObject waypointsList;
    public GameObject otherObjectList;
    public Transform[] Waypoints;
    public GameObject TargetEffectPrefab;
    public GameObject TargetEffect;
    public LevelStuffFromXML levelStuffFromXML;
    [HideInInspector]
    public GameObject EnemyPrefab;
    [HideInInspector]
    public static int MoneyAvailable { get; private set; }

    public int Lives = 10;
    [HideInInspector]
    public GameState CurrentGameState;
    public int currentRoundIndex = 0;
    [HideInInspector]
    public bool FinalRoundFinished;
    private object lockerObject = new object();


    public Text WaveNumText;
    public Text EnergyNumText;
    public Text HPNumText;
    public string WaveNum;
    public string HPNum;

    // Start is called before the first frame update
    void Start()
    {
        IgnoreLayerCollisions();
        Enemies = new List<GameObject>();
        Coins = new List<GameObject>();
        levelStuffFromXML = Utilities.ReadXMLFile();
        CreateLevelFromXML();
        CurrentGameState = GameState.Start;
        FinalRoundFinished = false;

        HPNum = " / " + GameManager.Instance.Lives;
        WaveNum = " / " + GameManager.Instance.levelStuffFromXML.Rounds.Count;
    }

    private void IgnoreLayerCollisions()
    {
        int enemyLayerID = LayerMask.NameToLayer("Enemy");
        // int enemyGeneratorLayerID = LayerMask.NameToLayer("EnemyGenerator");
        int pathLayerID = LayerMask.NameToLayer("Path");
        int backgroundID = LayerMask.NameToLayer("Background");
        // Physics.IgnoreLayerCollision(enemyLayerID,enemyGeneratorLayerID);
        Physics.IgnoreLayerCollision(enemyLayerID, enemyLayerID);
        Physics.IgnoreLayerCollision(enemyLayerID, pathLayerID);
        // Physics.IgnoreLayerCollision(enemyLayerID,backgroundID);
        // Physics.IgnoreLayerCollisions()
    }

    private void CreateLevelFromXML()
    {
        foreach (var attr in levelStuffFromXML.Paths)
        {
            GameObject go = Instantiate(PathPrefab, attr[0], Quaternion.Euler(attr[1])) as GameObject;
            go.transform.SetParent(pathList.transform);
        }

        for (int i = 0; i < levelStuffFromXML.Waypoints.Count; i++)
        {
            GameObject go = new GameObject();
            go.transform.position = levelStuffFromXML.Waypoints[i];
            go.tag = "Waypoints";
            go.name = i.ToString();
            go.transform.SetParent(waypointsList.transform);
        }

        GameObject tower = Instantiate(keyPrefab, levelStuffFromXML.Tower[0], Quaternion.Euler(levelStuffFromXML.Tower[1])) as GameObject;
        tower.transform.SetParent(otherObjectList.transform);
        GameObject EnemyGenerator = Instantiate(EnemyGeneratorPrefab, levelStuffFromXML.EnemyGenerator[0], Quaternion.Euler(levelStuffFromXML.EnemyGenerator[1])) as GameObject;
        EnemyGenerator.transform.SetParent(otherObjectList.transform);
        Waypoints = GameObject.FindGameObjectsWithTag("Waypoints").OrderBy(x => int.Parse(x.name)).Select(x => x.transform).ToArray();
        MoneyAvailable = levelStuffFromXML.InitialMoney;
    }

    IEnumerator NextRound()
    {
        // stop for 2 secs
        yield return new WaitForSeconds(2f);
        Round currentRound = levelStuffFromXML.Rounds[currentRoundIndex];
        int NoOfEnemies = currentRound.NoOfEnemy1 + currentRound.NoOfEnemy2 + currentRound.NoOfEnemy3 + currentRound.NoOfEnemy4 + currentRound.NoOfEnemy5 + currentRound.NoOfEnemy6;
        for (int i = 0; i < NoOfEnemies; i++)
        {
            if (i < currentRound.NoOfEnemy1) EnemyPrefab = EnemyPrefab1;
            else if (i < currentRound.NoOfEnemy1 + currentRound.NoOfEnemy2) EnemyPrefab = EnemyPrefab2;
            else if (i < currentRound.NoOfEnemy1 + currentRound.NoOfEnemy2 + currentRound.NoOfEnemy3) EnemyPrefab = EnemyPrefab3;
            else if (i < currentRound.NoOfEnemy1 + currentRound.NoOfEnemy2 + currentRound.NoOfEnemy3 + currentRound.NoOfEnemy4) EnemyPrefab = EnemyPrefab4;
            else if (i < currentRound.NoOfEnemy1 + currentRound.NoOfEnemy2 + currentRound.NoOfEnemy3 + currentRound.NoOfEnemy4 + currentRound.NoOfEnemy5) EnemyPrefab = EnemyPrefab5;
            else EnemyPrefab = EnemyPrefab6;
            GameObject enemy = Instantiate(EnemyPrefab, Waypoints[0].position, Quaternion.Euler(levelStuffFromXML.Enemy)) as GameObject;
            enemy.transform.SetParent(enemyList.transform);
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            enemyComponent.Speed = Mathf.Clamp(currentRoundIndex, enemyComponent.Speed, 2 * enemyComponent.Speed);
            enemyComponent.EnemyKilled += OnEnemyKilled;
            Enemies.Add(enemy);
            yield return new WaitForSeconds(1.5f / (currentRoundIndex == 0 ? 1 : currentRoundIndex));
        }
    }

    void OnEnemyKilled(object sender, EventArgs e)
    {
        bool startNewRound = false;
        lock (lockerObject)
        {
            if (Enemies.Where(x => x != null).Count() == 0 && CurrentGameState == GameState.Playing)
            {
                startNewRound = true;
            }
        }
        if (startNewRound)
        {
            CheckAndStartNewRound();
        }
    }

    private void CheckAndStartNewRound()
    {
        if (currentRoundIndex < levelStuffFromXML.Rounds.Count - 1)
        {
            currentRoundIndex++;
            StartCoroutine(NextRound());
        }
        else
        {
            FinalRoundFinished = true;
        }
    }

    public static void AlterMoney(int money)
    {
        MoneyAvailable += money;
    }

    // Update is called once per frame
    void Update()
    {
        if (Peoples.Count!=0)
        {
            GameObject targetPeople = Peoples.Where(x => x != null)
           .Aggregate((current, next) => Vector3.Distance(current.transform.position, peopleTransform.position)
           < Vector3.Distance(next.transform.position, peopleTransform.position) ? current : next);
            if (Vector3.Distance(targetPeople.transform.position, peopleTransform.position) < Constants.MinDistanceToDestroyPeople)
            {
                if (TargetEffect != null)
                {
                    TargetEffect.transform.position = targetPeople.transform.position + new Vector3(0, 0.8f, 0);
                }
                else
                {
                    TargetEffect = Instantiate(TargetEffectPrefab, targetPeople.transform.position + new Vector3(0, 0.8f, 0), targetPeople.transform.rotation);
                }
            }
            else
            {
                Destroy(TargetEffect);
            }
        }
        switch (CurrentGameState)
        {
            case GameState.Start:
                if (Input.GetMouseButtonUp(0))
                {
                    CurrentGameState = GameState.Playing;
                    CoinSpawner.Instance.StartCoinSpawning();
                    StartCoroutine(NextRound());
                }
                break;
            case GameState.Playing:
                // we lost
                if (Lives <= 0)
                {
                    StopCoroutine(NextRound());
                    DestroyExistingEnemiesAndCoins();
                    CoinSpawner.Instance.StopCoinSpawning();
                    CurrentGameState = GameState.Lost;
                }
                else if (FinalRoundFinished && Enemies.Where(x => x != null).Count() == 0)
                {
                    DestroyExistingEnemiesAndCoins();
                    CoinSpawner.Instance.StopCoinSpawning();
                    CurrentGameState = GameState.Won;
                }
                break;
            case GameState.Won:
                if (Input.GetMouseButtonUp(0))
                {
                    //TODO
                    print("WIN");
                }
                break;
            case GameState.Lost:
                if (Input.GetMouseButtonUp(0))
                {
                    //TODO
                    print("LOST");
                }
                break;
        }


        WaveNumText.text = (currentRoundIndex + 1) + WaveNum;
        EnergyNumText.text = MoneyAvailable.ToString();
        HPNumText.text = Lives + HPNum;
    }

    private void DestroyExistingEnemiesAndCoins()
    {
        //get all the enemies
        foreach (var item in Enemies)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
        //get all the coins
        foreach (var item in Coins)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
    }

    public void createPeople(int peopleType)
    {
        GameObject go;
        go = GameNetworkManager.KkInstantiate(KkPeoplePrefabs[peopleType], peopleTransform.position, peopleTransform.rotation, 0);
        //go = GameObject.Instantiate(KkPeoplePrefabs[peopleType], peopleTransform.position, peopleTransform.rotation);
        int moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
        /*
        switch (peopleType){
            case 0:
                go = GameObject.Instantiate(peoplePrefab_0,peopleTransform.position,peopleTransform.rotation);
                moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
                break;
            case 1:
                go = GameObject.Instantiate(peoplePrefab_1,peopleTransform.position,peopleTransform.rotation);
                moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
                break;
            case 2:
                go = GameObject.Instantiate(peoplePrefab_2,peopleTransform.position,peopleTransform.rotation);
                moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
                break;
            case 3:
                go = GameObject.Instantiate(peoplePrefab_3,peopleTransform.position,peopleTransform.rotation);
                moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
                break;
            case 4:
                go = GameObject.Instantiate(peoplePrefab_4,peopleTransform.position,peopleTransform.rotation);
                moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
                break;
            case 5:
                go = GameObject.Instantiate(peoplePrefab_5,peopleTransform.position,peopleTransform.rotation);
                moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
                break;  
            case 6:
                go = GameObject.Instantiate(peoplePrefab_6,peopleTransform.position,peopleTransform.rotation);
                moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
                break;
            case 7:
                go = GameObject.Instantiate(peoplePrefab_7,peopleTransform.position,peopleTransform.rotation);
                moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
                break;
            case 8:
                go = GameObject.Instantiate(peoplePrefab_8,peopleTransform.position,peopleTransform.rotation);
                moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
                break;
            case 9:
                go = GameObject.Instantiate(peoplePrefab_9,peopleTransform.position,peopleTransform.rotation);
                moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
                break;
            case 10:
                go = GameObject.Instantiate(peoplePrefab_10,peopleTransform.position,peopleTransform.rotation);
                moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
                break;
            case 11:
                go = GameObject.Instantiate(peoplePrefab_11,peopleTransform.position,peopleTransform.rotation);
                moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
                break;
            case 12:
                go = GameObject.Instantiate(peoplePrefab_12,peopleTransform.position,peopleTransform.rotation);
                moneyCost = ((People)go.GetComponent(typeof(People))).getCreateCost();
                break;  
        }*/
        if (MoneyAvailable < moneyCost) 
        { 
            Destroy(go); 
        }
        else
        {
            AlterMoney(-moneyCost);
            Peoples.Add(go);
        }
    }

    public void removePeople()
    {
        GameObject targetPeople = Peoples.Where(x => x != null)
            .Aggregate((current, next) => Vector3.Distance(current.transform.position, peopleTransform.position)
            < Vector3.Distance(next.transform.position, peopleTransform.position) ? current : next);
        if (Vector3.Distance(targetPeople.transform.position, peopleTransform.position) < Constants.MinDistanceToDestroyPeople)
        {
            Peoples.Remove(targetPeople);
            AlterMoney(((People)targetPeople.GetComponent(typeof(People))).getCreateCost() / 2);
            ((People)targetPeople.GetComponent(typeof(People))).remove();
            Destroy(TargetEffect);
        }
    }

    public void updatePeople()
    {
        GameObject targetPeople = Peoples.Where(x => x != null)
            .Aggregate((current, next) => Vector3.Distance(current.transform.position, peopleTransform.position)
            < Vector3.Distance(next.transform.position, peopleTransform.position) ? current : next);
        if (Vector3.Distance(targetPeople.transform.position, peopleTransform.position) < Constants.MinDistanceToDestroyPeople)
        {
            if (targetPeople)
            {
                int moneyCost = ((People)targetPeople.GetComponent(typeof(People))).getCreateCost() / 3;
                if (MoneyAvailable < moneyCost) Debug.Log("Not enough money");
                else
                {
                    AlterMoney(-moneyCost);
                    ((People)targetPeople.GetComponent(typeof(People))).updateLevel(targetPeople.transform);
                    ((People)targetPeople.GetComponent(typeof(People))).alterCreateCost(moneyCost);
                }
            }
        }
    }

    public void CoinStick()
    {
        Coin.isStick = true;
        Invoke("DisableCoinStick", Constants.CoinStickTime);
    }

    public void DisableCoinStick()
    {
        Coin.isStick = false;
    }

    public void MulCoinAward(float mul,float time){
        Coin.CoinAwardTimes *= mul;
        Invoke("resetCoinAward",time);
    }

    public void resetCoinAward(){
        Coin.CoinAwardTimes = 1.0f;
    }
    

    public void useProp(int proIndex)
    {
        switch (proIndex)
        {
            case 0:
                CoinStick();
                break;
            case 1:
                KKHeroManager.GetKKHeroManagerInstance().MulHeroSpeed(4f, 10f);
                break;
            case 2:
                CoinSpawner.GetCoinSpawnInstance().MulCoinSpawnSpeed(5f,20f);
                break;
            case 3:
                foreach(var item in Enemies){
                    ((Enemy)item.GetComponent(typeof(Enemy))).MulEnemySpeed(0.5f,20f);
                }
                break;
            case 4:
                MulCoinAward(5,15f);
                break;
            case 5:
                KKHeroManager.GetKKHeroManagerInstance().BiggerHero(3f,20f);
                break;
            case 6:
                Lives ++;
                break;
            case 7:
                foreach(var item in Enemies){
                    ((Enemy)item.GetComponent(typeof(Enemy))).Health *= 2.0f/3.0f;
                }
                break;
            case 8:
                foreach(var item in Peoples){
                    ((People)item.GetComponent(typeof(People))).MulShootWaitTimes(0.5f,20f);
                }
                break;
        }
    }


}
