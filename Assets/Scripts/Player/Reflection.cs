using UnityEngine;

public class Reflection : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private float initOffset; //Controla la distancia a la que el reflejo aparecer� en el espejo cuando el jugador se acerque
    private float offset;
    [SerializeField] private float sizeDif; //Hace que el tama�o del reflejo sea menor que el original
    private Animator playerAnim;
    public Object[] controllers;
    private Animator anim;
    private float initPosY; //Posición inicial del jugador en el eje y

    private void Awake()
    {
        playerAnim = player.GetComponent<Animator>();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = (RuntimeAnimatorController)controllers[IterationController.numIteration];
        initPosY = player.transform.position.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        offset = initPosY + initOffset;
        this.transform.position = new Vector3(player.transform.position.x, offset, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(player.transform.localScale.x - sizeDif, player.transform.localScale.y - sizeDif, player.transform.localScale.z - sizeDif);

        float playerOffset = player.transform.position.y - initPosY; //Offset entre la posición del jugador actual y su posición inicial
        float newPosY = offset - playerOffset;
        this.transform.position = new Vector3(player.transform.position.x, newPosY, 0);

        int direction = playerAnim.GetInteger("Direction");
        AnimateMirror(direction);
    }

    private void AnimateMirror(int direction)
    {
        //El reflejo sigue las acciones del personaje. 
        //Sin embargo, cuando el jugador se mueve verticalmente, la animaci�n del espejo ha de ser la contraria
        //(si el jugador mira hacia arriba, debe mirar hacia abajo, por ejemplo)
        if (direction == 1) direction = 3;
        else if (direction == 3) direction = 1;
        anim.SetInteger("Direction", direction);
        if (playerSpriteRenderer.flipX) GetComponent<SpriteRenderer>().flipX = true;
        else GetComponent<SpriteRenderer>().flipX = false;
    }
}
