using System;
namespace Singalong.Models
{
	public class Song
	{
		public int SongID { get; set; }
		public string Title { get; set; }
		public string Composer { get; set; }
		public string Artist { get; set; }
		public string YouTube { get; set; }
		public string Spotify { get; set; }
		public IEnumerable<SongLyrics> Lyrics { get; set; }
    }
}

