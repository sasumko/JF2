using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LitJson;
using UnityEngine.SceneManagement;

namespace JFrame
{
    public class SceneMGR : Singleton<SceneMGR>
    {
        public void Reset ()
        {
            //####  DO SOMETHING such as load infos

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public CSceneControllerBase ActiveScene
        {
            get
            {
                return _activeScene;
            }
        }

        public CUIRootBase ActiveCanvas
        {
            get
            {
                return ActiveScene == null ? null : ActiveScene.CanvasAssigned;
            }
        }

        CSceneControllerBase _activeScene;

        void OnSceneLoaded (Scene scene, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single)
            {
                Debug.Log("JFrame : scene change done " + scene.name);
                _activeScene = null;

                _activeScene = GameObject.FindObjectOfType<CSceneControllerBase>();

                if (_activeScene == null)
                {
                    Debug.LogError(string.Format("Cannot find Scene Controller @{0} !", scene.name));
                    return;
                }
            }
            
        }

        public void ReqToChangeScene (string _sceneName)
        {
            Debug.Log("JFrame : scene change req " + _sceneName);
            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
        }

        public void DispatchMessage (string msg, object param = null)
        {
            Instance.SendMessage(msg, param, SendMessageOptions.DontRequireReceiver);
            if (ActiveScene != null)
            {
                ActiveScene.SendMessage(msg, param, SendMessageOptions.DontRequireReceiver);
            }
            //if (ActiveCanvas != null)
            //{
            //    ActiveCanvas.SendMessage(msg, param, SendMessageOptions.DontRequireReceiver);
            //}
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
                if (PopupMGR.Instance.OnBackButtonEvent() == false)
                {
                    //####  TODO : Exit Popup
                    //####  check activeCanvase
                    Debug.Log("EXIT?");
                }
            }
        }

        


    }

}
