using System.Numerics;
using Lab5.Network.Common;
using Lab5.Network.Common.UserApi;

internal class Program
{
    private static object _locker = new object();

    public static async Task Main(string[] args)
    {
        var serverAdress = new Uri("tcp://127.0.0.1:5555");
        var client = new NetTcpClient(serverAdress);
        Console.WriteLine($"Connect to server at {serverAdress}");
        await client.ConnectAsync();

        var userApi = new UserApiClient(client);
        await ManageUsers(userApi);
        client.Dispose();
    }

    private static async Task ManageUsers(IUserApi userApi)
    {
        PrintMenu();

        while(true) {
            var key = Console.ReadKey(true);

            PrintMenu();

            if (key.Key == ConsoleKey.D1) 
            {
                var users = await userApi.GetAllAsync();
                Console.WriteLine($"| Номер окна     |      Вид Услуги         | Статус             |  ФИО               |");
                foreach (var user in users)
                {
                    Console.WriteLine($"| {user.Id,16} | {user.Name,25} | {user.Status,20} |{user.Customer,20}");
                }
            }

            if (key.Key == ConsoleKey.D2) 
            {
                Console.Write("Введите номер окна ");
                var userIdString = Console.ReadLine();
                int.TryParse(userIdString, out var userId);
                var user = await userApi.GetAsync(userId);
                //Console.WriteLine($"Id={user?.Id}, Name={user?.Name}, Active={user?.Active}");
                Console.WriteLine($"| {user?.Id,16} | {user?.Name,25} | {user?.Status,20} |{user?.Customer,20}");
            }

            if (key.Key == ConsoleKey.D3) 
            {
                
                var addUserName = ChoosePizza() ?? "empty";
                Console.Write("Напишите ваше ФИО: ");
                var addName = Console.ReadLine() ?? "empty";
                var addUser = new User(Id: 0,
                    Name: addUserName,
                    Active: true,
                    Customer: addName,
                    Status: "Ожидайте"

                );
                var addResult = await userApi.AddAsync(addUser);

                Console.WriteLine(addResult ? "Ok" : "Error");
                
            }
            if (key.Key == ConsoleKey.D4) // Обновление только статуса пользователя
            {
                Console.Write("Введите номер окна для обновления статуса: ");
                var updateIdString = Console.ReadLine();
                int.TryParse(updateIdString, out var updateId);

                // Получаем текущие данные пользователя
                var existingUser = await userApi.GetAsync(updateId);
                if (existingUser == null)
                {
                    Console.WriteLine("Такое окно не найдено");
                    continue;
                }


                // Создаем новый объект пользователя с измененным только статусом
                var updatedUser = new User(
                    Id: existingUser.Id,
                    Name: existingUser.Name,
                    Active: existingUser.Active,
                    Customer: existingUser.Customer,
                    Status: "Пройдите к окну"
                );

                var updateResult = await userApi.UpdateAsync(updateId, updatedUser);

                Console.WriteLine(updateResult ? "Статус окна обновлен" : "Ошибка при обновлении статуса окна");
            }
            if (key.Key == ConsoleKey.D5) // Удаление пользователя
            {
                Console.Write("Введите окно для удаления: ");
                var deleteIdString = Console.ReadLine();
                int.TryParse(deleteIdString, out var deleteId);

                var deleteResult = await userApi.DeleteAsync(deleteId);

                Console.WriteLine(deleteResult ? "окно удалено" : "Ошибка при удалении окна");
            }

            if (key.Key == ConsoleKey.Escape)
            {
                break;
            }
        }
        Console.ReadKey();
        //while (Console.Read)
    }

    private static void PrintMenu()
    {
        lock (_locker)
        {
            Console.WriteLine("1 - Вывести всю очередь");
            Console.WriteLine("2 - Показать инфо по окну");
            Console.WriteLine("3 - Выбрать услугу");
            Console.WriteLine("4 - Поменять статус услуги");
            Console.WriteLine("5 - Удалить услугу");
            Console.WriteLine("-------");
        }
    }
    static string ChoosePizza()
    {
        lock (_locker)
        {
            // Набор из 5 услуг
            string[]  services = { "Получение паспорта", "Получение ВД", "Справка", "Заявление", "Вопрос" };

            // Вывод списка услуг
            Console.WriteLine("Выберите услугу из следующего списка:");
            for (int i = 0; i < services.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {services[i]}");
            }

            // Запрос выбора пользователя
            Console.Write("Введите номер услуги (1-5): ");
            int choice;

            // Проверка корректности ввода
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > services.Length)
            {
                Console.Write("Некорректный ввод. Пожалуйста, введите номер услуги (1-5): ");
            }

            // Возврат названия выбранной услуги
            return services[choice - 1];
        }
    }
    

}
