using System;
using UnityEngine;

namespace Ommy.Animation
{
    public class EventStateMachine : StateMachineBehaviour
    {
        public enum EventTimeType
        {
            NormalizedTime,
            AnimationTime,
            Frame
        }

        [Serializable]
        public class EventInfo
        {
            public bool invokeInUpdate;
            public bool invokeOnceIfLoop;
            public StateMachineEventType eventType;
            public EventTimeType eventTimeType;
            [Min(0)]
            public float eventTimeValue; // The value for normalized time, animation time, or frame
            [Header("Debugging")]
            public bool isInvoked;
            public bool logs;
        }

        public Vector2 statRandomSpeed = Vector2.one;
        public EventInfo[] eventInfos;

        private float animationFrameRate;

        public void DrawValue(AnimatorStateInfo stateInfo, Animator animator)
        {
            // Ensure the Animator has a valid frame rate
            if (animationFrameRate == 0)
                animationFrameRate = animator.runtimeAnimatorController.animationClips[0].frameRate;

            for (int i = 0; i < eventInfos.Length; i++)
            {
                var eventInfo = eventInfos[i];
                float normalizedTime = stateInfo.normalizedTime % 1;
                float animationTime = stateInfo.length * normalizedTime;
                int currentFrame = Mathf.FloorToInt(animationTime * animationFrameRate);

                bool shouldInvoke = false;

                // Determine invocation based on selected time type
                switch (eventInfo.eventTimeType)
                {
                    case EventTimeType.NormalizedTime:
                        shouldInvoke = normalizedTime >= eventInfo.eventTimeValue;
                        break;
                    case EventTimeType.AnimationTime:
                        shouldInvoke = animationTime >= eventInfo.eventTimeValue;
                        break;
                    case EventTimeType.Frame:
                        shouldInvoke = currentFrame >= Mathf.FloorToInt(eventInfo.eventTimeValue);
                        break;
                }

                if (shouldInvoke)
                {
                    if (!eventInfo.invokeInUpdate && eventInfo.isInvoked) continue;

                    animator.GetComponent<StateMachineEventListner>().OnEventInvoke.Invoke(eventInfo.eventType);
                    if (eventInfo.logs)
                        Debug.Log($"Event Invoked: {eventInfo.eventType} | Time Type: {eventInfo.eventTimeType} | Value: {eventInfo.eventTimeValue}");
                    eventInfo.isInvoked = true;
                }
            }
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.speed = UnityEngine.Random.Range(statRandomSpeed.x, statRandomSpeed.y);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            DrawValue(stateInfo, animator);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.speed = 1;
            foreach (var item in eventInfos)
            {
                item.isInvoked = false;
            }
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Implement code that processes and affects root motion
        }

        // OnStateIK is called right after Animator.OnAnimatorIK()
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Implement code that sets up animation IK (inverse kinematics)
        }
    }
}