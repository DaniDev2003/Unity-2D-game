using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class EnemyVision : MonoBehaviour
{ 
    public float detectionAngle = 45f;
    public bool vision = false;
    public float detectionRange = 10f;
    public float rotationSpeed = 20f;
    private Quaternion initialRotation; 
    private float visualFollowDuration = 1f;
    private float visualFollowTimer = 0f; 
    float rotationSpeedMultiplier = 2f;
    private bool playerInRange = false;
    private Vector3? lastKnownPlayerPosition = null;
    private Vector3 defaultPosition = new Vector3(0f, 0f, 0f);
    private bool isFollowingLastKnownPosition = false;
    public bool obstacle = false;
    private bool cinematica = false;
    private Transform targetPlayerTransform;
    [SerializeField] public enemy Enemy;
    [SerializeField] private Transform PJTransform; 
    [SerializeField] private patrullar Patrullar;

    /*    private void OnDrawGizmosSelected()
    {
        // Convertir el ángulo de grados a radianes
        float coneAngleRadians = detectionAngle * Mathf.Deg2Rad;

        // Calcular los vectores de dirección para los límites del cono
        Vector3 forwardDirection = transform.up;
        Vector3 leftLimit = Quaternion.Euler(0f, 0f, -detectionAngle * 0.5f) * forwardDirection;
        Vector3 rightLimit = Quaternion.Euler(0f, 0f, detectionAngle * 0.5f) * forwardDirection;

        // Dibujar el cono utilizando líneas Gizmos
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, forwardDirection * detectionRange);
        Gizmos.DrawRay(transform.position, leftLimit * detectionRange);
        Gizmos.DrawRay(transform.position, rightLimit * detectionRange);

        // Dibujar los arcos del cono utilizando Handles
        float startAngle = Mathf.Atan2(leftLimit.y, leftLimit.x) * Mathf.Rad2Deg;
        float endAngle = Mathf.Atan2(rightLimit.y, rightLimit.x) * Mathf.Rad2Deg;
        Handles.DrawWireArc(transform.position, Vector3.back, Quaternion.Euler(0f, 0f, startAngle) * forwardDirection, detectionAngle, detectionRange);
        Handles.DrawWireArc(transform.position, Vector3.back, Quaternion.Euler(0f, 0f, endAngle) * forwardDirection, -detectionAngle, detectionRange);

    } */

public IEnumerator HandleDamage()
    {
            vision = true;
            Enemy.Visiones(true);
            StartCoroutine(RotateTowardsPlayerCoroutine(1f));
            StopCoroutine(Enemy.DelayedDamage(0f));
            yield return null;
    }

public IEnumerator RotateTowardsPlayerCoroutine(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            if (vision)
            {
                Vector3 directionToPlayer = PJTransform.position - transform.position;
                directionToPlayer.Normalize();
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * rotationSpeedMultiplier * Time.deltaTime);

                elapsedTime += Time.deltaTime;
            }
            else
            {
                // Si la visión es falsa, salimos de la corutina
                yield break;
            }

            yield return null;
        }
    }


    public void Visiones(bool estado)
    {
        vision = estado;
    }

        public void pir(bool estado)
    {
        playerInRange = estado;
    }

    public IEnumerator WaitAndDisableVisionCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        vision = false;
        Enemy.Visiones(false);
    }

public void Cinematica(bool estado)
    {
        cinematica = estado;
    }

void Start()
{
    targetPlayerTransform = PJTransform;
}

  private void Update()
    {
        if (!cinematica)
        {
                Vector3 directionToPlayer = PJTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

    RaycastHit2D[] obstacleHits = Physics2D.LinecastAll(transform.position, PJTransform.position, Physics2D.DefaultRaycastLayers);
    foreach (RaycastHit2D hit in obstacleHits)

    {
        if (hit.collider.CompareTag("os"))
        {
            obstacle = true;
            break;
        }
        else
        {
            obstacle = false;
        }
    }

                float angleToPlayer = Vector3.Angle(directionToPlayer, transform.up);
            if (!playerInRange && !obstacle && distanceToPlayer <= detectionRange && angleToPlayer <= detectionAngle * 0.5f)
            {
            directionToPlayer.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * rotationSpeedMultiplier * Time.deltaTime);
            Enemy.Persecucion(lastKnownPlayerPosition);
            lastKnownPlayerPosition = PJTransform.position;
            isFollowingLastKnownPosition = false;
            Enemy.hasreach(false);
            vision = true;
            Enemy.Visiones(true);
            Patrullar.enabled = false;
            }
            else
            { 
            isFollowingLastKnownPosition = true;
            }

    if (isFollowingLastKnownPosition && lastKnownPlayerPosition != null && !playerInRange)
    {
        Enemy.FollowLastKnownPosition(lastKnownPlayerPosition);
    } 
       else
    {
        isFollowingLastKnownPosition = false;
        initialRotation = transform.rotation;
        visualFollowTimer = visualFollowDuration;

            if (vision && !playerInRange)
            {
               StartCoroutine(RotateTowardsPlayerCoroutine(0.1f));
            }
    }
        }
}

public void RotateAndCenterVision(Vector3 targetPosition)
{
    // Calcular la dirección hacia el objetivo
    Vector3 directionToTarget = targetPosition - transform.position;
    directionToTarget.Normalize();

    // Calcular la rotación necesaria para mirar hacia el objetivo
    Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);

    // Rotar y centrar la visión gradualmente hacia el objetivo
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * rotationSpeedMultiplier * Time.deltaTime);
}



}