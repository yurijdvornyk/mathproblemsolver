namespace ProblemLibrary.Listener
{
    /// <summary>
    /// Base interface for listeners.
    /// </summary>
    public interface IListener
    {
        /// <summary>
        /// Listener filter. Use this filter, if you want to listen to only specific event.
        /// </summary>
        int Filter { get; set; }
    }
}