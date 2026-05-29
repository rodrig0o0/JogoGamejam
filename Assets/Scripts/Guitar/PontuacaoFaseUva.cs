using UnityEngine;
using TMPro;

public class PontuacaoFaseUva : MonoBehaviour
{
    private int pontos;
    [SerializeField] private TextMeshProUGUI textoPontuacao; 

    void Start()
    {
        // Puxa os pontos acumulados da fase anterior. Se não achar nada, começa com 0.
        pontos = PlayerPrefs.GetInt("PontosAcumulados", 0);
        AtualizarInterface();
    }

    public int AddPontos(int addPontos)
    {
        pontos += addPontos;
        AtualizarInterface();
        SalvarPontuacaoAtual(); // Salva sempre que ganhar pontos
        return pontos;
    }

    public int RedPontos(int redPontos)
    {
        pontos -= redPontos;
        AtualizarInterface();
        SalvarPontuacaoAtual(); // Salva sempre que perder pontos
        return pontos;
    }

    private void AtualizarInterface()
    {
        if (textoPontuacao != null)
        {
            textoPontuacao.text = "Pontos: " + pontos;
        }
    }

    // Salva o valor atualizado no PlayerPrefs para que a Fase 3 possa ler
    private void SalvarPontuacaoAtual()
    {
        PlayerPrefs.SetInt("PontosAcumulados", pontos);
        PlayerPrefs.Save();
    }
}