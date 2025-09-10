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
        private readonly ILogService<LogService> _logService;
        private readonly IClientService _clientService;

        public ClientsController(ILogService<LogService> logService, IClientService clientService)
        {
            _logService = logService;
            _clientService = clientService;
        }

        /// <summary>
        /// Retrieves all clients.
        /// </summary>
        [HttpGet("clients")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(OperationResult))]
        public async Task<ActionResult<IEnumerable<ClientReadDto>>> GetAllClients()
        {
            try
            {
                var clients = await _clientService.GetAllClients();

                if (clients == null || !clients.Any())
                {
                    return NotFound(new OperationResult
                    {
                        Result = false,
                        Status = 404,
                        Title = "No clients found",
                        Detail = "There are no clients available in the system."
                    });
                }

                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logService.LogError($"[ClientsController] - An error occurred while retrieving all clients: {ex.Message}");
                return StatusCode(500, new OperationResult
                {
                    Result = false,
                    Status = 500,
                    Title = "Internal server error",
                    Detail = "An unexpected error occurred. Please try again later."
                });
            }
        }

        /// <summary>
        /// Retrieves a client by their ID.
        /// </summary>
        [HttpGet("client/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(OperationResult))]
        public async Task<ActionResult<ClientReadDto>> GetByClientId([FromRoute] int id)
        {
            try
            {
                var client = await _clientService.GetByClientId(id);

                if (client == null)
                {
                    return NotFound(new OperationResult
                    {
                        Result = false,
                        Status = 404,
                        Title = "Client not found",
                        Detail = $"No client found with ID {id}."
                    });
                }

                return Ok(client);
            }
            catch (Exception ex)
            {
                _logService.LogError($"[ClientsController] - An error occurred while retrieving client with ID {id}: {ex.Message}");
                return StatusCode(500, new OperationResult
                {
                    Result = false,
                    Status = 500,
                    Title = "Internal server error",
                    Detail = "An unexpected error occurred. Please try again later."
                });
            }
        }

        /// <summary>
        /// Searches for clients by their name.
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(OperationResult))]
        public async Task<ActionResult<IEnumerable<ClientReadDto>>> SearchClientByName([FromQuery] string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest(new OperationResult
                    {
                        Result = false,
                        Status = 400,
                        Title = "Invalid search parameter",
                        Detail = "The 'name' query parameter cannot be null or empty."
                    });
                }

                var clients = await _clientService.SearchClientByNameAsync(name);

                if (clients == null || !clients.Any())
                {
                    return NotFound(new OperationResult
                    {
                        Result = false,
                        Status = 404,
                        Title = "No clients found",
                        Detail = $"No clients found matching the name '{name}'."
                    });
                }

                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logService.LogError($"[ClientsController] - An error occurred while searching for clients by name: {ex.Message}");
                return StatusCode(500, new OperationResult
                {
                    Result = false,
                    Status = 500,
                    Title = "Internal server error",
                    Detail = "An unexpected error occurred. Please try again later."
                });
            }
        }

        /// <summary>
        /// Adds a new client.
        /// </summary>
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(OperationResult))]
        public async Task<ActionResult<ClientReadDto>> AddNewClient([FromBody] ClientCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new OperationResult
                {
                    Result = false,
                    Status = 400,
                    Title = "Invalid client data",
                    Detail = "The client data provided is null."
                });
            }
            try
            {
                var result = await _clientService.AddNewClients(dto);
                if (!result)
                {
                    return StatusCode(500, new OperationResult
                    {
                        Result = false,
                        Status = 500,
                        Title = "Failed to add client",
                        Detail = "An error occurred while adding the new client."
                    });
                }

                return Ok(new OperationResult { Result = true, Status = 200, Title = "Success", Detail = "Client added successfully" });
            }
            catch (Exception ex)
            {
                _logService.LogError($"[ClientsController] - An error occurred while adding a new client: {ex.Message}");
                return StatusCode(500, new OperationResult
                {
                    Result = false,
                    Status = 500,
                    Title = "Internal server error",
                    Detail = "An unexpected error occurred. Please try again later."
                });
            }
        }

        /// <summary>
        /// Deletes a client by ID.
        /// </summary>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(OperationResult))]
        public async Task<ActionResult<OperationResult>> DeleteClient(int id)
        {
            try
            {
                var result = await _clientService.DeleteClient(id);
                if (!result)
                {
                    return NotFound(new OperationResult
                    {
                        Result = false,
                        Status = 404,
                        Title = "Client not found",
                        Detail = $"No client found with ID {id} to delete."
                    });
                }
                return Ok(new OperationResult
                {
                    Result = true,
                    Status = 200,
                    Title = "Client deleted",
                    Detail = $"Client with ID {id} has been successfully deleted."
                });
            }
            catch (Exception ex)
            {
                _logService.LogError($"[ClientsController] - An error occurred while deleting client with ID {id}: {ex.Message}");
                return StatusCode(500, new OperationResult
                {
                    Result = false,
                    Status = 500,
                    Title = "Internal server error",
                    Detail = "An unexpected error occurred. Please try again later."
                });
            }
        }

        /// <summary>
        /// Updates an existing client's information.
        /// </summary>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(OperationResult))]
        public async Task<ActionResult<ClientReadDto>> UpdateClient([FromBody] ClientUpdateDto dto)
        {
            if (dto == null || dto.ClientId <= 0)
            {
                return BadRequest(new OperationResult
                {
                    Result = false,
                    Status = 400,
                    Title = "Invalid client data",
                    Detail = "The client data provided is null or has an invalid ID."
                });
            }
            try
            {
                var result = await _clientService.UpdateClient(dto);
                if (!result)
                {
                    return NotFound(new OperationResult
                    {
                        Result = false,
                        Status = 404,
                        Title = "Client not found",
                        Detail = $"No client found with ID {dto.ClientId} to update."
                    });
                }

                return Ok(new OperationResult { Result = true, Status = 200, Title = "Success", Detail = "Client updated successfully" });
            }
            catch (Exception ex)
            {
                _logService.LogError($"[ClientsController] - An error occurred while updating client with ID {dto.ClientId}: {ex.Message}");
                return StatusCode(500, new OperationResult
                {
                    Result = false,
                    Status = 500,
                    Title = "Internal server error",
                    Detail = "An unexpected error occurred. Please try again later."
                });
            }
        }

    }
}
