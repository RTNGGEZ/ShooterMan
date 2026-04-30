using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移動")]
    public float moveSpeed = 5f;
    public CharacterController controller;

    [Header("ジャンプ")]
    public float jumpForce = 1.5f;

    [Header("重力")]
    public float gravity = -9.8f;

    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        // ===== 接地判定 =====
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // ===== 移動入力 =====
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.TransformDirection(new Vector3(h, 0, v)) * moveSpeed;

        // ===== ジャンプ =====
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // √(ジャンプ力 * -2 * 重力)
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // ===== 重力 =====
        velocity.y += gravity * Time.deltaTime;

        // ===== 移動適用 =====
        controller.Move((move + velocity) * Time.deltaTime);
    }
}