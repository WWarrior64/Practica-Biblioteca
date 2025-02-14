using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica_Biblioteca.Models;

namespace Practica_Biblioteca.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class libroController : ControllerBase
	{
		private readonly bibliotecaContext _bibliotecaContexto;

		public libroController(bibliotecaContext bibliotecaContexto)
		{
			_bibliotecaContexto = bibliotecaContexto;
		}

		/// <summary>
		/// EndPoint que retorna el listado de todos los equipos existentes
		/// </summary>
		[HttpGet]
		[Route("GetAllLibros")]
		public IActionResult GetLibros()
		{
			var listadoLibro = (from e in _bibliotecaContexto.Libro
								join t in _bibliotecaContexto.Autor
										  on e.AutorId equals t.IdAutor
								select new
								{
									e.IdLibro,
									e.Titulo,
									e.AnioPublicacion,
									e.AutorId,
									Nombre_Autor = t.Nombre,
									e.CategoriaId,
									e.Resumen,
								}).OrderBy(resultado => resultado.IdLibro)
									.ThenBy(resultado => resultado.AutorId).ToList();

			if (listadoLibro.Count == 0)
			{
				return NotFound();
			}

			return Ok(listadoLibro);
		}

		/// <summary> 
		/// EndPoint que retorna los registros de una tabla filtrados por su ID 
		/// </summary> 
		/// <param name="id"></param> 
		/// <returns></returns> 
		[HttpGet]
		[Route("GetByIdLibro/{id}")]
		public IActionResult GetLibro(int id)
		{
			var libro = (from e in _bibliotecaContexto.Libro join t in _bibliotecaContexto.Autor on e.AutorId equals t.IdAutor where e.IdLibro == id
							select new
							{
								e.IdLibro,
								e.Titulo,
								e.AnioPublicacion,
								e.AutorId,
								Nombre_Autor = t.Nombre,
								e.CategoriaId,
								e.Resumen,
							}).FirstOrDefault();

			if (libro == null)
			{
				return NotFound();
			}

			return Ok(libro);
		}

		/// <summary> 
		/// EndPoint que retorna los registros de una tabla filtrados por descripcion 
		/// </summary> 
		/// <param name="id"></param> 
		/// <returns></returns> 
		[HttpGet]
		[Route("Find/{filtro}")]

		public IActionResult FindByDescription(string filtro)
		{
			Libro? libro = (from e in _bibliotecaContexto.Libro where e.Resumen.Contains(filtro) select e).FirstOrDefault();

			if (libro == null)
			{
				return NotFound();
			}

			return Ok(libro);
		}

		[HttpPost]
		[Route("Add")]
		public ActionResult GuardarEquipo([FromBody] Libro libro)
		{
			try
			{
				_bibliotecaContexto.Libro.Add(libro);
				_bibliotecaContexto.SaveChanges();
				return Ok(libro);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut]
		[Route("actualizar/{id}")]
		public IActionResult ActualizarEquipo(int id, [FromBody] Libro libroModificar)
		{
			//Para actualizar un registro, se obtiene el registro original de la base de datos 
			//al cual alteraremos alguna propiedad 
			Libro? libroActual = (from e in _bibliotecaContexto.Libro where e.IdLibro == id select e).FirstOrDefault();

			//Verificamos que exista el registro segun su ID 
			if (libroActual == null)
			{
				return NotFound();
			}

			//Si se encuentra el registro, se alteran los campos modificables 
			libroActual.Titulo = libroModificar.Titulo;
			libroActual.AnioPublicacion = libroModificar.AnioPublicacion;
			libroActual.AutorId = libroModificar.AutorId;
			libroActual.CategoriaId = libroModificar.CategoriaId;
			libroActual.Resumen = libroModificar.Resumen;
			//Se marca el registro como modificado en el contexto 
			//y se envia la modificacion a la base de datos 
			_bibliotecaContexto.Entry(libroActual).State = EntityState.Modified;
			_bibliotecaContexto.SaveChanges();
			return Ok(libroModificar);
		}

		[HttpDelete]
		[Route("eliminar/{id}")]
		public IActionResult EliminarEquipo(int id)
		{
			//Para actualizar un registro, se obtiene el registro original de la base de datos 
			//al cual eliminaremos 
			Libro? libro = (from e in _bibliotecaContexto.Libro where e.IdLibro == id select e).FirstOrDefault();

			//Verificamos que exista el registro segun su ID 
			if (libro == null)
				return NotFound();

			//Ejecutamos la accion de elminar el registro 
			_bibliotecaContexto.Libro.Attach(libro);
			_bibliotecaContexto.Libro.Remove(libro);
			_bibliotecaContexto.SaveChanges();
			return Ok(libro);
		}
	}
}
