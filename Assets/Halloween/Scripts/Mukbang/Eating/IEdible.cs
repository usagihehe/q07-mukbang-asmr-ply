using UnityEngine;

namespace Capyboba.Mukbang
{
    /// <summary>An item a character can eat; the item itself declares how it is eaten.</summary>
    public interface IEdible
    {
        EatStyle Style { get; }
        EaterGroup Group { get; }
        Vector3 EatPoint { get; }
        SoundType EatSound { get; }
        bool IsDone { get; }
        EmotionType GetEmotion();
        void Consume();
    }
}
