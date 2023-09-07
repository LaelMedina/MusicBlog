using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicBlog.Models;

public class Album
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Released { get; set; }
    public string? Poster { get; set; }
    public int SingerId { get; set; }
    public Singer? Singer { get; set; }
}