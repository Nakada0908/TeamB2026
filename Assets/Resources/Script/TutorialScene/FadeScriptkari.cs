using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeScriptkari : MonoBehaviour
{
    public float speed = 0.01f;         // フェード速度
    private float alfa;                 // α値
    private float red, green, blue;     // RGB
    private bool isFadingOut = false;   // 暗転中
    private bool isFadingIn = false;    // 明転中
    private bool isSceneReloading = false;

    [SerializeField] private Transform player;
    [SerializeField] private float fallThreshold = -1000f; // 落下判定高さ

    void Start()
    {
        // 現在のImage色取得
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;

        // シーンロード時は明転スタート
        alfa = 1f;
        isFadingIn = true;
        GetComponent<Image>().color = new Color(red, green, blue, alfa);
    }

    void Update()
    {
        // 明転処理（シーン開始時）
        if (isFadingIn)
        {
            alfa -= speed;
            GetComponent<Image>().color = new Color(red, green, blue, alfa);

            if (alfa <= 0f)
            {
                alfa = 0f;
                isFadingIn = false;
            }
            return;
        }

        // フェードアウト（落下時）
        if (isFadingOut)
        {
            alfa += speed;
            GetComponent<Image>().color = new Color(red, green, blue, alfa);

            if (alfa >= 1f)
            {
                alfa = 1f;
                isFadingOut = false;
                if (!isSceneReloading)
                {
                    isSceneReloading = true;
                    ReloadScene();
                }
            }
            return;
        }

        // 落下検知
        //if (!isFadingOut && !isFadingIn && player.position.y < fallThreshold)
        //{
        //    isFadingOut = true;
        //}
    }

    private void ReloadScene()
    {
        // 現在のシーン名を取得して再ロード
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}
