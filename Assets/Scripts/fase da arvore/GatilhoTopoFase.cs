using UnityEngine;

public class GatilhoTopoArvore : MonoBehaviour
{
    [Tooltip("Arraste o objeto que tem o script PontuacaoArvore aqui")]
    [SerializeField] private PontuacaoArvore gerenciadorPontuacao;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se quem bateu no topo foi o jogador
        if (collision.CompareTag("Player"))
        {
            if (gerenciadorPontuacao != null)
            {
                gerenciadorPontuacao.FinalizarFaseArvore();
            }
            else
            {
                Debug.LogError("Esqueceu de arrastar o objeto com o script PontuacaoArvore para o Inspetor deste gatilho!");
            }
        }
    }
}