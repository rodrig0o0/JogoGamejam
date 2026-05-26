using System.Collections;
using UnityEngine;

public class MovimentoInfinito : MonoBehaviour
{
    [Header("Movimento Vertical")]
    [SerializeField] private float velocidadeSubidaPadrao = 5f; 
    private float velocidadeAtual;

    [Header("Mecânica de Lados (Backspace)")]
    [SerializeField] private float posicaoXEsquerda = -1f;
    [SerializeField] private float posicaoXDireita = 1f;
    private bool noLadoEsquerdo = true;

    [Header("Mecânica de Dash")]
    [SerializeField] private float forcaDash = 20f;
    [SerializeField] private float duracaoDash = 0.2f;
    [SerializeField] private float cooldownDash = 5f;
    
    private float tempoProximoDash = 0f;
    private bool estaTomandoDash = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Desativa a gravidade
        
        velocidadeAtual = velocidadeSubidaPadrao;
        DefinirPosicaoHorizontal();
    }

    void Update()
    {
        // 1. Alternar de lado (Backspace)
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            noLadoEsquerdo = !noLadoEsquerdo;
            DefinirPosicaoHorizontal();
        }

        if (estaTomandoDash) return;

        // 2. Ativar o Dash (Botão Esquerdo do Mouse)
        if (Input.GetMouseButtonDown(0) && Time.time >= tempoProximoDash)
        {
            StartCoroutine(ExecutarDash());
        }
    }

    void FixedUpdate()
    {
        if (estaTomandoDash) return;

        // Aplica a velocidade atualizada (seja a normal ou a de lentidão)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocidadeAtual);
    }

    void DefinirPosicaoHorizontal()
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

    // FUNÇÃO PÚBLICA: O Galho vai chamar essa função aqui embaixo!
    public void AplicarPenalidadeGalho(float novaVelocidade, float duracao, float forcaRecuo)
    {
        // Para o Dash imediatamente se o jogador bater num galho
        estaTomandoDash = false; 

        // Aplica o recuo para trás usando a física do Rigidbody (evita bugs de colisão)
        rb.position = new Vector2(rb.position.x, rb.position.y - forcaRecuo);

        // Inicia a rotina de lentidão interna do jogador
        StartCoroutine(RotinaLentidao(novaVelocidade, duracao));
    }

    IEnumerator RotinaLentidao(float novaVelocidade, float duracao)
    {
        velocidadeAtual = novaVelocidade; // Fica lento
        yield return new WaitForSeconds(duracao); // Espera o tempo
        velocidadeAtual = velocidadeSubidaPadrao; // Volta ao normal
    }
}
