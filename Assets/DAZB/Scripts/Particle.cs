using UnityEngine;

public class Particle : MonoBehaviour {
    public void Destroy(float delayTime) {
        Destroy(gameObject, delayTime);
    }
}