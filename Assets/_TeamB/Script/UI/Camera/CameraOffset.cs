using UnityEngine;
using Unity.Cinemachine;

public class CameraOffset : MonoBehaviour
{
    /*[SerializeField] private float followOffsetMin = 5.0f;
    [SerializeField] private float followOffestMax = 10.0f;*/

    [SerializeField] private float lookAmout = 2.0f;  // 上下移動の最大量
    [SerializeField] private float smoothSpeed = 2.0f;  // カメラ移動のスムーズさ
    public CinemachineCamera vcam;  // 追従する仮想カメラ
    [SerializeField] private CinemachineCameraOffset camOffset;
    [SerializeField] private Vector3 followOffest;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //CinemachineCamera に CinemachineCameraOffset コンポーネントが付いているか確認
        camOffset = vcam.GetComponent<CinemachineCameraOffset>();
        if (camOffset == null)
        {
            // もし付いていなければ自動で追加する
            camOffset = vcam.gameObject.AddComponent<CinemachineCameraOffset>();
        }
        followOffest = camOffset.Offset;  // 初期値を現在のオフセットに設定
    }

    // Update is called once per frame
    void Update()
    {
        CameraLookAt();
    }

    private void CameraLookAt()
    {
        float moveY = 0.0f;  // 上下入力の値を保持する変数
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveY = lookAmout;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -lookAmout;
        }

        // 目標オフセットを計算
        followOffest = new Vector3(0, moveY, 0);
        // 現在のオフセットから目標オフセットへスムーズに補間
        camOffset.Offset = Vector3.Lerp(camOffset.Offset, followOffest, smoothSpeed * Time.deltaTime);
    }
}
