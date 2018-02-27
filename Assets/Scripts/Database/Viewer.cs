using SQLite4Unity3d;

namespace dmdSpirit {
    /// <summary>
    /// Class representation on Viewer table.
    /// </summary>
    public class Viewer {
        [PrimaryKey]
        public string Name { get; set; }
        public int NumberOfMessages { get; set; }
        public int Follower { get; set; }
        public int SpriteId { get; set; }

        /// <summary>
        /// Formatted string representation of Viewer object.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return string.Format("[Viewer: Name={0}, NumberOfMessages={1}, Follower={2}], SpriteId ={3}",
                Name, NumberOfMessages, Follower, SpriteId);
        }
    }
}