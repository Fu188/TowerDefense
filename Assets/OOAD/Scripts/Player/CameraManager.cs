using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ooad
{
    public class CameraManager : MonoBehaviour
    {
		public Transform player;
		public float mouseSensitivity = 200; //鼠标灵敏度


		public float MaxAngleUp = 20f;
		public float MaxAngleDown = 35f;

		#region Private Fields

		private float mouseX, mouseY; //获取鼠标移动的值
		private float xRotation;

		
		[SerializeField]
		private bool followOnStart = true;

		// cached transform of the target
		Transform cameraTransform;

		// maintain a flag internally to reconnect if target is lost or camera is switched
		[SerializeField]
		bool isFollowing;



		#endregion

		#region MonoBehaviour Callbacks

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during initialization phase
		/// </summary>
		void Start()
		{
			// Start following the target if wanted.
			if (followOnStart)
			{
				OnStartFollowing();
			}
		}


		void LateUpdate()
		{
			// The transform target may not destroy on level load, 
			// so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
			if (cameraTransform == null && isFollowing)
			{
				OnStartFollowing();
			}

			// only follow is explicitly declared
			if (isFollowing)
			{
				Follow();
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Raises the start following event. 
		/// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
		/// </summary>
		public void OnStartFollowing()
		{

			cameraTransform = Camera.main.transform;
			isFollowing = true;
			
			Cursor.lockState = CursorLockMode.Locked; // 将鼠标锁定在屏幕中央
			// we don't smooth anything, we go straight to the right camera shot
			//Cut();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Follow the target smoothly
		/// </summary>
		void Follow()
		{
			mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
			mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, -MaxAngleUp, MaxAngleDown);

			

			// Rotate Player
			//this.transform.Rotate(Vector3.up * mouseX);
			player.Rotate(Vector3.up * mouseX);
			cameraTransform.forward = player.forward ;


			cameraTransform.position = player.position - 2.5f * player.forward + 4f * player.up;
			//cameraTransform.position = new Vector3(0, 2.5f, -4);
			
			//cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);
			//cameraTransform.rotation = Quaternion.Euler(xRotation, 0, 0);


		}

		#endregion

    }
}