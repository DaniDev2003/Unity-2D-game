using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionZombie : MonoBehaviour
{

    [SerializeField] private float velocidadMovimiento;

    [SerializeField] public enemy Enemy;

    private Vector2Int direccion;

    private Rigidbody2D rb2D;

    private Animator animator; 

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        direccion = Enemy.ObtenerMovimiento();

        animator.SetFloat("MovimientoX", direccion.x);
        animator.SetFloat("MovimientoY", direccion.y);

        if (direccion != Vector2Int.zero)
        {
            animator.SetFloat("UltimoX", direccion.x);
            animator.SetFloat("UltimoY", direccion.y);
        }
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + (Vector2)direccion * velocidadMovimiento * Time.fixedDeltaTime);
    }
}
