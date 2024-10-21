using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float initialScale; //Escala inicial del sprite
    private static float speed = 8; //Velocidad del personaje
    [SerializeField] private float initialSpeed; //Velocidad inicial ajustable desde el inspector (variables est�ticas no aparecen)
    [SerializeField] private float scaleFactor; //Velocidad a la que el personaje cambia de tama�o al alejarse o acercarse a la c�mara
    private bool onContactWithLateralWall = false; //Determina si est� en contacto con alguna pared lateral para anular el deslizamiento (no son verticales)
    private bool onContactWithVerticalWall = false; //Determina si est� en contacto con alguna pared lateral para anular el deslizamiento (no son verticales)
    private Rigidbody2D rb;
    private float direction; //Signo actual de la direcci�n en x (ayuda a anular el movimiento al entrar en contacto con las paredes)
    public bool stopMovement = false; //Determina si el movimiento del personaje debe ser bloqueado (di�logos, cuadros emergentes...)
    public bool noAnimation = true; //Indica que no hay animaciones en curso del personaje desde otros scripts
    private Animator anim;
    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Awake()
    {
        if (IterationController.numIteration == 0) speed = initialSpeed;
        rb = GetComponent<Rigidbody2D>();
        initialScale = this.transform.localScale.x;
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ScalePlayer();
        if (stopMovement && noAnimation) anim.SetInteger("Direction", 0);
    }

    private void FixedUpdate()
    {
        if(!stopMovement) PlayerMovement();
    }

    private void PlayerMovement()
    {
        float xMovement = 0;
        float yMovement = 0;
        
        if (Input.GetKey(KeyCode.W))
        {
            yMovement += speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            yMovement -= speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            xMovement -= speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            xMovement += speed;
        }

        if(xMovement != 0 && yMovement != 0) //Hay que mantener la velocidad constante en desplazamientos diagonales
        {
            xMovement *= Mathf.Sin(Mathf.PI / 4);
            yMovement *= Mathf.Cos(Mathf.PI / 4);
        }

        float aux = Mathf.Sign(xMovement);
        if (aux != direction) onContactWithLateralWall = false;

        if (onContactWithLateralWall && !onContactWithVerticalWall)
        {
            rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
        }
        else
        {
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }

        if (yMovement != 0 && onContactWithLateralWall) yMovement /= 2; //Se hace m�s lento el movimiento si te est�s chocando con la pared

        this.transform.position += new Vector3(xMovement, yMovement, 0.0f) * Time.deltaTime;
        if(xMovement != 0) direction = Mathf.Sign(xMovement);

        AnimateCharacter(xMovement, yMovement);
    }

    private void ScalePlayer()
    {
        float distance = this.transform.position.y;
        float scale = initialScale - distance * scaleFactor;
        this.transform.localScale = new Vector3(scale, scale, scale);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Lateral Wall"))
        {
            onContactWithLateralWall = true;
        }
        else if(collision.collider.CompareTag("Vertical Wall"))
        {
            onContactWithVerticalWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Lateral Wall"))
        {
            onContactWithLateralWall = false;
        }
        else if (collision.collider.CompareTag("Vertical Wall"))
        {
            onContactWithVerticalWall= false;
        }
    }

    private void AnimateCharacter(float xMovement, float yMovement)
    {
        if (!stopMovement) //Otros scripts pueden alterar las animaciones del personaje mientras el jugador no puede moverse
        {
            //Hay que animar al personaje. Para ello, se cambia el int que maneja la direcci�n del jugador. 
            //0 - quieto
            //1 - hacia abajo
            //2 - hacia la derecha
            //3 - hacia arriba
            //4 - hacia la izquierda
            if (yMovement == 0 && xMovement == 0) anim.SetInteger("Direction", 0);
            else if (yMovement < 0 && xMovement == 0) anim.SetInteger("Direction", 1);
            else if (xMovement > 0) 
            { 
                anim.SetInteger("Direction", 2);
                renderer.flipX = false;
            }
            else if (xMovement == 0 && yMovement > 0) anim.SetInteger("Direction", 3);
            else if (xMovement < 0)
            {
                anim.SetInteger("Direction", 2);
                renderer.flipX = true;
            }
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void DecreaseSpeed(float variation)
    {
        speed -= variation;
    }
}
