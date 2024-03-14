using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sliderSens = 1f;
    public float mouseXSens = 2.5f;
    public float mouseYSens = 2.5f; // New variable for vertical sensitivity
    public float mouseSmoothing = 1.5f;

    private float mouseXPos;
    private float mouseYPos; // New variable for vertical movement
    private float mouseXSmoothed;
    private float mouseYSmoothed; // New variable for smoothed vertical movement
    private float currentXLookPos;
    private float currentYLookPos; // New variable for vertical rotation

    // Start is called before the first frame update
    private void Start()
    {
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            GetInput();
            ModifyInput();
            PlayerLook();
        }
    }

    void GetInput()
    {
        mouseXPos = Input.GetAxisRaw("Mouse X");
        mouseYPos = Input.GetAxisRaw("Mouse Y"); // Get vertical mouse movement
    }

    void ModifyInput()
    {
        mouseXPos *= sliderSens*mouseXSens * mouseSmoothing;
        mouseYPos *= sliderSens*mouseYSens * mouseSmoothing; // Modify vertical mouse movement
        mouseXSmoothed = Mathf.Lerp(mouseXSmoothed, mouseXPos, 1 / mouseSmoothing);
        mouseYSmoothed = Mathf.Lerp(mouseYSmoothed, mouseYPos, 1 / mouseSmoothing); // Smooth vertical movement
    }

    void PlayerLook()
    {
        currentXLookPos += mouseXSmoothed;
        currentYLookPos -= mouseYSmoothed; // Invert for correct vertical rotation
        currentYLookPos = Mathf.Clamp(currentYLookPos, -80f, 80f); // Limit vertical rotation
        transform.localRotation = Quaternion.Euler(currentYLookPos, currentXLookPos, 0f); // Apply both horizontal and vertical rotation
    }
}
