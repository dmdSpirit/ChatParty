using SQLite4Unity3d;

namespace dmdSpirit {
    /// <summary>
    /// Class representation on SpriteDictionary table.
    /// </summary>
    public class SpriteDictionary {
        [PrimaryKey]
        public int id { get; set; }
        public string name { get; set; }

        /// <summary>
        /// Formatted string representation of SpriteDictionary object.
        /// </summary>
        /// <returns>Formatted string.</returns>
        public override string ToString() {
            return string.Format("[SpriteDictionary: Id={0}, Name={1}, Follower={2}], SpriteId ={3}",
                id, name);
        }
    }
}