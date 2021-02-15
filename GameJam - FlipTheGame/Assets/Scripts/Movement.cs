using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IMovement
{
    Vector2 movementValue;

    private void MoveCharacter()
    {
        // Program movement using values
    }

    void IMovement.Movement(Vector2 value)
    {
        movementValue = value;
    }
}
