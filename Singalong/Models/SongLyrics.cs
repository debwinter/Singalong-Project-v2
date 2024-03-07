using System;
namespace Singalong.Models
{
	public class SongLyrics
	{
		public int LyricID { get; set; }
		public int SectionID { get; set; }
		public int SongPartID { get; set; }
		public int SongID { get; set; }
		public string Section { get; set; }
		public string Text { get; set; }
        public IEnumerable<Section> Sections { get; set; }
		public string SongTitle { get; set; }
		public IEnumerable<SongLyrics> AllLyricsInThisSong { get; set; }
    }
}

