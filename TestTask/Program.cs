using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using TestTask.repository;
using TestTask.service;

namespace TestTask
{
    internal class Program
    {
        // Конфигурируемый разделитель
        private readonly static char LINE_SPLITTER = '|';
        // Разделитель колонок
        private readonly static char COLUMN_SPLITTER = ',';
        // Тестовые входные данные
        private readonly static string[,] DATA = {
            { "34c6f58c-556c-444b-ab47-56ac451e2ae9","USER1","DOMAIN1","dc202406-3dd9-4985-ba3d-8f0135e9bab2","TAG1","TAGDOMAIN1" },
            { "34c6f58c-556c-444b-ab47-56ac451e2ae9","USER1","DOMAIN1","017cb05d-ec03-4703-98c0-a065e5d5e466","TAG2","TAGDOMAIN2" },
            { "34c6f58c-556c-444b-ab47-56ac451e2ae9","USER1","DOMAIN1","0f1489c2-40e7-4016-bb09-0af56c16fe03","TAG3","TAGDOMAIN1" },
            { "0e3325ce-2391-4111-aab7-14b6a13e9ea9","USER2","DOMAIN1","dc202406-3dd9-4985-ba3d-8f0135e9bab2","TAG1","TAGDOMAIN1" },
            { "0e3325ce-2391-4111-aab7-14b6a13e9ea9","USER2","DOMAIN1","017cb05d-ec03-4703-98c0-a065e5d5e466","TAG2","TAGDOMAIN2" },
            { "3d56d738-d979-4e39-8d27-a20bcb4cd9e0","USER3","DOMAIN2","0f1489c2-40e7-4016-bb09-0af56c16fe03","TAG3","TAGDOMAIN1" },
            { "8753bdb1-9a65-476e-84d8-70c9add9978f","USER4","DOMAIN3","017cb05d-ec03-4703-98c0-a065e5d5e466","TAG2","TAGDOMAIN2" },
            { "8753bdb1-9a65-476e-84d8-70c9add9978f","USER4","DOMAIN3","dc202406-3dd9-4985-ba3d-8f0135e9bab2", "TAG1","TAGDOMAIN1" }
        };
        // Генерация тестового потока
        private static Stream getStream() => new MemoryStream(
            Encoding.ASCII.GetBytes(
                string.Join(LINE_SPLITTER,
                DATA.OfType<string>()
                .Select((str, idx) => new { index = idx, value = str })
                .GroupBy(a => a.index / DATA.GetLength(1))
                .Select(gr => gr.Select(n => n.value))
                .Select(a => string.Join(COLUMN_SPLITTER, a.Select(x => x)))
                .ToArray())));

        static void Main(string[] args)
        {
            // Настройка Dependency Injection
            var host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddSingleton<IUserService, UserService>();
                services.AddSingleton<IStreamLoader, StreamLoader>();
                services.AddSingleton<IApp, App>();
                services.AddDbContext<AppDbContext>();
            }).Build();

            // Запуск приложения
            var app = host.Services.GetRequiredService<IApp>();
            app.run(getStream(), Encoding.ASCII, LINE_SPLITTER, COLUMN_SPLITTER);
        }
    }
}