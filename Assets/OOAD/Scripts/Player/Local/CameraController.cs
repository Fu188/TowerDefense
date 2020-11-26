using UnityEngine;

namespace ooad
{
    public class CameraController : MonoBehaviour
    {
        public Transform player;
        private float mouseX, mouseY; //获取鼠标移动的值
        public float mouseSensitivity; //鼠标灵敏度
        private float xRotation;

        public float MaxAngleUp = 20f;
        public float MaxAngleDown = 35f;

        // Start is called before the first frame update
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            transform.position = player.position + new Vector3(0, 2.5f, -4);
            if(mouseSensitivity == 0)
            {
                Debug.LogWarning("mouse sensitivity is 0, we set 200");
                mouseSensitivity = 200;
            }
            
        }

        // Update is called once per frame
        private void Update()
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -MaxAngleUp, MaxAngleDown);

            player.Rotate(Vector3.up * mouseX);
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        }
    }
}
