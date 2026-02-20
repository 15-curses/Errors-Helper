Основные возможности
--------------------
КАК ИСПОЛЬЗОВАТЬ В КОДЕ:
[SerializeField] private LogReader errorReader;
// Получить ошибку по ID
var error = errorReader.GetError("ERR001");
            
// Получить все ошибки уровня Warning
var warnings = errorReader.GetErrorsByLevel("Warning");
            
// Проверить существование ошибки
bool exists = errorReader.HasError("ERR001");
            
// Узнать общее количество ошибок
int total = errorReader.TotalErrorsCount;
            
// Вывести ошибку в консоль
errorReader.LogError("ERR001");
