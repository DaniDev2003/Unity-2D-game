using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMove : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;

    [SerializeField] private Vector2 direccion; 

    [SerializeField] private Inventario inventario;

    private float tiempoUltimoAtaque;

    public Vida vidaEnemigo;

    private Rigidbody2D rb2D;

    private float movimientoX;

    private float movimientoY;

    private Animator animator; 

    private bool cinematica = false;

    private bool bloquearMovimiento = false;

    float distanciaAtaque = 1.3f;

    private Vector2 ultimaDireccion;

    public float knockbackForce = 22220f;

    public float knockbackDistance = 0.4f;

    private Coroutine knockbackCoroutine;

private void Start() 
{
    tiempoUltimoAtaque = -Mathf.Infinity;
    animator = GetComponent<Animator>();
    rb2D = GetComponent<Rigidbody2D>();
}

public void Cinematica(bool estado)
    {
        cinematica = estado;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cinematica)
        {

        if (Input.GetKeyDown(KeyCode.C))
        {
            float damage = 20f;
            Atacar(damage);
        }

    if (!bloquearMovimiento) // Verificar si el movimiento está bloqueado
    {
        movimientoX = Input.GetAxisRaw("Horizontal");
        movimientoY = Input.GetAxisRaw("Vertical");
        animator.SetFloat("MovimientoX", movimientoX);
        animator.SetFloat("MovimientoY", movimientoY);
    }
        else
    {
        // Si el movimiento está bloqueado, permitir solo cambiar la dirección sin moverse
        movimientoX = Input.GetAxisRaw("Horizontal");
        movimientoY = Input.GetAxisRaw("Vertical");
        direccion = new Vector2(movimientoX, movimientoY).normalized;
    }

        if(movimientoX != 0 || movimientoY != 0) 
        {
            ultimaDireccion = new Vector2(movimientoX, movimientoY).normalized;
            animator.SetFloat("UltimoX", movimientoX);
            animator.SetFloat("UltimoY", movimientoY);
        }
            if (!bloquearMovimiento) // Verificar nuevamente si el movimiento está bloqueado
    {
        direccion = new Vector2(movimientoX, movimientoY).normalized;
    }
  }
    else
    {
        // Bloquear el movimiento del jugador
        movimientoX = 0;
        movimientoY = 0;
        animator.SetFloat("MovimientoX", movimientoX);
        animator.SetFloat("MovimientoY", movimientoY);
        direccion = Vector2.zero;
    }
 }

public void MoverJugadorHaciaPosicion(Vector3 posicion)
{
    if (!bloquearMovimiento)
    {
        // Calcula la dirección hacia la posición objetivo
        direccion = (posicion - transform.position).normalized;

        // Mueve al jugador hacia la posición objetivo
        rb2D.MovePosition(rb2D.position + direccion * velocidadMovimiento * Time.deltaTime);

        // Actualiza los parámetros de animación
        movimientoX = direccion.x;
        movimientoY = direccion.y;
        animator.SetFloat("MovimientoX", movimientoX);
        animator.SetFloat("MovimientoY", movimientoY);

        if (movimientoX != 0 || movimientoY != 0)
        {
            ultimaDireccion = new Vector2(movimientoX, movimientoY).normalized;
            animator.SetFloat("UltimoX", movimientoX);
            animator.SetFloat("UltimoY", movimientoY);
        }
    }
}



    private void FixedUpdate() 
    {
         if (!bloquearMovimiento) // Verificar si el movimiento está bloqueado
        {
            rb2D.MovePosition(rb2D.position + direccion * velocidadMovimiento * Time.fixedDeltaTime);
        }
    }

    // Método para cambiar el estado de bloqueo del movimiento
    public void CambiarEstadoBloqueoMovimiento(bool estado)
    {
        bloquearMovimiento = estado;
    }


private void OnDrawGizmos()
{
    // Definir el ángulo del cono de ataque
    float anguloAtaque = 15f;

    // Obtener la dirección del movimiento del jugador
    Vector2 direccionMovimiento = new Vector2(movimientoX, movimientoY).normalized;

    // Obtener la dirección hacia adelante del personaje basada en la dirección del movimiento del jugador
    Vector2 direccionPersonaje = direccionMovimiento != Vector2.zero ? direccionMovimiento : ultimaDireccion;

    // Calcular el ángulo de ataque basado en la dirección del movimiento del jugador
    float anguloMovimiento = Vector2.SignedAngle(Vector2.right, direccionPersonaje);

    // Calcular los puntos para los límites izquierdo y derecho del cono de ataque
    Quaternion rotacionIzquierda = Quaternion.Euler(0f, 0f, -anguloAtaque / 2f + anguloMovimiento);
    Quaternion rotacionDerecha = Quaternion.Euler(0f, 0f, anguloAtaque / 2f + anguloMovimiento);

    Vector3 limiteIzquierdo = rotacionIzquierda * Vector2.right;
    Vector3 limiteDerecho = rotacionDerecha * Vector2.right;

    // Dibujar el cono de ataque como líneas
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, transform.position + (limiteIzquierdo * distanciaAtaque));
    Gizmos.DrawLine(transform.position, transform.position + (limiteDerecho * distanciaAtaque));

    // Dibujar el arco del cono de ataque
    int numSegments = 100;
    float angleStep = anguloAtaque / numSegments;

    for (int i = 0; i <= numSegments; i++)
    {
        float angle = -anguloAtaque / 2f + angleStep * i;
        Quaternion segmentRotation = Quaternion.Euler(0f, 0f, angle + anguloMovimiento);
        Vector3 segmentDirection = segmentRotation * Vector2.right;
        Gizmos.DrawLine(transform.position, transform.position + (segmentDirection * distanciaAtaque));
    }
}

