using UnityEngine;
using TMPro; // Necessário para usar o TextMeshPro

public class PontuacaoFaseUva : MonoBehaviour
{
    [SerializeField] private int pontos;
    [SerializeField] private TextMeshProUGUI textoPontuacao; // Arraste seu texto do Canvas para cá no Inspetor

    void Start()
    {
        AtualizarInterface();
    }

    public int AddPontos(int addPontos)
    {
        pontos += addPontos;
        AtualizarInterface();
        return pontos;
    }

    public int RedPontos(int redPontos)
    {
        pontos -= redPontos;
        AtualizarInterface();
        return pontos;
    }

    // Método auxiliar para manter o texto sempre atualizado
    private void AtualizarInterface()
    {
        if (textoPontuacao != null)
        {
            textoPontuacao.text = "Pontos: " + pontos;
        }
    }
}