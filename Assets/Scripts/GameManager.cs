// Importacions
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;

// Classe principal
public class GameManager : MonoBehaviour
{
    // Creem una instancia per garantir que només existeixi una instància en tot el joc.
    public static GameManager Instance;

    // Creem una variable per ajustar la vista durant el joc.
    public List<Vector3> cameraPositions;

    // Creem una variable per mantindre la puntuació del jugador que es manté entre escenes.
    public static int playerScore;

    // Creem una variable per gestionar la pilota, la càmera principal i el text de la puntuació.
    private GameObject ball;
    private Camera mainCamera;
    private TextMeshProUGUI scoreText;

    // Creem una variable per l'entrada del nom del jugador, botó de començament, rànquing de puntuacions.
    public TMP_InputField nameInputField;
    public Button submitButton;
    public TextMeshProUGUI rankingText;

    // Creem una variable emmagatzemar el nom del jugador introduït.
    private string playerName;

    // Creem un metode per garantir que només hi ha una instància del GameManager.
    private void Awake()
    {
        // Utilitzem un condicional per comprovar si existeix una instància d'aquest script.
        if (Instance == null)
        {
            // En el cas de que no existeixi, assignem aquesta instància.
            Instance = this;
            // Per evitar que l'objecte es destrueixi al carregar noves escenes.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // En el cas de que ja existeixi una instància, destruïm aquesta per evitar duplicats.
            Destroy(gameObject);
        }
    }

