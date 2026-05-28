using System.Collections;
using UnityEngine;

public class SpawnUvaController : MonoBehaviour
{
    [Header("Pontos de Spawn")]
    public Transform spawn1;
    public Transform spawn2;
    public Transform spawn3;

    [Header("Configurações do Prefab")]
    public GameObject prefab;

    [Header("Configurações de Tempo")]
    [SerializeField] private float tempoTotalDoJogo = 30f;
    [SerializeField] private float intervaloBaseSpawn = 1.5f;

    private int ultimoIndiceSpawn = -1;
    private float tempoDecorrido = 0f;  
    
    // Propriedade para controlar se o minigame acabou
    private bool minigameTerminou = false;
    public bool MinigameTerminou => minigameTerminou;

    // Removeu o Start() automático para ele não começar sozinho!
    public void IniciarMinigame()
    {
        tempoDecorrido = 0f;
        minigameTerminou = false;
        StartCoroutine(RotinaGeralSpawn());
    }

    IEnumerator RotinaGeralSpawn()
    {
        while (tempoDecorrido < tempoTotalDoJogo)
        {
            SpawnarUvaAleatoria();

            float tempoMinimo = intervaloBaseSpawn * 0.5f;
            float tempoMaximo = intervaloBaseSpawn * 1.5f;
            float tempoEsperaAleatorio = Random.Range(tempoMinimo, tempoMaximo);

            yield return new WaitForSeconds(tempoEsperaAleatorio);
            tempoDecorrido += tempoEsperaAleatorio;
        }

        Debug.Log("O tempo acabou!");
        minigameTerminou = true; // Avisa que o minigame chegou ao fim
    }

    void SpawnarUvaAleatoria()
    {
        int indiceSorteado;
        do
        {
            indiceSorteado = Random.Range(1, 4);
        } 
        while (indiceSorteado == ultimoIndiceSpawn);

        ultimoIndiceSpawn = indiceSorteado;

        Transform pontoEscolhido = null;
        if (indiceSorteado == 1) pontoEscolhido = spawn1;
        else if (indiceSorteado == 2) pontoEscolhido = spawn2;
        else if (indiceSorteado == 3) pontoEscolhido = spawn3;

        if (pontoEscolhido != null)
        {
            GameObject novaNota = Instantiate(prefab, pontoEscolhido.position, Quaternion.identity);
            novaNota.tag = "NotaUva"; // Garante a tag configurada via código
        }
    }
}