// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;
// using Assets.Scripts;

// public class PlacePeople : MonoBehaviour
// {
//     private Camera mainCamera;
//     public GameObject PeoplePrefab;
//     public GameObject PeopleGenerator;
//     bool isDragging = false;
//     private GameObject newPeople;
//     private GameObject tempBackgroundBehindPath;

//     // Start is called before the first frame update
//     void Start()
//     {
//         mainCamera = Camera.main;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // if we have money and we can drag a new people
//         if(Input.GetMouseButtonDown(0)&& !isDragging && GameManager.Instance.MoneyAvailable >= Constants.PeopleCost){
//             ResetTempBackgroundColor();
//             Vector2 location = mainCamera.ScreenToWorldPoint(Input.mousePosition);
//             // if user has tapped onto the peopleGenertor
//             if(PeopleGenerator.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(location,1<<LayerMask.NameToLayer("PeopleGenerator"))){
//                 // initiate dragging operation
//                 isDragging = true;
//                 newPeople = Instantiate(PeoplePrefab,PeopleGenerator.transform.position,Quaternion.identity) as GameObject;
//             }
//         }else if(Input.GetMouseButton(0)&& isDragging){
//             Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//             RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin,ray.direction);
//             if(hits.Length>0 && hits[0].collider!=null){
//                 newPeople.transform.position = hits[0].collider.gameObject.transform.position;
//                 if(hits.Where(x => x.collider.gameObject.tag == "Path"|| x.collider.gameObject.tag == "Tower").Count()>0 || hits.Where(x=>x.collider.gameObject.tag=="People").Count()>1){
//                     // we cannot place a people
//                     GameObject backgroundBehindPath = hits.Where(x => x.collider.gameObject.tag=="Background").First().collider.gameObject;
//                     backgroundBehindPath.GetComponent<SpriteRenderer>().color = Constants.RedColor;
//                     if(tempBackgroundBehindPath != backgroundBehindPath){
//                         ResetTempBackgroundColor();
//                     }
//                     tempBackgroundBehindPath = backgroundBehindPath;
//                 }else{
//                     ResetTempBackgroundColor();
//                 }
//             }
//         }else if(Input.GetMouseButtonUp(0) && isDragging){
//             ResetTempBackgroundColor();
//             Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//             RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin,ray.direction,Mathf.Infinity,~(1<<LayerMask.NameToLayer("PeopleGenerator")));
//             if(hits.Where(x=>x.collider.gameObject.tag=="Background").Count()>0 && hits.Where(x=>x.collider.gameObject.tag=="Path").Count()==0 && hits.Where(x=>x.collider.gameObject.tag=="People").Count()==1){
//                 GameManager.Instance.AlterMoneyAvailable(-Constants.PeopleCost);
//                 newPeople.transform.position = hits.Where(x=>x.collider.gameObject.tag=="Background").First().collider.gameObject.transform.position;
//                 newPeople.GetComponent<People>().Activate();
//             }else{
//                 // we cannot leave a people here
//                 Destroy(newPeople);
//             }
//             isDragging = false;
//         }
//     }

//     private void ResetTempBackgroundColor(){
//         if(tempBackgroundBehindPath != null){
//             tempBackgroundBehindPath.GetComponent<SpriteRenderer>().color = Constants.BlackColor;
//         }
//     }
// }
