using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    public List<GameObject> Bag = new List<GameObject>();
    public GameObject inv; 
    public bool Activar_inv; 

    public GameObject Selector;
    public int ID;
    private GameObject objetoActual;

    public float additionalDistance = 0f;
    public float additionalKnockback = 0f;
    public float additionalRange = 0f;
    public float additionalDamage = 0f;
    public float additionalCooldown = 0f;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Item"))
        {
            for (int i = 0; i < Bag.Count; i++)
            {
                if (Bag[i].GetComponent<Image>().enabled == false)
                {
                    Bag[i].GetComponent<Image>().enabled = true; 
                    Bag[i].GetComponent<Image>().sprite = coll.GetComponent<SpriteRenderer>().sprite;

                // Almacenar el objeto recogido
                objetoActual = coll.gameObject;

                // Eliminar el objeto recogido
                Destroy(coll.gameObject);

                    break;
                }
            }
        }
    }

public void Navegar()
{
    // Obtener el número presionado del teclado (1 al 9)
    int numKeyPressed = -1;
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
        numKeyPressed = 1;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2))
    {
        numKeyPressed = 2;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha3))
    {
        numKeyPressed = 3;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha4))
    {
        numKeyPressed = 4;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha5))
    {
        numKeyPressed = 5;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha6))
    {
        numKeyPressed = 6;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha7))
    {
        numKeyPressed = 7;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha8))
    {
        numKeyPressed = 8;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha9))
    {
        numKeyPressed = 9;
    }

    // Verificar si se presionó un número válido y actualizar el índice ID
    if (numKeyPressed != -1 && numKeyPressed <= Bag.Count)
    {
        ID = numKeyPressed - 1; // Restar 1 para que coincida con el índice de la lista
    }

    // Actualizar la posición del selector
    Selector.transform.position = Bag[ID].transform.position;
}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
void Update()
{
    Navegar();

    if (Activar_inv)
    {
        inv.SetActive(true);
    }
    else 
    {
        inv.SetActive(false);
    }

    if (Input.GetKeyDown(KeyCode.Return))
    {
        Activar_inv = !Activar_inv;
        Debug.Log("Activar_inv");
    }

    // Verificar si hay una casilla seleccionada en el inventario
    if (ID >= 0 && ID < Bag.Count)
    {
        // Obtener el componente Image de la casilla actual
        Image image = Bag[ID].GetComponent<Image>();

        // Verificar si el componente Image existe y tiene un sprite asignado
        if (image != null && image.sprite != null)
        {
            // Verificar el nombre del sprite del item seleccionado
            if (image.sprite.name == "Palo")
            {
                additionalKnockback = 2.5f;
                additionalRange = 20f;
                additionalDamage = 15f;
                additionalCooldown = 1f;
                additionalDistance = 1f;
            } 
            else
            {
                // No es el item "Palo", restablecer las variables a 0 o a sus valores base
                additionalKnockback = 0f;
                additionalRange = 0f;
                additionalDamage = 0f;
                additionalCooldown = 0f;
                additionalDistance = 0f;
                
            }
        }
        else
        {
            // No hay sprite asignado a la casilla actual, restablecer las variables a 0 o a sus valores base
            additionalKnockback = 0f;
            additionalRange = 0f;
            additionalDamage = 0f;
            additionalCooldown = 0f;
            additionalDistance = 0f;
        }
    }
    else
    {
        // No hay ninguna casilla seleccionada, restablecer las variables a 0 o a sus valores base
        additionalKnockback = 0f;
        additionalRange = 0f;
        additionalDamage = 0f;
        additionalCooldown = 0f;
        additionalDistance = 0f;
    }
}
}
