using UnityEngine;

//インタラクト可能なオブジェクトが実装すべきインターフェース
public interface IInteractable
{
    KeyCode InteractKey { get; }//インタラクトに使用するキー
    string itemText { get; }//UI に表示するインタラクト用のテキスト
    void Interact(GameObject interactor);//インタラクト処理本体。interactor はインタラクトを実行したオブジェクト（通常はプレイヤー）
}

