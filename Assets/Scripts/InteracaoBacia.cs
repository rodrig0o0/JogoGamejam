using System.Collections;
using UnityEngine;

public class InteracaoBacia : MonoBehaviour
{
    [Header("Referências do Minigame")]
    [SerializeField] private GameObject uvaMinigameSystem; // Arraste o objeto "UvaMinigameSystem" aqui
    [SerializeField] private SpawnUvaController spawnerContoller; // Arraste o "UvaMinigameSystem" aqui também

    private GameObject playerNoGatilho;
    private bool minigameAcontecendo = false;

    void Start()
    {
        // Garante que o sistema comece desativado ao iniciar a fase
        if (uvaMinigameSystem != null)
            uvaMinigameSystem.SetActive(false);
    }

    void Update()
    {
        // Se o player estiver no collider, apertar E e o jogo não tiver começado ainda
        if (playerNoGatilho != null && Input.GetKeyDown(KeyCode.E) && !minigameAcontecendo)
        {
            StartCoroutine(FluxoMinigame());
        }
    }

    IEnumerator FluxoMinigame()
    {
        minigameAcontecendo = true;

        // 1. Trava o movimento do Player
        // Nota: Altere "PlayerController" para o nome exato do script de movimento do seu personagem
        if (playerNoGatilho.TryGetComponent(out JogadorFase2 scriptPlayer))
        {
            scriptPlayer.enabled = false; 
        }

        // 2. Ativa o sistema visual do minigame e inicia os spawns
        uvaMinigameSystem.SetActive(true);
        spawnerContoller.IniciarMinigame();

        // 3. Fica esperando até o spawner dizer que os 30 segundos acabaram
        yield return new WaitUntil(() => spawnerContoller.MinigameTerminou);

        // 4. O tempo acabou! Desativa o sistema visual
        uvaMinigameSystem.SetActive(false);

        // 5. Destrói as uvas que porventura ainda estejam caindo na tela para limpar a cena
        GameObject[] uvasRestantes = GameObject.FindGameObjectsWithTag("NotaUva");
        foreach (GameObject uva in uvasRestantes)
        {
            Destroy(uva);
        }

        // 6. Devolve o movimento para o jogador
        if (scriptPlayer != null)
        {
            scriptPlayer.enabled = true;
        }

        minigameAcontecendo = false;
    }

    // Detecta quando o jogador chega perto da Bacia
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNoGatilho = collision.gameObject;
        }
    }

    // Detecta quando o jogador se afasta da Bacia
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNoGatilho = null;
        }
    }
}