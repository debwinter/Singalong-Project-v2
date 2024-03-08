using System;
using System.Data;
using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Singalong.Models;

namespace Singalong.Repositories
{
	public class LyricsRepo : ILyricsRepository
	{
        private readonly IDbConnection _conn;

        public LyricsRepo(IDbConnection conn)
        {
            _conn = conn;
        }

        public IEnumerable<SongLyrics> GetAllLyrics()
        {
            return _conn.Query<SongLyrics>("SELECT LyricID, l.SectionID as SectionID, " +
                "l.SongID as SongID, s.Name as Section, Text " +
                "FROM Lyrics as l " +
                "INNER JOIN Sections as s ON l.SectionID = s.SectionID;");
        }

        public void InsertLyric(SongLyrics lyric)
        {
            bool alreadyEntered = false;
            var fullDatabase = GetAllLyrics();
            
            foreach (var item in fullDatabase)
            {
                if (item.Text == lyric.Text) alreadyEntered = true;
            }

            if (!alreadyEntered)
            {
                _conn.Execute("INSERT INTO Lyrics (SongID, SectionID, Text) " +
                "VALUES (@songID, @sectionID, @newText);", new
                {
                    songID = lyric.SongID,
                    sectionID = lyric.SectionID,
                    newText = lyric.Text
                });
            }

            int lyricID = _conn.QuerySingle<int>("SELECT LyricID FROM Lyrics " +
                "WHERE Text = @newText AND SongID = @song AND SectionID = @section;",
                new { newText = lyric.Text, song = lyric.SongID, section = lyric.SectionID });

            _conn.Execute("INSERT INTO SongParts (SongID, SectionID, LyricID) " +
                "VALUES (@songID, @sectionID, @newLyricID);", new
                {
                    songID = lyric.SongID,
                    sectionID = lyric.SectionID,
                    newLyricID = lyricID
                });
        }
        public IEnumerable<Section> GetSections()
        {
            return _conn.Query<Section>("SELECT * FROM Sections;");
        }

        public SongLyrics AssignSongInfo(int songID)
        {
            var songRepo = new SongRepo(_conn);
            var thisSong = songRepo.GetSong(songID);
            var lyric = new SongLyrics();
            lyric.SongID = songID;
            lyric.SongTitle = thisSong.Title;
            lyric.AllLyricsInThisSong = thisSong.Lyrics;
            lyric.Sections = GetSections();
            return lyric;
        }

        public void UpdateLyric(SongLyrics lyric)
        {
            _conn.Execute("UPDATE Lyrics SET Text = @newText WHERE LyricID = @id;",
                new { newText = lyric.Text, id = lyric.LyricID });
        }

        public void DeleteLyric(int songPartID)
        {
            _conn.Execute("DELETE FROM SongParts WHERE SongPartID = @id;", new { id = songPartID });
        }

        //public void DeleteLyric(SongLyrics lyric)
        //{
            //var songParts = _conn.Query<int>("SELECT SongPartID FROM SongParts WHERE LyricID = @id;", new { id = lyric.LyricID });
            //foreach (var sp in songParts)
            //{
            //    _conn.Execute("DELETE FROM SongParts WHERE SongPartID = @id;", new { id = sp });
            //}
            //if (songParts.Count() == 1)
            //{
            //    _conn.Execute("DELETE FROM Lyrics WHERE LyricID = @id;", new { id = lyric.LyricID });
            //}
        //}

        public SongLyrics GetLyric(int songPartID)
        {
            return _conn.QuerySingle<SongLyrics>("SELECT l.LyricID AS LyricID, " +
                "l.SectionID as SectionID, l.SongID as SongID, " +
                "SongPartID, s.Name as Section, Text " +
                "FROM SongParts as sp " +
                "INNER JOIN Lyrics as l ON l.LyricID = sp.LyricID " +
                "INNER JOIN Sections as s ON l.SectionID = s.SectionID " +
                "WHERE SongPartID = @id;", new { id = songPartID });
        }

        public int GetSongID(int songPartID)
        {
            return _conn.QuerySingle<int>("SELECT SongID FROM SongParts WHERE SongPartID = @id;", new { id = songPartID });
        }
    }
}

