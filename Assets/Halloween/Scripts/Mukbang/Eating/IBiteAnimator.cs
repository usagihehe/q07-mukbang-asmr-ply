namespace Capyboba.Mukbang
{
    /// <summary>Plays the animation and sound of one bite/sip.</summary>
    public interface IBiteAnimator
    {
        void PlayBite(IEdible item);
    }
}
