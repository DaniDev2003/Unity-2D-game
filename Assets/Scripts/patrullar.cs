using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class patrullar : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float distanciaMinima;
    [SerializeField] private float radioBusqueda;
    [SerializeField] private LayerMask obstaculoLayer;
    [SerializeField] private List<Transform> puntosMovimiento;
    [SerializeField] private EnemyVision enemyVision;
    private Transform objetivo = null;

    private void Start()
    {
        Girar();
    }

private void Update()
{
    if (puntosMovimiento.Count == 0)
        return;

    if (objetivo == null)
    {
        objetivo = SeleccionarPuntoSinObstaculo();
    }
    else
    {
        if (Vector3.Distance(transform.position, objetivo.position) < distanciaMinima)
        {
            objetivo = SeleccionarPuntoSinObstaculo();
            Girar();
        }

        if (objetivo != null)
        {
            enemyVision.RotateAndCenterVision(objetivo.position);
            transform.Translate((objetivo.position - transform.position).normalized * velocidadMovimiento * Time.deltaTime);
        }
    }
}

private Transform SeleccionarPuntoSinObstaculo()
{
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radioBusqueda);
    List<Transform> puntosValidos = new List<Transform>();

    foreach (Collider2D collider in colliders)
    {
        if (collider.CompareTag("PuntoMovimiento"))
        {
            Transform punto = collider.transform;
            Vector3 direccion = punto.position - transform.position;

            // Verificar si hay obstáculos en el camino
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direccion, direccion.magnitude, obstaculoLayer);

            bool obstaculoEnMedio = false;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("os"))
                {
                    obstaculoEnMedio = true;
                    break;
                }
            }

            if (!obstaculoEnMedio)
            {
                // Verificar si la distancia al punto de movimiento es mayor que la distancia mínima
                float distancia = Vector3.Distance(transform.position, punto.position);
                if (distancia > distanciaMinima && distancia < radioBusqueda)
                {
                    puntosValidos.Add(punto);
                }
            }
        }
    }

    if (puntosValidos.Count > 0)
    {
        int indiceAleatorio = Random.Range(0, puntosValidos.Count);
        return puntosValidos[indiceAleatorio];
    }

    return null;
}
    private void Girar()
    {
        if (puntosMovimiento.Count == 0)
            return;

        Transform objetivo = SeleccionarPuntoSinObstaculo();
    }

        private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioBusqueda);
    }
}