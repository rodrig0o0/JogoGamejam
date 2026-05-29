using System.Collections;
using UnityEngine;

public class SpawnerUvas : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    [SerializeField] private GameObject prefabNotaUva; // O Prefab da uva que tem o script "NotesController"
    [SerializeField] private Transform pontoDeSpawn; // De onde a uva vai sair (geralmente embaixo da tela)
    [SerializeField] private float intervaloSpawn = 1.5f; // Tempo entre uma bolinha e outra
    [SerializeField] private int quantidadeTotal = 10; // Altere aqui a quantidade de bolinhas por minigame

    private int uvasCriadas = 0;
    private bool terminouDeSpawnar = false;

    // Propriedade pública para o script da bacia saber se os spawns acabaram
    public bool TerminouDeSpawnar => terminouDeSpawnar;

    public void AtivarSpawner()
    {
        uvasCriadas = 0;
        terminouDeSpawnar = false;
        StartCoroutine(RotinaSpawn());
    }

    IEnumerator RotinaSpawn()
    {
        while (uvasCriadas < quantidadeTotal)
        {
            // Cria a nota na posição do ponto de spawn
            GameObject novaNota = Instantiate(prefabNotaUva, pontoDeSpawn.position, pontoDeSpawn.rotation);
            
            // GARANTA QUE O SEU PREFAB TENHA A tag "NotaUva" configurada na unity
            novaNota.tag = "NotaUva"; 

            uvasCriadas++;
            yield return new WaitForSeconds(intervaloSpawn);
        }

        terminouDeSpawnar = true;
    }
}

