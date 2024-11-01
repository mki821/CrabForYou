using System.Collections;
using UnityEngine;

public class PlayerRope : MonoBehaviour
{
    public Vector2 anchorPosition;
    public Enemy targetEnemy;
    
    [SerializeField] private float _ropeDistance;
    [SerializeField] private float _ropeSpeed = 2f;
    [SerializeField] private Transform _grabTrm;
    [SerializeField] private LayerMask _whatIsRopeable;
    [SerializeField] private LayerMask _whatIsEnemy;

    private bool _isGrab = false;
    private Vector2 _originPosition;

    private Coroutine _coroutine = null;

    private Player _player;

    public void Initialize(Player player){
        _player = player;

        _originPosition = _grabTrm.localPosition;

        _player.Input.RopeEvent += Rope;
        _player.Input.RopeCancelEvent += RopeCancel;
    }

    private void OnDestroy() {
        _player.Input.RopeEvent -= Rope;
        _player.Input.RopeCancelEvent -= RopeCancel;
    }

    private void Rope() {
        Vector2 direction = _player.Input.MouseWorldPos - (Vector2)transform.position;
        float distance = Mathf.Min(direction.magnitude, _ropeDistance);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, _whatIsRopeable | _whatIsEnemy);

        if(hit.collider != null) {
            if((1 << hit.collider.gameObject.layer) == _whatIsRopeable.value) {
                anchorPosition = hit.point;

                if(_coroutine != null) StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(GrabCoroutine());
            }
            else if(hit.collider.TryGetComponent(out Enemy enemy)) {
                targetEnemy = enemy;
                
                if(_coroutine != null) StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(GrabCoroutineEnemy());
            }
        }
        else {
            if(_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(GrabCoroutine((Vector2)transform.position + direction));
        }
    }

    private IEnumerator GrabCoroutine() {
        Vector2 startPosition = (Vector2)transform.position + _originPosition;
        float timer = 0f;
        _grabTrm.SetParent(null);

        Vector2 direction = anchorPosition - startPosition;
        direction.Normalize();
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _grabTrm.rotation = Quaternion.Euler(0, 0, angle - 45f);

        while(timer < 1f) {
            timer += Time.deltaTime * 5f;

            Vector2 currentPosition = Vector2.Lerp(startPosition, anchorPosition, timer);
            _grabTrm.position = currentPosition;

            yield return null;
        }

        SoundManager.Instance.PlaySFX("GGANG");
        _isGrab = true;
        _player.StateMachine.ChangeState(PlayerStateEnum.Rope);
    }

    private IEnumerator GrabCoroutine(Vector2 targetPosition) {
        Vector2 startPosition = (Vector2)transform.position + _originPosition;
        float timer = 0f;
        _grabTrm.SetParent(null);

        Vector2 direction = targetPosition - startPosition;
        direction.Normalize();
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _grabTrm.rotation = Quaternion.Euler(0, 0, angle - 45f);

        while(timer < 1f) {
            timer += Time.deltaTime * 5f;

            Vector2 currentPosition = Vector2.Lerp(startPosition, targetPosition, timer);
            _grabTrm.position = currentPosition;

            yield return null;
        }
    }

    private IEnumerator GrabCoroutineEnemy() {
        Vector2 startPosition = (Vector2)transform.position + _originPosition;
        float timer = 0f;
        _grabTrm.SetParent(null);

        Vector2 direction = (Vector2)targetEnemy.transform.position - startPosition;
        direction.Normalize();
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _grabTrm.rotation = Quaternion.Euler(0, 0, angle - 45f);

        while(timer < 1f) {
            timer += Time.deltaTime * 5f;

            Vector2 currentPosition = Vector2.Lerp(startPosition, targetEnemy.transform.position, timer);
            _grabTrm.position = currentPosition;

            yield return null;
        }
        
        _isGrab = true;
        targetEnemy.Catched();

        startPosition = _grabTrm.position;
        timer = 0f;

        while(timer < 1f) {
            timer += Time.deltaTime * 0.5f;

            Vector2 currentPosition = Vector2.Lerp(startPosition, (Vector2)transform.position + _originPosition, timer);
            _grabTrm.position = currentPosition;
            targetEnemy.transform.position = currentPosition;

            if(!_isGrab) {
                Coroutine tempCoroutine = _coroutine;
                _coroutine = StartCoroutine(ComeBackCoroutine());
                StopCoroutine(tempCoroutine);
            }

            yield return null;
        }
        _grabTrm.SetParent(transform);
        _grabTrm.localPosition = _originPosition;
        _grabTrm.localRotation = Quaternion.identity;
    }

    private void RopeCancel() {
        _isGrab = false;

        if(_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(ComeBackCoroutine());

        if(targetEnemy != null)
            targetEnemy.isCatchCanceled = true;
    }

    private IEnumerator ComeBackCoroutine() {
        Vector2 startPosition = _grabTrm.position;
        float timer = 0f;

        while(timer < 1f) {
            timer += Time.deltaTime * 5f;

            Vector2 currentPosition = Vector2.Lerp(startPosition, (Vector2)transform.position + _originPosition, timer);
            _grabTrm.position = currentPosition;

            yield return null;
        }
        _grabTrm.SetParent(transform);
        _grabTrm.localPosition = _originPosition;
        _grabTrm.localRotation = Quaternion.identity;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _ropeDistance);
    }
}
