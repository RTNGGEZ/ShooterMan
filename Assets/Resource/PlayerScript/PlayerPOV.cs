using UnityEngine;

public class PlayerPOV : MonoBehaviour
{
    [Header("視点")]
    public Transform neck;
    public float sensitivity = 2.0f;
    public float minVertical = -90.0f;
    public float maxVertical = 90.0f;

    private float rotationX = 0f;

    [Header("傾き（Lean）")]
    public float tiltAmount = 5f;
    public float tiltSpeed = 8f;
    private float currentTilt = 0f;

    [Header("FOV")]
    public Camera cam;
    public float baseFov = 60f;
    public float fovMultiplier = 10f;
    public float fovSpeed = 5f;

    [Header("ボブ（揺れ）")]
    public float bobSpeed = 8f;
    public float bobAmount = 0.05f;
    private float bobTimer = 0f;

    private float defaultNeckY; // 追加

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // エディタで設定した Neck の初期 Y 座標を記録しておく
        defaultNeckY = neck.localPosition.y;
    }

    void Update()
    {
        // ===== マウス視点 =====
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(0, mouseX, 0);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minVertical, maxVertical);

        // ===== ストレイフ傾き =====
        float h = Input.GetAxis("Horizontal");
        float targetTilt = -h * tiltAmount;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);

        // ===== ボブ =====
        float move = new Vector2(h, Input.GetAxis("Vertical")).magnitude;

        Vector3 bobOffset = Vector3.zero;

        if (move > 0.1f)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float y = Mathf.Sin(bobTimer) * bobAmount;
            bobOffset = new Vector3(0, y, 0);
        }
        else
        {
            bobTimer = 0f;
        }

        neck.localRotation = Quaternion.Euler(rotationX, 0, currentTilt);

        // bobOffset に初期の高さを加算した目標地点を作る
        Vector3 targetPosition = new Vector3(0, defaultNeckY + bobOffset.y, 0); 
        neck.localPosition = Vector3.Lerp(neck.localPosition, targetPosition, Time.deltaTime * 10f);

            // ===== FOV変化 =====
        float speed = move;
        float targetFov = baseFov + speed * fovMultiplier;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * fovSpeed);
    }
}