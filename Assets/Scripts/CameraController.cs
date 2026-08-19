using Unity.Netcode;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float distance = 8f;
    public float mouseSpeed = 3f;

    float yaw;
    float pitch = 30f;

    void Start()
    {
        NetworkObject player = NetworkManager.Singleton.LocalClient.PlayerObject;

        if (player != null)
        {
            target = player.transform;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 마우스 움직임
        yaw += Input.GetAxis("Mouse X") * mouseSpeed;
        pitch -= Input.GetAxis("Mouse Y") * mouseSpeed;

        // 너무 위/아래까지 돌아가지 않게 제한
        pitch = Mathf.Clamp(pitch, 10f, 70f);

        // 마우스로 정한 회전
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}