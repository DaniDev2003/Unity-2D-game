using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{  
    private Vector2Int movimiento;
    private Vector3 previousPosition;
    public float movementSpeed = 0.10f;
    private bool damageDelayActive = false;
    private float bloqueoMovimientoDuration = 0.5f;
    private float dañoDelay = 1.3f;
    private Transform targetPlayerTransform;
    private Vector3 defaultPosition = new Vector3(0f, 0f, 0f);
    public bool vision = false;
    private Quaternion initialRotation; 
    private bool hasReachedLastKnownPosition = false;
        private Rigidbody2D rb2D;
    private Vector3? savedLastKnownPlayerPosition = null;
    public float rangoDeteccion = 1f;
    private BoxCollider2D boxCollider;

    [SerializeField] private Image enemigoHP;
    [SerializeField] private PMove playerMovement;
    [SerializeField] private EnemyVision enemyVision;
    [SerializeField] private patrullar Patrullar;

    public float damage = 10f;
    public float delay = 2f;
      private float timer = 0f;
    [SerializeField] public VidaJugador vidaJugador;

    private bool playerInRange = false;
    public bool obstacle = false;


private void ActualizarMovimiento()
{
    Vector3 currentPosition = transform.position;
    Vector3 deltaPosition = currentPosition - previousPosition;

    float absDeltaX = Mathf.Abs(deltaPosition.x);
    float absDeltaY = Mathf.Abs(deltaPosition.y);

    if (absDeltaX > absDeltaY)
    {
        movimiento.x = (int)Mathf.Sign(deltaPosition.x);
        movimiento.y = 0;
    }
    else
    {
        movimiento.x = 0;
        movimiento.y = (int)Mathf.Sign(deltaPosition.y);
    }

    previousPosition = currentPosition;
}



    public Vector2Int ObtenerMovimiento()
    {
        return movimiento;
    }

        private void Start()
        {
            // Obtener el componente BoxCollider2D al iniciar
            boxCollider = GetComponent<BoxCollider2D>();
        }

    private void Update()
    {

        ActualizarMovimiento();

    if (playerMovement != null)
    {
    Vector3 playerPosition = playerMovement.transform.position;
    Vector3 enemyPosition = transform.position;
    enemyPosition.y -= 0.1f; // Ajuste de la posición en el eje Y
    float distance = Vector3.Distance(playerPosition, enemyPosition);

        bool enemyInRange = false;
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion);
    foreach (Collider2D collider in colliders)
    {
        if (collider.CompareTag("enemy") && collider.gameObject != gameObject)
        {
            enemyInRange = true;
        }
    }

    if (enemyInRange)
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
    else
    {
       GetComponent<BoxCollider2D>().enabled = true;
    }

        if (distance <= rangoDeteccion && !playerInRange && !obstacle)
        {
            // El jugador está dentro del rango de detección
            playerInRange = true;
            boxCollider.isTrigger = true;
        }
        else if (distance > rangoDeteccion && playerInRange && !obstacle)
        {
            // El jugador está fuera del rango de detección
            playerInRange = false;
            enemyVision.pir(false);
            boxCollider.isTrigger = false;
            timer = 0f; 
            playerMovement.CambiarEstadoBloqueoMovimiento(false); 
            vision = true;
             enemyVision.Visiones(true);
                StartCoroutine(enemyVision.RotateTowardsPlayerCoroutine(1f));
        }
    }

      if (playerInRange && vision)
        {
            timer += Time.deltaTime;

            if (timer >= bloqueoMovimientoDuration && !damageDelayActive)
            {
                playerMovement.CambiarEstadoBloqueoMovimiento(true); // Bloquear movimiento del jugador
                timer = 0f;
                StartCoroutine(DelayedDamage(dañoDelay));
                damageDelayActive = true; // Establecer damageDelayActive como verdadero para evitar que se active nuevamente mientras está en curso el retraso
            }
        }

        if (!hasReachedLastKnownPosition && savedLastKnownPlayerPosition != null && Vector3.Distance(transform.position, (Vector3)savedLastKnownPlayerPosition) <= 0.1f)
        {
                StartCoroutine(enemyVision.RotateTowardsPlayerCoroutine(0.5f));
                hasReachedLastKnownPosition = true;
                StartCoroutine(ResetLastKnownPlayerPositionCoroutine());
        }
    }

    private IEnumerator ResetLastKnownPlayerPositionCoroutine()
{
    yield return new WaitForSeconds(5f);
    savedLastKnownPlayerPosition = null;
    Patrullar.enabled = true;
}

public void Persecucion(Vector3? lastknow)
{
    if (lastknow != null && !playerInRange)
    {
        Vector3 direction = ((Vector3)lastknow - transform.position).normalized;
        Vector3 velocity = direction * movementSpeed * Time.deltaTime;

        transform.Translate(velocity);
    }
}

    public void Visiones(bool estado)
    {
        vision = estado;
    }

        public void hasreach(bool estado)
    {
        hasReachedLastKnownPosition = estado;
    }

    public void FollowLastKnownPosition(Vector3? lastKnownPlayerPosition)
    {
        if (lastKnownPlayerPosition != null)
        savedLastKnownPlayerPosition = lastKnownPlayerPosition;
    {
        Vector3 directionToLastKnownPosition = (Vector3)lastKnownPlayerPosition - transform.position;
            if (directionToLastKnownPosition.magnitude > 0.1f)
        {
            directionToLastKnownPosition.Normalize();
            transform.Translate(directionToLastKnownPosition * movementSpeed * Time.deltaTime);
        }
    } 
        
    }


  public IEnumerator DelayedDamage(float delay)
{
    yield return new WaitForSeconds(delay);

    if (vidaJugador != null && playerInRange)
    {
        vidaJugador.TakeDamage(damage);
    }

    playerMovement.CambiarEstadoBloqueoMovimiento(false); // Restaurar movimiento del jugador después del daño
    damageDelayActive = false;
    timer = 0f;
     
}

private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Vector3 gizmoPosition = transform.position;
    gizmoPosition.y -= 0.1f; // Ajuste de la posición en el eje Y
    Gizmos.DrawWireSphere(gizmoPosition, rangoDeteccion);
}
}