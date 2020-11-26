// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Coin : MonoBehaviour
// {
//     Camera mainCamera;

//     void Start(){
//         mainCamera = Camera.main;
//         Invoke("Disable",5f);
//     }

//     public void Disable()
//     {
//         CancelInvoke();
//         this.gameObject.SetActive(false);
//     }

//     void Update(){
//         transform.position = new Vector3(
//             transform.position.x,
//             transform.position.y,
//             transform.position.z);
//         transform.Rotate(0,0,Time.deltaTime * 10);

//         if(Input.GetMouseButtonDown(0)){
//             //check if the user touches the coin
//             Vector2 location = mainCamera.ScreenToWorldPoint(Input.mousePosition);
//             if(this.GetComponent<BoxCollider2D>()==Physics2D.OverlapPoint(location,1<<LayerMask.NameToLayer("Coin"))){
//                 // increase player money
//                 GameManager.Instance.AlterMoneyAvailable(Constants.CoinAward);
//                 // destroy coin
//                 Destroy(this.gameObject);
//             }
//         }
//     }


// }
