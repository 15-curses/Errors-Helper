Основные возможности
--------------------

### 1\. Чтение ошибки по ID

LogReader.Read("ERROR\_001");

### 2\. Проверка существования ошибки

bool exists \= LogReader.ErrorExists("ERROR\_001");

### 3\. Получение ошибок по уровню

// Получить все ошибки уровня ERROR
var errors \= LogReader.GetErrorsByLevel("ERROR");

// Использовать полученные ошибки
foreach (var error in errors)
{
    Debug.Log($"ID: {error.ID}, Сообщение: {error.UnityMessage}");
}

### 4\. Получение общего количества ошибок

int totalErrors \= LogReader.GetTotalErrorCount();

Требования к файлу ошибок
-------------------------

1.  **Расположение файла**: `Assets/Error/ErrorLog.xml`
    
2.  **Формат XML**:
    


<Errors\>
  <Error\>
    <ID\>ERROR\_001</ID\>
    <Category\>Графика</Category\>
    <Type\>Шейдер</Type\>
    <Level\>ERROR</Level\>
    <UnityMessage\>Шейдер не найден</UnityMessage\>
    <SuggestedFix\>Проверьте наличие шейдера в проекте</SuggestedFix\>
    <AdditionalInfo\>Дополнительная информация</AdditionalInfo\>
  </Error\>
</Errors\>

Уровни важности (Level)
-----------------------

*   **ERROR** - Критические ошибки (красный текст в консоли)
    
*   **WARNING** - Предупреждения (желтый текст в консоли)
    
*   **INFO** - Информационные сообщения (белый текст в консоли)
    

Пример использования
--------------------

    void Start()
    {
        // Пример 1: Проверка и чтение ошибки
        string errorId \= "MISSING\_TEXTURE";
        
        if (LogReader.ErrorExists(errorId))
        {
            LogReader.Read(errorId);
        }
        else
        {
            Debug.LogWarning($"Ошибка {errorId} не найдена");
        }

        // Пример 2: Получение всех предупреждений
        var warnings \= LogReader.GetErrorsByLevel("WARNING");
        Debug.Log($"Найдено предупреждений: {warnings.Count}");

        // Пример 3: Общая статистика
        int total \= LogReader.GetTotalErrorCount();
        Debug.Log($"Всего ошибок в базе: {total}");
    }
