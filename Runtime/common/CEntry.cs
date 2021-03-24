using UnityEngine;

namespace JFrame
{
    public class CEntry : MonoBehaviour
    {
        void Awake ()
        {
            SceneMGR.Instance.Reset();
            SceneMGR.Instance.ReqToChangeScene("test");
        }

        private void OnDestroy()
        {
            Debug.Log("JFrame: Destroy");
        }
    }
}