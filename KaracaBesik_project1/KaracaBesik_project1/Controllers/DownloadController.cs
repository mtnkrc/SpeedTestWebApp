using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KaracaBesik_project1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadController : ControllerBase
    {
        private readonly ILogger<DownloadController> _logger;
        private readonly HttpClient _httpClient;
        private const string TestFileUrl = "http://ipv4.download.thinkbroadband.com/50MB.zip";

        public DownloadController(ILogger<DownloadController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }


        [HttpGet]
        public async Task<IActionResult> DownloadFile()
        {
            try
            {
                _logger.LogInformation("Download test started at {Time}", DateTime.UtcNow);
                var stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                // Setting a user-agent
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

                var response = await _httpClient.GetAsync(TestFileUrl, HttpCompletionOption.ResponseHeadersRead);

                _logger.LogInformation("Response status code: {StatusCode}", response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Non-success status code received.");
                    return StatusCode((int)response.StatusCode, "Failed to download the file.");
                }

                var fileStream = await response.Content.ReadAsStreamAsync();

                stopwatch.Stop();
                var downloadTimeInSeconds = stopwatch.Elapsed.TotalSeconds;
                var fileSizeInBytes = response.Content.Headers.ContentLength ?? 0;
                var speedInMbps = (fileSizeInBytes / downloadTimeInSeconds) * 8 / 1_000_000; // Convert bytes per second to Mbps

                _logger.LogInformation("Download test completed in {DownloadTimeInSeconds} seconds with a speed of {SpeedInMbps} Mbps", downloadTimeInSeconds, speedInMbps);

                return File(fileStream, "application/octet-stream", "50MB.zip");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while downloading the test file.");
                return StatusCode(500, "An internal error occurred");
            }
        }


    }
}
