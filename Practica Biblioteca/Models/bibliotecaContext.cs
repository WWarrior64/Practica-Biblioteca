using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Practica_Biblioteca.Models
{
	public class bibliotecaContext : DbContext
	{
		public bibliotecaContext(DbContextOptions<bibliotecaContext> options) : base(options) 
		{
			
		}
	}
}
