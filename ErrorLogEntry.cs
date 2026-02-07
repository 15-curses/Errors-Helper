using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class LogReader
{
    private static string xmlPath = Path.Combine(Application.dataPath, "Error/ErrorLog.xml");
    private static XDocument _cachedDoc;
    private static DateTime _lastCacheTime;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(30);

    public static void Read(string id)
    {
        try
        {
            XDocument doc = GetCachedDocument();

            var errorElement = doc.Descendants("Error")
                     .FirstOrDefault(x => x.Element("ID")?.Value == id);

            if (errorElement != null)
            {
                ProcessErrorElement(id, errorElement);
            }
            else
            {
                Debug.LogWarning($"Ошибка с ID '{id}' не найдена");
            }
        }
        catch (FileNotFoundException)
        {
            Debug.LogError($"Файл не найден: {xmlPath}");
        }
        catch (XmlException xmlEx)
        {
            Debug.LogError($"Ошибка формата XML: {xmlEx.Message}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка чтения XML: {e.Message}");
        }
    }
    /// <summary>
    /// получение всех данных о сообшении по его id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="errorElement"></param>
    private static void ProcessErrorElement(string id, XElement errorElement)
    {
        string category = errorElement.Element("Category")?.Value ?? "Category не найдено";
        string type = errorElement.Element("Type")?.Value ?? "Type не найден";
        string level = errorElement.Element("Level")?.Value ?? "Level не найден";
        string unityMessage = errorElement.Element("UnityMessage")?.Value ?? "UnityMessage не найден";
        string suggestedFix = errorElement.Element("SuggestedFix")?.Value ?? "SuggestedFix не найден";
        string additionalInfo = errorElement.Element("AdditionalInfo")?.Value ?? string.Empty;

        string logMessage = FormatLogMessage(id, level, category, type, unityMessage, suggestedFix, additionalInfo);

        LogByLevel(level, logMessage);
    }
    /// <summary>
    /// формирование сообшения
    /// </summary>
    /// <param name="id"></param>
    /// <param name="level"></param>
    /// <param name="category"></param>
    /// <param name="type"></param>
    /// <param name="unityMessage"></param>
    /// <param name="suggestedFix"></param>
    /// <param name="additionalInfo"></param>
    /// <returns>готовое к отправке сообшение</returns>
    private static string FormatLogMessage(string id, string level, string category, string type,
                                          string unityMessage, string suggestedFix, string additionalInfo)
    {
        var message = $"[{level}] ID: {id}\n" +
                     $"Категория: {category}\n" +
                     $"Тип: {type}\n" +
                     $"Сообщение: {unityMessage}\n" +
                     $"Решение: {suggestedFix}\n";

        if (!string.IsNullOrEmpty(additionalInfo))
        {
            message += $"Доп. информация: {additionalInfo}\n";
        }

        return message;
    }
    /// <summary>
    /// логирование с учетом уровня важности сообщения
    /// </summary>
    /// <param name="level"></param>
    /// <param name="message"></param>
    private static void LogByLevel(string level, string message)
    {
        switch (level.ToUpper())
        {
            case "ERROR":
                Debug.LogError(message);
                break;
            case "WARNING":
                Debug.LogWarning(message);
                break;
            case "INFO":
            default:
                Debug.Log(message);
                break;
        }
    }
    /// <summary>
    /// кэширование XML-документа для оптимизации производительности
    /// </summary>
    /// <returns></returns>
    private static XDocument GetCachedDocument()
    {
        var lastWriteTime = File.GetLastWriteTime(xmlPath);

        if (_cachedDoc == null || DateTime.Now - _lastCacheTime > CacheDuration || lastWriteTime > _lastCacheTime)
        {
            _cachedDoc = XDocument.Load(xmlPath);
            _lastCacheTime = DateTime.Now;
        }
        return _cachedDoc;
    }
    /// <summary>
    /// получение всех ошибок определенного уровня
    /// </summary>
    /// <param name="level"></param>
    /// <returns>уровень(ERROR, WARNING, INFO) ошибки </returns>
    public static List<ErrorInfo> GetErrorsByLevel(string level)
    {
        try
        {
            var doc = GetCachedDocument();
            return doc.Descendants("Error")
                     .Where(x => x.Element("Level")?.Value?.Equals(level, StringComparison.OrdinalIgnoreCase) == true)
                     .Select(e => new ErrorInfo
                     {
                         ID = e.Element("ID")?.Value,
                         Category = e.Element("Category")?.Value,
                         Type = e.Element("Type")?.Value,
                         Level = e.Element("Level")?.Value,
                         UnityMessage = e.Element("UnityMessage")?.Value,
                         SuggestedFix = e.Element("SuggestedFix")?.Value
                     })
                     .ToList();
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка получения ошибок по уровню: {e.Message}");
            return new List<ErrorInfo>();
        }
    }
    /// <summary>
    /// проверка существования ошибки
    /// </summary>
    /// <param name="id"></param>
    /// <returns>true - в файле сушествует такая ошибка, false - не сушествует</returns>
    public static bool ErrorExists(string id)
    {
        try
        {
            var doc = GetCachedDocument();
            return doc.Descendants("Error")
                     .Any(x => x.Element("ID")?.Value == id);
        }
        catch
        {
            return false;
        }
    }
    /// <summary>
    /// получение общего количества ошибок
    /// </summary>
    /// <returns>количество ошибок всего в файле</returns>
    public static int GetTotalErrorCount()
    {
        try
        {
            var doc = GetCachedDocument();
            return doc.Descendants("Error").Count();
        }
        catch
        {
            return 0;
        }
    }

    public class ErrorInfo
    {
        public string ID { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Level { get; set; }
        public string UnityMessage { get; set; }
        public string SuggestedFix { get; set; }
    }
}


