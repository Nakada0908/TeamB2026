using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIText : MonoBehaviour
{
    public GameObject imageboxObject;//UI's image box
    public Text imageboxText;//text of image box
    public string itemText;
    public TextMeshProUGUI tmpText;

    private bool IsPlayerHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(IsPlayerHit==true)//&&Input.GetKeyDown(KeyCode.Z)
        {
            imageboxObject.SetActive(false);//hide UI
            //gameObject.SetActive(false);//hide object
            Debug.Log("canInteraction");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //imageboxText.text = itemText;
            tmpText.text = itemText;
            imageboxObject.SetActive(true);
            IsPlayerHit = true;
            Debug.Log("hit");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            imageboxObject.SetActive(false);
            IsPlayerHit = false;
            Debug.Log("out");
        }
    }
}
