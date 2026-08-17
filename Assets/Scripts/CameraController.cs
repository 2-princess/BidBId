using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + new Vector3(0, 8, -6);
    }
}
