using Intuit.Api.Dtos;
using Intuit.Api.Interfaces;
using Intuit.Api.Logging;
using Intuit.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intuit.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ILogService<LogService> _log;
        private readonly IClientService _svc;

        public ClientsController(ILogService<LogService> log, IClientService svc)
        {
            _log = log;
            _svc = svc;
        }

        /// <summary>
        /// Mapea el resultado del servicio al ActionResult correspondiente.
        /// </summary>
        private ActionResult Map(ServiceResult res) => res.Status switch
        {
            StatusCodes.Status200OK => res.Data is not null ? Ok(res.Data) : Ok(new { res.Result, res.Message }),
            StatusCodes.Status404NotFound => NotFound(res.Message),
            StatusCodes.Status409Conflict => Conflict(res.Message),
            StatusCodes.Status400BadRequest => BadRequest(res.Message),
            StatusCodes.Status204NoContent => NoContent(),
            _ => StatusCode(res.Status, res.Message ?? "Unexpected error")
        };

        /// <summary>
        /// Obtiene todos los clientes.
        /// </summary>
        [HttpGet("getClients", Name = "GetClients")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ClientReadDto>))]
        public async Task<ActionResult> GetAll()
            => Map(await _svc.GetAllClients());

        /// <summary>
        /// Obtiene un cliente por id.
        /// </summary>
        [HttpGet("getClientById/{id:int}", Name = "GetClientById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
            => Map(await _svc.GetByClientId(id));

        /// <summary>
        /// Busca por nombre/apellido (substring, case-insensitive).
        /// </summary>
        [HttpGet("searchClients", Name = "SearchClients")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ClientReadDto>))]
        public async Task<ActionResult> Search([FromQuery] string name = "")
            => Map(await _svc.SearchClientByNameAsync(name));

        /// <summary>
        /// Crea un cliente.
        /// </summary>
        [HttpPost("createClient", Name = "CreateClient")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ClientReadDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] ClientCreateDto dto)
        {
            var res = await _svc.AddNewClients(dto);

            if (res.Result && res.Data is ClientReadDto created)
            {
                _log.LogInfo($"[Controller] 201 -> id={created.ClientId}");
                return CreatedAtAction(nameof(GetById), new { id = created.ClientId }, created);
            }

            return Map(res);
        }

        /// <summary>
        /// Actualiza un cliente.
        /// </summary>
        [HttpPut("updateClient", Name = "UpdateClient")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Update([FromBody] ClientUpdateDto dto)
        {
            return Map(await _svc.UpdateClient(dto));
        }

        /// <summary>
        /// Elimina un cliente.
        /// </summary>
        [HttpDelete("deleteClient/{id:int}", Name = "DeleteClient")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
            => Map(await _svc.DeleteClient(id));

    }
}
