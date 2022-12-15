using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This code belongs to Unity Ace
    Source of tutorial: https://www.youtube.com/watch?v=5Rq8A4H6Nzw
    The code has not been edited
    The comments were made by the game developers to show understanding of the code

    The Clipping Planes Near value of the camera has been set to 0.1 because of an issue with seeing through walls
    https://answers.unity.com/questions/143056/need-to-stop-camera-from-seeing-through-walls.html
*/

public class FirstPersonCamera : MonoBehaviour
{
    //used for rotating the player horizontally, camera moves with the player as it is a child object
    public Transform player;
    public float mouseSensivity = 2f;
    //the camera should be facing straight ahead at the start
    float cameraVerticalRotation = 0f;

    void Start()
    {
        //hiding the cursor
        Cursor.visible = false;
        //the cursor is placed in the center of the view and cannot be moved, the cursor is invisible in this state, regardless of the value of Cursor.visible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float inputX = Input.GetAxis("Mouse X")*mouseSensivity;
        float inputY = Input.GetAxis("Mouse Y")*mouseSensivity;

        //to move up, the camera needs a negative angle even though the value on the Y axis is positive
        cameraVerticalRotation -= inputY;
        //to make sure the player can't do backflips and turn his head backwards like a horror movie character
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        //rotate the camera
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        //rotate both the player and the child object - camera left/right
        player.Rotate(Vector3.up * inputX);
    }
}
