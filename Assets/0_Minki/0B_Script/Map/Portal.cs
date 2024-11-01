using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal connectedPortal;
    public Map map;

    private bool _isOpened = true;


    public void Init(Map map) {
        this.map = map;
        gameObject.SetActive(false);
    }

    public void Open() {
        _isOpened = true;
        gameObject.SetActive(true);
    }

    public void Close() {
        _isOpened = false;
        gameObject.SetActive(false);
    }

    public void Use(Transform trm) {
        if(_isOpened) {
            trm.position = connectedPortal.transform.position;
            MapManager.Instance.SetPlayerPosition(connectedPortal.map.mapPosition);
            connectedPortal.map.EnterPlayer();
        }
    }
}
