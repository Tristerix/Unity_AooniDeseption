using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform playerBody; // プレイヤーのルート（カメラ回り込みのため）
    public bool lockCursor = true;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    float pitch = 0f;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (playerBody == null)
        {
            playerBody = transform.parent; // デフォルトで親を使う
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // プレイヤーのボディは Y 軸（yaw）のみ回転
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
        else
        {
            transform.Rotate(Vector3.up * mouseX, Space.World);
        }

        // カメラはピッチ（上下）を管理
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }
}
