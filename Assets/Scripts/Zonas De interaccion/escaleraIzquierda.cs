using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaleraIzquierda : MonoBehaviour
{
    [SerializeField] private GameObject characterObject;
    private bool isRanged;
    private Vector3 targetPosition;

    void Update()
    {
        if (isRanged)
        {
            targetPosition = new Vector3(-0.66f, 62.89f, 0f);
            MoveCharacterToPosition(targetPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        { 
           isRanged = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isRanged = false; 
        }
    }

    private void MoveCharacterToPosition(Vector3 targetPosition)
    {
        characterObject.transform.position = targetPosition;
    }
}

