//  Importació
using UnityEngine;

// Creem la clase que amb la que és realitzara el procés de moviment de la plataforma.
public class MovingObject : MonoBehaviour
{
    // Creem les variables per controlar la distancia, la velocitat y la orientació del moviment.
    public float moveDistance = 5f;
    public float speed = 2f;
    public bool isVertical = false;

    //  Creem una variable per indicar la posició inicial de l'objecte abans de començar el moviment.
    private Vector3 startPoint;

    //  Creem una altre variable per indicar la posició final on l'objecte s'ha de moure.
    private Vector3 endPoint;

    //  Creem una altre variable per fer la transició entre la posició inicial i la posició final.
    private float t;

    //  Creem aquesta variable per indicar si l'objecte s'està movent cap al punt final indicant 'true'
    //  o cap al punt inicial indicant 'false'.
    private bool goingToEnd = true;

    //  Constructor Start
    void Start()
    {
        // Emmagatzemem la posició actual de l'objecte, que serà el punt inicial.
        startPoint = transform.position;

        // Fem el calcul del punt final 'endPoint' segons si el moviment es vertical o horizontal.
        endPoint = isVertical
            ? startPoint + new Vector3(0, moveDistance, 0)  // Indiquem com ha de ser el moviment vertical.
            : startPoint + new Vector3(moveDistance, 0, 0); // Indiquem com ha de ser el moviment horizontal.
    }

    //  Constructor Update
    void Update()
    {
        // Creem un condicional per comprobar si la plataforma s'esta movent al punt final
        if (goingToEnd)
        {
            // En el cas que sigui així incrementarem la variable 't' per moure la plataforma al punt final.
            t += Time.deltaTime * speed;

            // Creem una altre condicional per que si la variable 't' ha arribat o superat a 1, és el final del moviment cap al punt final.
            if (t >= 1f)
            {
                t = 1f;  // Inicialitzem la variable 't' a 1 per evitar que superi a 1.
                goingToEnd = false;  // Indiquem  'goingToEnd' sigui 'false' per anar al punt inicial.  
            }
        }
        else
        {
            // En el cas que sigui així restem 1 a la variable 't' per moure la plataforma al punt inicial.
            t -= Time.deltaTime * speed;

            // Creem una altre condicional per que si la variable 't' ha arribat al 0, és el final del moviment cap al punt inicial.
            if (t <= 0f)
            {
                t = 0f;  // Inicialitzem la variable 't' a 0 per evitar que sigui menor a 0.
                goingToEnd = true;  // Indiquem  'goingToEnd' sigui 'true' per anar al punt final.
            }
        }

        // Fem la transició de la plataforma entre 'startPoint' y 'endPoint' segons el valor obtingut de la variable 't'.
        transform.position = Vector3.Lerp(startPoint, endPoint, t);
    }
}

