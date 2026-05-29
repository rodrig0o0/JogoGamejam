using UnityEngine;

public class GerenciadorCiclo : MonoBehaviour
{
    // Estados do nosso ciclo
    public enum EstadoDoCiclo { ProcurandoUva, ComUva, VinhoPronto }
    
    // Variável estática para que todos os scripts saibam o estado atual do jogo
    public static EstadoDoCiclo EstadoAtual = EstadoDoCiclo.ProcurandoUva;

    void Start()
    {
        // Garante que o jogo comece do início
        EstadoAtual = EstadoDoCiclo.ProcurandoUva;
    }
}