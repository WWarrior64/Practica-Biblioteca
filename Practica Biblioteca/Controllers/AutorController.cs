using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Practica_Biblioteca.Models;
using Microsoft.EntityFrameworkCore;
namespace Practica_Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {

        private readonly bibliotecaContext _bibliotecaContexto;

        /// <summary>
        /// EndPoint que retorna el listado de todos los equipos existentes
        /// </summary>
        /// 
        public AutorController(bibliotecaContext bibliotecaContexto)
        {
            _bibliotecaContexto = bibliotecaContexto;
        }

        [HttpGet]
        [Route("GetAllAutores")]
        public IActionResult GetAutores()
        {
            var listadoAutor = (from e in _bibliotecaContexto.Autor select e).ToList();

            if (listadoAutor.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoAutor);
        }

        /// <summary> 
		/// EndPoint que retorna los registros de una tabla filtrados por su ID 
		/// </summary> 
		/// <param name="id"></param> 
		/// <returns></returns> 
		[HttpGet]
        [Route("GetByIdAutor/{id}")]
        public IActionResult GetAutor(int id)
        {
            var autorConLibros = (from a in _bibliotecaContexto.Autor
                                  where a.IdAutor == id
                                  select new
                                  {
                                      a.IdAutor,
                                      a.Nombre,
                                      a.Nacionalidad,
                                      Libros = (from l in _bibliotecaContexto.Libro
                                                where l.AutorId == a.IdAutor
                                                select new
                                                {
                                                    l.IdLibro,
                                                    l.Titulo,
                                                    l.AnioPublicacion,
                                                    l.Resumen
                                                }).ToList()
                                  }).FirstOrDefault();

            if (autorConLibros == null)
            {
                return NotFound();
            }

            return Ok(autorConLibros);
        }




        //Metodo para guardar el registro 

        [HttpPost]
        [Route("AddAutor")]
        public IActionResult GuardarAutor([FromBody] Autor Autor)
        {

            try
            {
                _bibliotecaContexto.Add(Autor);
                _bibliotecaContexto.SaveChanges();
                return Ok(Autor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //Metodo para modificar
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarAutor(int id, [FromBody] Autor autorModificar)
        {

            //Actualizar registro original de BD
            Autor? autorActual = (from e in _bibliotecaContexto.Autor
                                     where e.IdAutor == id
                                     select e).FirstOrDefault();

            //Verificamos que exista el registro
            if (autorActual == null)
            { return NotFound(); }

            //Si se encuentra el registro se alteran los campos modificables
            autorActual.Nombre = autorModificar.Nombre;
            autorActual.Nacionalidad = autorModificar.Nacionalidad;


            //Se marca el registro como modificado y se envia la notificacion 
            // a la BD ****************
            _bibliotecaContexto.Entry(autorActual).State = EntityState.Modified;
            _bibliotecaContexto.SaveChanges();

            return Ok(autorModificar);

        }

        //Metodo para eliminar 
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult ElimiarAutor(int id)
        {
            //Se obtiene el registro que se desea actualizar
            Autor? Autor = (from e in _bibliotecaContexto.Autor
                               where e.IdAutor == id
                               select e).FirstOrDefault();

            if (Autor == null)
                return NotFound();

            _bibliotecaContexto.Autor.Attach(Autor);
            _bibliotecaContexto.Autor.Remove(Autor);
            _bibliotecaContexto.SaveChanges();

            return Ok(Autor);

        }
    }
}
