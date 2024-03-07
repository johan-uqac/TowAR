using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }
    public MoveDirection MoveDirection { get; set; }
    [SerializeField]
    private float moveSpeed = 0.25f;

public void SetMoveSpeed(float speed)
{
    moveSpeed = speed;

}
    private void OnEnable()
    {
        if (LastCube == null)
            LastCube = GameObject.Find("Start").GetComponent<MovingCube>();

        CurrentCube = this;
        GetComponent<Renderer>().material.color = GetColor();

        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);
    }

    private Color GetColor()
    {
        return Color.magenta;
    }

    internal void Stop()
    {
        moveSpeed = 0;
        float movement = GetMovement();

        // Know if the cube is going to fall entirely = Game over
        float max = MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;
        if (Mathf.Abs(movement) >= max)
        {
            LastCube = null;
            CurrentCube = null;
            SceneManager.LoadScene(0);
        }

        float direction = movement > 0 ? 1f : -1f;

        if (MoveDirection == MoveDirection.Z)
            SplitCubeOnZ(movement, direction);
        else
            SplitCubeOnX(movement, direction);

        LastCube = this;
    }

    private float GetMovement()
    {
        if (MoveDirection == MoveDirection.Z)
            return transform.position.z - LastCube.transform.position.z;
        else
            return transform.position.x - LastCube.transform.position.x;
    }

    private void SplitCubeOnX(float hangover, float direction)
    {
        float newXSize = LastCube.transform.localScale.x - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.x - newXSize;

        float newXPosition = LastCube.transform.position.x + (hangover / 2);
        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newXSize / 2f * direction);
        float fallingBlockXPosition = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockXPosition, fallingBlockSize);
    }

    private void SplitCubeOnZ(float hangover, float direction)
    {
        float newZSize = LastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZSize;

        float newZPosition = LastCube.transform.position.z + (hangover / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        float fallingBlockZPosition = cubeEdge + fallingBlockSize / 2f * direction;
      
        SpawnDropCube(fallingBlockZPosition, fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockZPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (MoveDirection == MoveDirection.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockZPosition);
        }
        else
        {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockZPosition, transform.position.y, transform.position.z);
        }

        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;

        // Destroy(cube.gameObject, 1f);
    }

    private void Update()
    {
        // if (MoveDirection == MoveDirection.Z)
        //     transform.position += transform.forward * Time.deltaTime * moveSpeed;
        // else
        //     transform.position += transform.right * Time.deltaTime * moveSpeed;
        // Variable pour suivre la position actuelle de l'objet
        Vector3 currentPosition = transform.position;

        // Déplacer l'objet selon la direction actuelle
        if (MoveDirection == MoveDirection.Z)
        {
            currentPosition += transform.forward * Time.deltaTime * moveSpeed;
        }
        else
        {
            currentPosition += transform.right * Time.deltaTime * moveSpeed;
        }

        if (currentPosition.x <= -2 || currentPosition.x >= 2)
        {
            // Inverser le vecteur de déplacement sur l'axe x
            transform.forward = -transform.forward;
        }

        if (currentPosition.z <= -2 || currentPosition.z >= 2)
        {
            // Inverser le vecteur de déplacement sur l'axe z
            transform.right = -transform.right;
        }


        // Affecter la nouvelle position à l'objet
        transform.position = currentPosition;

    }
}
