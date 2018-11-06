namespace FoxKit.Modules.Lua.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Modules.DataSet.FoxCore;

    using FoxLib;

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

        private void ExposeNativeTypes(lua_State L)
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
                                                             select m
                           };

            foreach (var typeToExpose in typesToExpose)
            {
                CreateMetatableForType(L, typeToExpose.Type);
                this.constructors.Add(typeToExpose.Type, CreateConstructor(typeToExpose.Type, typeToExpose.FunctionsToExpose));
            }
        }

        private static Action<lua_State> CreateConstructor(Type type, IEnumerable<MethodInfo> methods)
        {
            return L =>
                {
                    var newEntity = lua_newuserdata(L, typeof(Entity)) as Entity;
                    lua_getglobal(L, "Entity");
                    lua_setmetatable(L, -2);

                    foreach (var method in methods)
                    {
                        // TODO Check for instance vs static, for now assume all are instance
                        ExposeInstanceMethod(L, method.Name, method);
                    }
                };
        }

        private Dictionary<Type, Action<lua_State>> constructors = new Dictionary<Type, Action<lua_State>>();

        private static void CreateMetatableForType(lua_State L, Type type)
        {
            var className = type.Name;

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
            // TODO This leaves the Entity table on the stack
        }
        
        private class MethodContext
        {
            public MethodInfo Method { get; set; }
            public ulong Flag { get; set; }
        }

        private static void ExposeInstanceMethod(lua_State L, string name, MethodInfo method, ulong flag = 0)
        {
            var methodContext = lua_newuserdata(L, typeof(MethodContext)) as MethodContext;
            methodContext.Method = method;
            methodContext.Flag = flag;
            lua_pushcclosure(L, ExecuteMethod, 1);
            lua_setfield(L, -2, name);
        }

        private static int ExecuteMethod(lua_State L)
        {
            var methodContext = lua_touserdata(L, lua_upvalueindex(1)) as MethodContext;
            var entity = lua_touserdata(L, 1) as Core.Entity; // TODO This is wrong and bad

            if (entity == null)
            {
                return luaL_error(L, "entity is null");
            }

            // TODO Check if Entity is derived from Entity
            // TODO Is there a way to do this in a thread-safe way?
            return (int)methodContext.Method.Invoke(entity, new object[] { L });
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
            var style = EditorStyles.textArea;
            style.richText = true;
            this.text = EditorGUILayout.TextArea(this.text, style, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            this.text = DoLuaSyntaxHighlighting(this.text);
        }

        private static readonly string[] SyntaxLiterals = { "true", "false", "nil" };
        private static readonly Color SyntaxLiteralColor = new Color(59.0f/255.0f, 153.0f/255.0f, 144.0f/255.0f, 1);
        private static readonly string[] SyntaxKeywords = { "and", "break", "do", "else", "elseif", "end", "for", "goto", "if", "in", "local", "not", "or", "repeat", "return", "then", "until", "while" };
        private static readonly Color SyntaxKeywordColor = new Color(124.0f / 255.0f, 145.0f / 255.0f, 20.0f / 255.0f, 1);

        private static string DoLuaSyntaxHighlighting(string inputSource)
        {
            // TODO
            var result = inputSource;
            return result;
        }
        
        void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            GUI.SetNextControlName("Load");
            if (GUILayout.Button("Load Script", EditorStyles.toolbarButton))
            {
                OnMenu_Load();
                GUI.FocusControl("Load");
            }

            if (GUILayout.Button("Save Script", EditorStyles.toolbarButton))
            {
                OnMenu_Save();
            }

            GUI.SetNextControlName("Clear");
            if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
            {
                OnMenu_Clear();
                GUI.FocusControl("Clear");
            }

            if (GUILayout.Button("Execute", EditorStyles.toolbarButton))
            {
                Execute();
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        void OnMenu_Create()
        {
            // Do something!
        }

        void OnMenu_Save()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save Lua script", null, "lua", "Enter a file name for the script.");
            if (path.Length == 0)
            {
                return;
            }

            System.IO.File.WriteAllText(path, this.text);
            AssetDatabase.Refresh();
        }

        void OnMenu_Load()
        {
            var path = EditorUtility.OpenFilePanel("Load Lua script", "", "lua");
            if (path.Length != 0)
            {
                var fileContent = System.IO.File.ReadAllText(path);
                this.text = fileContent;
            }
        }

        void OnMenu_Clear()
        {
            this.text = string.Empty;
        }

        private void Execute()
        {
            var result = luaL_loadbuffer(L, this.text, (uint)strlen(this.text), null);
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