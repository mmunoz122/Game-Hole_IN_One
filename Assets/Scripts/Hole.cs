//  Importacions
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Creem les variables per gestionar el procés quan la pilota 'cau' al forat i ens dirigeixi a un altre nivell.
public class Hole : MonoBehaviour
{
    // Variable pública per indicar l'escena a la qual volem carregar després de la col·lisió.
    public string id_Screen;

    // Referència al GameManager per gestionar la fi del joc i altres funcions.
    private GameManager gm;

    //  Mètode Start
    void Start()
    {
        // Cerquem un objecte a l'escena amb el tag "GameController" i obtenim el seu component GameManager.
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Mètode Update (no s'usa actualment, així que el podem deixar buit o eliminar si no cal)
    void Update()
    {
    }

    // Mètode per detectar col·lisions amb la pilota
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprovem si l'objecte que ha col·lisionat és la pilota (que té el nom "Ball").
        if (other.gameObject.name == "Ball")
        {
            // Obtenim la velocitat de la pilota.
            Vector2 velocity = other.GetComponent<Rigidbody2D>().velocity;

            // Definim un llindar de velocitat (threshold) per la pilota.
            float speedThreshold = 1.5f;

            // Comprovem si la magnitud de la velocitat de la pilota és inferior al llindar definit.
            if (velocity.magnitude < speedThreshold)
            {
                // Comprovem si estem a l'escena "Home" per reiniciar el joc.
                if (id_Screen == "Home")
                {
                    SceneManager.LoadScene(id_Screen); // Carreguem l'escena "Home".
                    gm.EndGame(); // Finalitzem el joc.
                }
                else
                {
                    // Si no estem a l'escena "Home", destruïm la pilota i carreguem la nova escena indicada.
                    Destroy(other.gameObject); // Eliminem la pilota que ha entrat al forat.
                    SceneManager.LoadScene(id_Screen); // Carreguem l'escena especificada en 'id_Screen'.
                }
            }
        }
    }
}
