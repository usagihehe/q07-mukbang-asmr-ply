namespace Capyboba.Mukbang
{
    /// <summary>State of the current bite/sip animation. Interrupted = replaced by another animation, so it never completes.</summary>
    public enum BiteStatus
    {
        Playing,
        Completed,
        Interrupted
    }
}
