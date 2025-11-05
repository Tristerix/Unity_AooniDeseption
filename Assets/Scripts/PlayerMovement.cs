using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float PlayerStamina;
    public float HealStaminaPerflame;
    public float MaxStamina;

    [Header("References")]
    public Transform cameraTransform; // Main Camera (推奨: Head の子)
    public Transform headTransform;   // プレイヤーの頭位置（カメラと一致させる）

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.6f;
    public float gravity = -9.81f;
    public bool lockCameraToHead = true; // trueならスクリプトでカメラ位置を強制ロック

    [Header("Ground")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public float jumpHeight = 1.2f;

    CharacterController cc;
    Vector3 velocity;
    bool isGrounded;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (cameraTransform == null)
        {
            Camera cam = Camera.main;
            if (cam) cameraTransform = cam.transform;
        }
    }

    void Update()
    {
        GroundCheck();

        // --- カメラを頭に固定（必要なら） ---
        if (lockCameraToHead && cameraTransform != null && headTransform != null)
        {
            // ワールド空間で頭の位置にカメラを置く（回転は MouseLook に任せる）
            cameraTransform.position = headTransform.position;
        }

        // --- 移動入力（カメラの向きに従う） ---
        float h = Input.GetAxis("Horizontal"); // A/D, ←/→
        float v = Input.GetAxis("Vertical");   // W/S, ↑/↓

        // カメラの前方向を水平成分だけ取り出す（y を無視する）
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 desired = camForward * v + camRight * h;
        if (desired.sqrMagnitude > 1f) desired.Normalize();

        float speed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) && (PlayerStamina >= 10))
        {
            speed *= sprintMultiplier;
            PlayerStamina -= 1;
        }
        else if(MaxStamina >= PlayerStamina)
        {
            PlayerStamina += HealStaminaPerflame;
        }

        cc.Move(desired * speed * Time.deltaTime);

        // --- ジャンプ＆重力 ---
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f; // 少し押さえておく
        }
    /*
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    */
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    void GroundCheck()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
        else
        {
            // 簡易：下方向にレイ
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        }
    }

    // デバッグ：ヒット範囲をSceneで見やすくする
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}