    //  Constructor Start
    private void Start()
    {
        //  Utilitzem un condicional per comprovar si l'escena actual és la pantalla inicial 'Home', si és així configurem la 'UI' d'inici.
        if (SceneManager.GetActiveScene().name == "Home")
        {
            // Mostrem el rànquing de puntuacions de jugadors anteriors.
            DisplayRanking();

            // Obtenim els elements de la UI com el botó de començar i el camp d'entrada del nom del jugador.
            submitButton = GameObject.FindWithTag("StartButton").GetComponent<Button>();
            nameInputField = GameObject.FindWithTag("EnterNameSpace").GetComponent<TMP_InputField>();
        }
        else// En el cas que estiguem en algun nivell, inicialitzem la pilota, el text de la puntuació i la càmera principal.
        {
            
            ball = GameObject.FindWithTag("Ball");//    Inicialitzem la pilota.
            scoreText = GameObject.FindWithTag("Score").GetComponent<TextMeshProUGUI>();//  Inicialitzem el text de la puntuació.
            mainCamera = Camera.main;// Inicialitzem la càmera principal.

            // Actualitzem el text de la puntuació.
            UpdateScoreText();
        }

        // Utilitzem un altre condicional per comprovar si el botó de començar existeix, si és així li afegim una funció per a la seva acció de clic.
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(OnSubmitButtonClicked);//  Li afegim una funció per l'acció de clic.
        }
    }

    //  Constructor Update
    private void Update()
    {
        // Utilitzem un condicional per comprovar si estem en una escena de nivell i si la pilota no és visible a la pantalla.
        if (CheckCurrentScene())
        {
            // Utilitzem un altre condicional per comprovar si la pilota no es veu, si és el cas reposicionem la càmera per intentar veure-la.
            if (ball != null && !IsObjectVisible(ball))
            {
                RepositionCameraToViewBall();// Reposicionem la càmera.
            }
        }

        // Utilitzem uns quants condicionals per canviar d'escena quan l'usuari pressiona una tecla numèrica especifica.
        if (Input.GetKeyDown(KeyCode.Alpha1)) SceneManager.LoadScene("Level_One");
        if (Input.GetKeyDown(KeyCode.Alpha2)) SceneManager.LoadScene("Level_Two");
        if (Input.GetKeyDown(KeyCode.Alpha3)) SceneManager.LoadScene("Level_Three");
        if (Input.GetKeyDown(KeyCode.Alpha4)) SceneManager.LoadScene("Level_Four");
        if (Input.GetKeyDown(KeyCode.Alpha5)) SceneManager.LoadScene("Level_Five");
    }

    // Creem un metode per comprovar si un objecte com pot ser la pilota si és visible a la pantalla.
    private bool IsObjectVisible(GameObject obj)
    {
        // Obtenim les coordenades de la pilota en l'espai de pantalla (viewport).
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(obj.transform.position);

        // Retornem si les coordenades estan dins de la finestra de visibilitat de la càmera.
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1 && screenPoint.z > 0;
    }

    // Creem un metode per reposicionar la càmera quan la pilota no hi està visible.
    private void RepositionCameraToViewBall()
    {
        // Utilitzem un bucle amb el qual iterarem totes les posicions de càmera possibles per veure la pilota.
        foreach (Vector3 position in cameraPositions)
        {
            // Movem la càmera a la nova posició.
            mainCamera.transform.position = position;

            // Utilitzem un condicional per comprovar si la pilota es fa visible, si és així aturem el canvi de posició de la càmera.
            if (IsObjectVisible(ball)) break;
        }
    }

    // Creem un metode per subscrivirnos a l'esdeveniment quan es carrega una nova escena.
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;//    Ho subscrivim a l'event.
    }

    // Creem un metode per desinscrivirnos de l'esdeveniment quan es desactiva l'objecte.
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;//    Ho desinscrivim de l'event.
    }

    // Creem un metode per comprovar si la scena carregada és una escena de nivell.
    private bool CheckCurrentScene()
    {
        // Obtenim el nom de la escena actual.
        string sceneName = SceneManager.GetActiveScene().name;

        // Retornem que si el nom de l'escena comença per 'Level' és un nivell.
        return sceneName.StartsWith("Level");
    }

    // Creem un metode per afegir la puntuació del jugador.
    public void AddScore(int score)
    {
        playerScore += score;// Incrementem 1 al 'Score'.
        UpdateScoreText();//    Actualitzem el text.
    }

    // Creem un metode per obtenir la puntuació actual del jugador.
    public int getScore()
    {
        return playerScore;//   Retornem el 'Score' actual.
    }

    // Creem un metode per controlar quan es carrega una nova escena.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Utilitzem un condicional per comprobar si l'escena carregada no és "Home", si no és inicialitzarem la pilota, la puntuació i la càmera.
        if (scene.name != "Home")
        {
            //  Inicialitzem la pilota, puntuació, i la càmera.
            ball = GameObject.FindWithTag("Ball");
            scoreText = GameObject.FindWithTag("Score").GetComponent<TextMeshProUGUI>();//  Inicialització i obtenció del text de 'Score'.
            mainCamera = Camera.main;

            UpdateScoreText();//    Actualitzem el text del 'Score'.
        }
        else// En el cas contari mostrarem el rànquing de puntuacions.
        {
            
            DisplayRanking();// Mostrem el rànquing.
            submitButton = GameObject.FindWithTag("StartButton").GetComponent<Button>();//  Inicialització i obtenció del botó.
            nameInputField = GameObject.FindWithTag("EnterNameSpace").GetComponent<TMP_InputField>();//  Inicialització i obtenció del text del nom introduït.
        }
        // Utilitzem un altre condicional per afegir l'esdeveniment del clic del botó de començar.
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(OnSubmitButtonClicked);//  En el cas de que hagi donat una valor diferent a 'null', afegim l'esdeviment del clic.
        }
    }

    // Creem un metode per actualitzar el text que mostra la puntuació del jugador.
    private void UpdateScoreText()
    {
        //  Utilitzem una altre vegada un condional però ara per actualitzar el text de el 'Score'.
        if (scoreText != null)
        {
            scoreText.text = "Attempts: " + playerScore.ToString();//  En el cas de que hagi donat una valor diferent a 'null', mostrem el 'Score'.
        }
    }

    // Creem un metode que controla quan és clica el botó redirigeixi al primer nivell.
    private void OnSubmitButtonClicked()
    {
        // Creem un condicional per comprovar si el 'imput' no ha donat 'null' i dirigir al primer nivell.
        if (nameInputField != null)
        {
            // Obtenim el nom del jugador del camp de text.
            playerName = nameInputField.text;

            // Creem un altre condicional per comprovar que que el camp del nom no estigui buit, si esta buit ens dirigira al primer nivell.
            if (!string.IsNullOrEmpty(playerName))
            {
                playerScore = 0; // Indiquem la puntuació a zero per reniciar de nou el joc.
                SceneManager.LoadScene("Level_One"); // Carreguem el primer nivell.
            }
        }
    }

    // Creem un metode per emmagatzemar la puntuació en el ranquing.
    public void SaveScore()
    {
        // Carreguem el rànquing existent des de PlayerPrefs.
        List<(string, int)> ranking = LoadRanking();

        // Afegim el nom del jugador i la seva puntuació al rànquing.
        ranking.Add((playerName, playerScore));

        // Ordenem el rànquing per puntuació, de menor a major.
        ranking.Sort((x, y) => x.Item2.CompareTo(y.Item2));

        // Utilitzem un 'if ' per compraovar no hi ha més de 5 en el ranquing inclòs aquest.
        if (ranking.Count > 5) ranking.RemoveAt(5);

        // Creem un metode per emmagatzemar-lo al rànquing.
        for (int i = 0; i < ranking.Count; i++)
        {
            PlayerPrefs.SetString($"RankingName{i}", ranking[i].Item1);
            PlayerPrefs.SetInt($"RankingScore{i}", ranking[i].Item2);
        }
        PlayerPrefs.Save(); // Guardem els canvis.
    }

    // Creem un metode per carregar el rànquing.
    private List<(string, int)> LoadRanking()
    {
        //  Creem la llista del rànquing.
        List<(string, int)> ranking = new List<(string, int)>();

        // Creem un bucle per llegir totes les dades de les puntuació dels primers jugadors.
        for (int i = 0; i < 5; i++)
        {
            //  Utilitzem un condicional per comprova si existeix una clau en PlayerPrefs per al nom i la puntuació del jugador en la posició 'i'.
            if (PlayerPrefs.HasKey($"RankingName{i}") && PlayerPrefs.HasKey($"RankingScore{i}"))
            {
                // Obtenim el nom del jugador i la seva puntuació de PlayerPrefs.
                string name = PlayerPrefs.GetString($"RankingName{i}");
                int score = PlayerPrefs.GetInt($"RankingScore{i}");
                ranking.Add((name, score));//   Afegim el nom i el 'Score'.
            }
        }
        return ranking;//   Retornem el rànquing.
    }

    // Creem un metode per mostrar el rànquing dels jugadors a la pantalla d'inici.
    private void DisplayRanking()
    {
        // Obtenim el text del rànquing per mostrar-lo a la UI.
        rankingText = GameObject.FindWithTag("Ranking").GetComponent<TextMeshProUGUI>();
        // Carreguem el rànquing des de PlayerPrefs.
        List<(string, int)> ranking = LoadRanking();
        // Mostrem els 5 millors jugadors amb les seves puntuacions.
        rankingText.text = "Top 5 Players:\n";

        //  Creem un bucle per recorrer totes les posicions de les puntuacións del rànquing.
        for (int i = 0; i < ranking.Count; i++)
        {
            rankingText.text += $"{i + 1}. {ranking[i].Item1} - {ranking[i].Item2} hits\n";//   Afegim cada nom i 'Score'.
        }
    }

    // Creem un metode per finalitzar el joc i tornar a l'inici el 'Score' aconseguit en el rànquing.
    public void EndGame()
    {
        // Guardem la puntuació del jugador.
        SaveScore();
        playerScore = 0; // Indiquem la puntuació a 0 per reniciar el'Score'.
        SceneManager.LoadScene("Home"); // Carreguem l'escena d'inici.
    }
}
