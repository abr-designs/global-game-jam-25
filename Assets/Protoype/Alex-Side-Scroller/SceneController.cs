using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Protoype.Alex_Side_Scroller
{
    public class SceneController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(0);
        }
    }
}