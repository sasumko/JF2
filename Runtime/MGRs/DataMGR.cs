using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using LitJson;
using System;

namespace JFrame
{
    public class DataMGR : Singleton<DataMGR>
    {
        List<CResInfo> ResInfos;

        public void Reset()
        {
            //####  DO SOMETHING such as load infos
            ResInfos = CreateList<CResInfo>("Json/ResourceInfo");
        }

        public static T Create<T>(string _jsonStr) where T : CInfoBase, new()
        {
            if (string.IsNullOrEmpty(_jsonStr) == true)
            {
                return default(T);
            }

            return JsonMapper.ToObject<T>(_jsonStr);
        }

        public static List<T> CreateList<T>(string _jsonFileName) where T : CInfoBase, new()
        {
            JsonData _d = DataMGR.Instance.ReadJson(_jsonFileName);

            if (_d == null)
            {
                return null;
            }

            List<T> _ret = new List<T>();

            for (int i = 0; i < _d.Count; i++)
            {
                T _info = Create<T>(JsonMapper.ToJson(_d[i]));
                if (_info == null)
                {
                    continue;
                }

                _ret.Add(_info);
            }

            return _ret;
        }

        public static List<T> CreateListWithIdx <T>(int _idx) where T : CInfoBase, new()
        {
            CResInfo _info = DataMGR.Instance.GetResInfoByIdx(_idx);
            if (_info == null)
            {
                return null;
            }

            return CreateList<T>(_info.Path);
        }

        public static string ReadText(string szTextFileName)
        {
            TextAsset txtAsset = (TextAsset)Resources.Load(szTextFileName);
            return txtAsset.text;
        }

        public JsonData ReadJson(string szJsonFileName)
        {
            //		JsonData _ret = ReadJsonFromExternal(szJsonFileName);
            //		
            //		if (_ret == null)
            JsonData _ret = ReadJsonFromBundle(szJsonFileName);


            int _a = 100000;

            return _ret;
        }

        public JsonData ReadJsonFromBundle(string szJsonFileName)
        {
            string _strBuf = ReadText(szJsonFileName);

            JsonReader _reader = new JsonReader(_strBuf);
            _reader.AllowComments = true;
            _reader.AllowSingleQuotedStrings = false;

            JsonData _data = JsonMapper.ToObject(_reader);

            return _data;
        }

        public T Load<T>(int _index) where T : UnityEngine.Object
        {
            T _ret = default(T);

            CResInfo _info = GetResInfoByIdx(_index);

            if (_info != null)
            {
                //####  TODO : do it in assetbundle first!

                //####
                try
                {
                    _ret = Resources.Load(_info.Path) as T;
                }
                catch (Exception _e)
                {

                }

            }


            return _ret;
        }

        public T Load<T> (string _id) where T: UnityEngine.Object
        {
            T _ret = default(T);

            CResInfo _info = GetResInfoByResID(_id);

            if (_info != null)
            {
                //####  TODO : do it in assetbundle first!

                //####
                try
                {
                    _ret = Resources.Load(_info.Path) as T;
                }
                catch (Exception _e)
                {

                }
                
            }


            return _ret;
        }

        public T Instantiate<T> (string _id, Transform _parent = null) where T : UnityEngine.Object
        {
            T _ret = default(T);

            T _obj = Load<T>(_id);

            if (_obj != null)
            {
                _ret = Instantiate(_obj, _parent);
            }

            return _ret;
        }

        CResInfo GetResInfoByResID (string _id)
        {
            CResInfo _info = null;

            if (ResInfos != null && string.IsNullOrEmpty(_id) == false)
            {
                _info = ResInfos.Find(_item => string.Equals(_item.ResId, _id, System.StringComparison.OrdinalIgnoreCase) == true);
            }

            return _info;
        }

        CResInfo GetResInfoByIdx (int _idx)
        {
            CResInfo _info = null;

            if (ResInfos != null)
            {
                _info = ResInfos.Find(_item => _item.Idx == _idx);
            }

            return _info;
        }

        public static void SetLayer(Transform _tr, string _name)
        {
            if (_tr == null)
            {
                return;
            }

            _tr.gameObject.layer = LayerMask.NameToLayer(_name);

            foreach (Transform _child in _tr)
            {
                SetLayer(_child, _name);
            }
        }


        public static void SetFloatOnMaterialRecursive(Transform _trTarget, string _strId, float _value)
        {
            if (_trTarget == null || string.IsNullOrEmpty(_strId) == true)
            {
                return;
            }

            Renderer _r = _trTarget.GetComponent<Renderer>();
            if (_r != null && _r.material.HasProperty(_strId) == true)
            {
                _r.material.SetFloat(_strId, _value);// _bOnOff == true ? 1f : 0f);
            }

            foreach (Transform _child in _trTarget)
            {
                SetFloatOnMaterialRecursive(_child, _strId, _value);
            }
        }


        public static void DeleteFile (string _fName)
        {
            string _fPath = Path.Combine(GetWorkingPath(), _fName);
            if (File.Exists(_fPath) == false)
            {
                return;
            }

            File.Delete(_fPath);
        }

        public static void WriteFile (string _fName, string _body)
        {
            string _fPath = Path.Combine(GetWorkingPath(), _fName);
            FileStream _file = new FileStream(_fPath, FileMode.Append, FileAccess.Write);
            StreamWriter _writer = new StreamWriter(_file);

            _writer.WriteLine(_body);

            _writer.Close();
            _file.Close();
        }

        public static string GetWorkingPath ()
        {
#if UNITY_EDITOR
            return Path.Combine(Application.dataPath, "../../Working");
#else
            return Application.persistentDataPath;
#endif
        }

    }

    public class CResInfo : CInfoBase
    {
        public int Idx;
        public string ResId;
        public string Path; //asset name if use assetbundle
        public string AssetbundleName; //bundle name
        public string Type;
    }
}
