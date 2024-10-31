using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
    [HideInInspector] public bool isDead = false;

    #region Components

    public SpriteRenderer SpriteRendererCompo { get; private set; }
    public Animator AnimatorCompo { get; private set; }
    public Rigidbody2D RigidbodyCompo { get; private set; }
    public Collider2D ColliderCompo { get; private set; }

    #endregion
    
    public int FacingDirection { get; protected set; } = 1;
    public bool CanStateChangeable { get; set; } = true;

    protected virtual void Awake() {
        Transform visaulTrm = transform.Find("Visual");
        SpriteRendererCompo = visaulTrm.GetComponent<SpriteRenderer>();
        AnimatorCompo = visaulTrm.GetComponent<Animator>();
        RigidbodyCompo = GetComponent<Rigidbody2D>();
        ColliderCompo = GetComponent<Collider2D>();
    }

    #region DelayCallbackCoroutine

    public Coroutine StartDelayCallback(float delayTime, Action callback) {
        return StartCoroutine(DelayCoroutine(delayTime, callback));
    }

    private IEnumerator DelayCoroutine(float delayTime, Action callback) {
        yield return new WaitForSeconds(delayTime);
        callback?.Invoke();
    }

    #endregion

    #region VelocityControl

    public void SetVelocity(float x, float y, bool doNotFlip = false) {
        RigidbodyCompo.linearVelocity = new Vector2(x, y);

        if(!doNotFlip) FlipController(x);
    }

    public void StopImmediately(bool withYAxis) {
        if(withYAxis) RigidbodyCompo.linearVelocity = Vector2.zero;
        else RigidbodyCompo.linearVelocity = new Vector2(0, RigidbodyCompo.linearVelocity.y);
    }

    #endregion

    #region FlipController

    public virtual void Flip() {
        FacingDirection *= -1;
        transform.rotation = Quaternion.Euler(0, 90f - 90f * FacingDirection, 0);
    }

    public virtual void FlipController(float x) {
        if(Mathf.Abs(x) < 0.05f) return;
        
        if(Mathf.Abs(FacingDirection + Mathf.Sign(x)) < 0.5f)
            Flip();
    }

    #endregion
}
