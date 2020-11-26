// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ObjectPoolerManager : MonoBehaviour
// {
//     public ObjectPooler ArrowPooler;
//     public GameObject ArrowPrefab;

//     public static ObjectPoolerManager Instance {get; private set;}

//     void Awake(){
//         Instance = this;
//     }

//     void Start(){
//         // just instantiate the pools
//         if (ArrowPooler==null){
//             GameObject go = new GameObject("ArrowPooler");
//             ArrowPooler = go.AddComponent<ObjectPooler>();
//             ArrowPooler.PooledObject = ArrowPrefab;
//             go.transform.parent = this.gameObject.transform;
//             ArrowPooler.Initialize();
//         }
//     }
// }
