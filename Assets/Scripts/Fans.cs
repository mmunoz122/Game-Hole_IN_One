//  Importacions
using UnityEngine;

// Creem  la clase per realitzar el procés de fer la animació del vent i la colició del vente i la pilota.
public class Fans : MonoBehaviour
{
    // Creem les variables de la direcció i la força del ventilador.
    public Vector2 direccion = Vector2.right;  // Indiquem la direcció del ventilador, i la inicialitzem.
    public float fuerza = 5f;                  // Indiquem la força del ventilador, inicialitzan-lo a '5f'.

    private void Trigger2D(Collider2D other)
    {
        // Utilitzem un condicional per per comprobar si l'objecte que entra es la pilota.
        if (other.gameObject.name == "Ball")
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();// Indiquem el 'Rigidbody2D' de la pilota.

            if (rb != null)
            {
                rb.AddForce(direccion.normalized * fuerza);// Indiquem la força a la dirección indicada.
            }
        }
    }
}
