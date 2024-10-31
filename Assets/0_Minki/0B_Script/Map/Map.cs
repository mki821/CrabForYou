using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Portal _upPortal;
    [SerializeField] private Portal _downPortal;
    [SerializeField] private Portal _rightPortal;
    [SerializeField] private Portal _leftPortal;

    public void PortalOpenAll() {
        _upPortal.Open();
        _downPortal.Open();
        _rightPortal.Open();
        _leftPortal.Open();
    }
}
