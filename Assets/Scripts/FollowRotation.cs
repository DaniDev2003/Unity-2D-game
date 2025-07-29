using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    private Transform parentTransform;

    private void Start()
    {
        parentTransform = transform.parent;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity; // Anula la rotación del objeto "CollidersContainer"
        transform.position = parentTransform.position; // Mantiene la posición del objeto "CollidersContainer" igual a la del cuerpo del enemigo
    }
}
