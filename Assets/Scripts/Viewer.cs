using SQLite4Unity3d;

public class Viewer{
	[PrimaryKey]
	public string Name { get; set;}
	public int NumberOfMessages { get; set;}
	public int Follower { get; set;}

	public override string ToString () {
		return string.Format ("[Viewer: Name={0}, NumberOfMessages={1}, Follower={2}]", 
			Name, NumberOfMessages, Follower);
	}
}
