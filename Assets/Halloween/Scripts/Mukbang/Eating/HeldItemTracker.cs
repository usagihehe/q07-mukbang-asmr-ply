using System;

namespace Capyboba.Mukbang
{
    /// <summary>Tracks the item the player is holding, from OnSelectItem / OnDropItem.</summary>
    public sealed class HeldItemTracker
    {
        private readonly Func<IEdible, bool> _accepts;
        private IEdible _held;

        public HeldItemTracker(Func<IEdible, bool> accepts)
        {
            _accepts = accepts;
        }

        /// <summary>Null when nothing is held or the held item is finished.</summary>
        public IEdible Held => _held != null && !_held.IsDone ? _held : null;

        /// <summary>Other draggables (e.g. ScanItem) and items of other eaters share the topic; they are ignored.</summary>
        public void OnSelect(object data)
        {
            if (data is IEdible item && _accepts(item)) _held = item;
        }

        /// <summary>Only a drop of the held item itself releases it.</summary>
        public void OnDrop(object data)
        {
            if (ReferenceEquals(data, _held)) _held = null;
        }

        /// <summary>A Spoon item is no longer held after one bite (its tool hides).</summary>
        public void OnBitten(IEdible item)
        {
            if (item == _held && item.Style == EatStyle.Spoon) _held = null;
        }

        public void Clear()
        {
            _held = null;
        }
    }
}
