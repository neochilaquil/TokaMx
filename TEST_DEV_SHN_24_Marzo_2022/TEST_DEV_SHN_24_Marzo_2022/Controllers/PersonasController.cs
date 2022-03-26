using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TEST_DEV_SHN_24_Marzo_2022.Models;

namespace TEST_DEV_SHN_24_Marzo_2022.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : Controller
    {
        private readonly PersonasContext _context;
        public PersonasController(PersonasContext context)
        {
            _context = context;
        }

        // GET: PersonasController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonasModel>>> GetAll()
        {
            var Personas = await _context.TbPersonasFisicas.ToListAsync();
            var LstPersonas = (from p in Personas.AsParallel()
                               where p.Activo == true
                               select new {Id = p.IdPersonaFisica, Nombre = p.Nombre, ApellidoPaterno = p.ApellidoPaterno,
                                   ApellidoMaterno = p.ApellidoMaterno, RFC = p.Rfc,
                                   FechaNacimiento = Convert.ToDateTime(p.FechaNacimiento),
                                   UsuarioAgrega = p.UsuarioAgrega }).ToList();

            if (LstPersonas.Count() <= 0)
            {
                return NotFound("No hay datos para mostrar");
            }
            List<PersonasModel> LstReult = new List<PersonasModel>();          

            foreach (var item in LstPersonas)
            {
                LstReult.Add(new PersonasModel
                {
                    IdPersonaFisica = item.Id,
                    Nombre = item.Nombre,
                    ApellidoPaterno = item.ApellidoPaterno,
                    ApellidoMaterno = item.ApellidoMaterno,
                    FechaNacimiento = item.FechaNacimiento,
                    RFC = item.RFC,
                    UsuarioAgrega = Convert.ToInt32(item.UsuarioAgrega)
                });
            }

           return LstReult;                   
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonasModel>> GetById(int id)
        {
            var LstPersonas = await _context.TbPersonasFisicas.Where(p=> p.Activo == true).ToListAsync();

            if (LstPersonas.Count() <= 0)
            {
                return NotFound("No hay datos para mostrar");
            }
            List<PersonasModel> LstReult = new List<PersonasModel>();

            foreach (var item in LstPersonas)
            {
                LstReult.Add(new PersonasModel
                {
                    IdPersonaFisica = item.IdPersonaFisica,
                    Nombre = item.Nombre,
                    ApellidoPaterno = item.ApellidoPaterno,
                    ApellidoMaterno = item.ApellidoMaterno,
                    FechaNacimiento = item.FechaNacimiento,
                    RFC = item.Rfc,
                    UsuarioAgrega = Convert.ToInt32(item.UsuarioAgrega)
                });
            }

            var person = LstReult.AsParallel().Where(p => p.IdPersonaFisica == id).FirstOrDefault();

            //var person = await  _context.TbPersonasFisicas.FindAsync(id);
            if (person == null)
            {
                return NotFound("No se ha encontrado la persona especificada");
            }                

            return person;
        }

        // POST: PersonasController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<PersonasModel>> CreatePerson([FromBody] PersonasModel personaFisica)
        {
            try
            {
                bool flag = false;
                Messages messages = new Messages();
                using (IDbConnection conexion = _context.Database.GetDbConnection())
                {
                    conexion.Open();
                    var parametros = new DynamicParameters();
                    parametros.Add("@Nombre", personaFisica.Nombre);
                    parametros.Add("@ApellidoPaterno", personaFisica.ApellidoPaterno);
                    parametros.Add("@ApellidoMaterno", personaFisica.ApellidoMaterno);
                    parametros.Add("@RFC", personaFisica.RFC);
                    parametros.Add("@FechaNacimiento", personaFisica.FechaNacimiento);
                    parametros.Add("@UsuarioAgrega", personaFisica.UsuarioAgrega);
                   
                    var result = await conexion.QueryAsync<ErrorModel>("dbo.sp_AgregarPersonaFisica", param: parametros, commandType: CommandType.StoredProcedure);

                    foreach (var item in result)
                    {
                        if (!String.IsNullOrEmpty(item.Error))
                        {
                      
                            if (Int32.Parse(item.Error) > 0)
                            {
                                item.Id = Int32.Parse(item.Error);
                                personaFisica.IdPersonaFisica = item.Id;
                                messages.Message = item.MENSAJEERROR;
                                personaFisica.Message = item.MENSAJEERROR;
                                flag = true;
                            
                            }
                            else
                            {
                                flag = false;
                                messages.ErrorCode = item.Error;
                                messages.Message = item.MENSAJEERROR;
                            }

                        }
                    }

                    
                }

                if (flag)
                {
                    return CreatedAtAction("GetById", new { id = personaFisica.IdPersonaFisica }, personaFisica);
                }
                
                return BadRequest(messages);

            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }        

        // POST: PersonasController/Edit/5
        [HttpPut("{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<PersonasModel>> EditPerson(int id, [FromBody] PersonasModel personaFisica)
        {
            try
            {
                if (id != personaFisica.IdPersonaFisica)
                {
                    return BadRequest();
                }
                
                bool flag = false;
                Messages messages = new Messages();
                using (IDbConnection conexion = _context.Database.GetDbConnection())
                {
                    conexion.Open();
                    var parametros = new DynamicParameters();
                    parametros.Add("@IdPersonaFisica", personaFisica.IdPersonaFisica);
                    parametros.Add("@Nombre", personaFisica.Nombre);
                    parametros.Add("@ApellidoPaterno", personaFisica.ApellidoPaterno);
                    parametros.Add("@ApellidoMaterno", personaFisica.ApellidoMaterno);
                    parametros.Add("@RFC", personaFisica.RFC);
                    parametros.Add("@FechaNacimiento", personaFisica.FechaNacimiento);
                    parametros.Add("@UsuarioAgrega", 1);
                    
                    var result = await conexion.QueryAsync<ErrorModel>("dbo.sp_ActualizarPersonaFisica", param: parametros, commandType: CommandType.StoredProcedure);

                    foreach (var item in result)
                    {
                        if (!String.IsNullOrEmpty(item.Error))
                        {

                            if (Int32.Parse(item.Error) > 0)
                            {
                                messages.Message = item.MENSAJEERROR;
                                personaFisica.Message = item.MENSAJEERROR;
                                flag = true;

                            }
                            else
                            {
                                flag = false;
                                messages.ErrorCode = item.Error;
                                messages.Message = item.MENSAJEERROR;
                            }

                        }
                    }


                }

                if (flag)
                {
                    return CreatedAtAction("GetById", new { id = personaFisica.IdPersonaFisica }, personaFisica);
                }

                return BadRequest(messages);

            }
            catch (Exception e)
            {
                return BadRequest();
            }


        }

        

        // POST: PersonasController/Delete/5
        [HttpDelete("{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                var person = await _context.TbPersonasFisicas.FindAsync(id);
                if (person == null || (bool)!person.Activo)
                {
                    return NotFound();
                }
                               
                bool flag = false;
                Messages messages = new Messages();
                using (IDbConnection conexion = _context.Database.GetDbConnection())
                {
                    conexion.Open();
                    var parametros = new DynamicParameters();
                    parametros.Add("@IdPersonaFisica", id);

                    var result = await conexion.QueryAsync<ErrorModel>("dbo.sp_EliminarPersonaFisica", param: parametros, commandType: CommandType.StoredProcedure);

                    if (result.Count() == 0)
                    {
                       flag = true;
                    }
                    else
                    {
                        foreach (var item in result)
                        {
                            if (!String.IsNullOrEmpty(item.Error))
                            {
                                flag = false;
                                messages.ErrorCode = item.Error;
                                messages.Message = item.MENSAJEERROR;

                            }
                        }
                    }

                   

                }

                if (flag)
                {
                    return NoContent();
                }

                return BadRequest(messages);


            }
            catch
            {
                return View();
            }
        }
    }
}
