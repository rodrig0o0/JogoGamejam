using UnityEngine;

public class InteragiveisFase2 : MonoBehaviour
{
    public enum TipoEstacao { MesaUvas, BaciaVinho, Copa }
    
    [Header("Configuração desta Estação")]
    public TipoEstacao estacaoAtual;

    [Header("Referência ao Minigame (Apenas para a Bacia)")]
    //[SerializeField] private MiniGameVinho scriptMiniGame;

    private bool playerPerto = false;
    private JogadorFase2 jogador;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jogador = collision.GetComponent<JogadorFase2>();
            playerPerto = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPerto = false;
            jogador = null;
        }
    }

    void Update()
    {
        if (!playerPerto || jogador == null) return;

        switch (estacaoAtual)
        {
            case TipoEstacao.MesaUvas:
                // Pega 1 uva por vez OU pega o copo depois do vinho pronto
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!jogador.vinhoPronto && !jogador.segurandoUva && jogador.uvasNaBacia < 3)
                    {
                        jogador.segurandoUva = true;
                        Debug.Log("Pegou um cacho de uva. Leve até a bacia.");
                    }
                    else if (jogador.vinhoPronto && !jogador.segurandoCopo && jogador.coposComVinho < 3)
                    {
                        jogador.segurandoCopo = true;
                        Debug.Log("Pegou um copo vazio. Vá encher na Copa.");
                    }
                }
                break;

            case TipoEstacao.BaciaVinho:
                // Solta a uva na bacia
                if (Input.GetKeyDown(KeyCode.E) && jogador.segurandoUva)
                {
                    jogador.segurandoUva = false;
                    jogador.uvasNaBacia++;
                    Debug.Log($"Colocou uva na bacia. ({jogador.uvasNaBacia}/3)");
                }

                // Inicia o minigame com M se tiver as 3 uvas
                if (Input.GetKeyDown(KeyCode.M) && jogador.uvasNaBacia >= 3 && !jogador.vinhoPronto)
                {
                    //scriptMiniGame.IniciarMiniGame(jogador);
                }
                break;

            case TipoEstacao.Copa:
                // Enche o copo com vinho (3 vezes ao todo)
                if (Input.GetKeyDown(KeyCode.E) && jogador.segurandoCopo && jogador.vinhoPronto)
                {
                    jogador.segurandoCopo = false;
                    jogador.coposComVinho++;
                    GerenciadorPontos.Instancia.AdicionarPontosComBonus(200); // Pontos por copo cheio
                    Debug.Log($"Copo entregue na copa! ({jogador.coposComVinho}/3)");
                }

                // Fecha a copa com H após os 3 copos
                if (Input.GetKeyDown(KeyCode.H) && jogador.coposComVinho >= 3)
                {
                    GerenciadorPontos.Instancia.AdicionarPontosComBonus(1000); // Super bônus de fim de fase
                    Debug.Log("Fase concluída! Copa Fechada.");
                }
                break;
        }
    }
}
