using UnityEngine;

//プレイヤーがインタラクトできる球体オブジェクトの処理クラス
//IInteractable インターフェースを実装しており、UI 表示やキー入力に対応
public class ItemSphere : MonoBehaviour,IInteractable
{
    public KeyCode InteractKey => KeyCode.X;//インタラクトに使用するキー
    public string itemText => "Press X";//UI に表示されるインタラクト用のテキスト

    //インタラクト時に呼び出される処理
    //interactor はこのオブジェクトと接触してインタラクトを実行したプレイヤーなど
    public void Interact(GameObject interactor)
    {
        gameObject.SetActive(false);//このオブジェクトを非表示にする
        Debug.Log("Interact");
    }
}
