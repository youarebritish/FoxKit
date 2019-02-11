namespace FoxKit.Modules.Lua
{
    using FoxLib;

    using KopiLua;

    /// <summary>
    /// Global Lua state.
    /// </summary>
    public class LuaVM
    {
        /// <summary>
        /// The print() library.
        /// </summary>
        private static readonly Lua.luaL_Reg[] PrintLib = { new Lua.luaL_Reg("print", Print), new Lua.luaL_Reg(null, null) };

        /// <summary>
        /// The global instance.
        /// </summary>
        private static LuaVM instance;

        /// <summary>
        /// Gets the Lua state.
        /// </summary>
        public Lua.lua_State L { get; private set; }

        /// <summary>
        /// Create the global LuaVM instance.
        /// </summary>
        /// <returns>The global LuaVM instance.</returns>
        public static LuaVM Create()
        {
            var lua = Lua.lua_open();
            instance = new LuaVM { L = lua };

            // Initialize Lua state.
            Lua.luaL_openlibs(lua);

            // Overwrite default print() function to write to Debug.Log().
            Lua.lua_getglobal(lua, "_G");
            Lua.luaL_register(lua, null, PrintLib);

            // Open custom libraries.
            OpenFoxLibraries(lua);

            // TODO expose native types

            return instance;
        }

        /// <summary>
        /// Get the global LuaVM instance.
        /// </summary>
        /// <returns>The global LuaVM instance.</returns>
        public static LuaVM GetInstance()
        {
            return instance;
        }

        private static void OpenFoxLibraries(Lua.lua_State lua)
        {
            // TODO Implement foxde library
            Foxmath.Define(lua);
            Fox.Define(lua);

            // TODO fox math types
            // TODO fox color type
            // TODO fox file type
        }
        
        /// <summary>
        /// Lua print function.
        /// </summary>
        /// <param name="lua">
        /// The Lua state.
        /// </param>
        /// <returns>
        /// The Lua result code.
        /// </returns>
        private static int Print(Lua.lua_State lua)
        {
            var nargs = Lua.lua_gettop(lua);
            for (var i = 1; i <= nargs; i++)
            {
                UnityEngine.Debug.Log(Lua.lua_tostring(lua, i));
            }

            return 0;
        }
    }
}