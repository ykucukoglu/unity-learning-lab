using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 1.6f;
    public float runSpeed = 3.5f;
    private float moveSpeed = 0f;

    [Header("Ground Check")]
    private Transform _groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundLayer;

    [Header("Jump")]
    public int maxJumpCount = 2;
    public float jumpForce = 5f;

    public Animator _animator;

    private Rigidbody _rb;
    private PlayerState currentState;
    private int jumpCount;
    private bool isGrounded;
    private Vector3 _moveInput;

    private static readonly int HashIsJumping = Animator.StringToHash("IsJumping");
    private static readonly int HashSpeed = Animator.StringToHash("Speed");


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        ChangeState(PlayerState.Idle);


        Transform gc = transform.Find("GroundCheck");
        if (gc != null) _groundCheck = gc;

        _animator = GetComponent<Animator>();

    }

    private void Update()
    {
        print(moveSpeed);
        ReadInput();
        HandleState();
        CheckGround();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    //Player hareket inputlarını okuma
    void ReadInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        _moveInput = new Vector3(x, 0, z);

        // Hız belirleme
        if (_moveInput.magnitude > 0.1f)
            moveSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        else
            moveSpeed = 0f;
    }

    //Player state değiştirme
    void ChangeState(PlayerState newState)
    {
        currentState = newState;
    }

    //Player state'ine göre hareket ve animasyon kontrolü
    void HandleState()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
            case PlayerState.Walk:
            case PlayerState.Run:
                HandleRotation();
                HandleJump();
                break;

            case PlayerState.Jump:
                HandleRotation();
                HandleJump();
                break;

        }
    }

    //Player hareketlerini kontrol etme
    void HandleRotation()
    {
        if (_moveInput == Vector3.zero) return;

        // Kamera yönünü baz al
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f; // sadece yatay düzlem
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Inputu kamera yönüne çevir
        Vector3 moveDir = camForward * _moveInput.z + camRight * _moveInput.x;
        moveDir.Normalize();

        // Karakteri hareket yönüne döndür
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation,  // şu anki yön
                                              targetRotation,      // gitmek istediği yön
                                              Time.deltaTime * 10f // dönüş hızı
                                                                  );

        _moveInput = moveDir;
    }

    //Player hareketlerini uygulama
    void ApplyMovement()
    {
        if (_moveInput.magnitude > 0.1f)
        {
            Vector3 velocity = _moveInput.normalized * moveSpeed;
            _rb.linearVelocity = new Vector3(velocity.x, _rb.linearVelocity.y, velocity.z);
        }
        else
        {
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
        }
    }

    //Player zıplama kontrolü
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
            jumpCount++;

            // Y eksenini sıfırla ve zıplama kuvveti uygula
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            ChangeState(PlayerState.Jump);
        }
    }

    //Ground check yaparak yere temas durumunu kontrol etme
    void CheckGround()
    {
        // önceki grounded durumunu kaydet
        bool wasGrounded = isGrounded;
        // Şu anki grounded durumunu kontrol et
        isGrounded = Physics.CheckSphere(_groundCheck.position, groundDistance, groundLayer);

        // Karakter yere tam temas ettiğinde jumpCount sıfırla
        if (isGrounded && !wasGrounded)
        {
            jumpCount = 0;       // Çift zıplama sonrası tekrar zıplayabilmesi için
            ChangeState(PlayerState.Idle);
        }
    }

    //Animasyon parametrelerini güncelleme
    void UpdateAnimator()
    {
        _animator.SetBool(HashIsJumping, !isGrounded);
        // Animasyon speed doğrudan moveSpeed üzerinden
        float animSpeed = 0f;
        if (moveSpeed == walkSpeed) animSpeed = 0.5f;
        else if (moveSpeed == runSpeed) animSpeed = 1f;
        else animSpeed = 0f;

        _animator.SetFloat(HashSpeed, animSpeed, 0.1f, Time.deltaTime);
    }

}
public enum PlayerState
{
    Idle,
    Walk,
    Run,
    Jump,
    Dash,
    Dead
}
