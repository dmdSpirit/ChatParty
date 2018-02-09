using SQLite4Unity3d;

public class SpriteDictionary{
	[PrimaryKey]
	public int id { get; set;}
	public string name { get; set;}

	public override string ToString () {
		return string.Format ("[SpriteDictionary: Id={0}, Name={1}, Follower={2}], SpriteId ={3}", 
			id, name);
	}
}