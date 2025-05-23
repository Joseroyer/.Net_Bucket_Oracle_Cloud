using AppBucket.Request;
using AppBucket.Service;
using Microsoft.AspNetCore.Mvc;

namespace AppBucket.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BucketController : Controller
    {
        private readonly OracleBucketService _bucketService;

        public BucketController(OracleBucketService bucketService)
        {
            _bucketService = bucketService;
        }

        [HttpGet("objetos")]
        public async Task<IActionResult> ListarObjetos()
        {
            try
            {
                var objetos = await _bucketService.ListObjectsAsync();
                return StatusCode(200, objetos);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadRequest request)
        {
            if (request.Arquivo is null)
                return StatusCode(400, "Nenhum arquivo enviado");

            try
            {
                using var stream = request.Arquivo.OpenReadStream();
                await _bucketService.UploadObjectAsync(request.Arquivo.FileName, stream);
                return StatusCode(200, "Arquivo enviado com sucesso");
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("download/{nome}")]
        public async Task<IActionResult> Download(string nome)
        {
            try
            {
                var stream = await _bucketService.DownloadObjectAsync(nome);
                if (stream is null)
                    return StatusCode(404, "Arquivo não encontrado");

                return File(stream, "application/octet-stream", nome);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("excluir/{nome}")]
        public async Task<IActionResult> Excluir(string nome)
        {
            try
            {
                await _bucketService.DeleteObjectAsync(nome);
                return StatusCode(200, "Arquivo excluído com sucesso");
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