private void Atacar(float damage)
{
    // Sumar el daño adicional del inventario
    damage += inventario.additionalDamage;

    // Definir el ángulo del cono de ataque
    float anguloAtaque = 15f + inventario.additionalRange;

    // Obtener la dirección del movimiento del jugador
    Vector2 direccionMovimiento = new Vector2(movimientoX, movimientoY).normalized;

    // Obtener la dirección hacia adelante del personaje basada en la dirección del movimiento del jugador
    Vector2 direccionPersonaje = direccionMovimiento != Vector2.zero ? direccionMovimiento : ultimaDireccion;

    // Calcular el ángulo de ataque basado en la dirección del movimiento del jugador
    float anguloMovimiento = Vector2.SignedAngle(Vector2.right, direccionPersonaje);

        // Activar el trigger correspondiente según la dirección de ataque
    if (anguloMovimiento > -45f && anguloMovimiento <= 45f)
    {
        animator.SetTrigger("AtacarD"); // Ataque hacia la derecha
    }
    else if (anguloMovimiento > 45f && anguloMovimiento <= 135f)
    {
        animator.SetTrigger("AtacarA"); // Ataque hacia arriba
    }
    else if (anguloMovimiento > 135f || anguloMovimiento <= -135f)
    {
        animator.SetTrigger("AtacarI"); // Ataque hacia la izquierda
    }
    else
    {
        animator.SetTrigger("AtacarB"); // Ataque hacia abajo
    }

    // Obtener todos los colliders en el cono de ataque
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, distanciaAtaque + inventario.additionalDistance);

    // Verificar si hay un enemigo en el cono de ataque
    foreach (Collider2D collider in colliders)
    {
        if (collider.CompareTag("enemy"))
        {
                    // Obtener el componente EnemyVision del objeto enemigo o de uno de sus hijos
        EnemyVision Enemy = collider.GetComponentInChildren<EnemyVision>();


            // Calcular la dirección hacia el enemigo
            Vector2 direccionEnemigo = collider.transform.position - transform.position;

            // Calcular el ángulo entre la dirección del personaje y la dirección del enemigo
            float anguloEnemigo = Vector2.Angle(Vector2.right, direccionEnemigo);

            // Calcular la diferencia de ángulo entre el ángulo de ataque y el ángulo de movimiento del jugador
            float diferenciaAngulo = Mathf.Abs(anguloEnemigo - anguloMovimiento);

            // Verificar si el enemigo está dentro del ángulo de ataque
            if (diferenciaAngulo <= anguloAtaque)
            {
                Vector2 direccionAtaque = direccionPersonaje.normalized;

                                // Verificar si hay un obstáculo entre el jugador y el enemigo
                RaycastHit2D hit = Physics2D.Linecast(transform.position, collider.transform.position, LayerMask.GetMask("os"));
                if (hit.collider == null)
                {
                    Vida vidaEnemigo = collider.GetComponent<Vida>();
                    if (Enemy != null && Enemy.vision && Time.time - tiempoUltimoAtaque >= 0.7f)
                {
                    // Aplicar daño normal si el enemigo tiene visión
                    vidaEnemigo.TakeDamage(damage);
                    bloquearMovimiento = false;


                    // Aplicar knockback al enemigo
                    Rigidbody2D enemyRigidbody = collider.GetComponent<Rigidbody2D>();
                    if (enemyRigidbody != null)
                    {
                        Vector2 knockbackDirection = (collider.transform.position - transform.position).normalized;
                        Vector2 knockbackTarget = (Vector2)collider.transform.position + knockbackDirection * (knockbackDistance + inventario.additionalKnockback);
                        enemyRigidbody.MovePosition(knockbackTarget);
                        
                        // Aplicar cooldown adicional del inventario
                        tiempoUltimoAtaque = Time.time + inventario.additionalCooldown;
                    }
                }
                else if ((Enemy != null && !Enemy.vision && Time.time - tiempoUltimoAtaque >= 0.7f))
                {
                    // Aplicar daño x3 si el enemigo no tiene visión
                    vidaEnemigo.TakeDamage(damage * 3);
                    bloquearMovimiento = false;

                    // Aplicar knockback al enemigo
                    Rigidbody2D enemyRigidbody = collider.GetComponent<Rigidbody2D>();
                    if (enemyRigidbody != null)
                    {
                        Vector2 knockbackDirection = (collider.transform.position - transform.position).normalized;
                        Vector2 knockbackTarget = (Vector2)collider.transform.position + knockbackDirection * (knockbackDistance + inventario.additionalKnockback);
                        enemyRigidbody.MovePosition(knockbackTarget);
                        
                        // Aplicar cooldown adicional del inventario
                        tiempoUltimoAtaque = Time.time + inventario.additionalCooldown;
                    }
                }
            }
       }
    }  
  } 
}
}