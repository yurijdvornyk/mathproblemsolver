using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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