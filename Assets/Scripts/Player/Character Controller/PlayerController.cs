using System.Collections;
// using System.Diagnostics;
// using System.Numerics;
using UnityEngine;

namespace KinematicCharacterController
{
    public class PlayerController : MonoBehaviour, ICharacterController
    {
        [Header("Mouse Look")]
        [SerializeField] private MouseLook mouseLook;

        [Header("Stable Movement")]
        public float MaxStableMoveSpeed = 10f;
        public float StableMovementSharpness = 15;
        public float OrientationSharpness = 10;

        [Header("Air Movement")]
        public float jumpStrength;
        public float AirAccelerationSpeed = 5f;
        public float AirControl;
        public float MaxVelocity;
        public float Drag = 0.1f;
        
        [Header("Jump")]
        public AnimationCurve landCurve;
        public float landDuration;
        public float landThreshold;
        private float landTimer;
        private bool hasLanded;
        private float velocityMagnitude;

        [Header("Misc")]
        [SerializeField] private LayerMask groundMask;
        public bool RotationObstruction;
        public Vector3 Gravity = new Vector3(0, -30f, 0);
        public float FallAcceleration;
        public float MaxAcceleration;
        private Vector3 gravityStorage;

        private Vector3 moveInputVector;
        private Vector3 lookInputVector;

        private float xMovement;
        private float yMovement;

        private float forward;
        private float sideways;

        private bool jumpRequested = false;
        private bool jumpConsumed = false;

        private bool _wasGrounded;
        private Vector3 _velocityLeavingGround;

        public static Vector3 playerVelocity;
        private Vector3 currentGravity;
        
        //New Refactored State Machine Stuff
        public KinematicCharacterMotor Motor;

        public struct ControllerBlackboard
        {
            public Vector3 CurrentVelocity;
            public Vector3 LastGroundVelocity;
            public Vector3 Gravity;
        };

        public ControllerBlackboard blackboard;

        private void Start()
        {
            Motor.CharacterController = this;
            gravityStorage = Gravity;
            blackboard = new ControllerBlackboard();
        }

        public Vector3 GetMoveInputVector()
        {
            //Recieve Movement Input
            xMovement = Input.GetAxisRaw("Horizontal");
            yMovement = Input.GetAxisRaw("Vertical");

            //Recieve Camera Orientation
            Quaternion CameraRotation = Quaternion.Euler(mouseLook.rotX, mouseLook.rotY, 0.0f);

            //Project Orientation on Normal Vector
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(CameraRotation * Vector3.up, Motor.CharacterUp).normalized;
            }

            //Send Input Vector Out
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);
            Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(xMovement, 0f, yMovement), 1f);
            moveInputVector = cameraPlanarRotation * moveInputVector;

            forward = MathHelpers.ExpDecay(forward, moveInputVector.z, 6, Time.deltaTime);
            sideways = MathHelpers.ExpDecay(sideways, moveInputVector.x, 6, Time.deltaTime);

            return moveInputVector;
        }
        
        // private void Update()
        // {
        //     // Motor.SetCapsuleHeight(MathHelpers.ExpDecay(Motor.GetCapsuleHeight(), standHeight, standSpeed, Time.deltaTime));

        //     StateMachine.HandleInput();

        //     cooldowns.Update(Time.deltaTime);
        // }

        private void LateUpdate()
        {
            // StateMachine.LateUpdate(Time.deltaTime);
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            // This is called before the motor does anything
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            // This is called when the motor wants to know what its rotation should be right now
            currentRotation = Quaternion.Euler(0, mouseLook.rotY, 0);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            var inputVector = GetMoveInputVector();
            // grounding.UpdateGrounding(Motor.GroundingStatus, currentVelocity, ref landOffset);

            //Left Ground, change to airborne state.
            if (!Motor.GroundingStatus.IsStableOnGround)
            {
                blackboard.LastGroundVelocity = currentVelocity;
                if (inputVector.sqrMagnitude > 0f)
                {
                    Vector3 targetMovementVelocity = new Vector3(blackboard.LastGroundVelocity.x + (inputVector.x * AirControl), 0, blackboard.LastGroundVelocity.z + (inputVector.z * AirControl));

                    // Prevent climbing on un-stable slopes with air movement
                    if (Motor.GroundingStatus.FoundAnyGround)
                    {
                        Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                        targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                    }

                    Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, currentGravity);
                    currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
                }

                // Accelerate gravity downward over time
                currentGravity -= Vector3.up * (FallAcceleration * deltaTime);
                currentGravity = Vector3.ClampMagnitude(currentGravity, MaxAcceleration);
                
                // Apply gravity to velocity
                currentVelocity += currentGravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (Drag * deltaTime)));
            }
            
            //Player is on the ground
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                // Reset gravity to default when grounded
                currentGravity = Vector3.zero;

                // Reorient current velocity to slope without losing magnitude
                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

                // Calculate slope-aware move direction
                Vector3 moveDir = Motor.GetDirectionTangentToSurface(inputVector.normalized, Motor.GroundingStatus.GroundNormal);

                // Target velocity along slope
                Vector3 targetVelocity = moveDir * MaxStableMoveSpeed;

                // Smoothly interpolate velocity toward target
                currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 1 - Mathf.Exp(-StableMovementSharpness * deltaTime));
            }

            //Recieve Jump Input
            if (jumpRequested)
            {
                jumpRequested = false;
            }

            playerVelocity = currentVelocity;
        }
        
        

        public void AfterCharacterUpdate(float deltaTime)
        {
            // This is called after the motor has finished everything in its update
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            // This is called after when the motor wants to know if the collider can be collided with (or if we just go through it)
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            // This is called when the motor's ground probing detects a ground hit
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            // This is called when the motor's movement logic detects a hit
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            // This is called after every hit detected in the motor, to give you a chance to modify the HitStabilityReport any way you want
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            // This is called after the motor has finished its ground probing, but before PhysicsMover/Velocity/etc.... handling
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            // This is called by the motor when it is detecting a collision that did not result from a "movement hit".
        }
    
    }
}