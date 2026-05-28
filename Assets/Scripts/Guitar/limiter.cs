using NUnit.Framework.Constraints;
using UnityEngine;

public class limiter : MonoBehaviour
{
    public Collider2D objetoDentroDoGatilho;
    void Update()
    {
        if (objetoDentroDoGatilho != null) {
            Destroy(objetoDentroDoGatilho.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        objetoDentroDoGatilho = other;
    }
}