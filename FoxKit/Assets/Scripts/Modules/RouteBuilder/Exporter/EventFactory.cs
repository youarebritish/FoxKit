using UnityEngine.Assertions;

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
        /// Delegate to get the StrCode32 hash of a RouteNodeEvent's type.
        /// </summary>
        /// <param name="event">Event whose type's hash to get.</param>
        /// <returns>StrCode32 hash of the RouteEvent's type.</returns>
        public delegate uint GetNodeEventTypeHashDelegate(RouteNodeEvent @event);

        /// <summary>
        /// Delegate to get the StrCode32 hash of a RouteEdgeEvent's type.
        /// </summary>
        /// <param name="event">Event whose type's hash to get.</param>
        /// <returns>StrCode32 hash of the RouteEvent's type.</returns>
        public delegate uint GetEdgeEventTypeHashDelegate(RouteEdgeEvent @event);

        /// <summary>
        /// Create a function to create RouteEvents.
        /// </summary>
        /// <param name="getEventTypeHash">Function to get the StrCode32 hash of a RouteEvent's type.</param>
        /// <returns>Function to construct RouteEvents.</returns>
        public static CreateEventDelegate CreateFactory(GetNodeEventTypeHashDelegate getNodeTypeHash, GetEdgeEventTypeHashDelegate getEdgeTypeHash)
        {
            return (data) => Create(data, getNodeTypeHash, getEdgeTypeHash);
        }

        /// <summary>
        /// Create a RouteEvent.
        /// </summary>
        /// <param name="data">Parameters of the RouteEvent to construct.</param>
        /// <param name="getEventTypeHash">Function to get the StrCode32 hash of a RouteEvent's type.</param>
        /// <returns>The constructed RouteEvent.</returns>
        private static FoxLib.Tpp.RouteSet.RouteEvent Create(RouteEvent data, GetNodeEventTypeHashDelegate getNodeTypeHash, GetEdgeEventTypeHashDelegate getEdgeTypeHash)
        {
            // TODO: Refactor this to not use type checking.
            uint eventTypeHash = uint.MaxValue;
            if (data is RouteNodeEvent)
            {
                eventTypeHash = getNodeTypeHash((data as RouteNodeEvent));
            }
            else if (data is RouteEdgeEvent)
            {
                eventTypeHash = getEdgeTypeHash((data as RouteEdgeEvent));
            }
            else
            {
                Assert.IsTrue(false, "Unrecognized RouteEvent type.");
            }

            return new FoxLib.Tpp.RouteSet.RouteEvent(
                eventTypeHash,
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