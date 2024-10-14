using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public sealed class ToolbarAddonManager : MonoBehaviour {
        [SerializeField] List<Addon> leftToolBarAddons;
        [SerializeField] List<Addon> rightToolBarAddons;
        void Start() {
            InitializeAddonList(leftToolBarAddons);
            InitializeAddonList(rightToolBarAddons);
            //Initialize with the File View
            ToggleAddon(leftToolBarAddons, 2);
        }
        static void InitializeAddonList(IReadOnlyList<Addon> addonList) {
            for (int i = 0; i < addonList.Count; i++) {
                Addon a = addonList[i];
                a.Initialize();
                int index = i;
                a.button.onClick.AddListener(delegate { ToggleAddon(addonList, index); });
            }
        }
        static void ToggleAddon(IReadOnlyList<Addon> addonList, int index) {
            for (int i = 0; i < addonList.Count; i++) {
                if (index == i) {
                    if (addonList[i].addon.Enabled) addonList[i].addon.DisableDisplay();
                     else addonList[i].addon.EnableDisplay();
                } else addonList[i].addon.DisableDisplay();
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