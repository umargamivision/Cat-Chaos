using System.Security.Cryptography;
using Cinemachine;
using Ommy.Audio;
using UnityEngine;
public class StateManager : MonoBehaviour
{
    public PlayerController playerController;
    public Animator animator;
    public IState idle, walk, jump, attack, underAttack, death;
    public IState currentState;
    public IState defaultState;
    public const string WalkParam = "isWalking";
    public const string JumpParam = "jump";
    public const string AttackParam = "push";
    public const string GrabParam = "grab";
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip walkSound;
    public AudioClip deathSound;
    private void Start()
    {
        // Initialize states
        underAttack = new UnderAttackState(this); 
        death = new DeathState(this); 
        idle = new IdleState(this);
        walk = new WalkState(this);
        jump = new JumpState(this);
        attack = new AttackState(this);
        defaultState = idle;
        // Set initial state
        SetDefaultState();
    }
    public void ChangeState(IState newState)
    {
        if (currentState == newState) return;
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter();
            Debug.Log($"Changed to state: {currentState.GetType().Name}");
        }
        else
        {
            Debug.LogError("Attempted to change to a null state!");
        }
    }
    public void SetDefaultState()
    {
        if (currentState != null) currentState.Exit();
        currentState = defaultState;
        currentState.Enter();
    }
    public virtual void Update()
    {
        currentState?.Update();
    }

    public virtual void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }
    public class IdleState : IState
    {
        private readonly StateManager stateManager;

        public IdleState(StateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public void Enter()
        {
            Debug.Log("Entering Idle State");
            stateManager.animator.SetBool(StateManager.WalkParam, false);
        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {
            // Handle physics logic, if any
        }

        public void Exit()
        {
            Debug.Log("Exiting Idle State");
        }
    }

    public class WalkState : IState
    {
        private readonly StateManager stateManager;

        public WalkState(StateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public void Enter()
        {
            Debug.Log("Entering Walk State");
            AudioManager.Instance?.PlaySFX(stateManager.walkSound);
            stateManager.animator.SetBool(StateManager.WalkParam, true);
        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {
            // Apply running physics logic
        }

        public void Exit()
        {
            Debug.Log("Exiting Run State");
        }
    }

    public class JumpState : IState
    {
        private readonly StateManager stateManager;
        public JumpState(StateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        public void Enter()
        {
            Debug.Log("Entering Jump State");
            AudioManager.Instance?.PlaySFX(stateManager.jumpSound);
            stateManager.animator.SetTrigger(StateManager.JumpParam);
            // Add jump force
        }
        public void Update()
        {
        }
        public void FixedUpdate()
        {
        }
        public void Exit()
        {
            Debug.Log("Exiting Jump State");
        }
        private bool IsGrounded()
        {
            // Replace with actual grounded logic
            return Physics.Raycast(stateManager.transform.position, Vector3.down, 0.1f);
        }
    }
    public class UnderAttackState : IState
    {
        private readonly StateManager stateManager;

        public UnderAttackState(StateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void FixedUpdate()
        {
        }

        public void Update()
        {
        }
    }
    public class AttackState : IState
    {
        private readonly StateManager stateManager;

        public AttackState(StateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public void Enter()
        {
            Debug.Log("Entering Attack State");
            AudioManager.Instance?.PlaySFX(stateManager.attackSound);
            stateManager.animator.SetTrigger(StateManager.AttackParam);
            // Trigger attack animation
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void Exit()
        {
            Debug.Log("Exiting Attack State");
        }
    }
    public class DeathState : IState
    {
        private readonly StateManager stateManager;

        public DeathState(StateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public void Enter()
        {
            AudioManager.Instance?.PlaySFX(stateManager.deathSound);
            Debug.Log("Entering Death State");
            stateManager.animator.speed=0;
            // Trigger attack animation
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void Exit()
        {
            Debug.Log("Exiting Death State");
        }
    }
}