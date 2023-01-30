using System.IO;
using IngameDebugConsole;
using UnityEngine;

namespace RGN.Sample.UI
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private DebugLogPopup _ingameDebugConsolePopupManager;
        private bool _debugPopupIsVisible;

        private void ShareLogs()
        {
            string filePath = SaveLogsToFile();
            NativeShare logFile = new NativeShare();
            logFile.AddFile(filePath);
            logFile.Share();
            Debug.Log("Shared log file: " + filePath);
        }

        private string SaveLogsToFile()
        {
            string path = Path.Combine(Application.persistentDataPath,
                System.DateTime.Now.ToString("dd-MM-yyyy--HH-mm-ss") + ".txt");
            File.WriteAllText(path, DebugLogManager.Instance.GetAllLogs());

            Debug.Log("Logs saved to: " + path);
            return path;
        }

        private void Start()
        {
            DebugLogConsole.AddCommand("share", "Shares logs file via native share dialog", ShareLogs);
            _ingameDebugConsolePopupManager.Hide();
        }

        private void Update()
        {
            if (ThreeFingersDragOneTapOrEditorMouseButtonTwoUp())
            {
                _debugPopupIsVisible = !_debugPopupIsVisible;
                if (_debugPopupIsVisible)
                {
                    _ingameDebugConsolePopupManager.Show();
                }
                else
                {
                    _ingameDebugConsolePopupManager.Hide();
                }
            }
        }

        private static bool ThreeFingersDragOneTapOrEditorMouseButtonTwoUp()
        {
            bool criteria = false;

#if UNITY_EDITOR
            criteria = Input.GetMouseButtonUp(2);
#else
        criteria =
            (Input.touchCount > 3 &&
            Input.GetTouch(0).phase == TouchPhase.Moved &&
            Input.GetTouch(1).phase == TouchPhase.Moved &&
            Input.GetTouch(2).phase == TouchPhase.Moved &&
            Input.GetTouch(3).phase == TouchPhase.Ended);
#endif

            return criteria;
        }
    }
}
