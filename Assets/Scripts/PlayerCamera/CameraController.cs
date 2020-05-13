using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;

    float rotY = 0;
    float rotX = 0;
    float inputSensitivity = 150;
    float clampAngle = 80;

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        //Locks screen and hides mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //Rotates camera based on Mouse movement and sensitivity
        rotY += mouseX * inputSensitivity * Time.deltaTime;
        rotX += mouseY * inputSensitivity * Time.deltaTime;

        //Restricts the camera from moving too far up and down
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;


        Vector3 Pos = Player.position;
        Pos.y += 1.5f;

        transform.position = Pos;
    }
}
