using UnityEngine;

public class JogadorFase2 : MonoBehaviour
{
    [Header("Movimentação lateral")]
    [SerializeField] private float velocidade = 5f;
    private Rigidbody2D rb;
    private float movimentoHorizontal;

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
        // Movimento lateral simples
        movimentoHorizontal = Input.GetAxisRaw("Horizontal");

        // Garante que os sprites que você está segurando sigam as variáveis
        AtualizarVisualItens();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimentoHorizontal * velocidade, rb.linearVelocity.y);
    }

    public void AtualizarVisualItens()
    {
        if(spriteVisualUva != null) spriteVisualUva.SetActive(segurandoUva);
        if(spriteVisualCopo != null) spriteVisualCopo.SetActive(segurandoCopo);
    }
}
