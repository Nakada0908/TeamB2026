using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorTestScript : MonoBehaviour
{
  
    [Header("床の設定")]
    public float fallDelay = 0.5f;        

    [Header("トリガーの設定")]
    public GameObject pracutreTrigger;

    private bool playerTrigger = false;
    private bool moveableTrigger = false;

    private void Update()
    {
        StartFallingInStage1();
    }

    private void OnTriggerEnter(Collider other)
    {

        if(SceneManager.GetActiveScene().name == "Stage1Color")
        {
            if (other.CompareTag("Player"))
            {
                playerTrigger = true;
            }

            if (other.CompareTag("moveable"))
            {
                moveableTrigger = true;
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("触れた" + other.name);
                Invoke(nameof(StartFalling), fallDelay);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (SceneManager.GetActiveScene().name == "Stage1Color")
        {
            if (other.CompareTag("Player"))
            {
                playerTrigger = false;
            }

            if (other.CompareTag("moveable"))
            {
                moveableTrigger = false;
            }
        }
    }

    private void StartFallingInStage1()
    {
        if(playerTrigger && moveableTrigger)
        {
            Invoke(nameof(StartFalling), fallDelay);
        }
    }

    private void StartFalling()
    {
        pracutreTrigger.SetActive(true);
    }

    public void PlaySound()
    {//破壊音
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFXSound("Destroy");
        }
    }
}
