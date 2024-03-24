using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator m_PlayerAnimator;
    [SerializeField] private bool m_UseNewInputSystem;

    [SerializeField] private float m_speed;
    [SerializeField] private float m_JumpForce;

    [SerializeField] Rigidbody2D m_Rigidbody;
    private PlayerInput m_PlayerInput;
    private Vector2 m_MovementInput;
    private int m_AttackHash;
    private int m_IdleHash;
    private int m_RunHash;
    private int m_DyingHash;

    private void OnEnable()
    {
        if(m_PlayerInput == null)
        {
            m_PlayerInput = new PlayerInput();
            m_PlayerInput.Player.Movement.started += OnMovement;
            m_PlayerInput.Player.Movement.canceled += OnMovement;
            m_PlayerInput.Player.Movement.performed += OnMovement;
            m_PlayerInput.Player.Jump.started += OnJump;
            m_PlayerInput.Player.Jump.canceled += OnJump;
            m_PlayerInput.Player.Jump.performed += OnJump;
        }
        m_PlayerInput.Enable();
    }

    

    private void OnDisable()
    {
        if (m_PlayerInput != null)
        {
            m_PlayerInput.Disable();
        }
    }
    private void OnMovement(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(obj.started ||  obj.performed) { 
            m_MovementInput = obj.ReadValue<Vector2>();
        }
        else
            m_MovementInput = Vector2.zero;
    }
    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (obj.started || obj.performed)
        {
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_JumpForce);
        }
        else
            m_MovementInput = Vector2.zero;
    }


    private void Start()
    {
        m_AttackHash = Animator.StringToHash("Attack");
        m_IdleHash = Animator.StringToHash("Idle");
        m_RunHash = Animator.StringToHash("Run");
        m_DyingHash = Animator.StringToHash("Dying");

    }
    private void FixedUpdate()
    {
        checkMovement();
    }
    private void checkMovement()
    {
        m_Rigidbody.velocity = new Vector2(m_MovementInput.x * m_speed, m_Rigidbody.velocity.y);
        //check sang trai hay phai
        if(m_Rigidbody.velocity.x >= 0) {
            transform.localScale = new Vector3(5,5,5);
        }
        else transform.localScale = new Vector3(-5,5,5);

        if(m_Rigidbody.velocity.x != 0) PlayRunAnim();
        else PlayIdleAnim();
    }
    [ContextMenu("Play Attack Anim")]
    private void PlayAttackAnim()
    {
        m_PlayerAnimator.SetBool(m_AttackHash, true);
        m_PlayerAnimator.SetBool(m_RunHash, false);
        m_PlayerAnimator.SetBool(m_IdleHash, false);
    }
    [ContextMenu("Play Idle Anim")]
    private void PlayIdleAnim()
    {
        m_PlayerAnimator.SetBool(m_AttackHash, false);
        m_PlayerAnimator.SetBool(m_RunHash, false);
        m_PlayerAnimator.SetBool(m_IdleHash, true);
    }
    [ContextMenu("Play Run Anim")]
    private void PlayRunAnim()
    {
        m_PlayerAnimator.SetBool(m_AttackHash, false);
        m_PlayerAnimator.SetBool(m_RunHash, true);
        m_PlayerAnimator.SetBool(m_IdleHash, false);
    }
    [ContextMenu("Play Die Anim")]
    private void PlayDieAnim()
    {
        m_PlayerAnimator.SetBool(m_DyingHash, true);     
    }
    private void ResetAnim()
    { 
        m_PlayerAnimator.SetBool(m_AttackHash, false);
        m_PlayerAnimator.SetBool(m_RunHash, false);
        m_PlayerAnimator.SetBool(m_IdleHash, false);
        m_PlayerAnimator.SetBool(m_DyingHash, false);
    }

}
