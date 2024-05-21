using UnityEngine;
using StatePattern.StateMachine;
using StatePattern.Player;
using System;

namespace StatePattern.Enemy
{
    public class OnePunchManController : EnemyController
    {
        private bool isIdle;
        private bool isRotating;
        private bool isActive;
        private bool isShooting;
        private float shootTimer;

        private float idleTimer;
        private float targetRotation;
        private PlayerController target;
        private OnePunchManStateMachine stateMachine;

        public OnePunchManController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            enemyView.SetController(this);
            InitializeVariable();
            CreateStateMachine();
            stateMachine.ChangeState(States.IDLE);
        }

        private void InitializeVariable()
        {
            isIdle = true;
            isRotating = false;
            isActive = false;
            isShooting = false;
            idleTimer = enemyScriptableObject.IdleTime;
        }

        private void CreateStateMachine() => stateMachine = new OnePunchManStateMachine(this);

        //Override of the base class method for updating the enemy's behavior
        public override void UpdateEnemy()
        {
            //Check if the enemy is in a deactivated state, and if so , return without futher processing
            if (!isActive)
            {
                return;
            }
            //if the enemy is in an idle state, decrement the idle timer.
            if (isIdle)
            {
                idleTimer -= Time.deltaTime;
            }
            //Check if the idle timer has elapsed, indicating the end of the idle state.
            if (idleTimer <= 0)
            {
                isIdle = false;
                isRotating = true;
                //Calculate the target rotation to face the opposite direction (180 degrees).
                targetRotation = (Rotation.eulerAngles.y + 180) % 360;
            }
            // if the enemy is rotating, set its rotation and check if the rotation is completed.
            if (isRotating)
            {
                SetRotation(CalculateRotation());
                //Check if the rotation is complete.
                if (IsRotatingComplete())
                {
                    isIdle = true;
                    isRotating = false;
                    ResetTimer();
                }
            }
            if (isShooting)
            {
                //Calculate the desired rotation to face the player.
                Quaternion desiredRotation = CalculateRotationTowardsPlayer();

                // Set the enemy's rotation towards the player
                SetRotation(RotateTowards(desiredRotation));

                //Check if the enemy is facing the player.
                if (IsFacingPlayer(desiredRotation))
                {
                    shootTimer -= Time.deltaTime;
                }
                if (shootTimer <= 0)
                {
                    shootTimer = enemyScriptableObject.RateOfFire;
                    Shoot();
                }
            }
            if (currentState == EnemyState.DEACTIVE)
                return;

            stateMachine.Update();
        }

        private bool IsFacingPlayer(Quaternion desiredRotation)
        {
            throw new NotImplementedException();
        }

        private Vector3 RotateTowards(Quaternion desiredRotation)
        {
            throw new NotImplementedException();
        }

        private Quaternion CalculateRotationTowardsPlayer()
        {
            throw new NotImplementedException();
        }

        private void ResetTimer()
        {
            throw new NotImplementedException();
        }

        private bool IsRotatingComplete()
        {
            throw new NotImplementedException();
        }

        private Vector3 CalculateRotation()
        {
            throw new NotImplementedException();
        }

        public override void PlayerEnteredRange(PlayerController targetToSet)
        {
            base.PlayerEnteredRange(targetToSet);
            isIdle = false;
            isRotating = false;
            isShooting = true;
            target = targetToSet;
            shootTimer = 0;
            stateMachine.ChangeState(States.SHOOTING);
        }

        public override void PlayerExitedRange()
        {
            stateMachine.ChangeState(States.IDLE);
            isShooting = false;
            isRotating = false;
            isIdle = true;

        }
    }
}