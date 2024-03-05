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
        private readonly SongRepo repo;

        public SongController(SongRepo repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            var songs = repo.GetAllSongs();
            return View(songs);
        }

        public IActionResult ViewLyrics(int id)
        {
            var song = repo.GetSong(id);
            return View(song);
        }

        public IActionResult UpdateSong(int id)
        {
            Song song = repo.GetSong(id);
            if (song == null)
            {
                return View("SongNotFound");
            }
            return View(song);
        }

        public IActionResult UpdateSongToDatabase(Song song)
        {
            repo.UpdateSong(song);
            return RedirectToAction("Index");
        }

        public IActionResult InsertSong()
        {
            return View();
        }

        public IActionResult InsertSongToDatabase(Song song)
        {
            repo.InsertSong(song);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteSong(Song song)
        {
            repo.DeleteSong(song);
            return RedirectToAction("Index");
        }
    }
}

