using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;

public class Arrow : MonoBehaviour
{
    // [HideInInspector]
    public GameObject TargetEnemy;
    public GameObject blastPrefab;
    public static Arrow Instance{get;set;}
    public int attackType;
    void Awake(){
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Disable",2f);
    }

    public void Disable(){
        CancelInvoke();
        Destroy(this.gameObject);
    }

    public void BlastEffect(Transform transform){
        GameObject.Instantiate(blastPrefab,transform.position,transform.rotation);
    }

    public void ChangeTarget(GameObject targetEnemy,int attackType){
        TargetEnemy = targetEnemy;
        this.attackType = attackType;
    }

    void Update(){
        if(TargetEnemy!=null){
            Quaternion diffRotation = Quaternion.LookRotation(TargetEnemy.transform.position-transform.position,Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,diffRotation,Time.deltaTime*2000);
            transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);

            Vector3 diffPosition = TargetEnemy.transform.position-transform.position;
            transform.position = transform.position + new Vector3(0,0.017f,0) + 2*Time.deltaTime*diffPosition;
        }else{
            Destroy(this.gameObject);
        }
    }
}
