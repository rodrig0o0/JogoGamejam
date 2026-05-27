using UnityEngine;

public class GerenciadorPontos : MonoBehaviour
{
    public static GerenciadorPontos Instancia;

    [Header("Configurações de Pontos")]
    public int pontosAtuais = 0;
    [SerializeField] private float tempoMaximoParaBonus = 60f; // 1 minuto para bônus máximo
    private float cronometroFase = 0f;

    void Awake()
    {
        if (Instancia == null) { Instancia = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    void Update()
    {
        cronometroFase += Time.deltaTime;
    }

    // Chamado quando uma tarefa é concluída
    public void AdicionarPontosComBonus(int pontosBase)
    {
        // Calcula bônus baseado em quão rápido foi
        float fatorTempo = Mathf.Clamp01(1f - (cronometroFase / tempoMaximoParaBonus));
        int pontosBonus = Mathf.RoundToInt(pontosBase * fatorTempo);
        
        pontosAtuais += pontosBase + pontosBonus;
        Debug.Log($"Pontos Ganhos: {pontosBase} + Bônus de Velocidade: {pontosBonus}. Total: {pontosAtuais}");
    }

    public void ResetarCronometro()
    {
        cronometroFase = 0f;
    }
}
