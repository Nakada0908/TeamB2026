using UnityEngine;

//プレイヤーがインタラクトできる球体オブジェクトの処理クラス
//IInteractable インターフェースを実装しており、UI 表示やキー入力に対応
public class ItemCube : MonoBehaviour, IInteractable
{
    public KeyCode InteractKey => KeyCode.Z;//インタラクトに使用するキー
    public string itemText => "Press Z";//UI に表示されるインタラクト用のテキスト

    public void Interact(GameObject interactor)
    {
        gameObject.SetActive(false);//このオブジェクトを非表示にする
        Debug.Log("Interact");
    }
}
