using UnityEngine;
using System.Collections;

namespace JFrame
{

    public class CSceneControllerBase : MonoBehaviour
    {
        public CUIRootBase CanvasAssigned
        {
            get
            {
                return _canvasAssigned;
            }
        }

        protected CUIRootBase _canvasAssigned;
        protected IEnumerator _coroutineState = null;

        protected virtual void Awake ()
        {
            Debug.Log("JFrame: sceneControllerbase awake " + this.name);
            _canvasAssigned = GameObject.FindObjectOfType<CUIRootBase>();
            _coroutineState = coroutineState();

            if (_coroutineState != null)
            {
                StartCoroutine(_coroutineState);
            }
            
        }

        private void OnDestroy()
        {
            Exit();
        }

        IEnumerator coroutineState ()
        {
            Enter();

            while (true)
            {
                Execute(Time.deltaTime);
                yield return null;
            }
            
        }

        protected virtual void Enter ()
        {
            Debug.Log("JFrame: sceneControllerbase enter " + this.name);
            
        }

        protected virtual void Execute (float _deltaTime)
        {

        }

        protected virtual void Exit ()
        {
            Debug.Log("JFrame: sceneControllerbase exit " + this.name);
            if (_coroutineState != null)
            {
                StopCoroutine(_coroutineState);
            }
        }
    }
}