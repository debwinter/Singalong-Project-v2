using System;
using System.Collections.Generic;
using Singalong.Models;

namespace Singalong.Repositories
{
	public interface ISongRepository
	{
		public IEnumerable<Song> GetAllSongs();
		public Song GetSong(int id);
		public IEnumerable<SongLyrics> GetLyrics(int songID);
		public void UpdateSong(Song song);
		public void InsertSong(Song song);
		public void DeleteSong(Song song);
		//public IEnumerable<Section> GetSections();
		//public Song AssignSection(int songID);
	}
}

