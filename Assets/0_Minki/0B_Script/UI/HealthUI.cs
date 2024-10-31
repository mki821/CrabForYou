using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Transform _hpUI;
    [SerializeField] private Image _hpUIPrefab;

    public void Init(int maxHealth) {
        for(int i = 0; i < maxHealth; ++i) {
            Instantiate(_hpUIPrefab, _hpUI);
        }
    }
    
    public void Damaged() {
        Destroy(_hpUI.GetChild(0).gameObject);
    }
}
