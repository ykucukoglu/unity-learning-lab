using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; } // Global erişim noktası

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
    
    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina = 100f;
    public float staminaRegenRate = 12f; // Biraz daha yavaş yenilensin
    public float sprintStaminaCost = 25f; // Koşarken daha hızlı tükensin
    public float dashStaminaCost = 30f; // Atılma biraz daha pahalı olsun

    [Header("Dash")]
    public float dashForce = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private float lastDashTime = 0f;

    public Animator _animator;

    private Rigidbody _rb;
    private PlayerState currentState;
    private int jumpCount;
    private bool isGrounded;
    private bool isExhausted; // Yorulma durumu
    private bool _isSprinting; // Koşma durumu takibi
    private Vector3 _moveInput;

    private static readonly int HashIsJumping = Animator.StringToHash("IsJumping");
    private static readonly int HashSpeed = Animator.StringToHash("Speed");


    private void Awake()
    {
        // Singleton yapısı: Her yerden erişim için kendisini Instance'a ata
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Eğer başka bir oyuncu varsa yenisini yok et
        }
    }

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
        HandleStamina();
        ProcessMovement();
        HandleAbilities();
        HandleState(); // Kamera yönüne çevirme ve rotasyon
        CheckGround();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ReadInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        _moveInput = new Vector3(x, 0, z);
    }

    private void HandleStamina()
    {
        // Yorulma (Exhaustion) mantığı
        if (currentStamina <= 0) isExhausted = true;
        if (isExhausted && currentStamina >= 20f) isExhausted = false;

        // Koşma (Sprint) durumu belirleme
        _isSprinting = Input.GetKey(KeyCode.LeftShift) && _moveInput.magnitude > 0.1f && isGrounded && currentStamina > 0 && !isExhausted;

        // Enerji değişimi
        if (_isSprinting)
        {
            currentStamina -= sprintStaminaCost * Time.deltaTime;
        }
        else if (!isDashing)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    private void ProcessMovement()
    {
        if (isDashing) return;

        if (_isSprinting)
        {
            moveSpeed = runSpeed;
            ChangeState(PlayerState.Run);
        }
        else if (_moveInput.magnitude > 0.1f)
        {
            moveSpeed = walkSpeed;
            ChangeState(PlayerState.Walk);
        }
        else
        {
            moveSpeed = 0f;
            ChangeState(PlayerState.Idle);
        }
    }

    private void HandleAbilities()
    {
        // Atılma (Dash) kontrolü
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastDashTime + dashCooldown && currentStamina >= dashStaminaCost && !isDashing)
        {
            Vector3 dashDir = GetCameraRelativeDirection(_moveInput);
            StartCoroutine(Dash(dashDir));
        }
    }

    private Vector3 GetCameraRelativeDirection(Vector3 input)
    {
        if (input == Vector3.zero) return Vector3.zero;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        return (camForward * input.z + camRight * input.x).normalized;
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

            case PlayerState.Dash:
                break;

        }
    }

    private IEnumerator Dash(Vector3 dashDir)
    {
        isDashing = true;
        currentStamina -= dashStaminaCost;
        lastDashTime = Time.time;
        ChangeState(PlayerState.Dash);

        // Ekranda yön girdisi yoksa (sadece E'ye basıldıysa) baktığı yöne atıl
        if (dashDir == Vector3.zero) dashDir = transform.forward;

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            _rb.linearVelocity = new Vector3(dashDir.x * dashForce, _rb.linearVelocity.y, dashDir.z * dashForce);
            yield return null;
        }

        isDashing = false;
        ChangeState(isGrounded ? PlayerState.Idle : PlayerState.Jump);
    }

    //Player hareketlerini kontrol etme
    void HandleRotation()
    {
        if (_moveInput == Vector3.zero) return;

        // Kamera yönünü baz alarak inputu çevir
        Vector3 moveDir = GetCameraRelativeDirection(_moveInput);

        // Karakteri hareket yönüne döndür
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                              targetRotation,
                                              Time.deltaTime * 10f
                                             );

        _moveInput = moveDir;
    }

    //Player hareketlerini uygulama
    void ApplyMovement()
    {
        if (isDashing) return; // Atılma sırasında normal hareketi durdur

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

        // Animasyon hızı (0 ile 1 arasına normalize edildi)
        float animSpeed = moveSpeed / runSpeed;
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
