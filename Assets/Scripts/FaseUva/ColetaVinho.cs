using UnityEngine;

public class ColetaVinho : MonoBehaviour
{
    private bool playerNoGatilho = false;

    void Update()
    {
        // Só funciona se o minigame já acabou (Estado: VinhoPronto)
        if (playerNoGatilho && Input.GetKeyDown(KeyCode.E) && GerenciadorCiclo.EstadoAtual == GerenciadorCiclo.EstadoDoCiclo.VinhoPronto)
        {
            PegarVinho();
        }
    }

    void PegarVinho()
    {
        Debug.Log("Você coletou o vinho pronto! O ciclo foi resetado. Vá buscar mais uvas na mesa.");
        
        // Reseta o ciclo de volta para o começo
        GerenciadorCiclo.EstadoAtual = GerenciadorCiclo.EstadoDoCiclo.ProcurandoUva;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNoGatilho = true;
            
            // Avisa o jogador caso ele chegue aqui na hora certa
            if(GerenciadorCiclo.EstadoAtual == GerenciadorCiclo.EstadoDoCiclo.VinhoPronto)
            {
                Debug.Log("O vinho está pronto aqui! Aperte E para coletar.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerNoGatilho = false;
    }
}