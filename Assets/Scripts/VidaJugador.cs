using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaJugador : MonoBehaviour
{
    [SerializeField] public Image BarraHP;

    public float HPactual;

    public float MAXHP;

    public float def = 0f; 

        public void TakeDamage(float damage)
    {
        float damageTaken = Mathf.Max(damage - def, 1f);
        HPactual -= damageTaken;

        // Actualizar la barra de vida
        BarraHP.fillAmount = HPactual / MAXHP;

        // Verificar si el jugador ha perdido toda su vida
        if (HPactual <= 0f)
        {
            // Aquí puedes agregar lógica para manejar la muerte del jugador

            // Reiniciar el juego cargando nuevamente la escena actual
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
        }
    }

    void Update()
    {
        BarraHP.fillAmount= HPactual / MAXHP;
    }
}
