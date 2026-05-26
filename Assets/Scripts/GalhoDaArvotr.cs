using UnityEngine;

public class Galhos : MonoBehaviour
{
    [Header("Configurações do Efeito")]
    [SerializeField] private float velocidadeComLentidao = 2f; // Velocidade que o player terá ao colidir
    [SerializeField] private float duracaoLentidao = 2f; // Tempo que dura a lentidão
    [SerializeField] private float forcaEmpurraoAtras = 1.5f; // Distância do recuo para trás

    private bool jaAtivou = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MovimentoInfinito jogador = collision.GetComponent<MovimentoInfinito>();

        if (jogador != null && !jaAtivou)
        {
            jaAtivou = true;

            // Ativa a função corrigida dentro do script do jogador
            jogador.AplicarPenalidadeGalho(velocidadeComLentidao, duracaoLentidao, forcaEmpurraoAtras);

        
        }
    }
}
