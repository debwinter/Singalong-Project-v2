using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Singalong.Repositories;
using Singalong.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Singalong.Controllers
{
    public class SongController : Controller
    {
        private readonly SongRepo songRepo;
        private readonly LyricsRepo lyricsRepo;

        public SongController(SongRepo songRepo, LyricsRepo lyricsRepo)
        {
            this.songRepo = songRepo;
            this.lyricsRepo = lyricsRepo;
        }

        public IActionResult Index()
        {
            var songs = songRepo.GetAllSongs();
            return View(songs);
        }

        public IActionResult ViewLyrics(int id)
        {
            var song = songRepo.GetSong(id);
            return View(song);
        }

        public IActionResult UpdateSong(int id)
        {
            Song song = songRepo.GetSong(id);
            if (song == null)
            {
                return View("SongNotFound");
            }
            return View(song);
        }

        public IActionResult UpdateSongToDatabase(Song song)
        {
            songRepo.UpdateSong(song);
            return RedirectToAction("Index");
        }

        public IActionResult InsertSong()
        {
            return View();
        }

        public IActionResult InsertSongToDatabase(Song song)
        {
            songRepo.InsertSong(song);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteSong(Song song)
        {
            songRepo.DeleteSong(song);
            return RedirectToAction("Index");
        }

        public IActionResult InsertLyrics(int id)
        {
            var lyric = lyricsRepo.AssignSongInfo(id);
            return View(lyric);
        }

        public IActionResult InsertLyricsToDatabase(SongLyrics lyric)
        {
            lyricsRepo.InsertLyric(lyric);
            return RedirectToAction("ViewLyrics", "Song", new {id = lyric.SongID});
        }

        public IActionResult UpdateLyrics(int id)
        {
            var lyric = lyricsRepo.GetLyric(id);
            return View(lyric);
        }

        public IActionResult UpdateLyricsToDatabase(SongLyrics lyric)
        {
            lyricsRepo.UpdateLyric(lyric);
            return RedirectToAction("ViewLyrics", "Song", new { id = lyric.SongID });
        }

        public IActionResult DeleteLyrics(int id)
        {
            var songID = lyricsRepo.GetSongID(id);
            lyricsRepo.DeleteLyric(id);
            return RedirectToAction("ViewLyrics", "Song", new { id = songID });
        }
    }
}

