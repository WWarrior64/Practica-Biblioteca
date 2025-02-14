using System.ComponentModel.DataAnnotations;
namespace Practica_Biblioteca.Models
{
    public class Libro
    {


        [Key]
        public int IdLibro { get; set; }

        public string Titulo { get; set; }

        public int AnioPublicacion { get; set; }

        public int? AutorId { get; set; }

        public int? CategoriaId { get; set; }

        public string Resumen { get; set; }

    }
}
