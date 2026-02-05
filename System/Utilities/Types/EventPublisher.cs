using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skitter.Utilities.Types
{
    /// <summary> Allows for asynchronous execution of events. When an event is published, the publisher awaits the completion by all subscribers. </summary>
    public class EventPublisher
    {
        /// <summary> An array of all the event's subscribers. </summary>
        private readonly HashSet<Func<Task>> SUBSCRIBERS = new HashSet<Func<Task>>();


        /// <summary> Subscribe a callback to the publisher. </summary>
        /// <param name="action"> A pointer to the callback. </param>
        public void Subscribe(Func<Task> action) => SUBSCRIBERS.Add(action);


        /// <summary> Remove a callback from the publisher. </summary>
        /// <param name="action"> A pointer to the callback. </param>
        public void Unsubscribe(Func<Task> action) => SUBSCRIBERS.Remove(action);


        /// <summary> Invoke the event. Awaits for all the tasks to be complete before finishing. </summary>
        public async Task InvokeAsync()
        {
            // Execute all subscribers in parallel. Cache the tasks.
            IEnumerable<Task> tasks = SUBSCRIBERS.Select(s => s());
            await Task.WhenAll(tasks);
        }
    }
}
