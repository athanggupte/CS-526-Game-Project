using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform[] cameraPositions;
    public float[] transitionPoints;

    private int currentStage = 0;

    private void Update()
    {
        // If not at the last camera position, check if the player has moved forward.
        if (currentStage < cameraPositions.Length - 1 && player.position.x > transitionPoints[currentStage])
        {
            currentStage++;
            MoveCameraToStage(currentStage);
        }
        // If not at the first camera position, check if the player has moved backward.
        else if (currentStage > 0 && player.position.x < transitionPoints[currentStage - 1])
        {
            currentStage--;
            MoveCameraToStage(currentStage);
        }
    }



    private void MoveCameraToStage(int stage)
    {
        transform.position = new Vector3(cameraPositions[stage].position.x, cameraPositions[stage].position.y, transform.position.z);
    }
}