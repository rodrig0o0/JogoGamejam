using UnityEngine;
using TMPro; // Necessário para usar o TextMeshPro
using UnityEngine.SceneManagement; // Necessário para trocar de cena

public class TimerFase : MonoBehaviour
{
    [Header("Configurações do Tempo")]
    [SerializeField] private float tempoRestante = 60f; // Tempo em segundos (ex: 1 minuto)
    
    [Header("Referências de UI")]
    [SerializeField] private TextMeshProUGUI textoTimer; // Arraste o texto do Canvas aqui

    [Header("Próxima Fase")]
    [SerializeField] private string nomeDaProximaCena; // Digite o nome EXATO da próxima fase

    private bool tempoAcabou = false;

    void Update()
    {
        if (tempoAcabou) return;

        if (tempoRestante > 0)
        {
            tempoRestante -= Time.deltaTime;
            AtualizarTextoDoTimer(tempoRestante);
        }
        else
        {
            tempoRestante = 0;
            tempoAcabou = true;
            MudarDeFase();
        }
    }

    void AtualizarTextoDoTimer(float tempoParaMostrar)
    {
        // Garante que não vai mostrar valores negativos
        if (tempoParaMostrar < 0) tempoParaMostrar = 0;

        // Calcula minutos e segundos
        float minutos = Mathf.FloorToInt(tempoParaMostrar / 60);
        float segundos = Mathf.FloorToInt(tempoParaMostrar % 60);

        // Formata o texto para ficar bonito na tela (Ex: 01:30 ou 00:45)
        textoTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        
        // Opcional: Se o tempo estiver abaixo de 10 segundos, muda a cor do texto para vermelho
        if (tempoParaMostrar <= 10f)
        {
            textoTimer.color = Color.red;
        }
    }

    void MudarDeFase()
    {
        Debug.Log("O Tempo acabou! Mudando para a próxima fase...");
        
        // Verifica se você digitou um nome para a cena antes de tentar carregar
        if (!string.IsNullOrEmpty(nomeDaProximaCena))
        {
            SceneManager.LoadScene(nomeDaProximaCena);
        }
        else
        {
            Debug.LogError("Erro: Você esqueceu de colocar o nome da próxima cena no script do Timer!");
        }
    }
}