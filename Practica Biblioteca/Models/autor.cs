﻿using System.ComponentModel.DataAnnotations;
namespace Practica_Biblioteca.Models
{
    public class Autor
    {
        [Key]
        public int IdAutor { get; set; }

        public string Nombre { get; set; }

        public string Nacionalidad { get; set; }
    }
}
