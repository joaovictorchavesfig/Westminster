using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

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

    [Header("Gravity")]
    [SerializeField] private float downwardMovementMultiplier;
    [SerializeField] private float upwardMovementMultiplier;
    [SerializeField] private float defaultGravity;
    [SerializeField] private float apexGravity;
    private float wallSlideGravity = 0.1f;
    [SerializeField] private float umbrellaGravity;

    [Header("Drag")]
    [SerializeField] private float airDrag;
    [SerializeField] private float floorDrag;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpx; //Forca do wall jump horizontal
    [SerializeField] private float wallJumpy; //Forca do wall jump vertical

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //Tempo que o player pode sair da plataforma e ainda assim pular
    private float coyoteCounter; //Tempo que passou desde que o player saiu da plataforma

    [Header("Jump Buffer timer")]
    [SerializeField] private float jumpBufferTime; //Tempo maximo para o pulo ainda ser contado mesmo se precionado antes de tocar o chao
    private float jumpBufferCounter; //Contador de tempo para o pulo ainda ser contado
    private bool desiredJump, isFalling, jumpAction;

    [Header("Dash")]
    [SerializeField] private float dashForce; //Potencia do dash
    [SerializeField] private float dashingTime; //Duracao do dash
    private bool isDashing,canDash;
    private TrailRenderer trail;

    [Header("Layer")]
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private float wallJumpCooldown;
    private float x;
    private float y;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        trail = GetComponent<TrailRenderer>();

    }

    private void Update()
    {
        if(isDashing)
        {
            return;
        }

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        anim.SetFloat("Speed", Mathf.Abs(body.velocity.x));
        anim.SetBool("Falling", isFalling);
        anim.SetBool("jumpAction", jumpAction);

        //Muda direcao que o player caminha
        if (x > 0.01f)
            transform.localScale = Vector3.one;
        else if (x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //Leitura da tecla do dash
        if ((Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftShift)) && !isDashing && canDash)
        {
            StartCoroutine(Dash());
        }

        //Leitura da tecla de pulo
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else if (jumpBufferCounter > 0)
            jumpBufferCounter -= Time.deltaTime;
        if (jumpBufferCounter > 0)
            desiredJump = true;

        //Ajuste da altura do pulo
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 3);



        //Suavizacao do movimento do pulo/Queda

        if (isFalling && Mathf.Abs(body.velocity.y) < 0.2f && !isDashing) //Apice do pulo
            body.gravityScale = apexGravity;

        else if (Input.GetKey(KeyCode.Space) && body.velocity.y > 0 && !isDashing)
            body.gravityScale = upwardMovementMultiplier;

        //else if (Input.GetKey(KeyCode.Space) && body.velocity.y < 0)
        //body.gravityScale = umbrellaGravity;

        else if (body.velocity.y < 0 && !isDashing)
            body.gravityScale = downwardMovementMultiplier;

        else if (body.velocity.y >= 0 && !isDashing)
            body.gravityScale = defaultGravity;


    }

    private void FixedUpdate()
    {
        if (isDashing)
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
        }
        else
        {
            body.drag = airDrag;
            isFalling = true;
        }

        //Player para instantaneamente quando a velocidade fica muito baixa
        if (Mathf.Abs(body.velocity.x) < 0.01f)
        {
            body.velocity = new Vector2(0, body.velocity.y);;
        }



        //Input do pulo
        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }




        /*
        //Movimentacao com velocity
        
        //calcula aceleracao e aplica a velocidade de acordo
        targetVelocity = new Vector2(x, 0f) * (maxSpeed * Time.fixedDeltaTime); //Velocidade maxima
        currentVelocity = body.velocity;

        acceleration = isGrounded() ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.fixedDeltaTime;
        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, targetVelocity.x, maxSpeedChange);

        body.velocity = currentVelocity;

        */


        // Movimentacao com force

        targetVelocity = new Vector2(x, 0f) * (maxSpeed * Time.fixedDeltaTime); //Velocidade maxima

        if(isGrounded())
            acceleration = (Mathf.Abs(targetVelocity.x) > 0.01f) ? maxAcceleration : maxDecceleration;
        else
            acceleration = (Mathf.Abs(targetVelocity.x) > 0.01f) ? maxAirAcceleration : maxAirDecceleration;

        //Conserva o impulso se tiver ja tiver acelerado acima do limite
        if (Mathf.Abs(body.velocity.x) > Mathf.Abs(targetVelocity.x) && Mathf.Sign(body.velocity.x) == Mathf.Sign(targetVelocity.x) && Mathf.Abs(targetVelocity.x) > 0.01f && !isGrounded())
        {
            //Previne o player de desacelerar se estiver sendo impulsionado
            acceleration = 0;
        }

        //Diferenca entre a velocidade atual e a desejada
        velDif = targetVelocity.x - body.velocity.x;

        float mov = velDif * acceleration;

        body.AddForce(mov * Vector2.right, ForceMode2D.Force);



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

    private void Jump()
    {
        if (coyoteCounter < 0 && !onWall()) return;

        //if (onWall())
            //WallJump();
        else
        {
            if (isGrounded() || coyoteCounter > 0)
            {

                // Calcula a direção do salto baseado na entrada horizontal (x)
                //Vector2 jumpDirection = new Vector2(x, 1f).normalized;

                // Aplica a força de salto usando AddForce
                //body.AddForce(jumpDirection * jumpSpeed, ForceMode2D.Impulse);

                jumpAction = true;
                body.velocity = new Vector2(body.velocity.x, 0);
                body.AddForce(Vector2.up * jumpSpeed,ForceMode2D.Impulse);
                coyoteCounter = 0; //Evita spam de pulos


                //Antigo

                //body.velocity = new Vector2(body.velocity.x, jumpSpeed * Time.fixedDeltaTime);
                //body.velocity = new Vector2(body.velocity.x + 100, 0);
                //body.velocity += dir * jumpSpeed;
            }
            
        }
            
    }

    private void WallJump()
    {
        //body.velocity = Vector2.Lerp(body.velocity, (new Vector2(x * speed, body.velocity.y)), .5f * Time.deltaTime);
        //body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpx, wallJumpy));
        wallJumpCooldown = 0;
    }

    //Checa hitbox do chao para definir se o player esta em contato com o chao
    private bool isGrounded()
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
            canDash = false;
            isDashing = true;

            body.gravityScale = 0f;

            body.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
            //rig.AddForce(new Vector2(DashForce * Input.GetAxis("Horizontal"), 0f), ForceMode2D.Impulse);
            trail.emitting = true;

            yield return new WaitForSeconds(dashingTime);

            trail.emitting = false;
            isDashing = false;
            trail.Clear();
    }


}
