using UnityEngine;

public class QuickClick : MonoBehaviour
{
    public Collider2D objetoDentroDoGatilho;
    public bool control;
    public PontuacaoFaseUva pontos;
    public QuickClick quick1, quick2, quick3;
    public float timer;
    private float timerControl;

    void Start()
    {
        timerControl = timer;
        timer = 0;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && (quick1.objetoDentroDoGatilho == null && quick2.objetoDentroDoGatilho == null && quick3.objetoDentroDoGatilho == null) && timer <= 0)
        {
            pontos.RedPontos(1);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && objetoDentroDoGatilho != null)
        {
            Destroy(objetoDentroDoGatilho.gameObject);
            pontos.AddPontos(1);
        }

        if (timer <= 0 && Input.GetKeyDown(KeyCode.Space)) {
            timer = timerControl;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        objetoDentroDoGatilho = other;
    }
}
