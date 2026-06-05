using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonHint : MonoBehaviour
{
    //public Transform targetTransform;
    //public Vector3 offset = new Vector3(0, 2.0f, 0);

    public GameObject imageboxObject;//UI's image box
    public TextMeshProUGUI tmpText;//text of image box
    public string itemText;

    private bool isHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
