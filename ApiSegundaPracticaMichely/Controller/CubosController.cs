using ApiSegundaPracticaMichely.Models;
using ApiSegundaPracticaMichely.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiSegundaPracticaMichely.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {
        private RepositoryCubo repo;

        public CubosController(RepositoryCubo repo)
        {
            this.repo = repo;
        }

      //DEVUELVE TODOS LOS CUBOS
        [HttpGet]
        public async Task<ActionResult<List<Cubo>>>
            GetEmpleados()
        {
            return await this.repo.GetCubosAsync();
        }
        //Buscar cubos por marca

        [HttpGet]
        [Route("[action]/{marca}")]
        public async Task<ActionResult<List<Cubo>>> CubosMarca(string marca)
        {
            return await this.repo.FindCubosMarcaAsync(marca);
        }
        [HttpGet]
        [Route("[action]/marcas")]
        public async Task<ActionResult<List<string>>> PersonajesSerie()
        {
            return await this.repo.GetMarcasAsync();
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult>
           InsertPersonaje(UsuarioCubosModel user)
        {
            await this.repo.InsertUsuarioAsync(user.Nombre, user.Email, user.Password, user.Imagen);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<UsuariosCubo>>
            PerfilEmpleado()
        {
            //INTERNAMENTE, CUANDO RECIBIMOS EL TOKEN 
            //EL USUARIO ES VALIDADO Y ALMACENA DATOS 
            //COMO HttpContext.User.Identity.IsAuthenticated
            //COMO HEMOS INCLUIDO LA KEY DE LOS Claims, 
            //AUTOMATICAMENTE TAMBIEN TENEMOS DICHOS CLAIMS
            //COMO EN LAS APLICACIONES MVC
            Claim claim = HttpContext.User
                .FindFirst(x => x.Type == "UserData");
            //RECUPERAMOS EL JSON DEL EMPLEADO
            string jsonEmpleado = claim.Value;
            UsuariosCubo empleado =
                JsonConvert.DeserializeObject<UsuariosCubo>(jsonEmpleado);
            return empleado;
        }
        [Authorize]
        [HttpGet]
        [Route("[action]/pedidosusuario")]
        public async Task<ActionResult<List<ComprasCubo>>>
           PedidosUsuario()
        {
            //INTERNAMENTE, CUANDO RECIBIMOS EL TOKEN 
            //EL USUARIO ES VALIDADO Y ALMACENA DATOS 
            //COMO HttpContext.User.Identity.IsAuthenticated
            //COMO HEMOS INCLUIDO LA KEY DE LOS Claims, 
            //AUTOMATICAMENTE TAMBIEN TENEMOS DICHOS CLAIMS
            //COMO EN LAS APLICACIONES MVC
            Claim claim = HttpContext.User
                .FindFirst(x => x.Type == "UserData");
            //RECUPERAMOS EL JSON DEL EMPLEADO
            string jsonUsuario = claim.Value;
            UsuariosCubo usuario =
                JsonConvert.DeserializeObject<UsuariosCubo>(jsonUsuario);
           return await this.repo.FindPedidosUsuarioAsync(usuario.IdUsuario);

        }



    }
}
