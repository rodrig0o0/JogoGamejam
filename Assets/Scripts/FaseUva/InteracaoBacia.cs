using System.Collections;
using UnityEngine;

public class InteracaoBacia : MonoBehaviour
{
    [Header("Referências do Minigame")]
    [SerializeField] private GameObject uvaMinigameSystem; 
    [SerializeField] private SpawnUvaController spawnerContoller; 

    private GameObject playerNoGatilho;
    private bool minigameAcontecendo = false;

    void Start()
    {
        if (uvaMinigameSystem != null) uvaMinigameSystem.SetActive(false);
    }

    void Update()
    {
        // MODIFICADO: Só inicia se o estado atual for "ComUva"
        if (playerNoGatilho != null && Input.GetKeyDown(KeyCode.E) && !minigameAcontecendo && GerenciadorCiclo.EstadoAtual == GerenciadorCiclo.EstadoDoCiclo.ComUva)
        {
            StartCoroutine(FluxoMinigame());
        }
        else if (playerNoGatilho != null && Input.GetKeyDown(KeyCode.E) && GerenciadorCiclo.EstadoAtual == GerenciadorCiclo.EstadoDoCiclo.ProcurandoUva)
        {
            Debug.Log("A bacia está vazia. Pegue as uvas na mesa primeiro!");
        }
    }

    IEnumerator FluxoMinigame()
    {
        minigameAcontecendo = true;

        if (playerNoGatilho.TryGetComponent(out JogadorFase2 scriptPlayer))
        {
            scriptPlayer.enabled = false; 
        }

        uvaMinigameSystem.SetActive(true);
        spawnerContoller.IniciarMinigame();

        yield return new WaitUntil(() => spawnerContoller.MinigameTerminou);

        uvaMinigameSystem.SetActive(false);

        GameObject[] uvasRestantes = GameObject.FindGameObjectsWithTag("NotaUva");
        foreach (GameObject uva in uvasRestantes) { Destroy(uva); }

        if (scriptPlayer != null) scriptPlayer.enabled = true;

        // MODIFICADO: Avança o ciclo para VinhoPronto
        GerenciadorCiclo.EstadoAtual = GerenciadorCiclo.EstadoDoCiclo.VinhoPronto;
        Debug.Log("Minigame Concluído! O vinho está pronto no barril/recipiente à esquerda.");

        minigameAcontecendo = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerNoGatilho = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerNoGatilho = null;
    }
}