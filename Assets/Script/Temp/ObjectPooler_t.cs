// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;

// public class ObjectPooler : MonoBehaviour
// {
//     public Transform Parent;
//     public GameObject PooledObject;
//     private List<GameObject> PooledObjects;
//     public int PoolLength = 10;
//     private Type[] componentsToAdd;

//     public void Initialize(){
//         PooledObjects = new List<GameObject>();
//         for(int i=0;i<PoolLength;i++){
//             CreateObjectInPool();
//         }
//     }

//     public void Initialize(params Type[] componentsToAdd){
//         this.componentsToAdd = componentsToAdd;
//         Initialize();
//     }

//     private void CreateObjectInPool(){
//         // if we do not have a prefab, then instantiate a new gameObject;
//         // else instantiate a prefab
//         GameObject go;
//         if(PooledObject==null){
//             go = new GameObject(this.name + "PooledObject");
//         }else{
//             go = Instantiate(PooledObject) as GameObject;
//         }
//         go.SetActive(false);
//         PooledObjects.Add(go);
//         // if we have component to add
//         if(componentsToAdd!=null){
//             foreach (Type item in componentsToAdd)
//             {
//                 go.AddComponent(item);
//             }
//         }
//         // if we have set the parent, assign it as the new object's parent
//         if(Parent!=null){
//             go.transform.parent = this.Parent;
//         }
//     }

//     public GameObject GetPooledObject(){
//         int index = PooledObjects.Count;
//         for(int i=0;i<index;i++){
//             if(!PooledObjects[i].activeInHierarchy){
//                 return PooledObjects[i];
//             }
//         }
//         CreateObjectInPool();
//         return PooledObjects[index];
//     }
// }
