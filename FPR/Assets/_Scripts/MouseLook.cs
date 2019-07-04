using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public enum RotAxis { mouseX, mouseY }
    public RotAxis axes = RotAxis.mouseY;

    private float currSensX = 1.5f, currSensY = 1.5f;

    private float sensX = 1.5f, sensY = 1.5f;

    private float rotX, rotY;
    private float minX = -360f, maxX = 360f;
    private float minY = -60f, maxY = 60f;

    private Quaternion origRot;
    private float mouseSens = 1.7f;

    // Start is called before the first frame update
    void Start()
    {
        origRot = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        HandleRotation();
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if(angle < -360)
        {
            angle += 360f;
        }

        if(angle > 360f)
        {
            angle -= 360f;
        }

        return Mathf.Clamp(angle, min, max);
    }

    void HandleRotation()
    {
        if(currSensX != mouseSens || currSensY != mouseSens)
        {
            currSensX = currSensY = mouseSens;
        }

        sensX = currSensX;
        sensY = currSensY;

        if(axes == RotAxis.mouseX)
        {
            rotX += Input.GetAxis("Mouse X") * sensX;

            rotX = ClampAngle(rotX, minX, maxX);
            Quaternion xQuat = Quaternion.AngleAxis(rotX, Vector3.up);
            transform.localRotation = origRot * xQuat;
        }


        if(axes == RotAxis.mouseY)
        {
            rotY += Input.GetAxis("Mouse Y") * sensY;

            rotY = ClampAngle(rotY, minY, maxY);

            Quaternion yQuat = Quaternion.AngleAxis(rotY, Vector3.right);

            transform.localRotation = origRot * yQuat;
        }
    }
}
