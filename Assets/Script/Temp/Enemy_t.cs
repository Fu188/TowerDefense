// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;

// public class Enemy : MonoBehaviour
// {

//     public int Health;
//     int nextWayPointIndex = 0;
//     public float Speed = 1f;
//     public event EventHandler EnemyKilled;

//     // Start is called before the first frame update
//     void Start()
//     {
//         Health = Constants.InitialEnemyHealth;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(Vector2.Distance(transform.position,GameManager.Instance.Waypoints[nextWayPointIndex].position)<0.01f){
//             // if it is the last waypoint
//             if(nextWayPointIndex == GameManager.Instance.Waypoints.Length - 1){
//                 RemoveAndDestroy();
//                 GameManager.Instance.Lives--;
//             }else{
//                 nextWayPointIndex++;
//                 transform.LookAt(GameManager.Instance.Waypoints[nextWayPointIndex].position,-Vector3.forward);
//                 transform.eulerAngles = new Vector3(0,0,transform.eulerAngles.z);
//             }
//         }
//         // enemy moves towards the next waypoint
//         transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.Waypoints[nextWayPointIndex].position,Time.deltaTime*Speed);
//     }

//     void OnCollisionEnter2D(Collision2D col){
//         if(col.gameObject.tag == "Arrow"){
//             // if we're hit by arrow
//             if(Health > 0){
//                 Health -= Constants.ArrowDamage;
//                 if(Health<=0){
//                     RemoveAndDestroy();
//                 }
//             }
//             col.gameObject.GetComponent<Arrow>().Disable();
//         }
//     }

//     void RemoveAndDestroy(){
//         GameManager.Instance.Enemies.Remove(this.gameObject);
//         Destroy(this.gameObject);
//         if(EnemyKilled != null){
//             EnemyKilled(this,EventArgs.Empty);
//         }
//     }

// }
