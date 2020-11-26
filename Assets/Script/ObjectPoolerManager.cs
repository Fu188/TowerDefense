using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPoolerManager : MonoBehaviour
{
    public ObjectPooler AudioPooler;
    public ObjectPooler CoinPooler;
    public GameObject CoinPrefab;

    public static ObjectPoolerManager Instance {get;private set;}
    void Awake(){
        Instance = this;
    }

    void Start(){
        if(CoinPooler==null){
            GameObject go = new GameObject("CoinPooler");
            go.transform.SetParent(GameManager.Instance.otherObjectList.transform);
            CoinPooler = go.AddComponent<ObjectPooler>();
            CoinPooler.PooledObject = CoinPrefab;
            CoinPooler.Initialize();
        }

        if(AudioPooler==null){
            GameObject go = new GameObject("AudioPooler");
            go.transform.SetParent(GameManager.Instance.otherObjectList.transform);
            AudioPooler = go.AddComponent<ObjectPooler>();
            AudioPooler.Initialize(typeof(AudioSource));
        }
    }
}
