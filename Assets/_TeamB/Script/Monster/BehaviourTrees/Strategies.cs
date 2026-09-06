using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using static Node;

public interface IStrategy
{
    Node.EStatus Process();

    void Reset()
    {
        // Noop
    }
}

public class ActionStrategy : IStrategy
{
    readonly Action doSomething;

    public ActionStrategy(Action doSomething)
    {
        this.doSomething = doSomething;
    }

    public Node.EStatus Process()
    {
        doSomething();
        return Node.EStatus.Success;
    }
}

public class Condition : IStrategy
{
    readonly Func<bool> predicate;

    public Condition(Func<bool> predicate)
    {
        this.predicate = predicate;
    }

    public Node.EStatus Process() => predicate() ? Node.EStatus.Success : Node.EStatus.Failure;
}

//ナビメッシュの例題
public class PatrolStrategy : IStrategy
{
    readonly Transform entity;
    readonly NavMeshAgent agent;
    readonly List<Transform> patrolPoints;
    readonly float patrolSpeed;
    int currentIndex;
    bool isPathCalculated;

    public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
    {
        this.entity = entity;
        this.agent = agent;
        this.patrolPoints = patrolPoints;
        this.patrolSpeed = patrolSpeed;
    }

    public Node.EStatus Process()
    {
        if (currentIndex == patrolPoints.Count) return Node.EStatus.Success;

        var target = patrolPoints[currentIndex];
        agent.SetDestination(target.position);
        //entity.LookAt(target.position.With(y: entity.position.y));

        if (isPathCalculated && agent.remainingDistance < 0.1f)
        {
            currentIndex++;
            isPathCalculated = false;
        }

        if (agent.pathPending)
        {
            isPathCalculated = true;
        }

        return Node.EStatus.Running;
    }

    public void Reset() => currentIndex = 0;
}

#region MONSTER_STRATEGIES

//Yに大きい差がある場合
public class TeleportToTarget : IStrategy
{
    readonly Transform entity;
    readonly Transform target;
    private float limitdistance;

    public TeleportToTarget(Transform entity, Transform target, float limitdistance)
    {
        this.entity = entity;
        this.target = target;
        this.limitdistance = limitdistance;
    }

    public Node.EStatus Process()
    {
        entity.position = new Vector3(target.position.x, target.position.y+ 0.5f, entity.position.z);
        return Node.EStatus.Success;
    }
}
public class RotateToTarget : IStrategy
{
    readonly Transform entity;
    readonly Transform target;

    Quaternion targetRotation;

    public RotateToTarget(Transform entity, Transform target)
    {
        this.entity = entity;
        this.target = target;
    }

    public Node.EStatus Process()
    {
        float directionX = Mathf.Sign(target.position.x - entity.position.x);

        //回転
        if (directionX != 0)
        {
            Vector3 lookDirection = new Vector3(directionX, 0f, 0f);
            targetRotation = Quaternion.LookRotation(lookDirection);

            if (Quaternion.Dot(entity.rotation, targetRotation) < 0.999f)
            {
                entity.rotation = Quaternion.Lerp(entity.rotation, targetRotation, Time.deltaTime * 10f);
                return Node.EStatus.Running;
            }
        }

        return Node.EStatus.Success;
    }
}

public class MoveToTarget : IStrategy
{
    readonly Transform entity;
    readonly Transform target;
    private float moveSpeed;
    private float stoppingDistance;

    Quaternion targetRotation;

    public MoveToTarget(Transform entity, Transform target, float moveSpeed, float stoppingDistance)
    {
        this.entity = entity;
        this.target = target;
        this.moveSpeed = moveSpeed;
        this.stoppingDistance = stoppingDistance;
    }

