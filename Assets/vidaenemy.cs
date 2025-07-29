using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 

public class vidaenemy : MonoBehaviour
{
   [SerializeField] public Image enemyHP;

    public float HPactual;

    public float MAXHP;

    public float def = 0f; 

public void TakeDamage(float damage)
    {
        float damageTaken = Mathf.Max(damage - def, 1f);
        
        HPactual -= damageTaken;

        // Verificar si el jugador ha perdido toda su vida
        if (HPactual <= 0f)
        {
            // Aquí puedes agregar lógica para manejar la muerte del jugador, como mostrar una pantalla de juego perdido, reiniciar el nivel, etc.
        }
    }

    void Update()
    {
        enemyHP.fillAmount= HPactual / MAXHP;
        
    }
}
