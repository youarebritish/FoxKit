namespace FoxKit.Modules.RouteBuilder.Exporter
{
    /// <summary>
    /// /// <summary>
    /// Collection of helper functions for constructing RouteEvents.
    /// </summary>
    /// </summary>
    public static class EventFactory
    {
        /// <summary>
        /// Delegate to create a RouteEevnt.
        /// </summary>
        /// <param name="data">Parameters of the RouteEvent to construct.</param>
        /// <returns></returns>
        public delegate FoxLib.Tpp.RouteSet.RouteEvent CreateEventDelegate(RouteEvent data);

        /// <summary>
        /// Delegate to get the StrCode32 hash of a RouteEvent's type.
        /// </summary>
        /// <param name="event">Event whose type's hash to get.</param>
        /// <returns>StrCode32 hash of the RouteEvent's type.</returns>
        public delegate uint GetEventTypeHashDelegate(RouteEvent @event);        

        /// <summary>
        /// Create a function to create RouteEvents.
        /// </summary>
        /// <param name="getEventTypeHash">Function to get the StrCode32 hash of a RouteEvent's type.</param>
        /// <returns>Function to construct RouteEvents.</returns>
        public static CreateEventDelegate CreateFactory(GetEventTypeHashDelegate getEventTypeHash)
        {
            return (data) => Create(data, getEventTypeHash);
        }

        /// <summary>
        /// Create a RouteEvent.
        /// </summary>
        /// <param name="data">Parameters of the RouteEvent to construct.</param>
        /// <param name="getEventTypeHash">Function to get the StrCode32 hash of a RouteEvent's type.</param>
        /// <returns>The constructed RouteEvent.</returns>
        private static FoxLib.Tpp.RouteSet.RouteEvent Create(RouteEvent data, GetEventTypeHashDelegate getEventTypeHash)
        {
            return new FoxLib.Tpp.RouteSet.RouteEvent(
                getEventTypeHash(data),
                data.Params[0],
                data.Params[1],
                data.Params[2],
                data.Params[3],
                data.Params[4],
                data.Params[5],
                data.Params[6],
                data.Params[7],
                data.Params[8],
                data.Params[9],
                data.Snippet
                );
        }
    }
}