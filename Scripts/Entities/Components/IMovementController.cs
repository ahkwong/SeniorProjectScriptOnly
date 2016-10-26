using UnityEngine;

namespace Assets.Scripts.Entities.Components
{
    public interface IMovementController
    {
        bool RotateBasedOnMovement { get; set; }
        bool OnlyRotate { get; set; }
        bool CanMove { get; set; }
        Vector3 MoveDirection { get; set; }

        Vector3 LastMoveDirection { get; set; }

        void Start();
        void Move();
        void FixedUpdate();
        void Update();
        void AnimationEvent(string eventName);

    }
}