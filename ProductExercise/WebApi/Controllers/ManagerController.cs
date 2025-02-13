using DAL;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly DBContext context;
        private readonly ILogger<ManagerController> logger;

        public ManagerController(DBContext context, ILogger<ManagerController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Create()
        {
            logger.Log(LogLevel.Information, "verification si la base existe OU création de la base");
            context.Database.EnsureCreated();
            logger.LogInformation("base de données crée OU déjà présente");

            return Ok("oki");
        }

        [HttpGet("destroy")]
        public IActionResult Destroy()
        {
            context.Database.EnsureDeleted();
            logger.LogInformation("base de données détruite OU non présente");

            return Ok("deleted");
        }
    }
}