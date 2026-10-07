using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject endGamePanel;
    public TMP_Text endGameTitulo;
    public TMP_Text endGameInfo;
    public GameObject botaoReiniciar;

    public TMP_Text tempoText;
    public TMP_Text scoreText;
    public TMP_Text cristaisText;
    public GameObject[] coracoes;
    public RectTransform barraStamina;
    public PlayerMovement player;

    public AudioClip somVitoria;
    public AudioClip somDerrota;

    private bool fimMostrado = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameController.SetColetaveis(GameObject.FindGameObjectsWithTag("Coletavel").Length);
    }

    // Update is called once per frame
    void Update()
    {
        GameController.PassaTempo(Time.deltaTime);

        tempoText.text = "Oxigênio " + GameController.FormataTempo(GameController.tempoRestante);
        tempoText.color = GameController.tempoRestante <= 10 ? Color.red : Color.white;
        scoreText.text = "Pontos: " + GameController.score;
        cristaisText.text = "Cristais: " + GameController.coletados + "/" + GameController.total;

        for (int i = 0; i < coracoes.Length; i++)
        {
            coracoes[i].SetActive(i < GameController.vidas);
        }

        barraStamina.localScale = new Vector3(player.stamina / player.staminaMax, 1, 1);

        // Esc ou botao B do controle volta para o menu a qualquer momento
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(0);
        }

        if (GameController.gameOver && !fimMostrado)
        {
            MostraFim();
        }
    }

    void MostraFim()
    {
        fimMostrado = true;
        endGamePanel.SetActive(true);

        AudioSource audio = GetComponent<AudioSource>();

        if (GameController.venceu)
        {
            endGameTitulo.text = "Missão cumprida!";
            audio.PlayOneShot(somVitoria);
        }
        else
        {
            endGameTitulo.text = GameController.vidas <= 0 ? "Você foi pego!" : "O oxigênio acabou!";
            audio.PlayOneShot(somDerrota);
        }

        endGameInfo.text = "Tempo final: " + GameController.FormataTempo(GameController.tempoJogado) +
                           "\nCristais: " + GameController.coletados + "/" + GameController.total +
                           "\nPontos: " + GameController.score;

        // deixa o botao selecionado para funcionar com o controle
        EventSystem.current.SetSelectedGameObject(botaoReiniciar);
    }
}
