using System.Collections.Generic;

namespace ProblemDevelopmentKit.Listener
{
    /// <summary>
    /// Base class of notifier for current listener.
    /// </summary>
    /// <typeparam name="T">Type of listener.</typeparam>
    public class Notifier<T> where T : IListener
    {
        private static List<T> listeners = new List<T>();

        /// <summary>
        /// Add given listener to listener collection if the collection does not contain it.
        /// </summary>
        /// <param name="listener">Listiner to be added.</param>
        public static void RegisterListener(T listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        /// <summary>
        /// Remove listener from listener collection if the collection contains it.
        /// </summary>
        /// <param name="listener">Listener to be removed.</param>
        public static void UnregisterListener(T listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        /// <summary>
        /// Get listener collection.
        /// </summary>
        /// <returns>Collection of registerd listeners.</returns>
        protected static List<T> GetListeners()
        {
            return listeners;
        }
    }
}
