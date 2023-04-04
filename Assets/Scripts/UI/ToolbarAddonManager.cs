using System;
using System.Collections.Generic;
using General;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ToolbarAddonManager : MonoBehaviour {
        [SerializeField] List<Addon> toolBarAddons;
        void Start() {
            for (int i = 0; i < toolBarAddons.Count; i++) {
                Addon a = toolBarAddons[i];
                a.Initialize();
                int index = i;
                a.button.onClick.AddListener(delegate { ToggleAddon(index); });
            }
        }
        void ToggleAddon(int index) {
            for (int i = 0; i < toolBarAddons.Count; i++) {
                if (index == i) {
                    if (toolBarAddons[i].addon.Enabled) {
                        toolBarAddons[i].addon.DisableDisplay();
                        CameraControl.SetCameraMovementState(true);
                    } else {
                        toolBarAddons[i].addon.EnableDisplay();
                        CameraControl.SetCameraMovementState(false);
                    }
                } else toolBarAddons[i].addon.DisableDisplay();
            }
        }
        [Serializable]
        public class Addon {
            public Button button;
            public GameObject addonHolder;
            public IAddon addon;
            public void Initialize() => addon = addonHolder.GetComponent<IAddon>();
        }
    }
    public interface IAddon {
        public void EnableDisplay();
        public void DisableDisplay();
        public bool Enabled { get; set; }
    }
}