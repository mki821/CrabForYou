using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJumpScare : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image _jumpScare;

    public void Bomb() {
        _jumpScare.sprite = _sprites[Random.Range(0, _sprites.Length)];
        _jumpScare.color = Color.white;
        _jumpScare.gameObject.SetActive(true);

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut() {
        float timer = 0f;
        yield return new WaitForSeconds(0.1f);

        while(timer < 1f) {
            timer += 2f * Time.deltaTime;

            _jumpScare.color = new Color(1f, 1f, 1f, 1f - timer);

            yield return null;
        }
        _jumpScare.gameObject.SetActive(false);
    }
}
