using System.Collections;
using UnityEngine;
using TMPro; // ADICIONADO: Necessário para controlar o texto do Canvas

public class MovimentoInfinito : MonoBehaviour
{
    [Header("Movimento Vertical e Aceleração")]
    [SerializeField] private float velocidadeSubidaPadrao = 5f; 
    [Tooltip("Quanto de velocidade o player ganha por segundo.")]
    [SerializeField] private float taxaAceleracaoPorSegundo = 0.2f;
    [Tooltip("Velocidade máxima que o player pode atingir.")]
    [SerializeField] private float velocidadeMaxima = 25f;
    
    private float velocidadeAtual;

    [Header("Mecânica de Lados (Espaço)")]
    [SerializeField] private float posicaoXEsquerda = -1f;
    [SerializeField] private float posicaoXDireita = 1f;
    [SerializeField] private bool noLadoEsquerdo = false;

    [Header("Mecânica de Dash")]
    [SerializeField] private float forcaDash = 20f;
    [SerializeField] private float duracaoDash = 0.2f;
    [SerializeField] private float cooldownDash = 5f;
    
    private float tempoProximoDash = 0f;
    private bool estaTomandoDash = false;
    private Rigidbody2D rb;

    [Header("UI do Dash (Canvas)")]
    [SerializeField] private TextMeshProUGUI textoCooldownDash; // Arraste o texto do Dash aqui

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; 
        
        velocidadeAtual = velocidadeSubidaPadrao;
        DefirnirPosicaoHorizontal();
    }

    void Update()
    {
        // 1. Alternar de lado (Espaço)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            noLadoEsquerdo = !noLadoEsquerdo;
            DefirnirPosicaoHorizontal();
        }

        if (estaTomandoDash) return;

        // Aceleração Constante
        if (velocidadeAtual < velocidadeMaxima)
        {
            velocidadeAtual += taxaAceleracaoPorSegundo * Time.deltaTime;
        }

        // 2. Ativar o Dash (Botão Esquerdo do Mouse)
        if (Input.GetMouseButtonDown(0) && Time.time >= tempoProximoDash)
        {
            StartCoroutine(ExecutarDash());
        }

        // ADICIONADO: Atualiza o texto do Cooldown na tela
        AtualizarTextoCooldown();
    }

    void FixedUpdate()
    {
        if (estaTomandoDash) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocidadeAtual);
    }

    void DefirnirPosicaoHorizontal()
    {
        float novoX = noLadoEsquerdo ? posicaoXEsquerda : posicaoXDireita;
        transform.position = new Vector3(novoX, transform.position.y, transform.position.z);
    }

    IEnumerator ExecutarDash()
    {
        estaTomandoDash = true;
        tempoProximoDash = Time.time + cooldownDash;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocidadeAtual + forcaDash);
        yield return new WaitForSeconds(duracaoDash);

        estaTomandoDash = false;
    }

    // ADICIONADO: Método para calcular o tempo do Dash e exibir de forma bonita
    void AtualizarTextoCooldown()
    {
        if (textoCooldownDash == null) return;

        // Calcula quanto tempo falta para o próximo dash
        float tempoRestante = tempoProximoDash - Time.time;

        if (tempoRestante > 0)
        {
            // Se ainda estiver no cooldown, mostra o tempo com 1 casa decimal (Ex: "Dash: 3.4s")
            textoCooldownDash.text = $"Dash: {tempoRestante:F1}s";
            textoCooldownDash.color = Color.yellow; // Opcional: fica amarelo no cooldown
        }
        else
        {
            // Se já puder usar, avisa o jogador
            textoCooldownDash.text = "Dash: PRONTO!";
            textoCooldownDash.color = Color.green; // Opcional: fica verde quando pronto
        }
    }

    public void AplicarPenalidadeGalho(float velocidadeImpacto, float duracaoNaoUtilizada, float forcaRecuo)
    {
        estaTomandoDash = false; 
        rb.position = new Vector2(rb.position.x, rb.position.y - forcaRecuo);
        velocidadeAtual = velocidadeSubidaPadrao; 
    }
}