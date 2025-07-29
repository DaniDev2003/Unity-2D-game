using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 

public class Vida : MonoBehaviour
{
   [SerializeField] public Image BarraHP;

    public float HPactual;

    public float MAXHP;

    public float def = 0f; 

    private Color originalColor; 

    private Coroutine healthBarCoroutine;

    [SerializeField] private EnemyVision enemy;

private IEnumerator HideHealthBarAfterDelay(float delay, Image barraHP)
{
    yield return new WaitForSeconds(2f);
    float duration = 2f; // Duración de la transición en segundos
    float targetAlpha = 0f; // Valor de transparencia final (completamente transparente)

    Color startColor = barraHP.color;
    Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
        float t = elapsedTime / duration;
        barraHP.color = Color.Lerp(startColor, targetColor, t);

        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // Asegurarse de establecer el valor final de transparencia para evitar errores de precisión
    barraHP.color = targetColor;

    // Desactivar la barra de vida
    barraHP.enabled = false;
}

public void TakeDamage(float damage)
{
    float damageTaken = Mathf.Max(damage - def, 1f);
        
    HPactual -= damageTaken;


    // Verificar si el enemigo ha perdido toda su vida
    if (HPactual <= 0f)
    {
         Destroy(gameObject);
        // Aquí puedes agregar lógica para manejar la muerte del enemigo
        // ...
    }
    else
    {
        // Reiniciar la corutina actual si existe
        if (healthBarCoroutine != null)
        {
            StopCoroutine(healthBarCoroutine);
        }

        // El enemigo ha recibido daño pero no ha sido derrotado
        BarraHP.enabled = true; // Activar la barra de vida
        BarraHP.fillAmount = HPactual / MAXHP; // Actualizar el valor de llenado de la barra de vida
        
        if (enemy != null)
        {
            StartCoroutine(enemy.HandleDamage());
        }

        // Restablecer el color original de la barra de vida
        BarraHP.color = originalColor;

        // Establecer la posición de la barra de vida sobre el enemigo
        Vector3 posicionBarraVida = transform.position + new Vector3(0f, 1.5f, 0f); // Ajusta los valores según sea necesario
        BarraHP.transform.position = posicionBarraVida;

        // Iniciar la nueva corutina y almacenar la referencia
        healthBarCoroutine = StartCoroutine(HideHealthBarAfterDelay(4f, BarraHP));
    }
}

    void Start()
    {
        originalColor = BarraHP.color; // Almacena el color original en el inicio
    }

    void Update()
    {
        BarraHP.transform.rotation = Quaternion.identity;
        BarraHP.fillAmount= HPactual / MAXHP;
        
    }
}
