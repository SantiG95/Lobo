using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Datos")]
    public int puntaje;
    // Start is called before the first frame update
    void Start()
    {
        puntaje = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cambiarPuntaje(int puntos)
    {
        puntaje += puntos;
    }
}
