using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoboController : MonoBehaviour
{
    [Header("Movimiento horizontal")]
    public float velocidadMovimiento;
    public Vector2 direccion;
    public bool mirandoDerecha = true;
    public bool agachado = false;

    [SerializeField] float velocidadXActual;
    [SerializeField] float velocidadYActual;

    [Header("Movimiento salto")]
    public int velocidadSalto;
    public float delaySalto;
    private float tiempoSalto;

    public float ultimaVelocidadSalto;

    [Header("Fisicas")]
    public float maxVelocidad;
    public float traccionLineal;
    public float gravedad = 1f;
    public float multiplicadorCaida = 5f;


    [Header("Parpadeo")]
    public float tiempoParpadeo = 0;
    public float horaParpadear;

    [Header("Componentes")]
    private Animator animador;
    private Rigidbody2D rb;
    public LayerMask layerPlataformas;

    [Header("Colision")]
    public bool enTierra = false;
    public float longitudRaycast;
    public Vector3 desvioCollider;

    // Start is called before the first frame update
    void Start()
    {
        animador = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        horaParpadear = Random.Range(3, 7);
    }

    // Update is called once per frame
    private void Update()
    {
        //Control de Parpadeos
        tiempoParpadeo += Time.deltaTime;
        if (tiempoParpadeo > horaParpadear)
        {
            tiempoParpadeo = 0;
            horaParpadear = Random.Range(3, 7);

            animador.SetTrigger("Parpadear");
        }

        bool estuvoEnTierra = enTierra;
        enTierra = Physics2D.Raycast(transform.position + desvioCollider, Vector2.down, longitudRaycast, layerPlataformas) || Physics2D.Raycast(transform.position - desvioCollider, Vector2.down, longitudRaycast, layerPlataformas);

        //Axis
        direccion = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (!estuvoEnTierra && enTierra)
        {
            animador.SetBool("HaSaltado", false);
        }

        if (!estuvoEnTierra && enTierra && direccion.x != 0)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * ultimaVelocidadSalto, 0);
        }

        if (!enTierra)
        {
            ultimaVelocidadSalto = Mathf.Abs(rb.velocity.x);
        }



        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            tiempoSalto = Time.time + delaySalto;
        }

        if (enTierra)
        {
            agachado = Input.GetKey(KeyCode.DownArrow) ? true : false;
            animador.SetBool("Agachado", agachado);
        }
        

        velocidadXActual = Mathf.Abs(rb.velocity.x);

        velocidadYActual = rb.velocity.y;
    }

    void FixedUpdate()
    {
        //Movimiento
        if (!agachado)
        {
            moverPersonaje(direccion.x);
        }

        if (agachado && enTierra)
        {
            rb.velocity = new Vector2(0, 0);
        }
        
        if(tiempoSalto > Time.time && enTierra)
        {
            agachado = false;
            animador.SetBool("Agachado", agachado);
            saltar();
        }

        cambiarFisicas();

    }

    void moverPersonaje(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * velocidadMovimiento);

        animador.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));
        animador.SetFloat("Vertical", rb.velocity.y);
        animador.SetBool("EnElAire", !enTierra);

        if((horizontal > 0 && !mirandoDerecha) || (horizontal < 0 && mirandoDerecha))
        {
            girar();
        }

        animador.SetFloat("Direccion", mirandoDerecha ? 1 : -1);

        if(Mathf.Abs(rb.velocity.x) > maxVelocidad)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxVelocidad, rb.velocity.y);
        }
    }

    void girar()
    {
        mirandoDerecha = !mirandoDerecha;
    }


    private void saltar()
    {
        //rigidbody.velocity = miraDireccion * velocidadXActual + Vector2.up * fuerzaSalto;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * velocidadSalto, ForceMode2D.Impulse);
        tiempoSalto = 0;

        animador.SetBool("HaSaltado", true);
    }

    void cambiarFisicas()
    {
        bool cambiandoDireccion = (direccion.x > 0 && rb.velocity.x < 0) || (direccion.x < 0 && rb.velocity.x > 0);

        if (enTierra)
        {
            if (Mathf.Abs(direccion.x) < 0.4f || cambiandoDireccion)
            {
                rb.drag = traccionLineal;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravedad;
            rb.drag = traccionLineal * 0.15f;
            if(rb.velocity.y < 0)
            {
                rb.gravityScale = gravedad * multiplicadorCaida;
            }
            else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                rb.gravityScale = gravedad * (multiplicadorCaida / 2);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + desvioCollider, transform.position + desvioCollider + Vector3.down * longitudRaycast);
        Gizmos.DrawLine(transform.position - desvioCollider, transform.position - desvioCollider + Vector3.down * longitudRaycast);
    }
}
