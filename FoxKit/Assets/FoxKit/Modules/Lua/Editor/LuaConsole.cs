namespace FoxKit.Modules.Lua.Editor
{
    using System;
    using System.Linq;

    using UnityEditor;

    using UnityEngine;

    using static KopiLua.Lua;

    public class LuaConsole : EditorWindow
    {
        [MenuItem("Window/FoxKit/Lua Console")]
        public static void Create()
        {
            var window = GetWindow<LuaConsole>();
            window.titleContent = new GUIContent("Lua Console");
            window.Show();
        }
        
        private static readonly luaL_Reg[] printlib = { new luaL_Reg("print", l_my_print), new luaL_Reg(null, null), };

        private void OnEnable()
        {
            if (this.L != null)
            {
                return;
            }

            this.L = lua_open();
            luaL_openlibs(this.L);

            // Overwrite default print() function to write to Debug.Log().
            lua_getglobal(this.L, "_G");
            luaL_register(this.L, null, printlib);

            ExposeNativeTypes(this.L);
        }

        private static void ExposeNativeTypes(lua_State L)
        {
            var typesToExpose =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(ExposeClassToLuaAttribute), true)
                where attributes != null && attributes.Length > 0
                select new
                           {
                               Type = t, FunctionsToExpose = from m in t.GetMethods()
                                                             let mAttributes = m.GetCustomAttributes(typeof(ExposeMethodToLuaAttribute), true)
                                                             where mAttributes != null && mAttributes.Length > 0
                                                             select mAttributes
                           };

            foreach (var typeToExpose in typesToExpose)
            {
                var className = typeToExpose.Type.Name;

                // Create a new global table for the type.
                // TODO get if exists
                lua_pushliteral(L, className);
                lua_type(L, -1);
                lua_settop(L, -2);
                lua_createtable(L, 0, 0);
                lua_pushliteral(L, className);
                lua_pushvalue(L, -2);
                lua_rawset(L, LUA_GLOBALSINDEX);

                // Define metamethods.
                // _className
                lua_pushliteral(L, className);
                lua_setfield(L, -2, "_className");

                // Assign metatable.
                lua_pushvalue(L, -1);
                lua_setmetatable(L, -2);

                // TODO handle __index, __newindex
            }
        }

        private void OnDisable()
        {
            lua_close(this.L);
            this.L = null;
        }

        private Vector2 scroll;

        private string text = string.Empty;

        private string history = string.Empty;

        private lua_State L;

        void OnGUI()
        {
            this.DrawToolbar();

            this.scroll = EditorGUILayout.BeginScrollView(this.scroll);
            this.DrawCommandPane();
            EditorGUILayout.EndScrollView();
        }

        private void DrawCommandPane()
        {
            this.text = EditorGUILayout.TextArea(this.text, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        }
        
        void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GUILayout.Button("Load Script", EditorStyles.toolbarButton))
            {
                OnMenu_Create();
                EditorGUIUtility.ExitGUI();
            }
            if (GUILayout.Button("Save Script", EditorStyles.toolbarButton))
            {
                OnMenu_Create();
                EditorGUIUtility.ExitGUI();
            }
            if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
            {
                OnMenu_Create();
                EditorGUIUtility.ExitGUI();
            }
            if (GUILayout.Button("Execute", EditorStyles.toolbarButton))
            {
                Execute();
                EditorGUIUtility.ExitGUI();
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            return;
        }

        void OnMenu_Create()
        {
            // Do something!
        }

        private void Execute()
        {
            int result = luaL_loadbuffer(L, this.text, (uint)strlen(this.text), null);
            if (result == 0)
            {
                result = lua_pcall(L, 0, LUA_MULTRET, 0);

                if (result != 0)
                {
                    printError(L);
                }
                else
                {
                    this.history = this.history + this.text + "\n";
                }
            }
            else
            {
                printError(L);
            }
        }

        private static int l_my_print(lua_State L)
        {
            int nargs = lua_gettop(L);

            for (var i = 1; i <= nargs; i++)
            {
                if (lua_isstring(L, i) == 1)
                {
                    /* Pop the next arg using lua_tostring(L, i) and do your print */
                    Debug.Log(lua_tostring(L, i));
                }
                else
                {
                    /* Do something with non-strings if you like */
                }
            }

            return 0;
        }

        private static int traceback(lua_State L)
        {
            lua_getfield(L, LUA_GLOBALSINDEX, "debug");
            lua_getfield(L, -1, "traceback");
            lua_pushvalue(L, 1);
            lua_pushinteger(L, 2);
            lua_call(L, 2, 1);
            fprintf(stderr, "%s\n", lua_tostring(L, -1));
            return 1;
        }

        private static void printError(lua_State L)
        {
            Debug.LogError(luaL_checkstring(L, -1));
        }

        void OnTools_OptimizeSelected()
        {
            // Do something!
        }

        void OnTools_Help()
        {
            Help.BrowseURL("http://example.com/product/help");
        }

        static bool luaL_dostring(lua_State L, CharPtr s)
        {
            return luaL_loadstring(L, s) == 0 || lua_pcall(L, 0, LUA_MULTRET, 0) == 0;
        }

        static int dostring(lua_State L, CharPtr s, CharPtr name)
        {
            int status = (luaL_loadbuffer(L, s, (uint)strlen(s), name) != 0) || (docall(L, 0, 1) != 0) ? 1 : 0;
            return report(L, status);
        }

        static void l_message(CharPtr pname, CharPtr msg)
        {
            if (pname != null) fprintf(stderr, "%s: ", pname);
            fprintf(stderr, "%s\n", msg);
            fflush(stderr);
        }

        static CharPtr progname = LUA_PROGNAME;

        static int report(lua_State L, int status)
        {
            if ((status != 0) && !lua_isnil(L, -1))
            {
                CharPtr msg = lua_tostring(L, -1);
                if (msg == null) msg = "(error object is not a string)";
                l_message(progname, msg);
                lua_pop(L, 1);
            }
            return status;
        }
        
        static int docall(lua_State L, int narg, int clear)
        {
            int status;
            int base_ = lua_gettop(L) - narg;  /* function index */
            lua_pushcfunction(L, traceback);  /* push traceback function */
            lua_insert(L, base_);  /* put it under chunk and args */
            //signal(SIGINT, laction);
            status = lua_pcall(L, narg, ((clear != 0) ? 0 : LUA_MULTRET), base_);
            //signal(SIGINT, SIG_DFL);
            lua_remove(L, base_);  /* remove traceback function */
            /* force a complete garbage collection in case of errors */
            if (status != 0) lua_gc(L, LUA_GCCOLLECT, 0);
            return status;
        }

    }
}