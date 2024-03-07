using System;
using Singalong.Models;
namespace Singalong.Repositories
{
	public interface ILyricsRepository
	{
		public IEnumerable<SongLyrics> GetAllLyrics();
		public IEnumerable<Section> GetSections();
		public void InsertLyric(SongLyrics lyric);
		public SongLyrics AssignSongInfo(int songID);
		public void UpdateLyric(SongLyrics lyric);
		public void DeleteLyric(int songPartID);
		public SongLyrics GetLyric(int songPartID);
    }
}

