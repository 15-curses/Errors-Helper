Основные функции:
1. Чтение ошибки по ID

// Просто передайте ID ошибки
LogReader.Read("ID_ВАШЕЙ_ОШИБКИ");

2. Проверка существования ошибки
// Вернет true, если ошибка есть в файле
bool exists = LogReader.ErrorExists("ID_ВАШЕЙ_ОШИБКИ");

3. Получение ошибок по уровню важности
// Получите все ошибки определенного уровня
var errors = LogReader.GetErrorsByLevel("ERROR"); // "ERROR", "WARNING" или "INFO"

// Можно использовать так:
foreach (var error in errors)
{
    Debug.Log($"Ошибка: {error.ID}, Сообщение: {error.UnityMessage}");
}

4. Получение общего количества ошибок
// Узнайте сколько всего ошибок в файле
int totalErrors = LogReader.GetTotalErrorCount();


Структура XML файла (ErrorLog.xml):
Файл должен быть в папке: Assets/Error/ErrorLog.xml

Пример содержимого:
<Errors>
  <Error>
    <ID>001</ID>
    <Category>Графика</Category>
    <Type>Shader</Type>
    <Level>ERROR</Level>
    <UnityMessage>Шейдер не найден</UnityMessage>
    <SuggestedFix>Проверьте наличие шейдера в проекте</SuggestedFix>
  </Error>
</Errors>


Уровни важности:
ERROR - Критическая ошибка (красный текст в консоли)
WARNING - Предупреждение (желтый текст в консоли)
INFO - Информация (белый текст в консоли)


Пример использования:
// 1. Проверяем есть ли ошибка
if (LogReader.ErrorExists("MISSING_TEXTURE"))
{
    // 2. Читаем информацию об ошибке
    LogReader.Read("MISSING_TEXTURE");
}
// 3. Получаем все предупреждения
var warnings = LogReader.GetErrorsByLevel("WARNING");



Важно:
Файл XML кэшируется на 30 секунд
При изменении файла кэш обновится автоматически
Если файл не найден, вы увидите ошибку в консоли
