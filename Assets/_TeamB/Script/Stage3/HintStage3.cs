using TMPro;
using UnityEngine;

public class HintStage3 : MonoBehaviour
{
    public GameObject imageboxObject;
    public TextMeshProUGUI tmpText;
    public string itemText;

    void Start()
    {
        imageboxObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tmpText.text = itemText;
            imageboxObject.SetActive(true);
            Debug.Log("hit");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            imageboxObject.SetActive(false);
            Debug.Log("out");
        }
    }
}
