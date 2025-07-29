using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class misiones : MonoBehaviour
{ 
    [SerializeField] private GameObject flecha1;
    [SerializeField] private GameObject flecha2;    
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text dialogueText2;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines2;
    [SerializeField] private PMove playerMovement;
    [SerializeField] private List<EnemyVision> enemyVisions;
    [SerializeField] private GameObject dia1Object;
    private bool didDialogueStart;
    private int lineIndex;
    public bool dia1 = true;

    private float typingTime = 0.05f;

//valor de misiones: 0 = inactivo. 1 = en juego 2 = terminada 
    private float Mision1 = 0;   

    private void Start()
    {
        dia1Object.SetActive(true);
        // Buscar todos los objetos "Enemy" en la escena y agregar sus componentes "EnemyVision" a la lista
        EnemyVision[] enemyVisionArray = FindObjectsOfType<EnemyVision>();
        enemyVisions = new List<EnemyVision>(enemyVisionArray);
    }

    // Update is called once per frame
    void Update()
    {
        if(dia1)
        {
            StartCoroutine(FadeOutDia1Image());
        }
        if (Mision1 == 0 && !dia1)
        {
            if(!didDialogueStart)
            {
              StartDialogue();
            }
            else if(dialogueText.text == dialogueLines[lineIndex] && Input.GetKeyDown(KeyCode.Space))
            {
                NextDialogueLine();
            }
            else
            { 
                if(Input.GetKeyDown(KeyCode.Space) && didDialogueStart)
                {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
                //dialogueText2.text = dialogueLines2[lineIndex];
                }
            }
        
        }

            if (Mision1 == 1)
        {
            Vector3 playerPosition = playerMovement.transform.position;
            Vector3 objetivo = new Vector3(-58.3f, 7.27f, 0f);
            float distance = Vector3.Distance(playerPosition, objetivo);

            if(distance < 1f && !didDialogueStart)
            {
              StartDialogue();
            }
            else if(dialogueText2.text == dialogueLines2[lineIndex] && Input.GetKeyDown(KeyCode.Space))
            {
                NextDialogueLine();
            }
            else
            { 
                if(Input.GetKeyDown(KeyCode.Space) && didDialogueStart)
                {
                StopAllCoroutines();
                dialogueText2.text = dialogueLines2[lineIndex];
                //dialogueText2.text = dialogueLines2[lineIndex];
                }
            }
        }
    }

    IEnumerator FadeOutDia1Image()
{
        Image dia1Image = dia1Object.GetComponentInChildren<Image>();
    // Configuración inicial de la transición
    Color startColor = dia1Image.color;
    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
    float duration = 3.5f; // Duración de la transición en segundos
    float elapsedTime = 0f;

    // Realizar la transición gradual
    while (elapsedTime < duration)
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        dia1Image.color = Color.Lerp(startColor, endColor, t);
        yield return null;
    }

    // Desactivar la imagen y actualizar la variable "dia1"
    dia1Object.SetActive(false);
    dia1 = false;
}

    private void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true); 
        lineIndex = 0;


        if (Mision1 == 0)
        {
        StartCoroutine(showLine()); 
        }
        else if (Mision1 == 1 )
        {
        dialogueText.text = string.Empty;
        StartCoroutine(showLine1()); 
        }


        foreach (EnemyVision vision in enemyVisions)
        {
            vision.Cinematica(true); // Desactivar la visión del enemigo
        }
        playerMovement.Cinematica(true);
    }

    private void NextDialogueLine()
    {
        lineIndex++; 
        if(lineIndex < dialogueLines.Length)
        {
            if(Mision1 == 0)
            {
            StartCoroutine(showLine());
            }
            if(Mision1 == 1)
            {
            StartCoroutine(showLine1());
            }
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            playerMovement.Cinematica(false);
            if (Mision1 == 0)
            {
            Mision1 = 1;
            }
            else if (Mision1 == 1)
            {
                Mision1 = 2;
            }
            flecha1.SetActive(true);
            flecha2.SetActive(true);

                        foreach (EnemyVision vision in enemyVisions)
            {
                vision.Cinematica(false); // Desactivar la cinematica del enemigo
            }
        }
    }

    private IEnumerator showLine()
    {
        dialogueText.text = string.Empty;


        foreach(char ch in dialogueLines[lineIndex]) 
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
    }

        private IEnumerator showLine1()
    {
        dialogueText2.text = string.Empty;


        foreach(char ch in dialogueLines2[lineIndex]) 
        {
            dialogueText2.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
    }
}
