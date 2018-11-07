namespace FoxKit.Modules.Lua
{
    using System;

    using static KopiLua.Lua;

    /// <summary>
    /// Foxmath Lua library.
    /// </summary>
    public static class Foxmath
    {
        /// <summary>
        /// The value of pi.
        /// </summary>
        private const double Pi = 3.141592741012573;

        /// <summary>
        /// The float value of pi.
        /// </summary>
        private const float FPi = 3.1415927f;

        /// <summary>
        /// The value of 2 times pi.
        /// </summary>
        private const float FTwoPi = 6.28318531f;

        /// <summary>
        /// The Lua library.
        /// </summary>
        private static readonly luaL_Reg[] FoxmathLib =
        {
            new luaL_Reg("Sin", Sin),
            new luaL_Reg("Cos", Cos),
            new luaL_Reg("Tan", Tan),
            new luaL_Reg("Absf", Absf),
            new luaL_Reg("Asin", Asin),
            new luaL_Reg("Acos", Acos),
            new luaL_Reg("Atan2", Atan2),
            new luaL_Reg("Atan", Atan),
            new luaL_Reg("Exp", Exp),
            new luaL_Reg("Floor", Floor),
            new luaL_Reg("Ceil", Ceil),
            new luaL_Reg("Round", Round),
            new luaL_Reg("Mod", Mod),
            new luaL_Reg("Log", Log),
            new luaL_Reg("Pow", Pow),
            new luaL_Reg("Sqrt", Sqrt),
            new luaL_Reg("Rsqrt", Rsqrt),
            new luaL_Reg("Saturate", Saturate),
            new luaL_Reg("FRnd", FRnd),
            new luaL_Reg("DegreeToRadian", DegreeToRadian),
            new luaL_Reg("RadianToDegree", RadianToDegree),
            new luaL_Reg("Min", Min),
            new luaL_Reg("Max", Max),
            new luaL_Reg("Clamp", Clamp),
            new luaL_Reg("NormalizeRadian", NormalizeRadian),
            new luaL_Reg(null, null)
        };

        /// <summary>
        /// Define the library.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        public static void Define(lua_State lua)
        {
            lua_pushcclosure(lua, LuaOpenFoxmath, 0);
            lua_pushstring(lua, "foxmath");
            lua_call(lua, 1, 0);
        }

        /// <summary>
        /// Register the library functions.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The Lua return code.</returns>
        private static int LuaOpenFoxmath(lua_State lua)
        {
            luaL_register(lua, "foxmath", FoxmathLib);
            lua_pushnumber(lua, Pi);
            lua_setfield(lua, -2, "PI");
            return 1;
        }

        /// <summary>
        /// Compute the sine.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Sin(lua_State lua)
        {
            lua_pushnumber(lua, Math.Sin(luaL_checknumber(lua, 1)));
            return 1;
        }

        /// <summary>
        /// Compute the cosine.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Cos(lua_State lua)
        {
            lua_pushnumber(lua, Math.Cos(luaL_checknumber(lua, 1)));
            return 1;
        }

        /// <summary>
        /// Compute the tangent.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Tan(lua_State lua)
        {
            lua_pushnumber(lua, Math.Tan(luaL_checknumber(lua, 1)));
            return 1;
        }

        /// <summary>
        /// Compute the absolute value.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Absf(lua_State lua)
        {
            lua_pushnumber(lua, Math.Abs(luaL_checknumber(lua, 1)));
            return 1;
        }

        /// <summary>
        /// Compute the arcsine.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Asin(lua_State lua)
        {
            lua_pushnumber(lua, Math.Asin(luaL_checknumber(lua, 1)));
            return 1;
        }

        /// <summary>
        /// Compute the arccosine.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Acos(lua_State lua)
        {
            lua_pushnumber(lua, Math.Acos(luaL_checknumber(lua, 1)));
            return 1;
        }

        /// <summary>
        /// Compute the two-argument arctangent.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Atan2(lua_State lua)
        {
            lua_pushnumber(lua, Math.Atan2(luaL_checknumber(lua, 1), luaL_checknumber(lua, 2)));
            return 1;
        }

        /// <summary>
        /// Compute the arctangent.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Atan(lua_State lua)
        {
            lua_pushnumber(lua, Math.Atan(luaL_checknumber(lua, 1)));
            return 1;
        }

        /// <summary>
        /// Compute e raised to the specified power.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Exp(lua_State lua)
        {
            lua_pushnumber(lua, Math.Exp(luaL_checknumber(lua, 1)));
            return 1;
        }

        /// <summary>
        /// Compute the largest integer less than or equal to the specified number.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Floor(lua_State lua)
        {
            lua_pushnumber(lua, Math.Floor(luaL_checknumber(lua, 1)));
            return 1;
        }

        /// <summary>
        /// Compute the smallest integer greater than or equal to the specified number.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Ceil(lua_State lua)
        {
            lua_pushnumber(lua, Math.Ceiling(luaL_checknumber(lua, 1)));
            return 1;
        }

        /// <summary>
        /// Round the specified number to the nearest integral value.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Round(lua_State lua)
        {
            lua_pushnumber(lua, Math.Round(luaL_checknumber(lua, 1)));
            return 1;
        }

        /// <summary>
        /// Round the specified number to the nearest integral value.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Mod(lua_State lua)
        {
            lua_pushnumber(lua, luaL_checknumber(lua, 1) % luaL_checknumber(lua, 2));
            return 1;
        }

        /// <summary>
        /// Compute the natural logarithm of the specified number.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Log(lua_State lua)
        {
            lua_pushnumber(lua, Math.Log(luaL_checknumber(lua, 1)));
            return 1;
        }

        /// <summary>
        /// Compute the value of a number raised to a specified power.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Pow(lua_State lua)
        {
            lua_pushnumber(lua, Math.Pow(luaL_checknumber(lua, 1), luaL_checknumber(lua, 2)));
            return 1;
        }

        /// <summary>
        /// Compute the square root of a number.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Sqrt(lua_State lua)
        {
            lua_pushnumber(lua, Math.Sqrt(luaL_checknumber(lua, 1)));
            return 1;
        }

        /// <summary>
        /// Compute the inverse square root of a number.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Rsqrt(lua_State lua)
        {
            var sqrt = Math.Sqrt(luaL_checknumber(lua, 1));
            double rsqrt;
            if (sqrt <= 0.0)
            {
                rsqrt = 1.0e30;
            }
            else
            {
                rsqrt = 1.0 / sqrt;
            }

            lua_pushnumber(lua, rsqrt);
            return 1;
        }

        /// <summary>
        /// Clamp a specified value between 0 and 1.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Saturate(lua_State lua)
        {
            var arg = luaL_checknumber(lua, 1);
            var result = arg;
            if (arg < 0.0)
            {
                result = 0.0;
            }

            if (1.0 - arg < 0.0)
            {
                result = 1.0;
            }

            lua_pushnumber(lua, result);
            return 1;
        }

        /// <summary>
        /// Generate a random value between 0 and 1.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int FRnd(lua_State lua)
        {
            lua_pushnumber(lua, UnityEngine.Random.Range(0, 1));
            return 1;
        }

        /// <summary>
        /// Convert a value in degrees to radians.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int DegreeToRadian(lua_State lua)
        {
            lua_pushnumber(lua, luaL_checknumber(lua, 1) * 0.017453292);
            return 1;
        }

        /// <summary>
        /// Convert a value in radians to degrees.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int RadianToDegree(lua_State lua)
        {
            lua_pushnumber(lua, luaL_checknumber(lua, 1) * 57.29577791868205);
            return 1;
        }

        /// <summary>
        /// Compute the smaller of two numbers.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Min(lua_State lua)
        {
            lua_pushnumber(lua, Math.Min(luaL_checknumber(lua, 1), luaL_checknumber(lua, 2)));
            return 1;
        }

        /// <summary>
        /// Compute the larger of two numbers.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Max(lua_State lua)
        {
            lua_pushnumber(lua, Math.Max(luaL_checknumber(lua, 1), luaL_checknumber(lua, 2)));
            return 1;
        }

        /// <summary>
        /// Clamp a number between two values.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        private static int Clamp(lua_State lua)
        {
            var arg3 = luaL_checknumber(lua, 3);
            var arg2 = luaL_checknumber(lua, 2);
            var arg1 = luaL_checknumber(lua, 1);

            if (arg1 - arg2 < 0.0)
            {
                arg1 = arg2;
            }

            if (arg3 - arg1 < 0.0)
            {
                arg1 = arg3;
            }

            lua_pushnumber(lua, arg1);
            return 1;
        }

        /// <summary>
        /// Normalize a radian value.
        /// </summary>
        /// <param name="lua">The Lua state.</param>
        /// <returns>The result.</returns>
        /// <remarks>Not sure this is correct. Someone verify this.</remarks>
        private static int NormalizeRadian(lua_State lua)
        {
            var arg = luaL_checknumber(lua, 1);
            arg = arg + FPi;

            double result;
            if (arg < 0.0)
            {
                result = FPi - (arg % FTwoPi);
            }
            else
            {
                result = (arg % FTwoPi) - FPi;
            }

            lua_pushnumber(lua, result);
            return 1;
        }
    }
}