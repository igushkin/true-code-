using System.Text;
using System.Text.Json;
using TestTask.extension;
using TestTask.service;

namespace TestTask
{
    internal class App : IApp
    {
        // Внедрение зависимостей
        private readonly IUserService userService;
        private readonly IStreamLoader streamLoader;
        // Меню приложения
        private readonly string[] MENU = {
            "1. Read from stream and save to db",
            "2. Find user by id",
            "3. Find users by domain with pagination",
            "4. Find all users by tag & domain",
            "5. Exit"
        };

        public App(IUserService userService, IStreamLoader streamLoader)
        {
            this.userService = userService;
            this.streamLoader = streamLoader;
        }

        // Запуск приложения
        public void run(Stream stream, Encoding encoding, char lineSplitter, char columnSplitter)
        {

            int input;

            do
            {
                // Чтение ввода пользователя
                input = MyConsole.askInt(string.Join(Environment.NewLine, MENU));

                switch (input)
                {
                    case 1:
                        // Сохранение данных в бд
                        var imported = streamLoader.Load(stream, encoding, lineSplitter, columnSplitter);
                        Console.WriteLine(string.Format("Import completed. {0} rows were processed.", imported));
                        break;
                    case 2:
                        // Поиск пользователя по ид
                        findById();
                        break;
                    case 3:
                        // Поиск пользователей по Domain с пагинцией
                        findByDomain();
                        break;
                    case 4:
                        // Поиск всех пользователей по Tag и Domain
                        findByTagAndDomain();
                        break;
                    case 5:
                        // Завершение приложения
                        Console.WriteLine("Done!");
                        break;
                    default:
                        Console.WriteLine("Wrong input. Please try again");
                        break;
                }
            } while (input != 5);
        }

        private void findById()
        {
            var id = MyConsole.askGuid("Enter user id:");
            var user = userService.getById(id);

            if (user is null)
                Console.WriteLine("User not found");
            else
                Console.WriteLine(JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void findByDomain()
        {
            var domain = MyConsole.ask("Enter users domain:");
            var pageLimit = MyConsole.askInt("Enter page limit:", 1);
            var pageNumber = MyConsole.askInt("Enter page number:", 1);

            var users = userService.getByDomain(domain, pageNumber, pageLimit);

            if (users.Count() == 0)
                Console.WriteLine("Empty result");
            else
                Console.WriteLine(JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void findByTagAndDomain()
        {
            var tag = MyConsole.ask("Enter users tag:");
            var domain = MyConsole.ask("Enter users domain:");

            var users = userService.getAllByTagAndDomain(tag, domain);

            if (users.Count() == 0)
                Console.WriteLine("Empty result");
            else
                Console.WriteLine(JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
        }
    }

    internal interface IApp
    {
        void run(Stream stream, Encoding encoding, char lineSplitter, char columnSplitter);
    }
}
