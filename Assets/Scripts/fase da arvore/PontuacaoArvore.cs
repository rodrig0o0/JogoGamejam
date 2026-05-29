using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PontuacaoArvore : MonoBehaviour
{
    [Header("Configurações de Tempo")]
    private float cronometro = 0f;
    private bool jogadorChegouAoTopo = false;

    [Header("Configurações de Pontos em Tempo Real")]
    [SerializeField] private int pontuacaoMaxima = 1000;
    [SerializeField] private int perdaPontosPorSegundo = 10;
    [SerializeField] private int pontuacaoMinima = 100;
    
    private int pontosAtuais;

    [Header("UI do Jogo")]
    [SerializeField] private TextMeshProUGUI textoTempo;   
    [SerializeField] private TextMeshProUGUI textoPontos;  

    [Header("UI do Placar de Bots (Fim de Fase)")]
    [SerializeField] private GameObject objetoPainelResultados; 
    [SerializeField] private TextMeshProUGUI textoListaColocados; 
    [SerializeField] private float tempoEsperaNoPainel = 6f; 

    [Header("Chances de Desempenho dos Bots (%)")]
    [Range(0, 100)] [SerializeField] private float chancePontuacaoAlta = 20f;
    [Range(0, 100)] [SerializeField] private float chancePontuacaoMedia = 60f;
    [Range(0, 100)] [SerializeField] private float chancePontuacaoBaixa = 20f;

    [Header("Próxima Fase")]
    [SerializeField] private string nomeDaProximaCena; 

    // Estrutura interna para organizar os dados de cada bot na hora de ordenar o ranking
    private class DadosCompetidor
    {
        public string nome;
        public float tempo;
        public int pontos;
    }

    void Start()
    {
        cronometro = 0f;
        jogadorChegouAoTopo = false;
        pontosAtuais = pontuacaoMaxima;
        
        PlayerPrefs.SetInt("PontosAcumulados", 0);
        PlayerPrefs.Save();

        if (objetoPainelResultados != null) objetoPainelResultados.SetActive(false);

        AtualizarInterface();
    }

    void Update()
    {
        if (!jogadorChegouAoTopo)
        {
            cronometro += Time.deltaTime;

            int pontosDescontados = Mathf.RoundToInt(cronometro * perdaPontosPorSegundo);
            pontosAtuais = pontuacaoMaxima - pontosDescontados;

            if (pontosAtuais < pontuacaoMinima) pontosAtuais = pontuacaoMinima;

            AtualizarInterface();
        }
    }

    void AtualizarInterface()
    {
        if (textoTempo != null) textoTempo.text = $"Tempo: {cronometro:F1}s";
        if (textoPontos != null) textoPontos.text = $"Pontos: {pontosAtuais}";
    }

    public void FinalizarFaseArvore()
    {
        if (jogadorChegouAoTopo) return;
        StartCoroutine(FluxoFimDeFase());
    }

    IEnumerator FluxoFimDeFase()
    {
        jogadorChegouAoTopo = true;

        // Salva os pontos do player no histórico global
        PlayerPrefs.SetInt("PontosAcumulados", pontosAtuais);
        PlayerPrefs.Save();

        // 1. Criar a lista de todos os competidores (Jogador + 9 Bots)
        List<DadosCompetidor> tabelaLideres = new List<DadosCompetidor>();

        // Adiciona o jogador real
        tabelaLideres.Add(new DadosCompetidor { nome = "<color=cyan>VOCÊ</color>", tempo = cronometro, pontos = pontosAtuais });

        // Lista de nomes para sorteio (3 Fixos + 6 Aleatórios de uma lista grande)
        List<string> nomesDisponiveis = new List<string> { "Senhor sultão P.", "NaspoBot", "Papai Ricardo" };
        string[] bancoNomesAleatorios = { "BarrilMascarado", "UvaPassa_99", "MestreDoVinho", "GamerSuco", "SuperDrunk", "ChapeuDePalha", "Fermentado", "SpeedrunGrapes", "DionisoGamer", "AdegaBoy" };
        
        // Mistura o banco de nomes e pega 6 para completar os 9 bots
        List<string> bancoEmbaralhado = new List<string>(bancoNomesAleatorios);
        for (int i = 0; i < bancoEmbaralhado.Count; i++)
        {
            string temp = bancoEmbaralhado[i];
            int randomIndex = Random.Range(i, bancoEmbaralhado.Count);
            bancoEmbaralhado[i] = bancoEmbaralhado[randomIndex];
            bancoEmbaralhado[randomIndex] = temp;
        }
        for (int i = 0; i < 6; i++) { nomesDisponiveis.Add(bancoEmbaralhado[i]); }

        // Normaliza as porcentagens do Inspector para garantir que somem 100%
        float somaChances = chancePontuacaoAlta + chancePontuacaoMedia + chancePontuacaoBaixa;
        float pesoAlto = chancePontuacaoAlta / somaChances;
        float pesoMedio = chancePontuacaoMedia / somaChances;

        // 2. Simular o desempenho de cada um dos 9 bots baseado nas probabilidades
        foreach (string nomeBot in nomesDisponiveis)
        {
            float sorteio = Random.value; // Gera um número entre 0.0 e 1.0
            float tempoBot = 0f;

            if (sorteio < pesoAlto)
            {
                // DESEMPENHO ALTO: Termina muito rápido (Sempre faz muitos pontos)
                tempoBot = Random.Range(5f, 15f);
            }
            else if (sorteio < pesoAlto + pesoMedio)
            {
                // DESEMPENHO MÉDIO: Tempo equilibrado, próximo ao do jogador
                tempoBot = cronometro + Random.Range(-5f, 10f);
            }
            else
            {
                // DESEMPENHO BAIXO: Demora bastante para subir
                tempoBot = Random.Range(35f, 60f);
            }

            // Evita tempos negativos ou bizarros se o player foi absurdamente rápido
            if (tempoBot < 4f) tempoBot = Random.Range(4f, 8f);

            // Calcula os pontos do bot com base no tempo que ele sorteou
            int pontosBot = pontuacaoMaxima - Mathf.RoundToInt(tempoBot * perdaPontosPorSegundo);
            if (pontosBot < pontuacaoMinima) pontosBot = pontuacaoMinima;

            tabelaLideres.Add(new DadosCompetidor { nome = nomeBot, tempo = tempoBot, pontos = pontosBot });
        }

        // 3. Ordenar a lista (Quem fez MAIS pontos fica em primeiro. Se empatar, quem tem MENOR tempo ganha)
        tabelaLideres.Sort((a, b) => {
            int comparacaoPontos = b.pontos.CompareTo(a.pontos);
            if (comparacaoPontos != 0) return comparacaoPontos;
            return a.tempo.CompareTo(b.tempo);
        });

        // 4. Montar o texto formatado para o Painel TextMeshPro
        string textoPlacar = "<align=center><b>RANKING DA CORRIDA</b></align>\n\n";
        for (int i = 0; i < tabelaLideres.Count; i++)
        {
            string formatoPosicao = (i + 1) == 1 ? "<color=yellow>1º</color>" : $"{(i + 1)}º";
            textoPlacar += $"{formatoPosicao} {tabelaLideres[i].nome} ->  {tabelaLideres[i].tempo:F1}s  |  {tabelaLideres[i].pontos} pts\n";
        }

        // Exibe o painel montado na tela do jogo
        if (objetoPainelResultados != null && textoListaColocados != null)
        {
            textoListaColocados.text = textoPlacar;
            objetoPainelResultados.SetActive(true);
        }

        yield return new WaitForSeconds(tempoEsperaNoPainel);

        if (!string.IsNullOrEmpty(nomeDaProximaCena))
        {
            SceneManager.LoadScene(nomeDaProximaCena);
        }
    }
}