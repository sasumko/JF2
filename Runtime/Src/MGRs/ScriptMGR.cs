using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using NLua;
using LitJson;

namespace JFrame
{
    //UNITY_3D; USE_KERALUA; LUA_CORE; CATCH_EXCEPTIONS

    public class ScriptMGR : Singleton<ScriptMGR>
    {
        Dictionary<string, CLuaModule> ActiveStates;
        CScriptHelper _helper;
        void Start()
        {
            _helper = gameObject.AddComponent<CScriptHelper>();
        }

        public void Reset()
        {
            if (ActiveStates != null)
            {
                foreach (KeyValuePair<string, CLuaModule> _p in ActiveStates)
                {
                    if (_p.Value == null)
                    {
                        continue;
                    }

                    _p.Value.Release();
                }
            }
           
        }
        
        public CLuaModule GetState (string _stateName)
        {
            if (string.IsNullOrEmpty(_stateName) == true
                || ActiveStates == null
                || ActiveStates.ContainsKey(_stateName) == false)
            {
                return null;
            }

            return ActiveStates[_stateName];
        }

        public CLuaModule OpenState (string _stateName)
        {
            if (GetState(_stateName) != null)
            {
                Debug.LogError("ERROR ! STATE NAME EXIST!");
                return null;
            }

            CLuaModule _newState = new CLuaModule();
            if (ActiveStates == null)
            {
                ActiveStates = new Dictionary<string, CLuaModule>();
            }
            _newState.LuaState["parent"] = this;

            ActiveStates.Add(_stateName, _newState);

            return _newState;
        }

        public void CloseState (string _stateName)
        {
            CLuaModule _activeState = GetState(_stateName);
            if (_activeState == null)
            {
                Debug.LogError("ERROR ! STATE NAME NOT EXIST!");
                return;
            }

            _activeState.Release();

            if (ActiveStates != null && ActiveStates.ContainsKey(_stateName) == true)
            {
                ActiveStates.Remove(_stateName);
            }
        }

        public void Call (CLuaModule _mod, string _methodName, object[] _params)
        {
            if (_mod == null || string.IsNullOrEmpty(_methodName) == true)
            {
                return;
            }

            NLua.LuaFunction _func = _mod.LuaState.GetFunction(_methodName);
            if (_func == null)
            {
                return;
            }

            object[] result = new  object[0];

            try
            {
                // Note: calling a function that does not
                // exist does not throw an exception.
                if (_params != null)
                {
                    result = _func.Call(_params);
                    //lf.Call(inputParam);
                }
                else
                {
                    result = _func.Call();
                    //lf.Call();
                }
            }
            catch (NLua.Exceptions.LuaException e)
            {
               
            }

            if (result != null &&
                result.Length > 0 &&
               result[0] != null)
            {
                Debug.Log("---");
                //if (FSNLuaEnum.ResultCode.LUA_METHODCALL_FALSE.ToString().Equals(result[0]))
                //{
                //    FSDebug.LogErr("NLuaManager", "NLuaCall - LUA_METHODCALL_FALSE");
                //    return FSNLuaEnum.ResultCode.LUA_METHODCALL_FALSE;
                //}
            }


        }

        public void Test ()
        {
            Debug.Log("------");
        }

        public void PrintLog (object _o)
        {
            Debug.Log("-- dd" + _o.ToString());
        }


    }

    public class CLuaModule
    {
        public Lua LuaState = null;

        public CLuaModule ()
        {
            LuaState = new Lua();
            LuaState.LoadCLRPackage();
        }
        public object[] LoadScriptByFile (string _fName)
        {
            string _luaString = DataMGR.ReadText("Lua/" + _fName);
            return LoadScriptByString(_luaString);
        }

        public object[] LoadScriptByString (string _luaString)
        {
            if (string.IsNullOrEmpty(_luaString) == true)
            {
                return null;
            }

            return LuaState.DoString(_luaString);
        }

        public void Release ()
        {
            LuaState.Close();
        }
    }

    public class CScriptHelper : MonoBehaviour
    {

        public void Test()
        {
            Debug.Log("------------ test");
        }
    }
}