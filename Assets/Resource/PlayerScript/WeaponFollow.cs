using UnityEngine;

public class WeaponFollow : MonoBehaviour
{
    public Transform cam; // カメラ
    public Vector3 offset = new Vector3(0.3f, -0.3f, 0.5f);
    public float smooth = 10f;

    void LateUpdate()
    {
        // 位置追従
        Vector3 targetPos = cam.position + cam.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smooth);

        // 回転追従（上下も含める）
        transform.rotation = Quaternion.Lerp(transform.rotation, cam.rotation, Time.deltaTime * smooth);
    }
}