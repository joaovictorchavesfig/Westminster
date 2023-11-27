using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float maxDecceleration;
    [SerializeField] private float maxAirAcceleration;
    [SerializeField] private float maxAirDecceleration;
    private Vector2 targetVelocity, currentVelocity;
    private float maxSpeedChange, acceleration, velDif;
    private bool walkSound;

    [Header("Gravity")]
    [SerializeField] private float downwardMovementMultiplier;
    [SerializeField] private float upwardMovementMultiplier;
    [SerializeField] private float defaultGravity;
    [SerializeField] private float apexGravity;
    [SerializeField] private float umbrellaGravity;

    [Header("Drag")]
    [SerializeField] private float airDrag;
    [SerializeField] private float floorDrag;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //Tempo que o player pode sair da plataforma e ainda assim pular
    private float coyoteCounter; //Tempo que passou desde que o player saiu da plataforma

    [Header("Jump Buffer timer")]
    [SerializeField] private float jumpBufferTime; //Tempo maximo para o pulo ainda ser contado mesmo se pressionado antes de tocar o chao
    private float jumpBufferCounter; //Contador de tempo para o pulo ainda ser contado
    [HideInInspector] public bool refreshJump, doubleJumping;
    private bool isFalling, jumpAction, desiredJump;

    [Header("Dash")]
    [SerializeField] private float dashForce; //Potencia do dash
    [SerializeField] private float dashingTime; //Duracao do dash
    [SerializeField] private float dashBufferTime;
    private bool isDashing, canDash, desiredDash;
    private float dashBufferCounter;
    private TrailRenderer trail;

    [Header("Layer")]
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private float x;
    private float y;

    [Header("Respawn")]
    private Vector2 respawnPoint;
    private bool isActive, isRespawning;

    //
    // FUNCAO START
    //

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        trail = GetComponent<TrailRenderer>();

        isActive = true;
        isDashing = false;
        walkSound = true;

        SetRespawnPoint(transform.position);
    }

    //
    // FUNCAO UPDATE
    //

    private void Update()
    {

        anim.SetBool("Active", isActive);
        anim.SetBool("Respawning", isRespawning);
        anim.SetBool("Dashing", isDashing);


        if (!isActive) //evita que qualquer coisa ocorra enquanto o player respawna
        {
            return;
        }

        /*
        //Leitura da tecla do dash
        if ((Input.GetButtonDown("Dash")) && !isDashing)
        {
            dashBufferCounter = dashBufferTime;
        }
        else if (dashBufferTime > 0)
        {
            dashBufferCounter -= Time.deltaTime;
        }
        */

        if (isDashing) //evita que o player faça algo durante o dash
        {
            AudioManager.StopStatic("Footsteps_Blocks");
            walkSound = true;
            return;
        }


        anim.SetFloat("Speed", Mathf.Abs(body.velocity.x));
        anim.SetBool("Falling", isFalling);
        anim.SetBool("jumpAction", jumpAction);


        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        //Muda direcao que o player caminha
        if (x > 0.01f)
        {
            transform.localScale = new Vector2(2, 2);
        }  
        else if (x < -0.01f)
        {
            transform.localScale = new Vector2(-2, 2);
        }

        /*
        if (dashBufferCounter > 0)
        {
            desiredDash = true;
        }
        */

        if(Input.GetButtonDown("Dash"))
        {
            StartCoroutine(Dash());
        }


        //Efeito sonoro ao caminhar
        if ((Input.GetKey("left") || Input.GetKey("right")) && isGrounded() && !onWall())
        {
            if(walkSound)
            {
                AudioManager.PlayStatic("Footsteps_Blocks");
            }
            walkSound = false;
        }
        else
        {
            AudioManager.StopStatic("Footsteps_Blocks");
            walkSound = true;
        }

        /*
            JUMP BUFFER
        */

        if (Input.GetButtonDown("Jump")) //pulo normal
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }


        /*
            REFRESH JUMP
        */
        if (refreshJump && !isGrounded())
        {
            if (Input.GetButtonDown("Jump")) //pulo duplo
            {
                desiredJump = true;
            }
        }

        /*
            NORMAL JUMP
        */
        else
        {
            if (jumpBufferCounter > 0)
            {
                desiredJump = true;
            }


                
            //Ajuste da altura do pulo
            if (Input.GetButtonUp("Jump") && body.velocity.y > 0)
            {
                body.velocity = new Vector2(body.velocity.x, body.velocity.y / 3);
            }

            //Suavizacao do movimento do pulo/Queda
            if (isFalling && Mathf.Abs(body.velocity.y) < 0.2f && !isDashing) //Apice do pulo
            {
                body.gravityScale = apexGravity;
            }
            else if (Input.GetButton("Jump") && body.velocity.y > 0 && !isDashing)
            {
                body.gravityScale = upwardMovementMultiplier;
            }
            /*
            else if (Input.GetButton("Jump") && body.velocity.y < 0)
            {
                body.gravityScale = umbrellaGravity;
            }
            */
            else if (body.velocity.y < 0 && !isDashing)
            {
                body.gravityScale = downwardMovementMultiplier;
            }
            else if (body.velocity.y >= 0 && !isDashing)
            {
                body.gravityScale = defaultGravity;
            }
        }
    }

    //
    // FUNCAO FIXED UPDATE
    //

    private void FixedUpdate()
    {
        if (!isActive) //evita que qualquer coisa ocorra enquanto o player respawna
        {
            return;
        }

        if (isDashing) //evita que o player faça algo durante o dash
        {
            return;
        }

        //Forca de arrasto do chao e do ar sao diferentes
        if (isGrounded())
        {
            body.drag = floorDrag;
            isFalling = false;
            jumpAction = false;
            canDash = true;
            refreshJump = false;
        }
        else
        {
            body.drag = airDrag;
            isFalling = true;
        }

        if (x != 0 || body.velocity.x != 0)
        {
            Move();
        }

        //Player para instantaneamente quando a velocidade fica muito baixa
        if (Mathf.Abs(body.velocity.x) < 0.1f)
        {
            body.velocity = new Vector2(0, body.velocity.y);
        }


        /*
        //Input do dash
        if (desiredDash)
        {
            desiredDash = false;
            StartCoroutine(Dash());
        }
        */


        //Input do pulo
        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }

        //Coyote time
        if (isGrounded())
        {
            coyoteCounter = coyoteTime; //Reinicia o tempo de coyote
        }
        else
        {
            coyoteCounter -= Time.fixedDeltaTime; //Decrementa o tempo de coyote
        }
    }

    //
    //  FUNCOES CRIADAS:
    //

    private void Jump()
    {
        if(refreshJump && !isDashing)
        {
            if(!isGrounded())
            {
                anim.SetTrigger("refreshJump");
                body.velocity = new Vector2(body.velocity.x, 0);
                body.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            }
            refreshJump = false;
        }
        else
        {
            if (coyoteCounter < 0 && !onWall())
            {
                return;
            }

            else
            {
                if ((isGrounded() || coyoteCounter > 0) && !isDashing)
                {

                    jumpAction = true;
                    body.velocity = new Vector2(body.velocity.x, 0);
                    body.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                    coyoteCounter = 0; //Evita spam de pulos

                }
            }
        }
    }

    //Checa hitbox do chao para definir se o player esta em contato com o chao
    public bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,boxCollider.bounds.size, 0, Vector2.down, 0.05f, groundLayer);
        return raycastHit.collider != null;
    }

    //Checa hitbox da parede para definir se o player esta em contato com uma parede
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x,0), 0.05f, groundLayer);
        return raycastHit.collider != null;
    }

    private IEnumerator Dash()
    {

        if(canDash && !isDashing)
        {
            canDash = false;
            isDashing = true;

            body.gravityScale = 0f;

            if ((SceneManager.GetActiveScene().buildIndex % 2) != 0)
            {
                FindObjectOfType<LevelLoader>().TravelTime();
                //LevelLoader.Instance.TravelTime();
            }

            body.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
            //body.AddForce(new Vector2(DashForce * Input.GetAxis("Horizontal"), 0f), ForceMode2D.Impulse);

            //trail.emitting = true;

            yield return new WaitForSeconds(dashingTime);

            //trail.emitting = false;
            //trail.Clear();
            body.gravityScale = defaultGravity;

            isDashing = false;
        }
    }
    public void Die()
    {
        AudioManager.PlayStatic("Player_Death");

        isActive = false;
        body.velocity = new Vector2(0, 0); //interrompe o movimento do player
        
        StartCoroutine(Respawn());
    }

    public void SetRespawnPoint(Vector2 position)
    {
        respawnPoint = position;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.3f); //animacao de morte

        transform.position = respawnPoint; 
        isRespawning = true;

        yield return new WaitForSeconds(0.8f); //animacao de respawn

        isActive = true;
        isRespawning = false;
    }

    private void Move()
    {
        if(!isDashing)
        {
            targetVelocity = new Vector2(x, 0f) * (maxSpeed * Time.fixedDeltaTime); //Velocidade maxima

            if (isGrounded())
            {
                acceleration = (Mathf.Abs(targetVelocity.x) > 0.01f) ? maxAcceleration : maxDecceleration;
            }

            else
            {
                acceleration = (Mathf.Abs(targetVelocity.x) > 0.01f) ? maxAirAcceleration : maxAirDecceleration;
            }

            //Conserva o impulso se tiver ja tiver acelerado acima do limite
            if (Mathf.Abs(body.velocity.x) > Mathf.Abs(targetVelocity.x) && Mathf.Sign(body.velocity.x) == Mathf.Sign(targetVelocity.x) && Mathf.Abs(targetVelocity.x) > 0.01f && !isGrounded() && !isDashing)
            {
                acceleration = 0; //Previne o player de desacelerar se estiver sendo impulsionado
            }

            //Diferenca entre a velocidade atual e a desejada
            velDif = targetVelocity.x - body.velocity.x;
            float mov = velDif * acceleration;
            body.AddForce(mov * Vector2.right, ForceMode2D.Force);
        }
    }

    public void ItemPickUp(GameObject pickeditem, float respawnTime)
    {
        pickeditem.SetActive(false);
        StartCoroutine(ItemRespawn(pickeditem, respawnTime));
    }

    public IEnumerator ItemRespawn(GameObject pickeditem, float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);
        if(pickeditem != null)
            pickeditem.SetActive(true);

    }
}