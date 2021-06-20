using System.Collections.Generic;
using UnityEngine;

namespace CustomUI
{
    public class PanelManager : MonoBehaviour
    {
        [Tooltip("The panel to use when starting, and when closing all other panels.\nLeave null to have no default panel.")]
        [SerializeField] private GameObject mainPanel = null;

        private Queue<GameObject> activePanels = new Queue<GameObject>();

        [Range(1, 10)]
        public int maxPanels = 1;

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
                        panel.SetActive(false);
                    }
                    else // Otherwise add it to our list and enable it
                    {
                        // If the active panel exceeds its limit, disable the oldest one
                        if (activePanels.Count >= maxPanels)
                        {
                            activePanels.Dequeue().SetActive(false);
                        }

                        panel.SetActive(true);
                        activePanels.Enqueue(panel);
                    }
                }
            }
        }
    }
}