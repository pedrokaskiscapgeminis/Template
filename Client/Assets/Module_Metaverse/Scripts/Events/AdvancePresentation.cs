using UnityEngine;
using Fusion;
using Manager;
namespace PresentationModule
{
    /// <summary>
    /// Event to activate the slice advance in the presentation
    /// </summary>
    public class AdvancePresentation : NetworkBehaviour, IMetaEvent
    {
        public Presentation presentation;

        GameObject _eventObject;
        PhotonManager photonManager;
        GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }

        public void activate(bool host)
        {
            photonManager = PhotonManager.FindInstance();
            RPCManager.RPC_AdvancePress(photonManager.Runner);
        }
    }
}
