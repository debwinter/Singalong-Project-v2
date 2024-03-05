using System;
namespace Singalong.Models
{
	public class SongParts
	{
		public int SongPartID { get; set; }
		public int SongID { get; set; }
		public int SectionID { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public int LyricID { get; set; }
	}
}

