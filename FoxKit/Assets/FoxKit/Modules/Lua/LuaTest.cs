using UnityEngine;

using Lua = KopiLua.Lua;

public class LuaTest : MonoBehaviour
{
	void Start ()
	{
	    Lua.lua_State L = Lua.lua_open();
	    if (luaL_dostring(L, "return 42, 'butts'"))
	    {
	        Debug.Log("Success");
            
	        Debug.Log(Lua.lua_tonumber(L, -2));
	        Debug.Log(Lua.lua_tostring(L, -1));
        }
	    else
	    {
	        Debug.Log("Failure");
	    }
	    Lua.lua_close(L);
    }

    static bool luaL_dostring(Lua.lua_State L, Lua.CharPtr s)
    {
        return !(Lua.luaL_loadstring(L, s) == 1 || Lua.lua_pcall(L, 0, Lua.LUA_MULTRET, 0) == 1);
    }
}
