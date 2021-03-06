﻿using ProblemDevelopmentKit.Listener;

namespace ProblemDevelopmentKit.Progress
{
    /// <summary>
    /// Manages the behavior of ISolutionProgressListener listeners.
    /// </summary>
    public class ProblemProgressNotifier: Notifier<ISolutionProgressListener>
    {
        /// <summary>
        /// Set progress mode.
        /// </summary>
        /// <param name="isEnabled">If "true" then display the progress in percent, otherwise just show that solving is in progress.</param>
        public static void SetProgressModeEnabled(bool isEnabled)
        {
            foreach (var listener in GetListeners())
            {
                listener.SetProgressModeEnabled(isEnabled);
            }
        }

        /// <summary>
        /// If progress mode is enabled, show the progress of problem solving.
        /// </summary>
        /// <param name="percent">Problem solving progress in percent.</param>
        public static void SetProgress(double percent)
        {
            foreach (var listener in GetListeners())
            {
                listener.SetProgress(percent);
            }
        }
    }
}
