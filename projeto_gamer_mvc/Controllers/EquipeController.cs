using Microsoft.AspNetCore.Mvc;
using projeto_gamer_mvc.infra;
using projeto_gamer_mvc.Models;

namespace projeto_gamer_mvc.Controllers
{
    [Route("[controller]")]
    public class EquipeController : Controller
    {
        private readonly ILogger<EquipeController> _logger;

        public EquipeController(ILogger<EquipeController> logger)
        {
            _logger = logger;
        }

        Context c = new Context();


        [Route("Listar")]  //http://localhost/Equipe/Listar
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            //"mochila" que contém a lista das equipes 
            // Podemos usar essa "mochila" na view de equipe
            ViewBag.Equipe = c.Equipe.ToList();
            return View();
        }

        [Route("Cadastrar")]
        public IActionResult Cadastrar(IFormCollection form)
        {
            Equipe novaEquipe = new Equipe();

            novaEquipe.Nome = form["Nome"].ToString();

            // aqui adicionamos uma string precisamos de uma imagem
            // novaEquipe.Imagem = form["Imagem"].ToString();

            // Aqui começa o upload de imagem            

            if (form.Files.Count > 0)
            {
                var file = form.Files[0];

                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Equipes");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var path = Path.Combine(folder, file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                novaEquipe.Imagem = file.FileName;
            }

            else
            {
                novaEquipe.Imagem = "padrao.png";
            }

            c.Equipe.Add(novaEquipe);
            // c.Add(novaEquipe)

            c.SaveChanges();

            return LocalRedirect("~/Equipe/Listar");
        }

        [Route("Excluir/{id}")]
        public IActionResult Excluir(int id)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            Equipe e = c.Equipe.First(e => e.IdEquipe == id);

            c.Equipe.Remove(e);

            c.SaveChanges();

            return LocalRedirect("~/Equipe/Listar");
        }

        [Route("Editar/{id}")]
        public IActionResult Editar(int id)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            Equipe e = c.Equipe.First(e => e.IdEquipe == id);

            ViewBag.Equipe = e;

            return View("Edit");
        }

        [Route("Atualizar")]
        public IActionResult Atualizar(IFormCollection form, Equipe e)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            Equipe novaEquipe = new Equipe();

            novaEquipe.Nome = e.Nome;

            // Upload de nova imagem(atualizada)
            if (form.Files.Count > 0)
            {
                var file = form.Files[0];

                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Equipes");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var path = Path.Combine(folder, file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                novaEquipe.Imagem = file.FileName;
            }
            else
            {
                novaEquipe.Imagem = "padrao.png";
            }

            Equipe equipe = c.Equipe.First(x => x.IdEquipe == e.IdEquipe);

            equipe.Nome = novaEquipe.Nome;
            equipe.Imagem = novaEquipe.Imagem;

            c.Equipe.Update(equipe);
            c.SaveChanges();

            return LocalRedirect("~/Equipe/Listar");


        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}