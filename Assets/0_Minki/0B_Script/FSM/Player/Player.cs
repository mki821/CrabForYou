using System;
using UnityEngine;

public enum PlayerStateEnum {
    Idle = 0, Move, Jump, Fall, Rope, Attack, Dead
}

public class Player : Entity, IDamageable
{
    [SerializeField] private InputReader _inputReader;

    [field: SerializeField] public PlayerStat Stat { get; private set; }
    private int _health;
    #region Settings

    [Header("Default Settings")]
    [SerializeField] private Vector2 _groundCheckOffset;
    [SerializeField] private Vector2 _groundCheckBoxSize;
    [SerializeField] private LayerMask _whatIsGround;

    #endregion

    public PlayerStateMachine StateMachine { get; private set; }
    public InputReader Input => _inputReader;

    public PlayerRope Rope { get; private set; }
    public Weapon Weapon { get; private set; }
    
    protected override void Awake() {
        base.Awake();

        PlayerManager.Instance.Player = this;
        PlayerManager.Instance.PlayerTrm = transform;

        //KJH code maybe conflict just merge
        Weapon = transform.Find("Weapon").GetComponent<Weapon>();
        _health = (int)Stat.maxHealth.GetValue();

        Rope = GetComponent<PlayerRope>();
        Rope.Initialize(this);
        
        GetComponent<PlayerPortal>().Initialize(this);

        StateMachine = new PlayerStateMachine();

        foreach(PlayerStateEnum stateEnum in Enum.GetValues(typeof(PlayerStateEnum))) {
            string typeName = stateEnum.ToString();

            try {
                Type t = Type.GetType($"Player{typeName}State");
                PlayerState state = Activator.CreateInstance(t, this, StateMachine, typeName) as PlayerState;

                StateMachine.AddState(stateEnum, state);
            }
            catch(Exception ex) {
                Debug.LogError($"[PlayerState] {typeName} is loading error");
                Debug.LogError(ex.Message);
            }
        }
    }

    private void Start() {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }

    private void Update() {
        StateMachine.CurrentState.UpdateState();
    }

    public bool IsDetecteGround() {
        return Physics2D.OverlapBox(transform.position + (Vector3)_groundCheckOffset, _groundCheckBoxSize, 0, _whatIsGround);
    }

    public void AnimationEndTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)_groundCheckOffset, _groundCheckBoxSize);
    }

    public void ChangeWeapon(Weapon weapon)
    {
        Weapon = weapon;
    }

    public void ApplyDamage(int amount)
    {
        return;
    }

    public void ApplyDamage()
    {
        _health--;
        if (_health <= 0) 
            StateMachine.ChangeState(PlayerStateEnum.Dead);
    }
}
