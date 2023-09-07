namespace MusicBlog.Models;

public class AlbumSong
{
    public int SongId { get; set; }
    public int AlbumId { get; set; }
    public Song Song { get; set; }
    public Album Album { get; set; }
}