using Fundamentos.Hangfire.Models.InputModels;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;

namespace Fundamentos.Hangfire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueuesController : ControllerBase
    {
        private readonly ILogger<QueuesController> _logger;
        public QueuesController(ILogger<QueuesController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("MultipleEnqueue")]
        public IActionResult MultipleEnqueue(DadosInputModel dados)
        {
            for (var i = 0; i < dados.QtdFilas; i++)
                BackgroundJob.Enqueue(() => Receive(i));

            return Accepted();
        }

        public void Receive(int i)
        {
            Console.WriteLine($"Enqueue: {i}!");
            Thread.Sleep(1000);
        }

        [HttpPost]
        [Route("Enqueue")]
        public IActionResult Enqueue(DadosInputModel dados)
        {
            BackgroundJob.Enqueue(() => Console.WriteLine($"Enqueue: {JsonConvert.SerializeObject(dados)}!"));

            return Accepted();
        }

        [HttpPost]
        [Route("Recurring")]
        public IActionResult Recurring(DadosInputModel dados)
        {
            RecurringJob.AddOrUpdate(dados.JobName, () => Console.WriteLine($"Recurring {JsonConvert.SerializeObject(dados)}!"), Cron.Minutely);

            return Accepted();
        }

        [HttpPost]
        [Route("Continuations")]
        public IActionResult Continuations(DadosInputModel dados)
        {
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine($"Continuations: {JsonConvert.SerializeObject(dados)}!"));
            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine($"Continuation jobid: {jobId}!"));

            return Accepted();
        }

        [HttpPost]
        [Route("Delayed")]
        public IActionResult Delayed(DadosInputModel dados)
        {
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine($"Delayed: {JsonConvert.SerializeObject(dados)}!"), TimeSpan.FromMinutes(10));

            return Accepted();
        }
    }
}
