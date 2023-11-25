using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace PoliBaza.Data;

public class LibraryItem
{
    public LibraryItem Clone()
    {
        return (LibraryItem)MemberwiseClone();
    }

    [Key]
    public Guid Id { get; set; }
    [Display(Name = "Tytuł")]
    [Required(ErrorMessage = "To pole jest wymagane.")]
    [MaxLength(200, ErrorMessage = "To pole może mieć od 3 do 200 znaków")]
    public string? Title { get; set; }
    [Display(Name = "Autor")]
    [Required(ErrorMessage = "To pole jest wymagane.")]
    [StringLength(200, ErrorMessage = "To pole może mieć maksymalnie 200 znaków")]
    public string? Author { get; set; }
    [Display(Name = "Wydawca")]
    [StringLength(200, ErrorMessage = "To pole może mieć maksymalnie 200 znaków")]
    public string? Publisher { get; set; }
    [Display(Name = "Opis")]
    [StringLength(1500, ErrorMessage = "To pole może mieć maksymalnie 1500 znaków")]
    public string? Description { get; set; }
    [Display(Name = "Grafika")]
    public byte[]? Photo { get; set; }
    [Display(Name = "Tagi")]
    [StringLength(200, ErrorMessage = "To pole może mieć maksymalnie 200 znaków")]
    public string? Tags { get; set; }
    [Display(Name = "Spis treści")]
    [StringLength(1500, ErrorMessage = "To pole może mieć maksymalnie 1500 znaków")]
    public string? Contents { get; set; }
    [Display(Name = "Typ zasobu")]
    public Type ItemType { get; set; } = Type.GENERAL;


    public enum Type
    {
        GENERAL, BOOK, MAGAZINE, MULTIMEDIA
    }

    public class Book : LibraryItem
    {
        [Display(Name = "Liczba stron")]
        [Range(0, 2000, ErrorMessage = "Liczba stron musi być z przedziału od 0 do 2000")]
        public int? PageCount { get; set; }
        [Display(Name = "Numer ISBN")]
        [StringLength(200, ErrorMessage = "To pole może mieć maksymalnie 200 znaków")]
        public string? IsbnNumber { get; set; }
        [Display(Name = "Typ zasobu")]
        public new Type ItemType { get; set; } = Type.BOOK;
    }
    
    public class Magazine : LibraryItem
    {
        [Display(Name = "Liczba stron")]
        public int? PageCount { get; set; }
        [Display(Name = "Numer ISBN")]
        [StringLength(200, ErrorMessage = "To pole może mieć maksymalnie 200 znaków")]
        public string? IsbnNumber { get; set; }
        [Display(Name = "Typ zasobu")]
        public new Type ItemType { get; set; } = Type.MAGAZINE;
    }
    
    public class Multimedia : LibraryItem
    {
        [Display(Name = "Czas trwania")]
        [PositiveTimeSpanValidator]
        [TimeSpanValidator]
        public TimeSpan? Duration { get; set; }
        [Display(Name = "Typ zasobu")]
        public new Type ItemType { get; set; } = Type.MULTIMEDIA;
        [Display(Name = "Wytwórnia")]
        [StringLength(1500, ErrorMessage = "To pole może mieć maksymalnie 1500 znaków")]
        public new string? Publisher { get; set; }
    }
    
}