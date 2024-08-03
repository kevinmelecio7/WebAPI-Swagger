using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using Microsoft.AspNetCore.Cors;

namespace WebAPI.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly string connectionSQL;

        public UsuarioController(IConfiguration conf)
        {
            connectionSQL = conf.GetConnectionString("Connection");
        }

        [HttpGet]
        public IActionResult ListaUsuario()
        {
            List<Usuario> dtos = new List<Usuario>();
            try
            {
                using (var conexion = new SqlConnection(connectionSQL))
                {
                    conexion.Open();
                    string sql = "SELECT * FROM Usuario;";
                    var cmd = new SqlCommand(sql, conexion);
                    cmd.ExecuteNonQuery();
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            dtos.Add(new Usuario()
                            {
                                Id = Convert.ToInt32(rd["id"]),
                                numCliente = rd["numCliente"].ToString(),
                                nombre = rd["nombre"].ToString(),
                                apellido_p = rd["apellido_p"].ToString(),
                                apellido_m = rd["apellido_m"].ToString(),
                                telefono = rd["telefono"].ToString(),
                                correo = rd["correo"].ToString(),
                                nacimiento = rd["nacimiento"].ToString()
                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", respose = dtos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, respose = dtos });
            }
        }

        [HttpGet]
        [Route("{idUsuario:int}")]
        public IActionResult ObtenerUsuario( int idUsuario)
        {
            List<Usuario> dtos = new List<Usuario>();
            try
            {
                using (var conexion = new SqlConnection(connectionSQL))
                {
                    conexion.Open();
                    string sql = "SELECT * FROM Usuario WHERE id = @id;";
                    var cmd = new SqlCommand(sql, conexion);
                    cmd.Parameters.AddWithValue("@id", idUsuario);
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            dtos.Add(new Usuario()
                            {
                                Id = Convert.ToInt32(rd["id"]),
                                numCliente = rd["numCliente"].ToString(),
                                nombre = rd["nombre"].ToString(),
                                apellido_p = rd["apellido_p"].ToString(),
                                apellido_m = rd["apellido_m"].ToString(),
                                telefono = rd["telefono"].ToString(),
                                correo = rd["correo"].ToString(),
                                nacimiento = rd["nacimiento"].ToString()
                            });
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", respose = dtos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, respose = dtos });
            }
        }


        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Usuario objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(connectionSQL))
                {
                    conexion.Open();
                    string sql = "INSERT INTO Usuario(numCliente, nombre, apellido_p, apellido_m, telefono, correo, nacimiento)" +
                        "VALUES (@numCliente, @nombre, @apellido_p, @apellido_m, @telefono, @correo, @nacimiento);";
                    var cmd = new SqlCommand(sql, conexion);
                    cmd.Parameters.AddWithValue("@numCliente", objeto.numCliente);
                    cmd.Parameters.AddWithValue("@nombre", objeto.nombre);
                    cmd.Parameters.AddWithValue("@apellido_p", objeto.apellido_p);
                    cmd.Parameters.AddWithValue("@apellido_m", objeto.apellido_m);
                    cmd.Parameters.AddWithValue("@telefono", objeto.telefono);
                    cmd.Parameters.AddWithValue("@correo", objeto.correo);
                    DateTime fechaNacimiento = DateTime.Parse(objeto.nacimiento);
                    cmd.Parameters.AddWithValue("@nacimiento", fechaNacimiento);
                    cmd.ExecuteNonQuery();

                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("{idUsuario:int}")]
        public IActionResult Editar([FromBody] Usuario objeto, int idUsuario)
        {
            try
            {
                using (var conexion = new SqlConnection(connectionSQL))
                {
                    conexion.Open();
                    string sql = "UPDATE Usuario set numCliente = @numCliente, nombre = @nombre, apellido_p = @apellido_p, apellido_m = @apellido_m, " +
                                  "telefono  = @telefono, correo = @correo, nacimiento  = @nacimiento " +
                                  "WHERE id = @id;";
                    var cmd = new SqlCommand(sql, conexion);
                    cmd.Parameters.AddWithValue("@id", idUsuario);
                    cmd.Parameters.AddWithValue("@numCliente", objeto.numCliente is null? DBNull.Value : objeto.numCliente);
                    cmd.Parameters.AddWithValue("@nombre", objeto.nombre is null ? DBNull.Value : objeto.nombre);
                    cmd.Parameters.AddWithValue("@apellido_p", objeto.apellido_p is null ? DBNull.Value : objeto.apellido_p);
                    cmd.Parameters.AddWithValue("@apellido_m", objeto.apellido_m is null ? DBNull.Value : objeto.apellido_m);
                    cmd.Parameters.AddWithValue("@telefono", objeto.telefono is null ? DBNull.Value : objeto.telefono);
                    cmd.Parameters.AddWithValue("@correo", objeto.correo is null ? DBNull.Value : objeto.correo);
                    if(objeto.nacimiento is null)
                    {
                        cmd.Parameters.AddWithValue("@nacimiento", DBNull.Value);
                    }
                    else
                    {
                        DateTime fechaNacimiento = DateTime.Parse(objeto.nacimiento);
                        cmd.Parameters.AddWithValue("@nacimiento", fechaNacimiento);
                    }
                    cmd.ExecuteNonQuery();

                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{idUsuario:int}")]
        public IActionResult Eliminar(int idUsuario)
        {
            try
            {
                using (var conexion = new SqlConnection(connectionSQL))
                {
                    conexion.Open();
                    string sql = "DELETE FROM Usuario where id = @id;";
                    var cmd = new SqlCommand(sql, conexion);
                    cmd.Parameters.AddWithValue("@id", idUsuario);
                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }
    }
}
