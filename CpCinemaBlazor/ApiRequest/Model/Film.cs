using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CpCinemaBlazor.ApiRequest.Model
{
    public class FilmShort
    {
        public int id_Film { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Namegenre { get; set; }
        public DateTime DateCreate { get; set; }
        public double Rating { get; set; }
        public bool Edit { get; set; } = false;
    }

    public class Film
    {
        public int id_Film { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Namegenre { get; set; }
        public DateTime DateCreate { get; set; }
        public double Rating { get; set; }
    }

    public class FilmData
    {
        public bool Status { get; set; }
        public FilmDataContainer Data { get; set; }
    }

    public class FilmDataContainer
    {
        public List<FilmShort> movies { get; set; }
    }

    public class AddFilm
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public string Namegenre { get; set; }
        public DateTime DateCreate { get; set; }
        public double Rating { get; set; }
    }
}
