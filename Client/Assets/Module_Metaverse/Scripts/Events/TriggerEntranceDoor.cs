using Fusion;
using UnityEngine;

public class TriggerEntranceDoor : MonoBehaviour
{
    [SerializeField] private Animator EntranceDoor = null;

    public int membersInside = 0;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.FindInstance();
    }

    /// <summary>
    /// Call GameManger.RPC_OpenDoor to detect if there is more than one user in the area and open the doors.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<NetworkPlayer>().ActorID == gameManager.GetRunner().LocalPlayer.PlayerId) 
        {
            GameManager.RPC_OpenDoor(gameManager.GetRunner());
        }
        
    }

    /// <summary>
    /// Call GameManger.RPC_CloseDoor to detect if there is no user in the area and close the doors.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<NetworkPlayer>().ActorID == gameManager.GetRunner().LocalPlayer.PlayerId)
        {
            GameManager.RPC_CloseDoor(gameManager.GetRunner());
        }

    }


    public void OpenDoor()
    {
        EntranceDoor.Play("OfficeEntranceGlassDoor", 0, 0.0f);
    }

    public void CloseDoor()
    {
        EntranceDoor.Play("OfficeEntranceGlassDoorInverse", 0, 0.0f);
    }

}