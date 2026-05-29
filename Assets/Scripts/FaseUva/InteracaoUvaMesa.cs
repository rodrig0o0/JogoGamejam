using UnityEngine;

public class ItemUvaMesa : MonoBehaviour
{
    private bool playerNoGatilho = false;

    void Update()
    {
        // Só funciona se o jogador estiver no gatilho E o ciclo estiver esperando pegar a uva
        if (playerNoGatilho && Input.GetKeyDown(KeyCode.E) && GerenciadorCiclo.EstadoAtual == GerenciadorCiclo.EstadoDoCiclo.ProcurandoUva)
        {
            PegarUvaECopo();
        }
    }

    void PegarUvaECopo()
    {
        GerenciadorCiclo.EstadoAtual = GerenciadorCiclo.EstadoDoCiclo.ComUva;
        Debug.Log("Você pegou a uva e o copo! Vá até a bacia pisotear.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerNoGatilho = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerNoGatilho = false;
    }
}