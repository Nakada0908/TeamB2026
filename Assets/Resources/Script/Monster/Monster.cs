using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private BehaviourTree tree;
    private new Rigidbody rigidbody;
    private Animator animator;

    public GameObject player;

    public LayerMask groundMask;

    private const float wallMaxDistnace = 1f;
    private const float cliffMaxDistnace = 2.5f;
    private const float moveSpeed = 2f;

    private float calDistance = 10.0f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if(player == null )
        {
            //Player_main
            player = GameObject.Find("Player_main");
        }
    }

    private void Start()
    {
        tree = new BehaviourTree("Monster");

        // 同じ条件なら高い重要度の行動を選択します -> PrioritySelector
        PrioritySelector FollowPlayer = new PrioritySelector("FollowPlayer");

        //Sequence teleportToPlayerSQ = new Sequence("Teleport", 10);
        //teleportToPlayerSQ.AddChild(new Leaf("CanMove", new Condition(CanMoveByPlayerGround)));
        //teleportToPlayerSQ.AddChild(new Leaf("CanTeleport?", new Condition(CanTeleport)));
        //teleportToPlayerSQ.AddChild(new Leaf("Teleport", new TeleportToTarget(transform, player.transform, 10.0f)));
        //FollowPlayer.AddChild(teleportToPlayerSQ);

        Sequence moveBackSQ = new Sequence("MoveBack", 6);
        moveBackSQ.AddChild(new Leaf("IsRiskOfJump?", new Condition(IsRiskOfJump)));
        moveBackSQ.AddChild(new Leaf("MoveBack", new MoveBack(transform, moveSpeed)));
        moveBackSQ.AddChild(new Leaf("MoveAnimation", new ChangeAnimation(animator, "Move")));
        FollowPlayer.AddChild(moveBackSQ);

        Sequence jumpToPlayerSQ = new Sequence("JumpToPlayer", 5);
        jumpToPlayerSQ.AddChild(new Leaf("IsCollisionOnWall?", new Condition(IsCollisionOnWall)));
        jumpToPlayerSQ.AddChild(new Leaf("JumpAction", new JumpToTarget(transform, rigidbody)));
        FollowPlayer.AddChild(jumpToPlayerSQ);

        Sequence walkToPlayerSQ = new Sequence("Move", 2);
        walkToPlayerSQ.AddChild(new Leaf("RotateToPlayer", new RotateToTarget(transform, player.transform)));
        walkToPlayerSQ.AddChild(new Leaf("IsSafeToMove?", new Condition(IsSafeToMove)));
        walkToPlayerSQ.AddChild(new Leaf("MoveToPlayer", new MoveToTarget(transform, player.transform, moveSpeed, 1f)));
        FollowPlayer.AddChild(walkToPlayerSQ);

        Sequence idleSQ = new Sequence("Idle", 1);

        tree.AddChild(FollowPlayer);
    }

    private void Update()
    {
        tree.Process();

        // RayCastテスト用
        // Game Viewに Gizmo設定必要！

        // 壁チェック
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Vector3 collisionPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            Debug.DrawRay(collisionPos, transform.forward * wallMaxDistnace, Color.blue, 0.3f);
        }

        // 崖チェック
        if (Input.GetKeyDown(KeyCode.X))
        {
            float directionX = transform.forward.x >= 0 ? 1f : -1f;

            Vector3 collisionPos = new Vector3(transform.position.x + directionX, transform.position.y + 0.5f, transform.position.z);
            Debug.DrawRay(collisionPos, Vector3.down * cliffMaxDistnace, Color.green, 0.3f);
        }
    }

    #region CONDITION_CHECK

    bool CanTeleport()
    {
        calDistance = Mathf.Abs(transform.position.y - player.transform.position.y);

        if (calDistance >= 5f)
        {
            return true;
        }

        return false;
    }

    // 壁がモンスターの前にある場合ジャンプ状態にいなる
    bool IsCollisionOnWall()
    {
        Vector3 collisionPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        if (Physics.Raycast(collisionPos, transform.forward * wallMaxDistnace, 1.0f, groundMask))
        {
            return true;
        }

        return false;
    }

    bool IsRiskOfJump()
    {
        Vector3 collisionPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        if (Physics.Raycast(collisionPos, transform.forward * wallMaxDistnace, 0.5f, groundMask))
        {
            return true;
        }

        return false;
    }

    bool IsSafeToMove()
    {
        float directionX = transform.forward.x >= 0 ? 1f : -1f;

        Vector3 collisionPos = new Vector3(transform.position.x + directionX, transform.position.y + 0.5f, transform.position.z);

        if (Physics.Raycast(collisionPos, Vector3.down, cliffMaxDistnace, groundMask))
        {
            return true;
        }

        return false;
    }

    //bool CanMoveByPlayerGround()
    //{
    //    return player.GetComponent<PlayerManager>().GetPlayerIsOnGround();
    //}

    #endregion
}
