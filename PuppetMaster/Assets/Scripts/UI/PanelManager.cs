using System.Collections.Generic;
using UnityEngine;

namespace CustomUI
{
    public class PanelManager : MonoBehaviour
    {
        [Tooltip("The panel to use when starting, and when closing all other panels.\nLeave null to have no default panel.")]
        [SerializeField] private GameObject mainPanel = null;

        [Tooltip("This can be left null, but if you want your panels automatically sorted set it.")]
        [SerializeField] private Transform panelParent = null;

        private Queue<GameObject> activePanels = new Queue<GameObject>();

        [Range(1, 10)]
        public int maxPanels = 1;

        private bool CheckParentIsPanelParent(Transform m_transform)
        {
            if (panelParent == null) return false;

            Transform parent = m_transform.parent;
            while (parent != null)
            {
                if (panelParent == parent)
                    return true;

                parent = parent.parent;
            }

            return false;
        }

        private void Start()
        {
            ChangePanel(mainPanel);
        }

        public void ChangePanel(GameObject panel)
        {
            if (panel == null)
            {
                // Return to main panel
                while (activePanels.Count > 0)
                {
                    activePanels.Dequeue().SetActive(false);
                }

                if (mainPanel != null)
                {
                    if (CheckParentIsPanelParent(mainPanel.transform))
                    {
                        panel.transform.SetAsFirstSibling();
                    }

                    mainPanel.SetActive(true);
                    activePanels.Enqueue(mainPanel);
                }
            }
            else
            {
                // Do things with the panel
                if (activePanels.Contains(panel))
                {
                    panel.SetActive(false);
                    var oldQueue = activePanels;
                    activePanels.Clear();

                    foreach (var item in oldQueue)
                    {
                        if (item != panel)
                        {
                            activePanels.Enqueue(item);
                        }
                    }
                }
                else
                {
                    // if the panel is active, disable it
                    if (panel.activeSelf)
                    {
                        if (CheckParentIsPanelParent(panel.transform))
                        {
                            if (panel.transform.GetSiblingIndex() == 0)
                            {
                                panel.SetActive(false);
                            }
                            else
                            {
                                panel.transform.SetAsFirstSibling();
                            }
                        }
                        else
                        {
                            panel.SetActive(false);
                        }
                    }
                    else // Otherwise add it to our list and enable it
                    {
                        // If the active panel exceeds its limit, disable the oldest one
                        if (activePanels.Count >= maxPanels)
                        {
                            activePanels.Dequeue().SetActive(false);
                        }

                        if (CheckParentIsPanelParent(panel.transform))
                        {
                            panel.transform.SetAsFirstSibling();
                        }

                        panel.SetActive(true);
                        activePanels.Enqueue(panel);
                    }
                }
            }
        }

        public void ClosePanel(GameObject panel)
        {
            panel.SetActive(false);
            var oldPanels = activePanels;
            activePanels.Clear();

            foreach (var item in oldPanels)
            {
                if (item.gameObject != panel.gameObject)
                {
                    activePanels.Enqueue(item);
                }
            }
        }
    }
}