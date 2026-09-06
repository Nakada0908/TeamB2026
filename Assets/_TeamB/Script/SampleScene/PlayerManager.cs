using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    //プレイヤーの行動リスト
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
    public Vector3 playerPos;
    public bool specialAction;
    public bool actionButton;

    [SerializeField]
    public CharacterController characterController;
    private Rigidbody rigidbody;
    public PlayerInput playerInput;
    public PlayerAnimations playerAnimations;
    [SerializeField]
    private float moveSpeed = 6.0f;
    [SerializeField]
    private float jumpSpeed = 2.0f;

    public TextMeshProUGUI debugText;

    private Vector3 moveDirection = Vector3.zero;
    float moveX;
    float moveY;

    private bool hiding;
    private GameObject pushingObject = null;
    private GameObject holdingRopeObejuct = null;
    public GameObject monster;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;

        if (monster)
        {
            Physics.IgnoreCollision(characterController, monster.GetComponent<Collider>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = new Vector3(moveX, 0, 0);
        Vector3 displacement = velocity * Time.deltaTime;
        transform.localPosition = displacement;

        debugText.text = "status: " + currentStates + "\n" + "hiding:" + hiding;

        playerPos = transform.position;

        moveX = UnityEngine.Input.GetAxis("Horizontal");
        moveY = UnityEngine.Input.GetAxis("Vertical");

        moveDirection.x = moveX;


        //仮アクションボタン
        if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
        {
            actionButton = true;
        }
        else
        {
            actionButton = false;
        }

        //行動リスト、条件指定（通常アクション）
        if (specialAction == false)
        {
            if (moveX < -0.1f)
            {
                //左に歩く
                currentStates = EPlayerStates.WalkL;
            }
            else if (moveX > 0.1f)
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
                if (moveY < -0.1f)
                {
                    //下を見る
                    currentStates = EPlayerStates.Lookdown;
                }
                else if (moveY > 0.1f)
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
                if (UnityEngine.Input.GetKey(KeyCode.Space))
                {
                    //スペースキーが押されたらジャンプ
                    currentStates = EPlayerStates.Jump;
                }
            }
            if (currentStates == EPlayerStates.Jumped)
            {
                //ジャンプ後の動作
                if (characterController.isGrounded)
                {
                    //着地後立ち状態に戻る
                    currentStates = EPlayerStates.Stand;
                }
            }
            if (characterController.isGrounded == false)
            {
                //着地していない状態であればジャンプ後
                currentStates = EPlayerStates.Jumped;
            }
        }

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
            PlayerWallhold();
        }
        if (currentStates == EPlayerStates.HoldingRope)
        {
            PlayerHoldRope();
        }
        if (currentStates == EPlayerStates.Death)
        {
            PlayerDeath();
        }

        // 仮条件
        if (currentStates != EPlayerStates.HoldingRope)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    #region FUNTION

    //立ち
    void PlayerStand()
    {
        moveDirection.y = -0.03f;
    }
    //歩き（左）
    void PlayerWalkL()
    {
        moveDirection.y = -0.03f;
    }
    //歩き（右）
    void PlayerWalkR()
    {
        moveDirection.y = -0.03f;
    }
    //上向き
    void PlayerLookup()
    {

    }
    //下向き
    void PlayerLookdown()
    {

    }
    //ジャンプ
    void PlayerJump()
    {
        moveDirection.y = jumpSpeed;
    }
    //ジャンプ後
    void PlayerJumped()
    {
        moveDirection.y -= 5.0f * Time.deltaTime;
    }
    //はしご等
    void PlayerClimbLadder()
    {
        //上キーが押されたら上に移動
        if (0.1f < moveY)
        {
            moveDirection.y = 0.75f;
        }
        //下キーが押されたら下に移動
        else if (moveY < -0.1f)
        {
            moveDirection.y = -0.75f;
        }
        //どちらも押されていないなら、y座標の速度を0にする
        else
        {
            moveDirection.y = 0;
        }
        //ジャンプキーが押されたら行動を止めて、ジャンプする。
        if (UnityEngine.Input.GetKey(KeyCode.Space))
        {
            specialAction = false;
            PlayerJump();
        }
    }
    //ロープ
    void PlayerClimbRope()
    {
        moveDirection.x = 0f;
        if (0.1f < moveY)
        {
            moveDirection.y = 0.5f;
        }
        //下キーが押されたら下に移動
        else if (moveY < -0.1f)
        {
            moveDirection.y = -0.5f;
        }
        else
        {
            moveDirection.y = 0;
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            specialAction = false;
            if (moveX < 0f)
            {
                moveDirection.x = -5f;
            }
            if (moveX > 0f)
            {
                moveDirection.x = 5f;
            }
            moveDirection.y = jumpSpeed;
        }
    }
    //壁につかまる
    void PlayerWallhold()
    {   //y座標の速度を0にする
        moveDirection.y = 0;
        //上キーが押されたらジャンプ
        if (UnityEngine.Input.GetKey(KeyCode.UpArrow))
        {
            PlayerJump();
        }
        //下キーが押されたら落下
        if (UnityEngine.Input.GetKey(KeyCode.DownArrow))
        {
            specialAction = false;
        }
    }

    void PlayerMoveObjects()
    {
        if (actionButton && pushingObject != null)
        {
            moveDirection.x = 0;
            if (moveX < 0f)
            {
                moveDirection.x = -25f * Time.deltaTime;
                pushingObject.transform.Translate(-1.65f * Time.deltaTime, 0, 0);
            }
            if (moveX > 0f)
            {
                moveDirection.x = 25f * Time.deltaTime;
                pushingObject.transform.Translate(1.65f * Time.deltaTime, 0, 0);
            }
        }
        else
        {
            specialAction = false;
        }
    }

    void PlayerHoldRope()
    {
        moveDirection.y = 0;
        moveDirection.x = 0;

        if (UnityEngine.Input.GetKey(KeyCode.F))
        {
            if (holdingRopeObejuct)
            {
                holdingRopeObejuct.GetComponent<Rigidbody>().AddForce(transform.right * 20f, ForceMode.Acceleration);
            }
            else
            {
                Debug.Log("ロープが無いです");
            }
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            transform.SetParent(null);
            currentStates = EPlayerStates.Jump;
        }
    }
    //死んだ
    void PlayerDeath()
    { //仮の処理です
        SceneManager.LoadScene("AnimationTest_Yoshi");
    }









    //入ったフレームのみ
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LightHitBox_Hide")
        {
            hiding = true;
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
                if (moveY != 0)
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
                if (moveY != 0 || actionButton)
                {
                    //プレイヤーの状態を変える処理
                    currentStates = EPlayerStates.ClimbRope;
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
                    pushingObject = other.gameObject;
                    specialAction = true;
                    //押しているか引いているかの判断（アニメーション用）
                    // アニメーション初期化

                }
            }
            if (characterController.isGrounded == false)
            {
                specialAction = false;
            }
            // 押し引き初期化（毎フレーム）
            playerAnimations.push = false;
            playerAnimations.pull = false;

            if (actionButton && pushingObject != null)
            {
                Vector3 toObject = pushingObject.transform.position - transform.position;
                float direction = Mathf.Sign(toObject.x);

                playerAnimations.targetRotation = Quaternion.Euler(0, direction * 90, 0);
                if (moveX != 0)
                {
                    if (Mathf.Sign(moveX) == direction)
                    {
                        playerAnimations.push = true;
                    }
                    else
                    {
                        playerAnimations.pull = true;
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
                    currentStates = EPlayerStates.Wallhold;
                    specialAction = true;
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
                    //ロープの一番下のオブジェクトを呼び出す
                    holdingRopeObejuct = other.gameObject.transform.parent.GetChild(4).gameObject;
                    transform.SetParent(holdingRopeObejuct.transform);
                    //transform.localPosition = Vector3.zero;

                    currentStates = EPlayerStates.HoldingRope;
                    specialAction = true;
                }
            }
        }

        //光
        if (other.gameObject.tag == "LightHitBox")
        {
            if (hiding == false)
            {
                //プレイヤーの状態を変える処理
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
        }
        //押せるもののコライダーから出た場合、通常のアクションにもどす。
        if (other.gameObject.tag == "moveable")
        {
            specialAction = false;
            pushingObject = null;
            playerAnimations.push = false;
            playerAnimations.pull = false;
        }

        //壁のコライダーから出た場合、通常のアクションにもどす。
        if (other.gameObject.tag == "Wall")
        {
            specialAction = false;
        }
        if (other.gameObject.tag == "HoldingRope")
        {
            specialAction = false;
            holdingRopeObejuct = null;
        }

        if (other.gameObject.tag == "LightHitBox_Hide")
        {
            hiding = false;
        }
    }

    #endregion

    // Getter, Setter
    public bool GetPlayerIsOnGround()
    {
        return characterController.isGrounded;
    }

}

