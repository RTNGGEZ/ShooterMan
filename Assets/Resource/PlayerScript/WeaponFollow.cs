using UnityEngine;

public class WeaponFollow : MonoBehaviour
{
    public Transform cam;
    public Vector3 offset = new Vector3(0.3f, -0.3f, 0.5f);
    public float smooth = 10f;

    void LateUpdate()
    {
        Vector3 targetPos = cam.position + cam.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smooth);

        transform.rotation = Quaternion.Lerp(transform.rotation, cam.rotation, Time.deltaTime * smooth);
    }
}