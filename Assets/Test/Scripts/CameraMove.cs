using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float sensitivityMouse = 2f;
    public Transform target;

    public float Distance = 5F;

    private float mX = 0.0F, mY = 0.0F;


    private float MinLimitY = 5;
    private float MaxLimitY = 180;

    public bool isNeedDamping = true;

    public float Damping = 2.5F;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        mX += Input.GetAxis("Mouse X") * sensitivityMouse * 0.02F;
        mY -= Input.GetAxis("Mouse Y") * sensitivityMouse * 0.02F;

        mY = ClampAngle(mY, MinLimitY, MaxLimitY);

        Quaternion mRotation = Quaternion.Euler(mY, mX, 0);
        Vector3 mPosition = mRotation * new Vector3(0.0F, 2.0F, -Distance) + target.position;

        if (isNeedDamping)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, mRotation, Time.deltaTime * Damping);

            transform.position = Vector3.Lerp(transform.position, mPosition, Time.deltaTime * Damping);
        }
        else
        {
            transform.rotation = mRotation;
            transform.position = mPosition;
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
