using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class ReceiveEventExample : MonoBehaviour
{
    public const byte MoveUnitsToTargetPositionEventCode = 1;
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == MoveUnitsToTargetPositionEventCode)
        {
            //photonEvent.Sender;
            object[] data = (object[])photonEvent.CustomData;
            Vector3 targetPosition = (Vector3)data[0];
            for (int index = 1; index < data.Length; ++index)
            {
                int unitId = (int)data[index];
                //UnitList[unitId].TargetPosition = targetPosition;
            }
        }
    }
}

class SendEventExample
{
    // If you have multiple custom events, it is recommended to define them in the used class
    public const byte MoveUnitsToTargetPositionEventCode = 1;

    private void SendMoveUnitsToTargetPositionEvent()
    {
        object[] content = new object[] { new Vector3(10.0f, 2.0f, 5.0f), 1, 2, 5, 10 }; // Array contains the target position and the IDs of the selected units
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(MoveUnitsToTargetPositionEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
}