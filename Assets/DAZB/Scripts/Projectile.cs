using UnityEngine;

public class Projectile : MonoBehaviour {
    private Rigidbody2D rigidbodyCompo;

    private void Awake() {
        rigidbodyCompo = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 position, Vector2 direction, float power) {
        transform.position = position;
        rigidbodyCompo.AddForce(direction * power, ForceMode2D.Impulse);
    }
}