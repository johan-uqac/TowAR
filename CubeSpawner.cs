using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private MovingCube cubePrefab;
    [SerializeField]
    private MoveDirection moveDirection;

    public float cubeSpeed = 0f;
    
    private void Start() {
        if(moveDirection == MoveDirection.X)
            SpawnCube();
    }

    void Update()
{
    Vector3 acceleration = Input.acceleration;
    Debug.Log("Acceleration X: " + acceleration.x);
    Debug.Log("Acceleration Y: " + acceleration.y);
    
    float speedMultiplier = 2f;
    float speedX = acceleration.x * speedMultiplier;
    float speedY = acceleration.y * speedMultiplier;

    

    // Créer un vecteur de déplacement en fonction de l'accélération
    Vector3 movementVector = new Vector3(speedX, 0, speedY);

    // Modifier la vitesse du cube en fonction du mouvement du téléphone
    cubeSpeed = movementVector.magnitude; // Utilisez la magnitude du vecteur comme vitesse
    
    if (cubeSpeed > 0f && MovingCube.CurrentCube != null)
        {
            MovingCube.CurrentCube.SetMoveSpeed(cubeSpeed);
        }

        Debug.Log("Cube Speed: " + cubeSpeed);
}


    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);

        if (MovingCube.LastCube != null && MovingCube.LastCube.gameObject != GameObject.Find("Start"))
        {
            float x = moveDirection == MoveDirection.X ? transform.position.x : MovingCube.LastCube.transform.position.x;
            float z = moveDirection == MoveDirection.Z ? transform.position.z : MovingCube.LastCube.transform.position.z;

            cube.transform.position = new Vector3(x,
                MovingCube.LastCube.transform.position.y + cubePrefab.transform.localScale.y,
                z);
        }
        else
        {
            cube.transform.position = transform.position;
        }

        cube.MoveDirection = moveDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }
}