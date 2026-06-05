using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager_Rigid : MonoBehaviour
{
    [Header("プレイヤー操作")]
    public TextMeshProUGUI debugText;
    public enum EPlayerStates
    {
        Stand,     //立ち
        WalkL,     //歩き（左）
        WalkR,     //歩き（右）
        Lookup,    //上を向く
        Lookdown,  //下を向く
        Jump,      //ジャンプした瞬間
        Jumped,    //ジャンプした後
        Climb,     //上る
        Wallhold,  //壁につかまる
        ClimbLadder,      //はしごを登る
        ClimbRope,        //ロープを登る
        MoveObject,       //オブジェクトの押し引き
        HoldingRope,    //ロープにつかまる
        Death           //死んだ
    }
    public EPlayerStates currentStates = EPlayerStates.Stand;

    public PlayerInput playerInput;
    public Quaternion targetRotation;
    public bool push = false;
    public bool pull = false;
    public bool dead = false;

    private bool hiding;
    private GameObject pushingObject = null;
    private GameObject holdingRopeObejuct = null;
    public GameObject monster;
    private AudioSource audioSource;

    public bool actionButton;
    public bool specialAction;
    bool jump;


    //bool isKinematic;
    Rigidbody rigid;
    [Header("RigidBody関連")]
    [SerializeField, Range(0f,100f)]
    public float maxSpeed = 10f;
    private float defaultSpeed;
    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f;
    [SerializeField, Range(0f, 100f)]
    float maxAirAcceleration = 1f;
    public Vector3 velocity;
    Vector3 desiredVelocity;
    Vector2 moveInput;

    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 25f;

    float minGroundDotProduct;
    bool desiredJump;
    public bool onGround;
    [SerializeField, Range(0f, 100f)]
    float jumpHeight = 2f;

    private void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    void Awake()
    {
        Application.targetFrameRate = 60;
        audioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();
        OnValidate();
        defaultSpeed = maxSpeed;
        dead = false;
    }

    void Start()
    {
        if (monster)
        {
            Physics.IgnoreCollision(this.GetComponent<Collider>(), monster.GetComponent<Collider>());
        }
    }

    void Update()
    {

        //仮処理
        //if(Input.GetKeyDown(KeyCode.F))
        //{
        //    currentStates = EPlayerStates.Death;
        //}

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    currentStates = EPlayerStates.Stand;
        //}

        if (debugText != null)
        {
            debugText.text = "status: " + currentStates + "\n" + "hiding:" + hiding + "\n" + "ActionButton:" + actionButton;
        }

        if (Input.GetButtonDown("ActionButton"))
        {
            actionButton = true;
        }
        if (Input.GetButtonUp("ActionButton"))
        {
            actionButton = false;
        }
        if (!specialAction)
        {
            PlayerStateChange();
        }
        PlayerAction();
        //RigidBody関連
        //playerInput = Vector2.ClampMagnitude(moveInput, 1f);
        if (!specialAction || currentStates == EPlayerStates.MoveObject)
        {
            desiredVelocity = new Vector3(moveInput.x, 0f, moveInput.y) * maxSpeed;
        }
        desiredJump |= Input.GetButtonDown("Jump2");
    }

    //Rigidbody関連
    void FixedUpdate()
    {
        if(rigid.isKinematic != true)
        {
            UpdateState();
            float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
            float maxSpeedChange = acceleration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            //velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

            if (desiredJump)
            {
                desiredJump = false;
                Jump();
            }
            rigid.linearVelocity = velocity;
            onGround = false;
        }

   
    }

    #region RigidBody
    void UpdateState()
    {
        velocity = rigid.linearVelocity;
    }


    void Jump()
    {
        if (onGround)
        {
            velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            audioSource.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= minGroundDotProduct;
        }
    }
    #endregion

    #region PlayerState
    void PlayerStateChange()
    {
        //if (currentStates == EPlayerStates.Death)
        //    return;


        {
            if (velocity.x < -0.1f)
            {
                //左に歩く
                currentStates = EPlayerStates.WalkL;
            }
            else if (velocity.x > 0.1f)
            {
                //右に歩く
                currentStates = EPlayerStates.WalkR;
            }
            else
            {
                //立ち
                currentStates = EPlayerStates.Stand;
            }
            if (currentStates == EPlayerStates.Stand)
            {
                //立っている時にできる動作
                if (moveInput.y < -0.1f)
                {
                    //下を見る
                    currentStates = EPlayerStates.Lookdown;
                }
                else if (moveInput.y > 0.1f)
                {
                    //上を見る
                    currentStates = EPlayerStates.Lookup;
                }
                else
                {
                    //通常の立ち
                    currentStates = EPlayerStates.Stand;
                }
            }
            if (currentStates == EPlayerStates.Stand ||
                currentStates == EPlayerStates.WalkL ||
                currentStates == EPlayerStates.WalkR ||
                currentStates == EPlayerStates.Lookup ||
                currentStates == EPlayerStates.Lookdown)
            {
                //ジャンプが出来る条件
                if (desiredJump)
                {
                    //スペースキーが押されたらジャンプ
                    currentStates = EPlayerStates.Jump;
                }
            }
            if (currentStates == EPlayerStates.Jumped)
            {
                //ジャンプ後の動作
                if (onGround)
                {
                    //着地後立ち状態に戻る
                    currentStates = EPlayerStates.Stand;
                }
            }
            if (!onGround)
            {
                //着地していない状態であればジャンプ後
                currentStates = EPlayerStates.Jumped;
            }
        }
    }

    void PlayerAction()
    {

        {
            //処理
            if (currentStates == EPlayerStates.Stand)
            {
                CameraManager.CameraIdle();
                PlayerStand();
            }
            if (currentStates == EPlayerStates.WalkL)
            {
                CameraManager.CameraIdle();
                PlayerWalkL();
            }
            if (currentStates == EPlayerStates.WalkR)
            {
                CameraManager.CameraIdle();
                PlayerWalkR();
            }
            if (currentStates == EPlayerStates.Lookup)
            {
                CameraManager.CameraLookUp();
                PlayerLookup();
            }
            if (currentStates == EPlayerStates.Lookdown)
            {
                CameraManager.CameraLookDown();
                PlayerLookdown();
            }
            if (currentStates == EPlayerStates.Jump)
            {
                CameraManager.CameraIdle();
                PlayerJump();
            }
            if (currentStates == EPlayerStates.Jumped)
            {
                PlayerJumped();
            }
            if (currentStates == EPlayerStates.ClimbLadder)
            {
                PlayerClimbLadder();
            }
            if (currentStates == EPlayerStates.ClimbRope)
            {
                PlayerClimbRope();
            }
            if (currentStates == EPlayerStates.MoveObject)
            {
                PlayerMoveObjects();
            }
            if (currentStates == EPlayerStates.Wallhold)
            {
                PlayerWallHold();
            }
            if (currentStates == EPlayerStates.HoldingRope)
            {
                PlayerHoldRope();
            }
            if (currentStates == EPlayerStates.Death)
            {
                PlayerDeath();
            }
        }
    }
    #endregion

    #region FUNCTION
    //立ち
    void PlayerStand()
    {
        CameraManager.CameraIdle();
    }
    //歩き（左）
    void PlayerWalkL()
    {
        CameraManager.CameraIdle();
    }
    //歩き（右）
    void PlayerWalkR()
    {
        CameraManager.CameraIdle();
    }
    //上向き
    void PlayerLookup()
    {
        CameraManager.CameraLookUp();
    }
    //下向き
    void PlayerLookdown()
    {
        CameraManager.CameraLookDown();
    }
    //ジャンプ
    void PlayerJump()
    {
        
    }
    //ジャンプ後
    void PlayerJumped()
    {
        
    }
    //はしごにつかまる
    void PlayerClimbLadder()
    {

    }
    //ロープにつかまる（古いバージョン）
    void PlayerClimbRope()
    {
        rigid.linearVelocity = Vector3.zero;
        desiredVelocity = Vector3.zero;
        if (moveInput.y < -0.5f)
        {
            rigid.MovePosition(rigid.position + (Vector3.down * Time.deltaTime * 4f));
        }
        if (moveInput.y > 0.5f)
        {
            rigid.MovePosition(rigid.position + (Vector3.up * Time.deltaTime * 4f));
        }
        if (desiredJump)
        {
            currentStates = EPlayerStates.Jump;
            specialAction = false;
            rigid.AddForce(new Vector3(moveInput.x * 5f, 4f, 0),ForceMode.Impulse);
        }
    }
    //崖につかまる
    void PlayerWallHold()
    {
        Vector3 toObject = pushingObject.transform.position - transform.position;
        float direction = Mathf.Sign(toObject.x);
        Vector3 targetPos = new Vector3(pushingObject.transform.position.x + direction * -0.35f,
                                        pushingObject.transform.position.y,
                                        transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 25f);


        rigid.linearVelocity = Vector3.zero;
        desiredVelocity = Vector3.zero;
        if (moveInput.y > 0.9f || desiredJump)
        {
            rigid.useGravity = true;
            currentStates = EPlayerStates.Jump;
            specialAction = false;
            rigid.AddForce(new Vector3(moveInput.x * 0f, 6f, 0), ForceMode.Impulse);
        }
        if (moveInput.y < -0.9f)
        {
            rigid.useGravity = true;
            currentStates = EPlayerStates.Jumped;
            specialAction = false;
        }
    }
    //物の押し引き
    void PlayerMoveObjects()
    {
        maxSpeed = 1.5f;
        if (pushingObject != null)
        {
            pushingObject.GetComponent<Rigidbody>().linearVelocity = desiredVelocity;
        }
        if (!actionButton)
        {
            maxSpeed = defaultSpeed;
            specialAction = false;
        }
    }
    //ロープにつかまる（新バージョン）
    void PlayerHoldRope()
    {
        if (!holdingRopeObejuct) return;

        Vector3 ropePos = holdingRopeObejuct.transform.position;

        Vector3 targetPos = new Vector3(ropePos.x, ropePos.y - 0.8f, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 15f);

        Rigidbody ropeRb = holdingRopeObejuct.GetComponent<Rigidbody>();

        if (ropeRb && Mathf.Abs(moveInput.x) >= 0.05f)
        {
            ropeRb.AddTorque(Vector3.forward * moveInput.x * 15000f * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        if (desiredJump)
        {
            Collider myCol = GetComponent<Collider>();
            Collider[] ropeCols = holdingRopeObejuct.GetComponentsInChildren<Collider>();

            foreach (var ropeCol in ropeCols)
            {
                Physics.IgnoreCollision(myCol, ropeCol);
            }

            rigid.isKinematic = false;
            rigid.useGravity = true;

            specialAction = false;
            currentStates = EPlayerStates.Jump;
            rigid.AddForce(new Vector3(moveInput.x * 2f, 7f, 0), ForceMode.Impulse);

            holdingRopeObejuct = null;
        }
    }
    //死ぬ
    void PlayerDeath()
    {
        PlayerSaveManager.instance.LoadPlayer(gameObject);
        currentStates = EPlayerStates.Stand;
        specialAction = false;
    }

    #endregion

    #region Trigger
    //入ったフレームのみ
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LightHitBox_Hide")
        {
            hiding = true;
        }

        if (other.gameObject.tag == "Yagi")
        {
            Debug.Log("Touch");
            specialAction = true;
            SoundManager.instance.PlayBGMSound("Stage2");
            currentStates = EPlayerStates.Death;
        }
    }

    //判定を用いた行動条件はここに追加
    private void OnTriggerStay(Collider other)
    {
        //はしご のタグ
        if (other.gameObject.tag == "ladder")
        {
            //行動条件を追加
            if (currentStates == EPlayerStates.Stand ||
                currentStates == EPlayerStates.WalkL ||
                currentStates == EPlayerStates.WalkR ||
                currentStates == EPlayerStates.Jumped)
            {
                //上下キーが０なら（つまり上か下か押されたら）
                if (moveInput.y != 0)
                {
                    //プレイヤーの状態を変える処理
                    currentStates = EPlayerStates.ClimbLadder;
                    specialAction = true;
                }

            }
        }
        //ロープ
        if (other.gameObject.tag == "rope")
        {
            if (currentStates == EPlayerStates.Stand ||
                currentStates == EPlayerStates.WalkL ||
                currentStates == EPlayerStates.WalkR ||
                currentStates == EPlayerStates.Jumped)
            {
                //上下キーが０なら（つまり上か下か押されたら）
                if (moveInput.y < -0.75f || moveInput.y > 0.75f || actionButton)
                {
                    //プレイヤーの状態を変える処理
                    currentStates = EPlayerStates.ClimbRope;
                    //rigid.constraints = RigidbodyConstraints.FreezeAll;
                    rigid.useGravity = false;
                    specialAction = true;
                }
            }
        }

        //物の押し引き
        if (other.gameObject.tag == "moveable")
        {
            if (currentStates == EPlayerStates.Stand ||
                currentStates == EPlayerStates.WalkL ||
                currentStates == EPlayerStates.WalkR)
            {
                //アクションボタンが押されたら
                if (actionButton)
                {
                    //プレイヤーの状態を変える処理
                    currentStates = EPlayerStates.MoveObject;
                    pushingObject = other.transform.parent.gameObject;
                    var movRB = pushingObject.GetComponent<Rigidbody>();
                    movRB.constraints &= ~RigidbodyConstraints.FreezePositionX;
                    specialAction = true;
                    //押しているか引いているかの判断（アニメーション用）
                    // アニメーション初期化

                }
                else
                {
                    if(pushingObject)
                    {
                        var movRB = pushingObject.GetComponent<Rigidbody>();
                           movRB.constraints |= RigidbodyConstraints.FreezePositionX;
                    }
                }
            }
            if (velocity.y > 1f)
            {
                specialAction = false;
            }
            // 押し引き初期化（毎フレーム）
            push = false;
            pull = false;

            if (actionButton && pushingObject != null)
            {
                Vector3 toObject = pushingObject.transform.position - transform.position;
                float direction = Mathf.Sign(toObject.x);

                targetRotation = Quaternion.Euler(0, direction * 90, 0);
                if (moveInput.x != 0)
                {
                    if (Mathf.Sign(moveInput.x) == direction)
                    {
                        push = true;
                    }
                    else
                    {
                        pull = true;
                    }
                }
            }

        }

        //タグはここに追加
        if (other.gameObject.tag == "")
        {
            //行動条件
            if (currentStates == EPlayerStates.Stand)
            {
                //プレイヤーの状態を変える処理
                currentStates = EPlayerStates.ClimbLadder;
                specialAction = true;
            }
        }

        //壁のタグ
        if (other.gameObject.tag == "Wall")
        {
            //行動条件を追加
            if (currentStates == EPlayerStates.Jumped)
            {
                if (actionButton == true)
                {
                    pushingObject = other.gameObject;
                    Vector3 toObject = pushingObject.transform.position - transform.position;
                    float direction = Mathf.Sign(toObject.x);

                    targetRotation = Quaternion.Euler(0, direction * 90, 0);

                    currentStates = EPlayerStates.Wallhold;
                    specialAction = true;
                    rigid.useGravity = false;
                    Debug.Log("つかんだ");
                }
            }
        }

        if (other.gameObject.tag == "HoldingRope")
        {
            if (currentStates == EPlayerStates.Jumped)
            {
                if (actionButton == true)
                {
                    //rigid.linearVelocity = Vector3.zero;
                    //desiredVelocity = Vector3.zero;
                    //ロープの一番下のオブジェクトを呼び出す
                    holdingRopeObejuct = other.gameObject.transform.parent.GetChild(4).gameObject;
                    //transform.SetParent(holdingRopeObejuct.transform);
                  
                    rigid.useGravity = false;
                    rigid.isKinematic = true;

                    specialAction = true;
                    currentStates = EPlayerStates.HoldingRope;

                }
            }
        }

        //光
        if (other.gameObject.tag == "LightHitBox")
        {
            if (hiding == false)
            {
                //プレイヤーの状態を変える処理
                SoundManager.instance.PlayBGMSound("Stage3");
                currentStates = EPlayerStates.Death;
                specialAction = true;
            }
        }
    }

    //コライダーから出た時
    private void OnTriggerExit(Collider other)
    {
        //はしごのコライダーから出た場合、通常のアクションにもどす。
        if (other.gameObject.tag == "ladder")
        {
            specialAction = false;
        }
        //はしごのコライダーから出た場合、通常のアクションにもどす。
        if (other.gameObject.tag == "rope")
        {
            specialAction = false;
            //rigid.constraints = RigidbodyConstraints.None;
            //rigid.constraints = RigidbodyConstraints.FreezePositionZ;
            //rigid.constraints = RigidbodyConstraints.FreezeRotation;
            rigid.useGravity = true;
        }
        //押せるもののコライダーから出た場合、通常のアクションにもどす。
        if (other.gameObject.tag == "moveable")
        {
            if(pushingObject)
            {
                var movRB = pushingObject.GetComponent<Rigidbody>();
                movRB.constraints |= RigidbodyConstraints.FreezePositionX;
            }

            specialAction = false;
            pushingObject = null;
            maxSpeed = defaultSpeed;

         
            //playerAnimations.push = false;
            //playerAnimations.pull = false;
        }

        //壁のコライダーから出た場合、通常のアクションにもどす。
        if (other.gameObject.tag == "Wall")
        {
            specialAction = false;
            rigid.useGravity = true;
            pushingObject = null;
        }
        if (other.gameObject.tag == "HoldingRope")
        {
            //specialAction = false;
            //transform.SetParent(null);
            //holdingRopeObejuct = null;
            //isKinematic = false;
            //rigid.useGravity = true;
        }

        if (other.gameObject.tag == "LightHitBox_Hide")
        {
            hiding = false;
        }
    }

    #endregion
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
