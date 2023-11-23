using System.ComponentModel.DataAnnotations;

namespace PoliBaza.Data;

public class LibraryItem
{
    [Key]
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Publisher { get; set; }
    public string? Description { get; set; }
    public byte[]? Photo { get; set; }
    public string[]? Tags { get; set; }
    public string? Contents { get; set; }
    public Type ItemType { get; set; } = Type.GENERAL;
    
    
    public enum Type { GENERAL, BOOK, MAGAZINE, MULTIMEDIA}

    public class Book : LibraryItem
    {
        public int? PageCount { get; set; }
        public string? IsbnNumber { get; set; }
        public new Type ItemType { get; set; } = Type.BOOK;
    }
    
    public class Magazine : LibraryItem
    {
        public int? PageCount { get; set; }
        public string? IsbnNumber { get; set; }
        public new Type ItemType { get; set; } = Type.MAGAZINE;
    }
    
    public class Multimedia : LibraryItem
    {
        public TimeSpan? Duration { get; set; }
        public new Type ItemType { get; set; } = Type.MULTIMEDIA;
    }
    
}