using System;
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
    private float mov;

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
    private bool desiredJump;
    [HideInInspector] public bool isFalling;

    [Header("Dash")]
    [SerializeField] private float dashForce; //Potencia do dash
    [SerializeField] private float dashingTime; //Duracao do dash
    [SerializeField] private float dashBufferTime;
    [HideInInspector] public bool isDashing;
    private bool canDash, desiredDash;
    private float dashBufferCounter;
    private TrailRenderer trail;

    [Header("Layer")]
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private Animator anim;
    [HideInInspector] public float x;
    private float y;
    [HideInInspector] public float yvel;
    [HideInInspector] public bool canTravelTime;
    [HideInInspector] public static bool firstTimeTravel = false;
    [HideInInspector] public FallingPlatform fallingPlatform;
    [HideInInspector] public CandelabroFall candelabroFall;
    [HideInInspector] public Platform_key platform_key;
    [HideInInspector] public bool isOnMola = false;


    [Header("Platform")]
    [HideInInspector] public bool isOnPlatform;
    [HideInInspector] public float platformSpeed;
    public ParticleSystem dust;


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
        canTravelTime = true;

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

        if (PauseMenu.gamePaused)
        {
            AudioManager.StopStatic("Footsteps_Blocks");
            return;
        }

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


        if (isDashing) //evita que o player faça algo durante o dash
        {
            AudioManager.StopStatic("Footsteps_Blocks");
            walkSound = true;
            return;
        }

        yvel = body.velocity.y;
        anim.SetFloat("Speed", Mathf.Abs(body.velocity.x));
        anim.SetBool("Falling", isFalling);


        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (Mathf.Abs(x) > 0)
            anim.SetBool("Inputing", true);
        else
            anim.SetBool("Inputing", false);

        //Muda direcao que o player caminha
        if (x > 0.01f)
        {
            if (transform.localScale.x < 0)
            {
                if (isGrounded())
                    dust.Play();
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
        }
        else if (x < -0.01f)
        {
            if (transform.localScale.x > 0)
            {
                if (isGrounded())
                    dust.Play();
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y);
            }
        }

        /*
        if (dashBufferCounter > 0)
        {
            desiredDash = true;
        }
        */

        if (Input.GetButtonDown("Dash"))
        {
            StartCoroutine(Dash());
        }


        //Efeito sonoro ao caminhar
        if ((Input.GetKey("left") || Input.GetKey("right") || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        && isGrounded() && !onWall() && !PauseMenu.gamePaused)
        {
            if (walkSound)
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

            var isJumping = anim.GetBool("Jumping");

            //Ajuste da altura do pulo
            if (Input.GetButtonUp("Jump") && isJumping && !isOnMola && body.velocity.y > 0)
            {
                body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2.5f);
            }

            //Suavizacao do movimento do pulo/Queda
            if (isFalling && Mathf.Abs(body.velocity.y) < 0.2f && !isDashing && !isOnPlatform) //Apice do pulo
            {
                body.gravityScale = apexGravity;
            }
            else if (Input.GetButton("Jump") && body.velocity.y > 0 && !isDashing && !isOnPlatform)
            {
                body.gravityScale = upwardMovementMultiplier;
            }
            /*
            else if (Input.GetButton("Jump") && body.velocity.y < 0)
            {
                body.gravityScale = umbrellaGravity;
            }
            */
            else if (body.velocity.y < 0 && !isDashing && !isOnPlatform)
            {
                body.gravityScale = downwardMovementMultiplier;
            }
            else if (body.velocity.y >= 0 && !isDashing && !isOnPlatform)
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

        if (isOnMola)
        {
            anim.SetBool("Jumping", true);
            anim.SetTrigger("jumpAction");
            canDash = true;
        }

        //Forca de arrasto do chao e do ar sao diferentes
        if (isGrounded())
        {
            isOnMola = false;
            body.drag = floorDrag;
            isFalling = false;
            canDash = true;
            refreshJump = false;
            anim.SetBool("Jumping", false);
            anim.ResetTrigger("jumpAction");
            anim.ResetTrigger("refreshJump");
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
        if (x == 0 && Mathf.Abs(body.velocity.x) < 2f)
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
        if (refreshJump && !isDashing)
        {
            if (!isGrounded())
            {
                anim.SetTrigger("refreshJump");
                anim.SetBool("Jumping", true);
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
                    dust.Play();
                    anim.SetBool("Jumping", true);
                    anim.SetTrigger("jumpAction");
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }


    //Checa hitbox da parede para definir se o player esta em contato com uma parede
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.05f, groundLayer);
        return raycastHit.collider != null;
    }

    private IEnumerator Dash()
    {
        if (canDash && !isDashing)
        {
            canDash = false;
            isDashing = true;

            body.gravityScale = 0f;

            if ((SceneManager.GetActiveScene().buildIndex % 2) != 0)
            {
                StartCoroutine(TimeTravelCooldown());
                FindObjectOfType<LevelLoader>().TravelTime();
            }

            if (isOnPlatform)
            {
                transform.parent = null;
                body.velocity = new Vector2((transform.localScale.x * dashForce) + platformSpeed, 0f);
            }
            else
            {
                body.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
            }

            //trail.emitting = true;

            yield return new WaitForSeconds(dashingTime);

            //trail.emitting = false;
            //trail.Clear();
            body.gravityScale = defaultGravity;

            coyoteCounter = 0;
            desiredJump = false;
            isDashing = false;
            anim.SetBool("Jumping", false);
        }
    }
    public void Die()
    {
        AudioManager.PlayStatic("Player_Death");

        PauseMenu.deathCounter++;

        isActive = false;
        body.velocity = new Vector2(0, 0); //interrompe o movimento do player

        var keyPlatforms = FindObjectsOfType<Platform_key>();
        var fallingPlatforms = FindObjectsOfType<FallingPlatform>();
        var candelabros = FindObjectsOfType<CandelabroFall>();

        foreach (FallingPlatform fallingPlatform in fallingPlatforms)
        {
            fallingPlatform.resetPos();
        }
        foreach (CandelabroFall candelabro in candelabros)
        {
            candelabro.resetPos();
        }
        foreach (Platform_key keyPlatform in keyPlatforms)
        {
            keyPlatform.resetPlatform();
        }

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
        if (!isDashing)
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

            if (isOnPlatform)
            {
                mov = (velDif * acceleration) + platformSpeed;
            }
            else
            {
                mov = velDif * acceleration;
            }

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
        if (pickeditem != null)
            pickeditem.SetActive(true);

    }

    private IEnumerator TimeTravelCooldown()
    {
        canTravelTime = false;
        yield return new WaitForSeconds(2);
        canTravelTime = true;
    }

    public void firstPortal()
    {
        firstTimeTravel = true;
    }

    public bool hasTraveledTime()
    {
        return (firstTimeTravel);
    }

    public void callDash()
    {
        canDash = true;
        isDashing = false;
        StartCoroutine(Dash());
    }

}