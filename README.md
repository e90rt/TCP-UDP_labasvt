
## Описание проекта

### Компоненты программы

Проект включает два основных компонента:

1. **Серверная часть**: управляет обработкой запросов от клиентов и взаимодействует с базой данных очереди услуг.
2. **Клиентская часть**: предоставляет интерфейс для пользователя, позволяя выполнять запросы к серверу.

## Функциональные возможности

### Управление услугами через TCP

- **Просмотр очереди**: Отображает все доступные окна и их текущий статус.
- **Информация по окну**: Выводит подробные данные по выбранному окну.
- **Добавление услуги**: Позволяет пользователю выбрать услугу из списка и добавить её в очередь.
- **Изменение статуса услуги**: Меняет статус для конкретного окна на "Пройдите к окну".
- **Удаление услуги**: Удаляет услугу по указанному ID окна.

---

## Как использовать

### 1. Запуск сервера

### 2. Запуск клиента

### D2 — Показать информацию по конкретному окну

### D3 — Добавить новую услугу

if (key.Key == ConsoleKey.D3) 
{
    var addUserName = ChooseService() ?? "empty";
    Console.Write("Введите ваше ФИО: ");
    var addName = Console.ReadLine() ?? "empty";
    var addUser = new User(Id: 0, Name: addUserName, Active: true, Customer: addName, Status: "Ожидайте");
    var addResult = await userApi.AddAsync(addUser);
    Console.WriteLine(addResult ? "Ok" : "Error");
}

### D4 — Обновление статуса услуги

if (key.Key == ConsoleKey.D4) 
{
    Console.Write("Введите номер окна для обновления статуса: ");
    var updateIdString = Console.ReadLine();
    int.TryParse(updateIdString, out var updateId);
    var existingUser = await userApi.GetAsync(updateId);
    if (existingUser != null) {
        var updatedUser = new User(Id: existingUser.Id, Name: existingUser.Name, Active: existingUser.Active, Customer: existingUser.Customer, Status: "Пройдите к окну");
        var updateResult = await userApi.UpdateAsync(updateId, updatedUser);
        Console.WriteLine(updateResult ? "Статус окна обновлен" : "Ошибка при обновлении статуса окна");
    }
}


### D5 — Удаление услуги из очереди


if (key.Key == ConsoleKey.D5) 
{
    Console.Write("Введите окно для удаления: ");
    var deleteIdString = Console.ReadLine();
    int.TryParse(deleteIdString, out var deleteId);
    var deleteResult = await userApi.DeleteAsync(deleteId);
    Console.WriteLine(deleteResult ? "Окно удалено" : "Ошибка при удалении окна");
}
```



