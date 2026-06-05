using UnityEngine;

public class NuigurumiDrop : MonoBehaviour
{
    public GameObject nuigurumiPrefab;
    public Transform spawnPoint;

    public float moveSpeed = 2f;
    public float moveDistance = 1.5f;

    private Rigidbody rb;
    private Vector3 startPos;

    private bool spawned = false;
    private bool waitingForButton = false;

    public GameObject deleteOnButton;

    enum State
    {
        MoveForward,
        Falling
    }
    private State currentState;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (spawned) return;

        if (gameObject.name == "ButtonTrigger")
        {
            waitingForButton = true;
            Debug.Log("ボタン");
        }
        else
        {
            SpawnNuigurumi();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        waitingForButton = false;
    }

    void Update()
    {
        if (waitingForButton && !spawned)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) ||Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                if (deleteOnButton != null)
                {
                    Destroy(deleteOnButton);
                }

                SpawnNuigurumi();
                waitingForButton = false;
            }
        }

        if (rb == null) return;

        if (currentState == State.MoveForward)
        {
            rb.transform.position += rb.transform.forward * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(startPos, rb.transform.position) >= moveDistance)
            {
                rb.useGravity = true;
                currentState = State.Falling;
            }
        }
    }

    void SpawnNuigurumi()
    {
        spawned = true;

        GameObject obj = Instantiate(nuigurumiPrefab,spawnPoint.position,Quaternion.Euler(0f, 180f, 0f));

        rb = obj.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;

        obj.AddComponent<NuigurumiGroundChecker>();

        startPos = obj.transform.position;
        currentState = State.MoveForward;
    }
}
