using CronProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TestController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            // Intenta abrir una conexión a la base de datos
            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();
                connection.Close();
            }
            return Ok("Conexión exitosa a la base de datos.");
        }
        catch (SqlException ex)
        {
            // Devuelve un error si hay un problema con la conexión
            return StatusCode(500, $"Error al conectar con la base de datos: {ex.Message}");
        }
    }
}
