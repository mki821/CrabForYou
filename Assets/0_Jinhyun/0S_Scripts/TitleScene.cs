using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private GameObject _crab;
    [SerializeField] private GameObject _leftHand;
    [SerializeField] private GameObject _rightHand;

    public void QuitGameBtn()
    {
        Application.Quit();
    }

    public void StartGameBtn()
    {
        Sequence seq = DOTween.Sequence();
        _crab.SetActive(true);
        seq.Append(_crab.transform.DOScale(new Vector2(5, 5), 1f).SetEase(Ease.InQuad));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() =>
        {
            _leftHand.transform.parent = null;
            _rightHand.transform.parent = null;
        });
        seq.AppendCallback(() =>
        {
            print(null);
            //SoundManager.Instance.PlayOneShot("DDAKDDAK.wav");
        });
        seq.Append(_leftHand.transform.DOMove(new Vector2(-5.12f, 2.92f), 1.1f).SetEase(Ease.InElastic));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() =>
        {
            print(null);
            //SoundManager.Instance.PlayOneShot("DRILLWIING.wav");
        });
        seq.Append(_rightHand.transform.DOMove(new Vector2(5.28f, 4.53f), 1.5f).SetEase(Ease.Linear));
        seq.Join(_rightHand.transform.DORotate(Vector3.zero, 0.8f));
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() => SceneManager.LoadScene(1));
    }
}
