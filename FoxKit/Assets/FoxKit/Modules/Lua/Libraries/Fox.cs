namespace FoxKit.Modules.Lua
{
    using System;

    using UnityEngine;

    using static KopiLua.Lua;

    /// <summary>
    /// Fox Lua library. Contains primitives such as Vector3 and Color.
    /// </summary>
    public static class Fox
    {
        /// <summary>
        /// The type name for the Vector3 metatable.
        /// </summary>
        private const string Vector3RegistryKey = "fox.Vector3";

        /// <summary>
        /// The Lua library.
        /// </summary>
        private static readonly luaL_Reg[] Vector3InstanceLib =
        {
            new luaL_Reg("GetX", GetX),
            new luaL_Reg("GetY", GetY),
            new luaL_Reg("GetZ", GetZ),
            new luaL_Reg("GetLength", GetLength),
            new luaL_Reg("GetLengthSqr", GetLengthSqr),
            new luaL_Reg("__add", Add),
            new luaL_Reg("__sub", Sub),
            new luaL_Reg("__mul", Mul),
            new luaL_Reg(null, null)
        };

        private static readonly luaL_Reg[] Vector3IStaticLib =
        {
            new luaL_Reg("__call", Call),
            new luaL_Reg(null, null)
        };

        /// <summary>
        /// Define the library.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        public static void Define(lua_State lua)
        {
            lua_pushcclosure(lua, LuaOpenFox, 0);
            lua_call(lua, 0, 0);
        }

        /// <summary>
        /// Register the library functions.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The Lua return code.</returns>
        private static int LuaOpenFox(lua_State lua)
        {
            // Create the Vector3 metatable.
            luaL_newmetatable(lua, Vector3RegistryKey);
            lua_pushvalue(lua, -1);

            // Assign the Vector3 metatable's __index metamethod to point to itself.
            lua_setfield(lua, -2, "__index");

            // Register Vector3 libraries.
            luaL_register(lua, null, Vector3InstanceLib);
            lua_settop(lua, -2);
            luaL_register(lua, "Vector3", Vector3IStaticLib);
            lua_pushvalue(lua, -1);

            // Assign the metatable.
            lua_setmetatable(lua, -2);

            return 1;
        }

        /// <summary>
        /// Get the X component of a Vector3.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int GetX(lua_State lua)
        {
            BoxedVector3 vector;
            GetXyz(out vector, lua, 1);
            lua_pushnumber(lua, vector.X);
            return 1;
        }

        /// <summary>
        /// Get the Y component of a Vector3.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int GetY(lua_State lua)
        {
            BoxedVector3 vector;
            GetXyz(out vector, lua, 1);
            lua_pushnumber(lua, vector.Y);
            return 1;
        }

        /// <summary>
        /// Get the Z component of a Vector3.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int GetZ(lua_State lua)
        {
            BoxedVector3 vector;
            GetXyz(out vector, lua, 1);
            lua_pushnumber(lua, vector.Z);
            return 1;
        }

        /// <summary>
        /// Gets the length of a Vector3.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int GetLength(lua_State lua)
        {
            BoxedVector3 vector;
            GetXyz(out vector, lua, 1);
            lua_pushnumber(lua, vector.Magnitude);
            return 1;
        }

        /// <summary>
        /// Gets the squared length of a Vector3.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int GetLengthSqr(lua_State lua)
        {
            BoxedVector3 vector;
            GetXyz(out vector, lua, 1);
            lua_pushnumber(lua, vector.SqrMagnitude);
            return 1;
        }

        /// <summary>
        /// Gets the sum of two Vector3 values.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Add(lua_State lua)
        {
            BoxedVector3 vectorA;
            GetXyz(out vectorA, lua, 1);

            BoxedVector3 vectorB;
            GetXyz(out vectorB, lua, 2);
            
            LuaPushVector3(lua, vectorA.X + vectorB.X, vectorA.Y + vectorB.Y, vectorA.Z + vectorB.Z);
            return 1;
        }

        /// <summary>
        /// Gets the difference of two Vector3 values.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Sub(lua_State lua)
        {
            BoxedVector3 vectorA;
            GetXyz(out vectorA, lua, 2);

            BoxedVector3 vectorB;
            GetXyz(out vectorB, lua, 1);

            LuaPushVector3(lua, vectorB.X - vectorA.X, vectorB.Y - vectorA.Y, vectorB.Z - vectorA.Z);
            return 1;
        }

        /// <summary>
        /// Gets the product of a Vector3 and a scalar value.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Mul(lua_State lua)
        {
            BoxedVector3 vector;
            int scalarIndex;
            if (lua_isnumber(lua, 1) != 0)
            {
                GetXyz(out vector, lua, 2);
                scalarIndex = 1;
            }
            else
            {
                GetXyz(out vector, lua, 1);
                scalarIndex = 2;
            }

            var scalar = luaL_checknumber(lua, scalarIndex);
            LuaPushVector3(lua, vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
            return 1;
        }

        /// <summary>
        /// Get the value of a Vector3.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="lua">
        /// The Lua state.
        /// </param>
        /// <param name="index">
        /// The Lua stack index.
        /// </param>
        /// <returns>
        /// The Lua return value..
        /// </returns>
        private static int GetXyz(out BoxedVector3 result, lua_State lua, int index)
        {
            var arg = lua_touserdata(lua, index);
            if (arg == null || lua_getmetatable(lua, index) == 0)
            {
                luaL_typerror(lua, index, Vector3RegistryKey);
                result = null;
                return 0;
            }

            lua_getfield(lua, LUA_REGISTRYINDEX, Vector3RegistryKey);
            if (lua_rawequal(lua, -1, -2) == 0)
            {
                arg = null;
            }

            lua_pop(lua, 2);
            if (arg == null)
            {
                result = null;
                return 0;
            }

            result = arg as BoxedVector3;
            return 1;
        }

        private static int Call(lua_State lua)
        {
            var numArgs = lua_gettop(lua);
            if (numArgs > 1)
            {
                if (numArgs == 2)
                {
                    // If we were passed a table, treat its contents as the components of the Vector3 to create.
                    if (lua_istable(lua, 2))
                    {
                        lua_rawgeti(lua, 2, 1);
                        if (!lua_isnil(lua, -1))
                        {
                            lua_rawgeti(lua, 2, 2);
                            lua_rawgeti(lua, 2, 3);
                        }
                        else
                        {
                            lua_getfield(lua, 2, "x");
                            lua_getfield(lua, 2, "y");
                            lua_getfield(lua, 2, "z");
                        }

                        var x = luaL_checknumber(lua, -3);
                        var y = luaL_checknumber(lua, -2);
                        var z = luaL_checknumber(lua, -1);
                        lua_settop(lua, 2);
                        LuaPushVector3(lua, x, y, z);
                    }
                    else if (lua_isnumber(lua, 2) != 0)
                    {
                        /* If we were passed a number, then use that number as the x and y components of the Vector3.
                        * TODO: Not quite sure this is the correct behavior. Test ingame. */
                        var val = (float)lua_tonumber(lua, 2);
                        var vector = new Vector3(val, val, 0);
                        LuaPushVector3(lua, vector);
                    }
                    else if (LuaIsVector3(lua, 2))
                    {
                        // If we were passed a Vector3, then duplicate it.
                        BoxedVector3 result;
                        LuaToVector3(out result, lua, 2);
                        LuaPushVector3(lua, result.X, result.Y, result.Z);
                    }
                    else
                    {
                        luaL_error(lua, "cannot create Vector3");
                    }
                }
                else
                {
                    if (numArgs == 3)
                    {
                        var x = luaL_checknumber(lua, 2);
                        var y = luaL_checknumber(lua, 3);
                        LuaPushVector3(lua, x, y, 0);
                    }
                    else
                    {
                        var x = luaL_checknumber(lua, 2);
                        var y = luaL_checknumber(lua, 3);
                        var z = luaL_checknumber(lua, 4);
                        LuaPushVector3(lua, x, y, z);
                    }
                }
            }
            else
            {
                LuaPushVector3(lua, 0, 0, 0);
            }

            return 1;
        }

        /// <summary>
        /// Gets a value off the Lua stack as a Vector3.
        /// </summary>
        /// <param name="result">The value.</param>
        /// <param name="lua">The Lua state.</param>
        /// <param name="index">The stack index.</param>
        /// <returns>The Lua result code.</returns>
        private static int LuaToVector3(out BoxedVector3 result, lua_State lua, int index)
        {
            var obj = lua_touserdata(lua, index);
            if (obj == null || lua_getmetatable(lua, index) == 0)
            {
                result = null;
                return 0;
            }

            LuaGetRegistryValue(lua, Vector3RegistryKey);
            if (lua_rawequal(lua, -1, -2) == 0)
            {
                obj = null;
            }

            lua_settop(lua, -3);
            if (obj != null)
            {
                result = obj as BoxedVector3;
                return 1;
            }

            result = null;
            return 0;
        }

        /// <summary>
        /// Creates a new Vector3 and pushes it onto the Lua stack.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        /// <param name="z">The z component.</param>
        /// <returns>The Lua return code.</returns>
        private static int LuaPushVector3(lua_State lua, double x, double y, double z)
        {
            var userdata = (BoxedVector3)lua_newuserdata(lua, typeof(BoxedVector3));
            userdata.X = x;
            userdata.Y = y;
            userdata.Z = z;

            LuaGetRegistryValue(lua, Vector3RegistryKey);
            return lua_setmetatable(lua, -2);
        }

        /// <summary>
        /// Creates a new Vector3 and pushes it onto the Lua stack.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <param name="vector">The vector to push.</param>
        /// <returns>The Lua return code.</returns>
        private static int LuaPushVector3(lua_State lua, Vector3 vector)
        {
            return LuaPushVector3(lua, vector.x, vector.y, vector.z);
        }

        /// <summary>
        /// Push a registry value onto the Lua stack.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <param name="key">The registry key.</param>
        private static void LuaGetRegistryValue(lua_State lua, string key)
        {
            lua_getfield(lua, LUA_REGISTRYINDEX, key);
        }

        /// <summary>
        /// Checks if a value on the Lua stack is a Vector3.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <param name="index">The index.</param>
        /// <returns>True if the value is a Vector3.</returns>
        private static bool LuaIsVector3(lua_State lua, int index)
        {
            var obj = lua_touserdata(lua, index);
            bool result;
            if (obj != null && lua_getmetatable(lua, index) != 0)
            {
                LuaGetRegistryValue(lua, Vector3RegistryKey);
                if (lua_rawequal(lua, -1, -2) == 0)
                {
                    result = false;
                }

                lua_settop(lua, -3);
            }
            else
            {
                result = false;
            }

            result = obj != null;
            return result;
        }

        /// <summary>
        /// Because Unity's Vector3 is a struct, we can't pass them by reference. This class is a wrapper to allow that.
        /// </summary>
        private class BoxedVector3
        {
            /// <summary>
            /// Gets or sets the x component.
            /// </summary>
            public double X { get; set; }

            /// <summary>
            /// Gets or sets the y component.
            /// </summary>
            public double Y { get; set; }

            /// <summary>
            /// Gets or sets the z component.
            /// </summary>
            public double Z { get; set; }

            /// <summary>
            /// Gets the length of the vector.
            /// </summary>
            public double Magnitude => Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z));

            /// <summary>
            /// Gets the squared length of the vector.
            /// </summary>
            public double SqrMagnitude => (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z);
        }
    }
}