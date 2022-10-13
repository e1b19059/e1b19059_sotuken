using Photon.Pun;
using Photon.Realtime;

public class ObjectContainerChild : MonoBehaviourPunCallbacks
{
    public Player Owner => photonView.Owner;

    public override void OnEnable()
    {
        base.OnEnable();

        var container = FindObjectOfType<ObjectContainer>();
        if (container != null)
        {
            transform.SetParent(container.transform);
        }
    }
}