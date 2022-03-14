using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public GameObject seguimiento;
    public float offset;

    public GameObject fondo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(seguimiento.gameObject.transform.position.x, offset);

        fondo.transform.Translate(new Vector3(seguimiento.GetComponent<Rigidbody2D>().velocity.x / -700, 0, 0));
    }
}
