using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicBlog.Models;
public class Song
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Released { get; set; }
    public string? Lyrics { get; set; }
    public string? Poster { get; set; }
}
