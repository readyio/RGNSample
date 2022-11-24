using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadyGamesNetwork.Sample.UI
{
    public sealed class UIRoot : MonoBehaviour
    {
        /// <summary>
        /// Stores all full-screen panels and provide simple methods for manipulation
        /// apart from direct access to instances
        /// </summary>
        public static UIRoot singleton { get; private set; }

        private readonly static Dictionary<string, AbstractPanel> panels = new Dictionary<string, AbstractPanel>();
        public string ActivePanelName { get; private set; }

        private readonly static Dictionary<string, AbstractPopup> popups = new Dictionary<string, AbstractPopup>();

        [SerializeField] private Transform particularPopUpsParent;

        private void Awake()
        {
            RefreshPanelsDatabase();
            singleton = this;
        }

        private void RefreshPanelsDatabase()
        {
            panels.Clear();
            // use iterative approach cause unity's "GetComponentInChildren" search only through active objects
            for (var i = 0; i < transform.childCount; i++)
            {
                var panel = transform.GetChild(i).GetComponent<AbstractPanel>();
                if (panel != null)
                {
                    panels.Add(panel.GetType().Name, panel);
                }
            }

            popups.Clear();
            for (int i = 0; i < particularPopUpsParent.childCount; i++)
            {
                AbstractPopup particularPopUp = particularPopUpsParent.GetChild(i).GetComponent<AbstractPopup>();
                if (particularPopUp != null)
                {
                    popups.Add(particularPopUp.GetType().Name, particularPopUp);
                }
            }
        }

        public T GetPopup<T>() where T : AbstractPopup
        {
            var pName = typeof(T).Name;
            if (!popups.ContainsKey(pName)) throw new System.Exception("Particular pop not found with name " + pName);

            return popups[pName] as T;
        }

        public T GetPopup<T>(string name) where T : AbstractPopup
        {
            if (!popups.ContainsKey(name)) throw new System.Exception("Particular pop not found with name " + name);

            return popups[name] as T;
        }

        public void ShowPopup<T>(bool isInstant = true, System.Action onComplete = null) where T : AbstractPopup
        {
            var pName = typeof(T).Name;

            ShowPopup(pName, isInstant, onComplete);
        }

        public void ShowPopup(string name, bool isInstant = true, System.Action onComplete = null)
        {
            if (!popups.ContainsKey(name)) throw new System.Exception("Particular pop up not found with name " + name);

            if (popups[name].IsActive() == false)
            {
                popups[name].transform.SetAsLastSibling();

                popups[name].Show(isInstant, () =>
                {
                    onComplete?.Invoke();
                });
            }
        }

        public void HidePopup<T>(bool isInstant = true, System.Action onComplete = null) where T : AbstractPopup
        {
            var pName = typeof(T).Name;

            HidePopup(pName, isInstant, onComplete);
        }

        public void HidePopup(string name, bool isInstant = true, System.Action onComplete = null)
        {
            if (!popups.ContainsKey(name)) throw new System.Exception("Particular pop up not found with name " + name);

            if (popups[name].IsActive() == true)
            {
                popups[name].Hide(isInstant, () =>
                {
                    onComplete?.Invoke();
                });
            }
        }

        public void HideAllPanels()
        {
            foreach (var kvp in panels)
            {
                if (kvp.Value.gameObject.activeSelf)
                    kvp.Value.Hide(true, null);
            }
        }

        public T GetPanel<T>() where T : AbstractPanel
        {
            var pName = typeof(T).Name;

            if (!panels.ContainsKey(pName)) throw new System.Exception("Panel not found with name " + pName);

            return panels[pName] as T;
        }

        public T GetPanel<T>(string name) where T : AbstractPanel
        {
            if (!panels.ContainsKey(name)) throw new System.Exception("Panel not found with name " + name);

            return panels[name] as T;
        }

        public void ShowPanel<T>(bool isInstant = true, System.Action onComplete = null)
        {
            ShowPanel(typeof(T).Name, isInstant, onComplete);
        }

        public void ShowPanel(string name, bool isInstant = true, System.Action onComplete = null)
        {
            if (!panels.ContainsKey(name)) throw new System.Exception("Panel not found with name " + name);

            panels[name].transform.SetAsLastSibling();
            panels[name].Show(isInstant, () =>
            {
                ActivePanelName = name;
                onComplete?.Invoke();
            });
        }

        public void HidePanel<T>(bool isInstant = true, System.Action onComplete = null)
        {
            HidePanel(typeof(T).Name, isInstant, onComplete);
        }

        public void HidePanel(string name, bool isInstant = true, System.Action onComplete = null)
        {
            if (!panels.ContainsKey(name)) throw new System.Exception("Panel not found with name " + name);

            panels[name].Hide(isInstant, () =>
            {
                ActivePanelName = "";
                // find any other panel left active
                foreach (var kvp in panels)
                {
                    if (kvp.Value.gameObject.activeSelf)
                    {
                        ActivePanelName = kvp.Key;
                        break;
                    }
                }

                onComplete?.Invoke();
            });
        }

        public void SwapToPanel<T>(bool isInstant = true, System.Action onComplete = null)
        {
            SwapToPanel(typeof(T).Name, isInstant, onComplete);
        }

        public void SwapToPanel(string toPanel, bool isInstant = true, System.Action onComplete = null)
        {
            if (!panels.ContainsKey(toPanel)) throw new System.Exception("Panel not found with name " + toPanel);

            if (ActivePanelName == toPanel) return;

            if (string.IsNullOrEmpty(ActivePanelName))
            {
                panels[toPanel].transform.SetAsLastSibling();
                panels[toPanel].Show(isInstant, () =>
                {
                    ActivePanelName = toPanel;
                    onComplete?.Invoke();
                });
            }
            else
            {
                panels[toPanel].transform.SetAsLastSibling();
                panels[toPanel].Show(isInstant, () =>
                {
                    panels[ActivePanelName].Hide(isInstant, () =>
                    {
                        ActivePanelName = toPanel;
                        onComplete?.Invoke();
                    });
                });
            }
        }
    }
}