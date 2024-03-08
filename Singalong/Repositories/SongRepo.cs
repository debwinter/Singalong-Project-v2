using System;
using System.Data;
using Dapper;
using Singalong.Models;

namespace Singalong.Repositories
{
	public class SongRepo : ISongRepository
	{
        private readonly IDbConnection _conn;

        public SongRepo(IDbConnection conn)
		{
            _conn = conn;
		}

        //public Song AssignSection(int songID)
        //{
        //    var sectionList = GetSections();
        //    var song = new Song();
        //    song.Sections = sectionList;
        //    return song;
        //}

        public void DeleteSong(Song song)
        {
            _conn.Execute("DELETE FROM SongParts WHERE SongID = @id;", new {id = song.SongID});
            _conn.Execute("DELETE FROM Lyrics WHERE SongID = @id;", new { id = song.SongID });
            _conn.Execute("DELETE FROM SongTags WHERE SongID = @id;", new { id = song.SongID });
            _conn.Execute("DELETE FROM Songs WHERE SongID = @id;", new { id = song.SongID });
        }

        public IEnumerable<Song> GetAllSongs()
        {
            var allSongs = _conn.Query<Song>("SELECT * FROM Songs ORDER BY Title;");
            foreach (var song in allSongs)
            {
                song.Lyrics = GetLyrics(song.SongID);
            }
            return allSongs;
        }

        public IEnumerable<SongLyrics> GetLyrics(int songID)
        {
            return _conn.Query<SongLyrics>("SELECT SongPartID, sp.SectionID AS SectionID, l.LyricID AS LyricID, Name AS Section, Text " +
                "FROM SongParts as sp " +
                "LEFT JOIN Lyrics as l ON sp.LyricID = l.LyricID " +
                "INNER JOIN Sections as sect ON l.SectionID = sect.SectionID " +
                "WHERE l.SongID = @id;",
                new { id = songID });
        }

        public IEnumerable<Section> GetSections()
        {
            return _conn.Query<Section>("SELECT * FROM Sections;");
        }

        public Song GetSong(int id)
        {
            var song = _conn.QuerySingle<Song>("SELECT * FROM Songs WHERE SongID = @songID;", new { songID = id });
            song.Lyrics = GetLyrics(id);
            return song;
        }

        public void InsertSong(Song song)
        {
            _conn.Execute("INSERT INTO Songs (Title, Composer, Artist, YouTube, Spotify) " +
                "VALUES (@newTitle, @newComposer, @newArtist, @newYoutube, @newSpotify);",
                new { newTitle = song.Title, newComposer = song.Composer, newArtist = song.Artist,
                    newYoutube = song.YouTube, newSpotify = song.Spotify });
        }

        public void UpdateLyric(SongLyrics lyric)
        {
            _conn.Execute("UPDATE lyrics SET sectionID = @newSection, text = @newText WHERE lyricID = @id;", new { newSection = lyric.SectionID, newText = lyric.Text, id = lyric.LyricID });
        }

        public void UpdateSong(Song song)
        {
            var originalArtist = _conn.QuerySingle<string>("SELECT Artist FROM Songs WHERE SongID = @id;", new { id = song.SongID });
            if (originalArtist == null && song.Artist == song.Composer)
            {
                _conn.Execute("UPDATE songs SET " +
                "title=@newTitle, composer=@newComposer, " +
                "youtube=@youtubeLink, spotify=@spotifyLink " +
                "WHERE songID = @id;", new
                {
                    newTitle = song.Title,
                    newComposer = song.Composer,
                    youtubeLink = song.YouTube,
                    spotifyLink = song.Spotify,
                    id = song.SongID
                });
            }
            else
            {
                _conn.Execute("UPDATE songs SET " +
                "title=@newTitle, composer=@newComposer, artist=@newArtist, " +
                "youtube=@youtubeLink, spotify=@spotifyLink " +
                "WHERE songID = @id;", new
                {
                    newTitle = song.Title,
                    newComposer = song.Composer,
                    newArtist = song.Artist,
                    youtubeLink = song.YouTube,
                    spotifyLink = song.Spotify,
                    id = song.SongID
                });
            }   
        }
    }
}

