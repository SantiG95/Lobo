using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColeccionableController : MonoBehaviour
{
    BoxCollider2D boxCollider;
    public int valor = 1;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        GameObject.Find("GameManager").GetComponent<GameManager>().cambiarPuntaje(valor);
    }
}
