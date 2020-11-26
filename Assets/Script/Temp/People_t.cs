// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;
// using Assets.Scripts;

// public class People : MonoBehaviour
// {
//     public Transform ArrowPosition;
//     public GameObject ArrowPrefab;
//     public float ShootWaitTime = 2f;
//     private float LastShootTime = 0f;
//     GameObject targetEnemy;
//     private float InitialArrowForce = 500f;
//     private PeopleState State;

//     public void Activate(){
//         State = PeopleState.Searching;
//     }

//     void Start(){
//         State = PeopleState.Inactive;
//         ArrowPosition = transform.Find("ArrowPosition");
//     }

//     void Update(){
//         // In the last round and we killed all enemies, do nothing
//         if(GameManager.Instance.FinalRoundFinished && GameManager.Instance.Enemies.Where(x => x!=null).Count()==0){
//             State = PeopleState.Inactive;
//         }

//         // searching for an enemy
//         if(State == PeopleState.Searching){
//             if(GameManager.Instance.Enemies.Where(x => x!=null).Count()==0){
//                 return;
//             }

//             // find the closest enemy
//             targetEnemy = GameManager.Instance.Enemies.Where(x => x!=null).Aggregate(
//                 (current,next) => Vector2.Distance(current.transform.position, transform.position) < Vector2.Distance(next.transform.position, transform.position)? current:next
//             );

//             // if there is an enemy
//             if(targetEnemy !=null && targetEnemy.activeSelf && Vector2.Distance(transform.position, targetEnemy.transform.position)<Constants.MinDistanceForPeopleToShoot){
//                 State = PeopleState.Targeting;
//             }
//         }else if(State == PeopleState.Targeting){
//             // if the target is still close to us, then shoot it
//             if(targetEnemy != null && Vector2.Distance(transform.position,targetEnemy.transform.position)<Constants.MinDistanceForPeopleToShoot){
//                 LookAndShoot();
//             }else{
//                 State = PeopleState.Searching;
//             }
//         }
//     }

//     private void LookAndShoot(){
//         //look at the enemy
//         Quaternion diffRotation = Quaternion.LookRotation(transform.position - targetEnemy.transform.position, Vector3.forward);
//         transform.rotation = Quaternion.RotateTowards(transform.rotation, diffRotation, Time.deltaTime*2000);
//         transform.eulerAngles = new Vector3(0,0,transform.eulerAngles.z);

//         // make sure we are almost looking at the enemy before we start shooting
//         Vector2 direction = targetEnemy.transform.position - transform.position;
//         float axisDif = Vector2.Angle(transform.up, direction);
//         if(axisDif <= 20f){
//             if(Time.time - LastShootTime > ShootWaitTime){
//                 Shoot(direction);
//                 LastShootTime = Time.time;
//             }
//         }
//     }

//     private void Shoot(Vector2 dir){
//         // if enemy is still close to us
//         if(targetEnemy!=null && targetEnemy.activeSelf && Vector2.Distance(transform.position,targetEnemy.transform.position)<Constants.MinDistanceForPeopleToShoot){
//             GameObject go = ObjectPoolerManager.Instance.ArrowPooler.GetPooledObject();
//             go.transform.position = ArrowPosition.position;
//             go.transform.rotation = transform.rotation;
//             go.SetActive(true);
//             // shoot
//             go.GetComponent<Rigidbody2D>().AddForce(dir * InitialArrowForce);
//         }else{
//             State = PeopleState.Searching;
//         }
//     }
// }
