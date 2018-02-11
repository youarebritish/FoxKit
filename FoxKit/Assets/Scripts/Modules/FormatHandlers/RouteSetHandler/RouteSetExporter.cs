using System.Collections.Generic;
using System.IO;
using System.Linq;

using FoxKit.Core;
using FoxKit.Modules.FormatHandlers.RouteSetHandler;

using UnityEngine.Assertions;
using System;

public static class RouteSetExporter
{
    private const uint HeaderSizeBytes = 28;
    private const uint NodeSizeBytes = 3 * sizeof(float);
    private const uint EventTableSizeBytes = 2 * sizeof(ushort);
    private const uint EventSizeBytes = sizeof(uint) + (10 * sizeof(uint)) + (4 * sizeof(byte));

    private const ushort GzFrtVersion = 2;
    private const ushort TppFrtVersion = 3;

    public static void ExportRouteSet(RouteSet routeSet, StrCode32HashManager hashManager, string exportPath)
    {
        Assert.IsNotNull(routeSet, "RouteSet must not be null.");
        Assert.IsNotNull(hashManager, "hashManager must not be null.");
        Assert.IsNotNull(exportPath, "exportPath must not be null.");
        Assert.IsNotNull(routeSet.Routes, "RouteSet.Routes must not be null.");

        var routeCount = routeSet.Routes.Count;
        Assert.IsTrue(routeCount > 0, "Invalid route count. Cannot write a routeset with no routes.");
        Assert.IsTrue(routeCount <= ushort.MaxValue, "Invalid route count. Only up to " + ushort.MaxValue + " routes can be written to file.");

        using (var writer = new BinaryWriter(new FileStream(exportPath, FileMode.Create)))
        {
            throw new NotImplementedException();
        }
    }
}
