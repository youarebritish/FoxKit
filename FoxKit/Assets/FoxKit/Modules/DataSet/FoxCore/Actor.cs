using System;
using System.Collections;
using System.Collections.Generic;

using FoxKit.Modules.DataSet.FoxCore;

using FoxLib;

using UnityEngine;

public class Actor : Entity
{
    public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
    {
        throw new NotImplementedException("Actors should not be written to file!");
    }
}
