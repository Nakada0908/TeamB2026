using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraEventSequencer : MonoBehaviour
{
    [Header("カメラの割り当て")]
    [Header("デフォルトのカメラ")]
    public CinemachineCamera defaultCamera;
    [Header("切り替え後のカメラ")]
    public CinemachineCamera eventCamera;

    [Space(10)]
    [Header("カメラ設定")]
    [Header("アニメーターで制御するかどうか")]
    public bool isAnimator;
    [SerializeField]
    private Animator animator;
    [Header("アニメーターを使わない移動")]
    [Header("初期カメラ位置（表示のみ）")]
    [SerializeField]
    private Vector3 defaultPos;
    [Header("カメラの終了位置")]
    public Vector3 afterPos;
    [Header("移動にかかる時間")]
    public float moveTimer;
    [Header("移動開始までの時間")]
    public float startTimer;
    [Header("移動終了後、カメラを戻すまでの時間")]
    public float endTimer;
    private void Start()
    {
        eventCamera.gameObject.SetActive(false);
        defaultPos = eventCamera.transform.position;
        //アニメーターが存在するなら、アニメーターを取得
        if (eventCamera.gameObject.GetComponent<Animator>() != null)
        {
            animator = eventCamera.gameObject.GetComponent<Animator>();
        }
        //アニメーターを使わないなら外す
        if (isAnimator == false)
        {
            if (eventCamera.gameObject.GetComponent<Animator>() != null)
            {
                animator.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //カメラオブジェクトの切り替え
            defaultCamera.gameObject.SetActive(false);
            eventCamera.gameObject.SetActive(true);
            //イベント開始
            //アニメーターを使わない場合
            if (isAnimator == false)
            {
                StartCoroutine(MoveCameraPos());
            }
            //アニメーターを使う場合
            else
            {
                MoveCameraAnim();
            }

        }
    }

    //カメラを移動させる（アニメーターなし）
    public IEnumerator MoveCameraPos()
    {
        //開始まで待つ
        yield return new WaitForSeconds(startTimer);

        if (isAnimator == true) yield break;

        float elapsed = 0f;
        Vector3 startPos = defaultPos;

        //moveTimer秒移動するやつ
        while (elapsed < moveTimer)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveTimer);
            eventCamera.transform.position = Vector3.Lerp(startPos,afterPos, t);
            yield return null;
        }

        eventCamera.transform.position = afterPos;
        //カメラ切り替えまで待つ
        yield return new WaitForSeconds(endTimer);
        defaultCamera.gameObject.SetActive(true);
        eventCamera.gameObject.SetActive(false);
    }

    //カメラを移動させる（アニメーターを使用）
    public void MoveCameraAnim()
    {
        animator.SetTrigger("Start");
    }

    //アニメーションが終了したら、カメラを戻す
    private void Update()
    {
        if (isAnimator == true)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("MoveCamera") && state.normalizedTime >= 1.0f)
            {
                defaultCamera.gameObject.SetActive(true);
                eventCamera.gameObject.SetActive(false);
            }
        }
    }
}
