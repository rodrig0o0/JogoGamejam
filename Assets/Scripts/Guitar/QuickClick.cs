using UnityEngine;

public class QuickClick : MonoBehaviour
{
    public Collider2D objetoDentroDoGatilho;
    public bool control;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && objetoDentroDoGatilho != null)
        {
            Destroy(objetoDentroDoGatilho.gameObject);
            Debug.Log("Tiro");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entrou");
        objetoDentroDoGatilho = other;
    }
}