    public Node.EStatus Process()
    {
        float distanceX = Mathf.Abs(entity.position.x - target.position.x);

        //プレイヤーと近くなると終了
        if (distanceX < stoppingDistance)
        {
            return Node.EStatus.Success;
        }

        float directionX = Mathf.Sign(target.position.x - entity.position.x);

        Vector3 finalPosition = new Vector3(directionX * moveSpeed * Time.deltaTime, 0f, 0f);
        entity.position += new Vector3(finalPosition.x, 0f, 0f);

        return Node.EStatus.Running;
    }
}

// ジャンプする前に壁と近すぎの場合後ろに移動
public class MoveBack : IStrategy
{
    readonly Transform entity;
    private float moveSpeed;
    private float backDistance;

    private float directionX;
    private float distanceX;
    private Vector3 startPosition;
    private Quaternion targetRotation;

    private bool isInitialSetting;

    public MoveBack(Transform entity, float moveSpeed, float backDistance = 1.2f)
    {
        this.entity = entity;
        this.moveSpeed = moveSpeed;
        this.backDistance = backDistance;
        isInitialSetting = false;
    }

    public Node.EStatus Process()
    {
        if(!isInitialSetting)
        {
            startPosition = entity.position;

            directionX = Mathf.Sign(entity.forward.x) * -1f;
            Vector3 lookDirection = new Vector3(directionX, 0f, 0f);
            targetRotation = Quaternion.LookRotation(lookDirection);

            distanceX = Mathf.Abs(entity.position.x - backDistance);
            isInitialSetting = true;
        }

        if (Quaternion.Dot(entity.rotation, targetRotation) < 0.999f)
        {
            entity.rotation = Quaternion.Lerp(entity.rotation, targetRotation, Time.deltaTime * 10f);

            return Node.EStatus.Running;
        }

        Vector3 offset = new Vector3(directionX * moveSpeed * Time.deltaTime, 0f, 0f);
        entity.position += offset;

        if (Vector3.Distance(startPosition, entity.position) >= backDistance)
        {
            isInitialSetting = false;
            return EStatus.Success;
        }

        return EStatus.Running;
    }
}

public class JumpToTarget : IStrategy
{
    readonly Rigidbody rigidbody;
    readonly Transform entity;

    bool isJumping = false;
    private float direction = 0.0f;

    private float jumpForward;
    private float jumpHeight;

    public JumpToTarget(Transform entity,Rigidbody rigidbody, float jumpForward =3f, float jumpHeight = 6f)
    {
        this.entity = entity;
        this.rigidbody = rigidbody;
        this.jumpForward = jumpForward;
        this.jumpHeight = jumpHeight;
    }

    public Node.EStatus Process()
    {
        if (!isJumping)
        {
           
            isJumping = true;

            float entityRotation = entity.rotation.y;

            if (entityRotation > 0)
            {
                direction = 1f;
            }
            else
            {
                direction = -1f;
            }

            rigidbody.linearVelocity = new Vector2(direction * jumpForward, jumpHeight); 
        }

        if (rigidbody.linearVelocity.y == 0 && isJumping)
        {
            isJumping = false;
            return Node.EStatus.Success;
        }

        return Node.EStatus.Running;
    }
}

public class Idle : IStrategy
{
    readonly Rigidbody rigidbody;
    readonly Transform entity;

    public Idle(Transform entity)
    {
        this.entity = entity; 
    }

    public Node.EStatus Process()
    { 
        return Node.EStatus.Success;
    }
}

public class ChangeAnimation : IStrategy
{
    readonly Animator animator;
    private string animName;
    private string currentAnimName;
    private float conversionTime;

    public ChangeAnimation(Animator animator, string animName, float conversionTime = 0.1f)
    {
        this.animator = animator;
        this.animName = animName;
        currentAnimName = " ";
        this.conversionTime = conversionTime;
    }

    public Node.EStatus Process()
    {
        // 同じアニメーションならSuccessで終了処理
        if (currentAnimName == animName)
        {
            return Node.EStatus.Success;
        }

        animator.CrossFade(animName, conversionTime);
        //animator.Play(animName);
        return Node.EStatus.Success;
    }
}

#endregion
