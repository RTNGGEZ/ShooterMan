using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Input Setup")]
    [SerializeField] PlayerInput playerInput; // Unity 2022用にこれを追加

    [Header("Settings")]
    [SerializeField] Transform cameraTarget;
    [SerializeField] float defaultFov = 90f;
    [SerializeField] float mouseSensitivity = 0.15f;
    [Range(0, 90), SerializeField] float maxCameraPitch = 85f;
    
    Vector3 _eulerAngles;
    InputAction _lookAction; // Lookアクション保持用

    void Awake()
    {
        // 1. 各種初期設定
        if (Camera.main != null)
        {
            Camera.main.fieldOfView = defaultFov;
        }

        if (cameraTarget != null)
        {
            transform.position = cameraTarget.position;
            transform.eulerAngles = _eulerAngles = cameraTarget.eulerAngles;
        }

        // 2. マウスカーソルの固定
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 3. Unity 2022用のAction取得
        if (playerInput != null)
        {
            _lookAction = playerInput.actions.FindAction("Look");
        }
    }

    void Update()
    {
        // Unity 6方式ではなく、取得しておいた_lookActionから値を読み取る
        if (_lookAction != null)
        {
            Vector2 look = _lookAction.ReadValue<Vector2>();
            UpdateRotation(look);
        }
    }

    void LateUpdate()
    {
        if (cameraTarget != null)
        {
            transform.position = cameraTarget.position; 
        }
    }

    public void UpdateRotation(Vector2 look)
    {
        // マウスの移動量に感度を掛けて回転を計算
        _eulerAngles.x += -look.y * mouseSensitivity;
        _eulerAngles.y += look.x * mouseSensitivity;
        
        // 上下の首振り制限
        _eulerAngles.x = Mathf.Clamp(_eulerAngles.x, -maxCameraPitch, maxCameraPitch);
        
        transform.eulerAngles = new Vector3(_eulerAngles.x, _eulerAngles.y, 0f);
    }
}