using UnityEngine;

public class PlayerRope : MonoBehaviour
{
    public Vector2 anchorPosition;
    // public Enemy targetEnemy;
    
    [SerializeField] private float _ropeDistance;
    [SerializeField] private float _ropeSpeed = 2f;
    [SerializeField] private LayerMask _whatIsRopeable;
    [SerializeField] private LayerMask _whatIsEnemy;

    private bool _isGrab = false;

    private LineRenderer _lineRenderer;

    private Player _player;

    private void Awake() {
        _lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    public void Initialize(Player player){
        _player = player;

        _player.Input.RopeEvent += Rope;
        _player.Input.RopeCancelEvent += RopeCancel;
    }

    private void OnDestroy() {
        _player.Input.RopeEvent -= Rope;
        _player.Input.RopeCancelEvent -= RopeCancel;
    }

    private void Update() {
        if(_isGrab) {
            _lineRenderer.SetPosition(0, _player.transform.position);
            _lineRenderer.SetPosition(1, anchorPosition);
        }
    }

    private void Rope() {
        Vector2 direction = _player.Input.MouseWorldPos - (Vector2)transform.position;
        float distance = Mathf.Min(direction.magnitude, _ropeDistance);

        direction.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, _whatIsRopeable | _whatIsEnemy);

        if(hit.collider != null) {
            if((1 << hit.collider.gameObject.layer) == _whatIsRopeable.value) {
                anchorPosition = hit.point;
                _isGrab = true;
                
                _player.StateMachine.ChangeState(PlayerStateEnum.Rope);
            }
            // else if(hit.collider.TryGetComponent(out Enemy enemy)) {
                // targetEnemy = enemy;
            // }
        }
    }

    private void RopeCancel() {
        _isGrab = false;
        _lineRenderer.SetPosition(0, Vector2.zero);
        _lineRenderer.SetPosition(1, Vector2.zero);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _ropeDistance);
    }
}
