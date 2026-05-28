using UnityEngine;

public class GerenciadorBacia : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private SpawnerUvas spawner; // Arraste o objeto Spawner aqui
    [SerializeField] private GameObject painelMiniGame; // O canvas/fundo visual do minigame (se houver)

    private bool playerNaBacia = false;
    private bool miniGameAtivo = false;

    void Start()
    {
        if (painelMiniGame != null) painelMiniGame.SetActive(false);
    }

    void Update()
    {
        // Se o player está perto, apertou E e o minigame não começou ainda
        if (playerNaBacia && Input.GetKeyDown(KeyCode.E) && !miniGameAtivo)
        {
            IniciarMinigame();
        }

        // Se o minigame está rodando, verifica se ele chegou ao fim
        if (miniGameAtivo)
        {
            VerificarFimDoMinigame();
        }
    }

    void IniciarMinigame()
    {
        miniGameAtivo = true;
        if (painelMiniGame != null) painelMiniGame.SetActive(true);

        // Manda o spawner começar a soltar as bolinhas
        spawner.AtivarSpawner();
        Debug.Log("Minigame de amassar uvas iniciado!");
    }

    void VerificarFimDoMinigame()
    {
        // O minigame termina quando o spawner acabou de criar todas as notas E
        // não existem mais notas ativas voando pela tela
        if (spawner.TerminouDeSpawnar && GameObject.FindGameObjectsWithTag("NotaUva").Length == 0)
        {
            FinalizarMinigame();
        }
    }

    void FinalizarMinigame()
    {
        miniGameAtivo = false;
        if (painelMiniGame != null) painelMiniGame.SetActive(false);
        
        Debug.Log("Minigame finalizado! Uvas amassadas.");
        // Aqui você pode colocar a lógica para liberar o vinho ou avançar de fase
    }

    // Detecta se o Player chegou perto da bacia
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNaBacia = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNaBacia = false;
        }
    }
}