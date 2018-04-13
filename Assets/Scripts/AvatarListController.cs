using UnityEngine;
using System;
using System.Collections.Generic;

namespace dmdSpirit {
    /// <summary>
    /// Component for handling list of all Avatars.
    /// </summary>
    public class AvatarListController : MonoSingleton<AvatarListController> {
        [SerializeField]
        bool addTestViewers = true;
        [SerializeField]
        DebugLogType logType = DebugLogType.Full;
        [SerializeField]
        GameObject avatarPrefab;
        [SerializeField]
        int maxTestViewers = 5;

        // We assume that we always get DisplayedName (with correct uppercase) from TwitchChat
        Dictionary<string, AvatarController> viewerDictionary;
        List<string> viewerLeftList;
        List<Tuple<string, string>> commandList;
        List<Tuple<string, string>> messageList;
        // FIXME: Replace simple last joined-first drawn logic.
        int avatarSortOrder = 1;

        private void Awake() {
            viewerDictionary = new Dictionary<string, AvatarController>();
            viewerLeftList = new List<string>();
            commandList = new List<Tuple<string, string>>();
            messageList = new List<Tuple<string, string>>();
        }

        private void Start() {
#if UNITY_EDITOR
            if (addTestViewers)
                AddTestViewers();
#endif
#if !UNITY_EDITOR
        logType = DebugLogType.Full;
#endif
            CheckIsSingleInScene();
        }

        private void Update() {
            for (int i = viewerLeftList.Count - 1; i >= 0; i--) {
                if (viewerDictionary.ContainsKey(viewerLeftList[i])) {
                    RemoveAvatar(viewerLeftList[i]);
                    viewerLeftList.Remove(viewerLeftList[i]);
                }
            }
            for (int i = messageList.Count - 1; i >= 0; i--) {
                string viewerName = messageList[i].Item1;
                string message = messageList[i].Item2;
                if (viewerDictionary.ContainsKey(viewerName)) {
                    viewerDictionary[viewerName].AddMessage(message);
                    messageList.Remove(messageList[i]);
                }
            }
            for (int i = commandList.Count - 1; i >= 0; i--) {
                string viewerName = messageList[i].Item1;
                string command = messageList[i].Item2;
                if (viewerDictionary.ContainsKey(viewerName)) {
                    viewerDictionary[viewerName].AddCommand(command);
                    messageList.Remove(messageList[i]);
                }
            }
        }

        /// <summary>
        /// Add new viewer.
        /// </summary>
        /// <param name="viewerName">Viewer Name.</param>
        public void AddViewer(string viewerName) {
            // TODO: Implement black list for viewer names.
            if (viewerName == "SeniorPomidor")
                return;
            // There is a chance that we can get UserLeft event before UserJoined. 
            // In this case we should not create new Avatar.
            if (viewerLeftList.Contains(viewerName))
                viewerLeftList.Remove(viewerName);
            else
                CreateAvatar(viewerName);
        }

        /// <summary>
        /// Remove viewer from viewer list.
        /// </summary>
        /// <param name="viewerName">Viewer Name.</param>
        public void RemoveViewer(string viewerName) {
            // If we can't find leftViewer in viewerDictionary in means that we got
            // UserLeft event before getting UserJoined, it will be handled by joinedViewerList
            // later. Nothing to remove. There is also slight chance that user can change his name
            // while having active avatar, but it is too small to bother.
            if (viewerDictionary.ContainsKey(viewerName))
                RemoveAvatar(viewerName);
            else
                viewerLeftList.Add(viewerName);
        }

        /// <summary>
        /// Add message to be shown by Avatar.
        /// </summary>
        /// <param name="messagePair">Item1: Viewer Name, Item2: Message.</param>
        public void AddMessage(Tuple<string, string> messagePair) {
            string viewerName = messagePair.Item1;
            string message = messagePair.Item2;
            // We can still can have MessageEvent before UserJoined.
            if (viewerDictionary.ContainsKey(viewerName))
                viewerDictionary[viewerName].AddMessage(message);
            else
                messageList.Add(messagePair);
        }

        /// <summary>
        /// Add command to be performed by Avatar.
        /// </summary>
        /// <param name="commandPair">Item1: Viewer Name, Item2: Command.</param>
        public void AddCommand(Tuple<string, string> commandPair) {
            string viewerName = commandPair.Item1;
            string command = commandPair.Item2;
            // We can still can have CommandEvent before UserJoined.
            if (viewerDictionary.ContainsKey(viewerName))
                viewerDictionary[viewerName].AddCommand(command);
            else
                commandList.Add(commandPair);
        }

        private void CreateAvatar(string viewerName) {
            Viewer viewer = ViewerBaseController.Instance.GetViewer(viewerName);
            GameObject avatarGO = Instantiate(avatarPrefab);
            AvatarController avatarController = avatarGO.GetComponent<AvatarController>();
            if (avatarController == null)
                Logger.LogMessage("AvatarListController::CreateAvatar -- " +
                    "Avatar prefab does not have AvatarController script attached.", LogType.Error);
            else {
                avatarController.viewer = viewer;
                avatarController.spriteSortOrder = avatarSortOrder++;
                viewerDictionary.Add(viewerName, avatarController);
                if (logType == DebugLogType.Full)
                    Logger.LogMessage($"{viewerName} Avatar was created");
            }
        }

        private void RemoveAvatar(string viewerName) {
            viewerDictionary[viewerName].DestroyAvatar();
            viewerDictionary.Remove(viewerName);
            if (logType == DebugLogType.Full)
                Logger.LogMessage($"{viewerName} Avatar was destroyed");
        }

        private void AddTestViewers() {
            if (logType == DebugLogType.Full)
                Logger.LogMessage($"Adding {maxTestViewers} test viewers.");
            foreach (var viewer in ViewerBaseController.Instance.GetAllViewers()) {
                CreateAvatar(viewer.Name);
                if (viewerDictionary.Count >= maxTestViewers)
                    break;
            }
        }

        public List<BrainController> GetAllFighters() {
            List<BrainController> fightersList = new List<BrainController>();
            foreach (var viewerPair in viewerDictionary)
                fightersList.Add(viewerPair.Value.brainController);
            return fightersList;
        }
    }
}
