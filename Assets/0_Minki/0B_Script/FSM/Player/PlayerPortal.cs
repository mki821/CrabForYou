using UnityEngine;

public class PlayerPortal : MonoBehaviour
{
    private Player _player;

    public void Initialize(Player player) {
        _player = player;

        _player.Input.UpEvent += UsePortal;
    }

    private void OnDestroy() {
        _player.Input.UpEvent -= UsePortal;
    }

    private void UsePortal() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        
        for(int i = 0; i < colliders.Length; ++i) {
            if(colliders[i].TryGetComponent(out Portal portal)) {
                portal.Use(_player.transform);
            }
        }
    }
}
