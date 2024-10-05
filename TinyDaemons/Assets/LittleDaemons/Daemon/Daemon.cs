using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
public class Daemon : MonoBehaviour
{

    #region Internals, Set up in unity
    [SerializeField]
    internal SelectionCircle selectionCircle;
    [SerializeField]
    internal DaemonBody body;
    #endregion

    #region Stats
    public int Health = 60;
    public float WalkSpeed = 1;
    public float RotateSpeed = 15;
    #endregion

    #region Internal State
    private DaemonGame game;
    private Daemon enemy;
    #endregion

    #region Publicly Visible State;
    #endregion

    // TODO EnemyTypes

    // TODO AllyTypes

    #region Actions
    [FormerlySerializedAs("OnIdle2")]
    public DaemonActionList OnIdle;

    public List<DaemonAction> OnFoundEnemy;

    public List<DaemonAction> OnSeeEnemy;

    public List<DaemonAction> OnCloseToEnemy;

    public List<DaemonAction> OnAttack;

    public List<DaemonAction> OnAttackMelee;

    public List<DaemonAction> OnCollision;

    public List<DaemonAction> OnDie;
    #endregion

    public enum DaemonState
    {
        Idle,
        // Patrolling, no need. Idle covers a patrol
        Hunting,
        Attacking,
        HandleCollision,
        Melee,
        Hurt,

        // DEADBEEF
        Dead
    }

    public enum InterruptState
    {
        // Interrupts
        EnemySpotted, // Or enemy target?
        CollidedWithWall,
        CollidedWithDaemon,
        CollidedWithEnemy,
        CollidedWithHurt,
    }

    private HashSet<InterruptState> unhandledInterrupts = new HashSet<InterruptState>();

    public DaemonState activeState;

    public Coroutine activeStateMachine;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<DaemonGame>();

        KickTheStateMachine();
    }

    private void KickTheStateMachine()
    {
        if (activeStateMachine != null)
        {
            StopCoroutine(activeStateMachine);
        }
        body.animator.StopPlayback();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        activeStateMachine = StartCoroutine(HackyStateMachine());
    }

    IEnumerator HackyStateMachine()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (activeState == DaemonState.Dead)
            {
                break;
            }

            if (Health <= 0)
            {
                yield return DoListOfActions(OnDie);
                activeState = DaemonState.Dead;
                continue;
            }

            CheckUnhandledInterrupts();

            // TODO Enemy detection bits

            // TODO Collision Handling bits
            if (DaemonState.HandleCollision == activeState)
            {
                yield return DoListOfActions(OnCollision);
                if (DaemonState.HandleCollision == activeState)
                {
                    activeState = DaemonState.Idle;
                }
            }

            else if (DaemonState.Idle == activeState)
            {
                yield return OnIdle.DoListOfActions(this);
                // Will loop forever, waiting for interrupts
            }
            else
            {
                Debug.LogWarning($"Tried handling a situation, but failed state={activeState}. Switching to Idle");
                activeState = DaemonState.Idle;
            }
        }

        // TODO Explode into bits.
    }

    IEnumerator DoListOfActions(List<DaemonAction> actions)
    {
        foreach (DaemonAction action in actions)
        {
            yield return action.PerformAction(this);
        }
    }

    private void Interrupt(InterruptState interruptState)
    {
        bool IsValidInterrupt = false;
        DaemonState newDaemonState = DaemonState.Idle;
        switch (interruptState)
        {
            case InterruptState.CollidedWithWall:
                IsValidInterrupt = (activeState == DaemonState.Hunting);
                newDaemonState = DaemonState.HandleCollision;
                break;
        }

        if (IsValidInterrupt)
        {
            unhandledInterrupts.Remove(interruptState);
            activeState = newDaemonState;
            KickTheStateMachine();
        }
        else
        {
            unhandledInterrupts.Add(interruptState);
        }
    }

    private void CheckUnhandledInterrupts()
    {
        if (unhandledInterrupts.Count > 0)
        {
            // Force another interrupt to see if things change
            InterruptState unhandled = unhandledInterrupts.First();
            Interrupt(unhandled);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            Vector3 knockBack = Vector3.Normalize(collision.impulse) * 0.5f;

            GetComponent<Rigidbody>().AddForce(knockBack, ForceMode.Impulse);
            
            Debug.Log("Bumped into wall.");
            Interrupt(InterruptState.CollidedWithWall);
        }

        if (collision.collider.tag == "Daemon")
        {
            Vector3 knockBack = Vector3.Normalize(collision.impulse) * 0.7f;

            GetComponent<Rigidbody>().AddForce(knockBack, ForceMode.Impulse);

            Debug.Log("Bumped into Daemon.");
            Interrupt(InterruptState.CollidedWithWall);
        }

    }

    private void OnMouseUpAsButton()
    {
        game.SelectDaemon(this);
    }

    private void OnMouseEnter()
    {
        selectionCircle.Hover();
    }

    private void OnMouseExit()
    {
        selectionCircle.Unhover();
    }

}
