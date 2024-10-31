using UnityEngine;

public class Portal : MonoBehaviour
{
    private bool _isOpened = true;

    public void Open() {
        _isOpened = true;
    }

    public void Close() {
        _isOpened = false;
    }
}
