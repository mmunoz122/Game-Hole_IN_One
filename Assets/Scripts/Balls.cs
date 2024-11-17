//  Importacions
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

// Creem la classe per controlar el funcionament de la pilota.
public class Ball : MonoBehaviour
{
    // Creem les variables per controlar la força, els segons i la velocitat amb què es mou la pilota.
    private float force;
    public int seconds = 1; // Inicialitzem els segons a 1.
    public float speed = 1; // Inicialitzem la velocitat a 1.

    // Creem la variable per al 'Slider' per mostrar de forma visual mitjançant una barra la força que s'aplica a la pilota.
    public Slider slider;

    // Creem la variable per al LineRenderer per incorporar la direcció en forma de línia.
    private LineRenderer lineRenderer;

    // Creem la variable per indicar la longitud màxima de la línia i la inicialitzem a '2.0f'.
    public float maxLineLength = 2.0f;

    // Constructor Start
    void Start()
    {
        // Amaguem el 'slider' al començament.
        slider.gameObject.SetActive(false);

        // Configurem el LineRenderer.
        lineRenderer = gameObject.AddComponent<LineRenderer>(); // Afegim el 'LineRenderer'.
        lineRenderer.positionCount = 2; // Indiquem la posició de la pilota i el cursor.
        lineRenderer.startWidth = 0.05f; // Indiquem l'amplada inicial de la línia.
        lineRenderer.endWidth = 0.05f; // Indiquem l'amplada final de la línia.
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Indiquem el material de la línia.
        lineRenderer.startColor = Color.blue; // Indiquem el color inicial de la línia.
        lineRenderer.endColor = Color.cyan; // Indiquem el color final de la línia.
        lineRenderer.sortingOrder = 1; // Indiquem l'ordre de classificació si és necessari.
    }

    // Constructor Update
    void Update()
    {
        // Obtenim la posició del ratolí.
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y));
        mousePosition.z = 0; // Indiquem que Z sigui 0 .

        // Calculem la direcció des de la pilota fins al ratolí.
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Obtenim la velocitat de la pilota.
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;

        // Utilitzem un condicional per mostrar la línia només si la velocitat és baixa.
        if (velocity.magnitude <= 0.08f)
        {
            lineRenderer.enabled = true;//  Indiquem 'true' en el cas que és cumpleixi la condició.
        }
        else
        {
            lineRenderer.enabled = false;//  Indiquem 'false' en el cas que no és cumpleixi la condició.
        }

        // Utilitzem un condicional per controlar la força quan la velocitat és baixa.
        if (velocity.magnitude <= 0.08f)
        {
            // Si aquest és el cas, fem servir un altre condicional per acumular la força mantenint la barra
            // espaiadora premuda.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                force = 0; // Indiquem ala variable de la força 0 per reniciar la força quan es prem la barra
                           // d'espai.
                slider.gameObject.SetActive(true); // Mostra el "Slider" en aquest moment com un indicador que
                                                   // la barra espaiadora s'està prement.
            }

            // Utilitzem un altre 'if' per incrementar la força mantenint premuda la tecla espaiadora.
            if (Input.GetKey(KeyCode.Space))
            {
                //  Comprobem amb altre condicional que els segons no tingui el valor zero en el cas que
                //  si li cambiem el valor a 1.
                if (seconds == 0) { 
                    seconds = 1; } 

                force += Time.deltaTime / seconds; // Incrementem la força amb el temps.

                //  Comprobem amb altre condicional que la força no tingui el valor superior a 1 en el cas que
                //  si li cambiem el valor a 1.
                if (force >= 1) { force = 1; } // Limitem la força a un màxim d'1.

                slider.value = force; // Introduïm el valor de la força al 'Slider'.
            }

            // Utilitzem un altre 'if' per aplicar la força quan es deixa premer la tecla espaiadora.
            if (Input.GetKeyUp(KeyCode.Space))
            {
                GetComponent<Rigidbody2D>().AddForce(direction * (force * speed), ForceMode2D.Impulse);//   Introduïm la força corresponent obtinguda.
                GameManager.Instance.AddScore(1);  // Afegim 1 punt al 'Score'.
                Debug.Log("Score: " + GameManager.Instance.getScore());//   Indiquem per consola el 'Score'.
                Invoke("resetForce", 2); // Reiniciem la força després de 2 segons.
            }
        }

        // Actualitzem la posició del LineRenderer.
        lineRenderer.SetPosition(0, transform.position); // Posició de la pilota.

        // Calculem la distància del ratolí a la pilota.
        float distance = Vector2.Distance(transform.position, mousePosition);

        // Utilitzem un altre 'if' per posar un limit a la longitud de la línia si la distància excedeix el màxim.
        if (distance > maxLineLength)
        {
            Vector2 adjustedPosition = (Vector2)transform.position + direction * maxLineLength;//   Indiquem la logitud maxima que pot tindre la linia, indicant la posició,
                                                                                               //   direcció i la longitud maxima.
            lineRenderer.SetPosition(1, adjustedPosition); // Indiquem la posició de la línia.
        }
        else // En el cas contari indiquem la posició del ratolí.
        {
            lineRenderer.SetPosition(1, mousePosition); // Indiquem la posició del ratolí.
        }
    }

    // Creem el metodè per reiniciar la força i amaga el 'Slider'.
    private void resetForce()
    {
        force = 0;// Indiquem 0 a la força per reniciar-la.
        slider.value = 0; // Indiquem 0 per reiniciar el valor de la força.
        slider.gameObject.SetActive(false); // Indiquem 'false' per amagar el 'Slider'.
    }
}
