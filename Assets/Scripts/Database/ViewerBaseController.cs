using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

namespace dmdSpirit {
    /// <summary>
    /// System component responsible for DataBase interaction.
    /// </summary>
    public class ViewerBaseController : MonoSingleton<ViewerBaseController> {
        [SerializeField]
        int goldenArmorId;

        int maxSpriteId;
        SQLiteConnection connection;

        private void Awake() {
            CheckIsSingleInScene();
            string dbPath = Application.dataPath;
            dbPath = dbPath.Substring(0, dbPath.LastIndexOf("/")) + "/DataBase/TwitchChatDB.db";
            Logger.LogMessage("ViewerBaseController :: Creating db connection with path: " + dbPath);
            connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite);
        }

        private void Start() {
            maxSpriteId = connection.Table<SpriteDictionary>().Where(s => s.id != goldenArmorId).Count();
        }

        private void OnApplicationQuit() {
            Logger.LogMessage("ViewerBaseController :: Closing db connection.");
            if (connection != null)
                connection.Close();
        }

        /// <summary>
        /// Search DataBase for existing viewer by name. If non found create new.
        /// </summary>
        /// <returns>Viewer.</returns>
        /// <param name="name">Name.</param>
        public Viewer GetViewer(string name) {
            Viewer viewer = connection.Table<Viewer>().Where(v => v.Name == name).FirstOrDefault();
            if (viewer == null)
                viewer = CreateViewer(name);
            else {
                if (viewer.SpriteId != goldenArmorId) {
                    viewer.SpriteId = Random.Range(1, maxSpriteId + 1);
                    connection.Update(viewer);
                }
            }
            return viewer;
        }

        /// <summary>
        /// Gets all viewers.
        /// </summary>
        /// <returns>All viewers.</returns>
        public IEnumerable<Viewer> GetAllViewers() {
            return connection.Table<Viewer>();
        }

        /// <summary>
        /// Gets viewer Sprite name from DataBase.
        /// </summary>
        /// <returns>Sprite name.</returns>
        /// <param name="viewer">Viewer.</param>
        public string GetSpriteName(Viewer viewer) {
            SpriteDictionary sprite = connection.Table<SpriteDictionary>().Where(v => v.id == viewer.SpriteId).First();
            return sprite.name;
        }

        Viewer CreateViewer(string viewerName) {
            int newSpriteID = Random.Range(1, maxSpriteId + 1);
            Viewer newViewer = new Viewer { Name = viewerName, NumberOfMessages = 0, Follower = 0, SpriteId = newSpriteID };
            connection.Insert(newViewer);
            return newViewer;
        }
    }
}