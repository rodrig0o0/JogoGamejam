using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovimentacaoTopDown : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 8f;
    public float velocidadeRotacao = 10f;

    private Rigidbody rb;
    private Vector3 direcaoMovimento;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Trava a gravidade (já que é visão de cima) e impede o boneco de capotar
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Pega os inputs (WASD / Setas / Analógico)
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        // No 3D, "cima e baixo" na tela corresponde ao eixo Z (frente/trás)
        direcaoMovimento = new Vector3(inputX, 0f, inputZ).normalized;
    }

    void FixedUpdate()
    {
        // Aplica a velocidade no Rigidbody (X e Z)
        rb.linearVelocity = new Vector3(direcaoMovimento.x * velocidade, 0f, direcaoMovimento.z * velocidade);

        // Rotaciona o personagem suavemente para a direção que está andando
        if (direcaoMovimento != Vector3.zero)
        {
            Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoMovimento);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, velocidadeRotacao * Time.fixedDeltaTime);
        }
    }
}

