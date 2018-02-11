using System.Collections;
using System.Collections.Generic;

using NUnit.Framework.Constraints;

using UnityEngine;

public interface IRouteEvent
{
}

public interface IRouteEvent<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> : IRouteEvent
{
}

public struct NoEventArg
{
}

public abstract class TppRouteNodeEvent<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> :
    IRouteEvent<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>
{
}

public abstract class TppRouteEdgeEvent<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> : 
    IRouteEvent<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>
{
}

public class TppRouteEdgeEventRelaxedWalk :
    TppRouteEdgeEvent<NoEventArg, NoEventArg, NoEventArg, NoEventArg, NoEventArg, NoEventArg, NoEventArg, NoEventArg, NoEventArg, NoEventArg>
{
    private bool enableEdgeActingRotateSholder;
    private bool enableEdgeActingSwingHead;
    private bool enableEdgeActingTidyUp;
    private bool enableEdgeActingCheckGun;
}

public class TppRouteNodeEventRelaxedIdle :
    TppRouteEdgeEvent<NoEventArg, NoEventArg, NoEventArg, NoEventArg, NoEventArg, NoEventArg, NoEventArg, NoEventArg, NoEventArg, NoEventArg>
{
    private bool enableStandActNormal;
    private bool enableStandActSmoking;
    private bool enableSquatActRelax;
}