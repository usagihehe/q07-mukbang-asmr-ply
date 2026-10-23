using UnityEngine;

namespace Capyboba.Mukbang
{
    /// <summary>Decides the eater's next state from the held item, its distance to the mouth and the current bite.</summary>
    public sealed class EaterBrain
    {
        // Leaving BeforeEat needs a bit more distance than entering it, so a hand resting on the
        // edge does not flip Idle <-> Mouth_Open every frame.
        private const float LeaveBeforeEatFactor = 1.2f;

        private readonly HeldItemTracker _tracker;
        private readonly IBiteAnimator _animator;
        private readonly float _distanceBeforeEat;
        private readonly float _distanceEating;

        public EaterBrain(HeldItemTracker tracker, IBiteAnimator animator, float distanceBeforeEat, float distanceEating)
        {
            _tracker = tracker;
            _animator = animator;
            _distanceBeforeEat = distanceBeforeEat;
            _distanceEating = distanceEating;
        }

        public IEdible Held => _tracker.Held;

        /// <summary>Item of the latest bite/sip; not replaced when the player picks up another item.</summary>
        public IEdible Eaten { get; private set; }

        public bool IsChewing { get; private set; }

        public EmotionType EatenEmotion => Eaten?.GetEmotion() ?? EmotionType.Happy;

        public void Reset()
        {
            Eaten = null;
            IsChewing = false;
        }

        public EaterState TickIdle(Vector3 mouth)
        {
            IEdible held = Held;
            return held != null && Distance(mouth, held) < _distanceBeforeEat ? EaterState.BeforeEat : EaterState.Idle;
        }

        public EaterState TickBeforeEat(Vector3 mouth)
        {
            IEdible held = Held;
            if (held == null) return EaterState.Idle;
            if (Distance(mouth, held) < _distanceEating) return EaterState.Eating;
            return HeldState(mouth, held);
        }

        public void BeginEating()
        {
            IEdible held = Held;
            if (held != null) StartBite(held);
        }

        public EaterState TickEating(Vector3 mouth, BiteStatus status)
        {
            IEdible held = Held;
            if (IsChewing && status == BiteStatus.Playing)
            {
                // Cup moved away from the mouth: the sip stops right away and costs nothing.
                if (Eaten.Style == EatStyle.Sip && held == Eaten && Distance(mouth, Eaten) >= _distanceEating)
                {
                    IsChewing = false;
                    return HeldState(mouth, held);
                }
                return EaterState.Eating;
            }
            if (IsChewing)
            {
                IsChewing = false;
                if (status == BiteStatus.Completed && Eaten.Style == EatStyle.Sip) Eaten.Consume();
                held = Held;
            }
            // Bite again in the same frame: going back through Idle cuts the Spine mix and makes the next bite snap.
            if (held != null && Distance(mouth, held) < _distanceEating)
            {
                StartBite(held);
                return EaterState.Eating;
            }
            return held != null ? HeldState(mouth, held) : EaterState.AfterEat;
        }

        private void StartBite(IEdible item)
        {
            Eaten = item;
            IsChewing = true;
            _animator.PlayBite(item);
            if (item.Style == EatStyle.Sip) return;
            item.Consume();
            _tracker.OnBitten(item);
        }

        private EaterState HeldState(Vector3 mouth, IEdible held)
        {
            return Distance(mouth, held) < _distanceBeforeEat * LeaveBeforeEatFactor ? EaterState.BeforeEat : EaterState.Idle;
        }

        private static float Distance(Vector3 mouth, IEdible item)
        {
            return Vector3.Distance(mouth, item.EatPoint);
        }
    }
}
