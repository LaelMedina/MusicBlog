using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicBlog.Models;

public class Singer
{

    public Singer()
    {
        Albums = new List<Album>();
    }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Age { get; set; }
    public string? Gender { get; set; }
    public string? Poster { get; set; }
    public List<Album> Albums { get; set; }


}