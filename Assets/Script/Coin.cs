using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Coin : MonoBehaviour
{
    public static bool isStick = false;
    private Transform targetTransform;
    public static float CoinAwardTimes = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void startInvoke(){
        Invoke("Disable",Constants.CoinSpawnLimitTime);
    }

    public void Disable(){
        CancelInvoke();
        this.gameObject.SetActive(false);
        GameManager.Instance.Coins.Remove(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(isStick){
            targetTransform = GameManager.peopleTransform;
            if(Vector3.Distance(transform.position,targetTransform.position)<Constants.MinDistanceForCoinToStick){
                Vector3 diffPosition = targetTransform.position-transform.position;
                Vector3 temp = 5*Time.deltaTime*diffPosition;
                transform.position = transform.position + new Vector3(temp[0],0,temp[2]);
            }
        }
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag=="Hero"){
            GameManager.AlterMoney((int)(Constants.CoinAward*CoinAwardTimes));
            Disable();
        }
    }

}
