using UnityEngine;
using Photon.Pun;

public class NewBehaviourScript : MonoBehaviour
{
    private PhotonView PV;
    public GameObject Stats;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            GetComponent<Bot>().enabled = false;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Stats.transform.LookAt(FirstPersonController.Instance.cam);
        Stats.transform.localRotation = new Quaternion(Stats.transform.localRotation.x, 0, Stats.transform.localRotation.z, 0);
    }
}
