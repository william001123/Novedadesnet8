using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI_PDF.Models;

namespace WebAPI_PDF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;

        public UsersController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: Mapea un modelo de dominio a un DTO
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            // Simulación de un usuario obtenido desde una base de datos
            var user = new User
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            // Mapear User a UserDto
            var userDto = _mapper.Map<UserDto>(user);
            userDto.FullName = $"{user.FirstName} {user.LastName}";

            return Ok(userDto);
        }

        // POST: Mapea un DTO a un modelo de dominio
        [HttpPost]
        public IActionResult CreateUser(UserDto userDto)
        {
            // Mapear UserDto a User
            var user = _mapper.Map<User>(userDto);

            // Simulación de guardar en la base de datos
            user.Id = new Random().Next(1, 1000);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
    }
}
