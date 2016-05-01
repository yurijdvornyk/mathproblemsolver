using ProblemLibrary.Listener;

namespace ProblemLibrary.Progress
{
    /// <summary>
    /// Implement this interface to listen to problem solving progress.
    /// </summary>
    public interface ISolvingProgressListener: IListener
    {
        /// <summary>
        /// Enable or disable progress show.
        /// </summary>
        /// <param name="isEnabled">"true" if progress mode should be enabled, "false" - if not.</param>
        void SetProgressModeEnabled(bool isEnabled);

        /// <summary>
        /// Set value to progress indicator if progress mode is enabled.
        /// </summary>
        /// <param name="percent">Progress value in percent (between 0 and 100).</param>
        void SetProgress(double percent);
    }
}
