using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.6f;
    public float jumpForce = 5f;

    [Header("Ground Check")]
    private Transform _groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundLayer;

    [Header("Jump")]
    public int maxJumpCount = 2;

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
                HandleRotation();
                HandleJump();
                UpdateMovementState();
                break;

            case PlayerState.Jump:
                HandleRotation();
                HandleJump();
                break;

        }
    }


    // Hareket durumuna göre state belirle
    void UpdateMovementState()
    {
        bool isMoving = _moveInput.magnitude > 0.1f;
        ChangeState(isMoving ? PlayerState.Walk : PlayerState.Idle);
    }

    //Player hareketlerini kontrol etme
    void HandleRotation()
    {
        if (_moveInput == Vector3.zero) return;

        // Hareket varsa karakterin yönünü değiştir
        Quaternion targetRotation = Quaternion.LookRotation(_moveInput);
        transform.rotation = Quaternion.Slerp(transform.rotation,  // şu anki yön
                                              targetRotation,      // gitmek istediği yön
                                              Time.deltaTime * 10f // dönüş hızı
                                                                  );
    }

    //Player hareketlerini uygulama
    void ApplyMovement()
    {
        _rb.linearVelocity = new Vector3(_moveInput.x * moveSpeed, _rb.linearVelocity.y, _moveInput.z * moveSpeed);
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
        _animator.SetFloat(HashSpeed, _moveInput.magnitude, 0.1f, Time.deltaTime);
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
