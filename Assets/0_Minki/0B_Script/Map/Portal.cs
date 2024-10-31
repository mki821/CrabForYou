using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal connectedPortal;

    private bool _isOpened = true;

    public void Open() {
        _isOpened = true;
    }

    public void Close() {
        _isOpened = false;
    }

    public void Use(Transform trm) {
        if(_isOpened)
            trm.position = connectedPortal.transform.position;
    }
}
