using UnityEngine;

public class NotesController : MonoBehaviour
{
    public Transform note;
    public float speed = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        note = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime);
    }
}
