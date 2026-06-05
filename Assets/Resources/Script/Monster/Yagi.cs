using UnityEngine;

public enum EYagiState
{
    Idle,
    Walk,
    Run,
    Attack,
    Death,
    None
}

public class Yagi : MonoBehaviour
{
    EYagiState currentState;

    private float walkMoveSpeed = 0.7f;
    private float runMoveSpeed = 3.0f;
    [SerializeField] private Animator animator;
    [SerializeField] private string currentAnimName;
    [SerializeField] private GameObject attackCollider;

    private const float attackColliderTimer = 2.5f;
    private const float attackTimer = 4.0f;
    private float checkTimer = 0.0f;

    private bool isEnter = false;

    #region Unity Method

    private void Start()
    {
        ChangeStatus(EYagiState.Walk, "Walk");

        if (attackCollider)
        {
            attackCollider.SetActive(false);
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case EYagiState.Idle:
                Idle();
                break;
            case EYagiState.Walk:
                Walk();
                break;
            case EYagiState.Run:
                Run();
                break;
            case EYagiState.Attack:
                Attack();
                break;
            case EYagiState.Death:
                Death();
                break;
            case EYagiState.None:
                break;
            default:
                break;
        }
    }

    #endregion

    #region Stat
    private void Idle()
    {
        if (!isEnter)
        {
            isEnter = true;
        }
    }

    private void Walk()
    {
        if (!isEnter)
        {
            isEnter = true;
        }

        walkMoveSpeed = 2.3f;
        Vector3 move = new Vector3(walkMoveSpeed * Time.deltaTime, 0f, 0f);
        transform.position += move;
    }

    private void Run()
    {
        if (!isEnter)
        {
            isEnter = true;
        }
        walkMoveSpeed = 4.0f;
        Vector3 move = new Vector3(runMoveSpeed * Time.deltaTime, 0f, 0f);
        transform.position += move;
    }

    private void Attack()
    {
        // AttackでObject壊す
        if (!isEnter)
        {
            checkTimer = 0.0f;
            isEnter = true;
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySFXSound("Monster");
            }
        }

        checkTimer += Time.deltaTime;
        if (checkTimer >= attackColliderTimer)
        {
            attackCollider.SetActive(true);
        }

        if (checkTimer >= attackTimer)
        {
            attackCollider.SetActive(false);
            ChangeStatus(EYagiState.Walk, "Walk");
        }

    }

    private void Death()
    {
        if (!isEnter)
        {
            isEnter = true;
        }

    }
    #endregion

    public void Reset()
    {

    }

    public void ChangeStatus(EYagiState yagiState, string animName, float conversionTime = 0.1f)
    {
        if (yagiState != currentState)
        {
            currentState = yagiState;
            ChangeAnimation(animName, conversionTime);
            isEnter = false;
        }
    }

    private void ChangeAnimation(string animName, float conversionTime = 0.1f)
    {
        if (currentAnimName != animName)
        {
            currentAnimName = animName;
            animator.CrossFade(animName, conversionTime);
        }
    }

}
