using UnityEditor;
using UnityEngine;
using System;

using System.Collections;
using System.Collections.Generic;

namespace JFrame
{
#if UNITY_EDITOR
    public class ScriptBatch
    {

        public static string ProjectRootPath = Application.dataPath + "/..";

        public static void DoBuild(BuildTarget _target)
        {
            CBuildInfo _buildInfo = GetBuildEnv(_target);
            string[] _levels = GetLevels();

            if (_buildInfo == null || _levels == null || _levels.Length == 0)
            {
                //FAIL!
                UnityEngine.Debug.LogError(" CANNOT build for " + _target.ToString() + " @" + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString());
                return;
            }

            string _output = string.Format("{0}/{1}/{2}", ProjectRootPath, _buildInfo.PathBinFolder, _buildInfo.PathBinFile);
            BuildPipeline.BuildPlayer(_levels, _output, _target, BuildOptions.None);

            Debug.Log("Build [" + _buildInfo.PathBinFile + "] : Done! (bin path = " + _buildInfo.PathBinFolder + ")");
        }

        public static CBuildInfo GetBuildEnv(BuildTarget _target)
        {
            CBuildInfo _ret = null;

            //read json
            try
            {
                string _json = System.IO.File.ReadAllText(ProjectRootPath + "/BuildInfo.json");

                LitJson.JsonData _data = LitJson.JsonMapper.ToObject(_json);

                if (_data != null)
                {
                    LitJson.JsonData _platformData = _data[_target.ToString()];
                    if (_platformData != null)
                    {
                        string _platformDataJsonString = LitJson.JsonMapper.ToJson(_platformData);
                        _ret = LitJson.JsonMapper.ToObject<CBuildInfo>(_platformDataJsonString);
                    }
                }
            }
            catch (Exception _e)
            {
                Debug.LogError("Error on reading BuildInfo.json @UnityProject root path");
                Debug.Log(_e);
            }


            return _ret;
        }

        public static string[] GetLevels()
        {
            List<string> _ret = new List<string>();

            EditorBuildSettingsScene[] _scenes = EditorBuildSettings.scenes;
            if (_scenes != null)
            {
                for (int i = 0; i < _scenes.Length; i ++)
                {
                    UnityEngine.SceneManagement.Scene _scene = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(i);
                    string _path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                    if (string.IsNullOrEmpty(_path) == true)
                    {
                        continue;
                    }
                    _ret.Add(_path);
                }
            }
            return _ret.ToArray();
        }

        //######################################################
        //####  MENU ITEMS
        [MenuItem("CustomMenu/Build/Android")]
        public static void BuildForAndroid()
        {
            DoBuild(BuildTarget.Android);
        }

        [MenuItem("CustomMenu/Build/IOS")]
        public static void BuildForIOS()
        {
            DoBuild(BuildTarget.iOS);
        }

        [MenuItem("CustomMenu/Build/Win32")]
        public static void BuildForWin32()
        {
            DoBuild(BuildTarget.StandaloneWindows);
        }
    }

    public class CBuildInfo
    {
        public string PathBinFolder;
        public string PathBinFile;
    }
#endif
}