using System.Collections;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public Transform playerViewTransform;
    public Transform bossViewTransform;
    public float moveSpeed = 3.0f;
    public float waiteTime = 2.0f;
    private bool isSwitching = false;

    public void TriggerBossFocus()
    {
        if (!isSwitching)
        {
            StartCoroutine(CameraBossFocus());
        }
    }

    IEnumerator CameraBossFocus()
    {
        isSwitching = true;
        yield return StartCoroutine(MoveCameraTo(bossViewTransform));
        yield return new WaitForSeconds(waiteTime);
        yield return StartCoroutine(MoveCameraTo(playerViewTransform));

        isSwitching = false;
    }
    IEnumerator MoveCameraTo(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.05f ||
            Quaternion.Angle(transform.rotation, target.rotation) > 1.0f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
