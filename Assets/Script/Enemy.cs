using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class Enemy : MonoBehaviour, Photon.Pun.IPunObservable
{
    public float Health;
    int nextWayPointIndex = 0;
    public float Speed;
    public Slider HPStrip;
    public Image fill;
    public float maxHp;
    public event EventHandler EnemyKilled;
    private float SpeedTimes = 1.0f;
    Animator ator;
    AnimatorStateInfo animatorInfo;

    void Start(){
        maxHp = Health;
        HPStrip.maxValue = maxHp;
        fill.color = Color.green;
    }

    void Update(){
        HPStrip.value = Health;
        if(Health < 0.25*maxHp) fill.color = Color.red;
        ator = this.gameObject.GetComponent<Animator>();
        animatorInfo = ator.GetCurrentAnimatorStateInfo(0);
        if ((animatorInfo.normalizedTime > 0.9f) && (animatorInfo.IsName("Die")))
        {
            GameNetworkManager.KkDestroy(this.gameObject);
        }
        if(Vector3.Distance(transform.position,GameManager.Instance.Waypoints[nextWayPointIndex].position)<0.01f){
            // if it is the last waypoint
            if(nextWayPointIndex == GameManager.Instance.Waypoints.Length-1){
                print(GameManager.Instance.Lives);
                AudioManager.Instance.PlayDeathSound();
                GameManager.Instance.Enemies.Remove(this.gameObject);
                if(EnemyKilled!=null){
                    EnemyKilled(this,EventArgs.Empty);
                }
                GameNetworkManager.KkDestroy(this.gameObject);
                GameManager.Instance.Lives--;
            }else{
                nextWayPointIndex++;
                transform.LookAt(GameManager.Instance.Waypoints[nextWayPointIndex].position,-Vector3.forward);
                transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
            }
        }
        // enemy moves toward the next waypoint
        transform.position = Vector3.MoveTowards(transform.position,GameManager.Instance.Waypoints[nextWayPointIndex].position,Time.deltaTime*Speed*SpeedTimes);
    }

    void OnParticleCollision(GameObject go) {
        if (go.tag=="Arrow"){
            if(Health > 0){
                Health -= int.Parse(go.name);
                if(Health<=0){
                    RemoveAndDestroy();
                }else{
                    ator.SetTrigger("Take Damage");
                    print(Health);
                }
            }
            // ((Arrow)go.GetComponent(typeof(Arrow))).Disable();
            ((Arrow)go.GetComponent(typeof(Arrow))).BlastEffect(transform);  
        }
    }

    void RemoveAndDestroy(){
        AudioManager.Instance.PlayDeathSound();
        GameManager.Instance.Enemies.Remove(this.gameObject);
        ator.SetTrigger("Die");
        if(EnemyKilled!=null){
            EnemyKilled(this,EventArgs.Empty);
        }
    }

    public void MulEnemySpeed(float mul, float time){
        SpeedTimes *= mul;
        Invoke("resetEnemySpeed",time);
    }

    public void resetEnemySpeed(){
        SpeedTimes = 1.0f;
    }

    #region IPunObservable implementation

    public void OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Health);
        }
        else
        {
            this.Health = (float)stream.ReceiveNext();
        }
    }

    #endregion
}
