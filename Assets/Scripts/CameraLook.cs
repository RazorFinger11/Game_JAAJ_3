using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public PlayerController player;
    public Transform playerBody;
    public float mouseSensitivity;

    float xAxisClamp = 0;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {        
        RotateCamera();
    }

    void RotateCamera()
    {
        float mouseX = 0;
        float mouseY = 0;

        if (!player.Locked) {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }

        float rotAmountX = mouseX * mouseSensitivity;
        float rotAmountY = mouseY * mouseSensitivity;

        xAxisClamp -= rotAmountY;

        Vector3 targetRotCam = transform.rotation.eulerAngles;
        Vector3 targetRotBody = playerBody.rotation.eulerAngles;

        targetRotCam.x -= rotAmountY;
        targetRotCam.z = 0;

        if (xAxisClamp > 90)
        {
            xAxisClamp = 90;
            targetRotCam.x = 90;
        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            targetRotCam.x = 270;
        }

        targetRotBody.y += rotAmountX;


        transform.rotation = Quaternion.Euler(targetRotCam);
        playerBody.rotation = Quaternion.Euler(targetRotBody);
    }
}
