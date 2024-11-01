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
        SoundManager.Instance.PlaySFX("UICLICK");
        Application.Quit();
    }

    public void StartGameBtn()
    {
        SoundManager.Instance.PlaySFX("UICLICK");
        Sequence seq = DOTween.Sequence();
        _crab.SetActive(true);
        seq.Append(_crab.transform.DOScale(new Vector2(5, 5), 1f).SetEase(Ease.InQuad));
        seq.AppendCallback(() => {
            SoundManager.Instance.PlaySFX("GGANG");
        });
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => {
            _leftHand.transform.parent = null;
            _rightHand.transform.parent = null;
        });
        seq.Join(_leftHand.transform.DOMove(new Vector2(-5.12f, 2.92f), 1.1f).SetEase(Ease.InElastic));
        seq.AppendCallback(() =>
        {
            SoundManager.Instance.PlaySFX("SHOOK");
        });
        seq.AppendInterval(1f);
        seq.AppendCallback(() => {
            SoundManager.Instance.PlaySFX("DRILLWIING");
        });
        seq.AppendInterval(0.1f);
        seq.Join(_rightHand.transform.DOMove(new Vector2(5.28f, 4.53f), 1.5f).SetEase(Ease.Linear));
        seq.Join(_rightHand.transform.DORotate(Vector3.zero, 0.8f));
        seq.AppendInterval(1.5f);
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() => SceneManager.LoadScene(1));
    }
}
