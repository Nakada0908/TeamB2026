using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineFocus : MonoBehaviour
{
    private bool hasTrigger = false;//if it has already happened
    public CinemachineCamera virtualCameraOne;//Player's view
    public CinemachineCamera virtualCameraTwo;//Boss's view
    public float viewDuration = 3.0f;//time on Boss

    public GameObject yagi;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTrigger == true || !other.CompareTag("Player")) return;//if has done then return
        hasTrigger = true;

        virtualCameraTwo.Priority = 20;//change to the currently active camera
        virtualCameraOne.Priority = 5;//

        var brain = Camera.main.GetComponent<CinemachineBrain>();//get the CinemachineBrain
        OnCameraCut(brain);//show the object under the camera(now)
        StartCoroutine(ReturnToPlayer(brain));

        GetComponent<Collider>().enabled = false;//fall the collider to stop the second play
    }

    public void OnCameraCut(CinemachineBrain brain)
    {
        if (brain.ActiveVirtualCamera.Name != "VirtualCameraOne")//if the currently active camera isn't the "V~One"(name) then hide the object(change the view)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    private IEnumerator ReturnToPlayer(CinemachineBrain brain)
    {
        yield return new WaitForSeconds(viewDuration);//wait for the time then go back to Player's view

        virtualCameraOne.Priority = 20;//change to the currently active camera
        virtualCameraTwo.Priority = 5;
        OnCameraCut(brain);//update the camera / return to the origina state
        if (yagi)
        {
            yagi.GetComponent<Yagi>().ChangeStatus(EYagiState.Walk, "Walk");
        }
    }
}
