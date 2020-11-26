using UnityEngine;

namespace ooad
{
    public class CameraController1 : MonoBehaviour
    {
        public Transform player;
        private float xRotation;


        // Start is called before the first frame update
        private void Start()
        {
            transform.position = player.position + new Vector3(0, 2.5f, -4);

        }

        // Update is called once per frame
        private void Update()
        {
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
    }
}
