using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    public GameObject imageboxObject;//UI's image box
    //public Text imageboxText;//text of image box
    public TextMeshProUGUI tmpText;

    public IInteractable CurrentInteractable { get; private set; }//現在プレイヤーが接触しているインタラクト可能なオブジェクト
    private bool IsPlayerHit; //プレイヤーがインタラクト範囲内にいるかどうかのフラグ

    public UIFollow UIFollow { get; private set; }

    private void Awake()
    {
        UIFollow = imageboxObject.GetComponent<UIFollow>();// imageboxObject にアタッチされている UIFollow コンポーネントを取得
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //インタラクト対象が存在し、かつ対応キーが押された場合にインタラクト処理を実行
        if (CurrentInteractable != null && Input.GetKeyDown(CurrentInteractable.InteractKey))
        {
            CurrentInteractable.Interact(gameObject);
            ClearInteractionUI();//UIを非表示、状態をリセット
        }
    }

    #region Trigger
    private void OnTriggerEnter(Collider other)//トリガーに他のオブジェクトが入ったときに呼ばれる
    {
        CurrentInteractable = other.GetComponent<IInteractable>();//接触したオブジェクトが IInteractable を実装しているか確認
        if (CurrentInteractable != null)
        {
            //UIを表示、テキストを設定
            imageboxObject.SetActive(true);
            tmpText.text = CurrentInteractable.itemText;
            IsPlayerHit = true;
            Debug.Log("hit");

            //UI follow item
            UIFollow.targetTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)//トリガーからオブジェクトが出たときに呼ばれる
    {
        //現在のインタラクト対象と一致する場合のみ処理
        if (other.GetComponent<IInteractable>() == CurrentInteractable)
        {
            ClearInteractionUI();//UIを非表示、状態をリセット
            Debug.Log("out");
        }
    }
    #endregion

    private void ClearInteractionUI()//インタラクト後にUIを非表示にし、状態をリセット
    {
        imageboxObject.SetActive(false);
        tmpText.text = "";
        CurrentInteractable = null;
        IsPlayerHit = false;
    }
}
