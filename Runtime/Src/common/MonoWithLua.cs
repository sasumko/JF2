using UnityEngine;
using System.Collections;

namespace JFrame
{


    public class MonoWithLua : MonoBehaviour 
    {
        CLuaModule _luaModule = null;
        void Start ()
        {

            _luaModule = ScriptMGR.Instance.OpenState("test2");
            _luaModule.LoadScriptByFile("lua_clr_header.lua");
            _luaModule.LuaState["gameObject"] = this;

            //ScriptMGR.Instance.Call(_luaModule, "CreateGameObject", null);
        }

        void Update()
        {
            ScriptMGR.Instance.Call(_luaModule, "Update", new object[] { Time.deltaTime });
        } 

        public void Print (float _d)
        {
            Debug.Log("--=-=" + _d);
        }
    }
}
