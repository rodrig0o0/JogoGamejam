using System.Collections;
using UnityEngine;

public class JogadorFase2 : MonoBehaviour
{
    [Header("Movimentação lateral")]
    [SerializeField] private float velocidade = 5f;
    private Rigidbody2D rb;
    private float movimentoHorizontal;
    private float ultimaDirecaoX = 1f; // Guarda o último lado para onde o jogador andou

    [Header("Mecânica de Dash")]
    [SerializeField] private float forcaDash = 20f;      // Velocidade do dash
    [SerializeField] private float tempoDash = 0.2f;      // Quanto tempo o dash dura
    [SerializeField] private float cooldownDash = 1f;    // Tempo para poder usar de novo
    private bool podeFazerDash = true;
    private bool estaEstouraDash = false; // Bloqueia comandos normais durante o dash

    [Header("Estado do Inventário")]
    public bool segurandoUva = false;
    public bool segurandoCopo = false;
    
    [Header("Progresso da Fase")]
    public int uvasNaBacia = 0;
    public int coposComVinho = 0;
    public bool vinhoPronto = false;

    [Header("Referências")]
    [SerializeField] private GameObject spriteVisualUva; // Arraste o quadrado da uva aqui no Inspector
    [SerializeField] private GameObject spriteVisualCopo; // Arraste o quadrado do copo aqui no Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        AtualizarVisualItens();
    }

    void Update()
    {
        // Se estiver no meio de um dash, ignora os inputs normais de movimento
        if (estaEstouraDash) return;

        // Movimento lateral simples
        movimentoHorizontal = Input.GetAxisRaw("Horizontal");

        // Salva a última direção movida para o dash não falhar se o jogador estiver parado
        if (movimentoHorizontal != 0)
        {
            ultimaDirecaoX = movimentoHorizontal;
        }

        // Verifica se apertou o Botão Esquerdo do Mouse (Mouse0) e se não está no cooldown
        if (Input.GetKeyDown(KeyCode.Mouse0) && podeFazerDash)
        {
            StartCoroutine(ExecutarDash());
        }

        // Garante que os sprites que você está segurando sigam as variáveis
        AtualizarVisualItens();
    }

    void FixedUpdate()
    {
        // Se estiver no meio de um dash, a física é controlada pela Corrotina, não por aqui
        if (estaEstouraDash) return;

        rb.linearVelocity = new Vector2(movimentoHorizontal * velocidade, rb.linearVelocity.y);
    }

    // Corrotina que controla o tempo e a física do Dash
    private IEnumerator ExecutarDash()
    {
        podeFazerDash = false;
        estaEstouraDash = true;

        // Salva a gravidade atual para o boneco não cair enquanto dá o dash no ar
        float gravidadeOriginal = rb.gravityScale;
        rb.gravityScale = 0f;

        // Se o jogador estiver parado, usa a última direção salva. Se estiver andando, usa o input atual.
        float direcaoDash = movimentoHorizontal != 0 ? movimentoHorizontal : ultimaDirecaoX;

        // Aplica a velocidade do dash
        rb.linearVelocity = new Vector2(direcaoDash * forcaDash, 0f);

        // Espera o tempo de duração do dash terminar
        yield return new WaitForSeconds(tempoDash);

        // Restaura o estado normal do jogador
        rb.gravityScale = gravidadeOriginal;
        estaEstouraDash = false;

        // Espera o tempo de recarga (cooldown) para permitir outro dash
        yield return new WaitForSeconds(cooldownDash);
        podeFazerDash = true;
    }

    public void AtualizarVisualItens()
    {
        if(spriteVisualUva != null) spriteVisualUva.SetActive(segurandoUva);
        if(spriteVisualCopo != null) spriteVisualCopo.SetActive(segurandoCopo);
    }
}