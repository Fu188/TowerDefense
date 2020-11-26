using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPooler : MonoBehaviour
{
    public GameObject PooledObject;
    private List<GameObject> PooledObjects;
    public int PoolLength = 10;

    private Type[] componentsToAdd;

    public void Initialize(){
        PooledObjects = new List<GameObject>();
        for(int i=0;i<PoolLength;i++){
            CreateObjectInPool();
        }
    }

    public void Initialize(params Type[] componentsToAdd){
        this.componentsToAdd = componentsToAdd;
        Initialize();
    }

    private void CreateObjectInPool(){
        //if we don't have a prefab, instantiate a new gameobject
        //else instantiate the prefab
        GameObject go;
        if(PooledObject == null){
            go = new GameObject(this.name+" PooledObject");
        }else{
            go = GameNetworkManager.KkInstantiate(PooledObject,Vector3.zero,new Quaternion()) as GameObject;
            go.transform.SetParent(GameManager.Instance.otherObjectList.transform);
        }

        //set the new object as inactive and add it to the list
        go.SetActive(false);
        PooledObjects.Add(go);

        if(componentsToAdd!= null){
            foreach (Type itemType in componentsToAdd)
            {
                go.AddComponent(itemType);
            }
        }
    }

    public GameObject GetPooledObject(){
        for(int i=0;i<PooledObjects.Count;i++){
            if(!PooledObjects[i].activeInHierarchy){
                return PooledObjects[i];
            }
        }
        int indexToReturn = PooledObjects.Count;
        CreateObjectInPool();
        return PooledObjects[indexToReturn];
    }
}
