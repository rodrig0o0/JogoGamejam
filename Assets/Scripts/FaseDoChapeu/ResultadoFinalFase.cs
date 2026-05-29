using UnityEngine;
using TMPro;

public class ResultadoFinalFase3 : MonoBehaviour
{
    [Header("UI do Resultado Final")]
    [SerializeField] private TextMeshProUGUI textoResultadoFinal;

    void Start()
    {
        // Recupera a pontuação final de todas as fases somadas
        int pontuacaoFinal = PlayerPrefs.GetInt("PontosAcumulados", 0);

        if (textoResultadoFinal != null)
        {
            textoResultadoFinal.text = $"PARABÉNS!\nPontuação Final: {pontuacaoFinal}";
        }
        else
        {
            Debug.Log($"Fase 3 Iniciada. Pontuação total guardada no sistema: {pontuacaoFinal}");
        }
    }

    // Se na fase 3 você ainda quiser dar mais pontos para o jogador, pode usar este método:
    public void AdicionarPontosFase3(int pontosGanhos)
    {
        int pontosAtuais = PlayerPrefs.GetInt("PontosAcumulados", 0);
        pontosAtuais += pontosGanhos;
        
        PlayerPrefs.SetInt("PontosAcumulados", pontosAtuais);
        PlayerPrefs.Save();

        if (textoResultadoFinal != null)
        {
            textoResultadoFinal.text = $"PARABÉNS!\nPontuação Final: {pontosAtuais}";
        }
    }
}