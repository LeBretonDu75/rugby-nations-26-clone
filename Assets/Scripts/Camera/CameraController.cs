using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 35, -25);
    public float smooth = 3f;

    void LateUpdate()
    {
        if (target == null)
        {
            var b = GameManager.Instance?.ball;
            if (b != null) target = b.transform;
            else return;
        }
        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);
        transform.LookAt(target);
    }
}