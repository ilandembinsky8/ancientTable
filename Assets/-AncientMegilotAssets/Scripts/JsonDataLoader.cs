using System;
using UnityEngine;

// Loads any JSON file into a [Serializable] data class, using JsonUtility.
//
// Usage:
//     [SerializeField] TextAsset _json;
//     MyData data = JsonDataLoader.Load<MyData>(_json);
//
// JsonUtility rules for the data class and the JSON:
// - The JSON root must be an object { ... }. A top-level array [ ... ] is not supported: wrap it in an object.
// - Fields must be public (or [SerializeField] private) and named exactly like the JSON keys.
//   A key with no matching field is ignored silently, so a typo means missing data, not an error.
// - No properties or dictionaries. Nested classes must be [Serializable].
public static class JsonDataLoader
{
    // Never returns null: logs an error and returns an empty T when the file is missing or invalid.
    public static T Load<T>(TextAsset jsonFile) where T : class, new()
    {
        if (jsonFile == null)
        {
            Debug.LogError($"[{nameof(JsonDataLoader)}] No JSON file assigned for {typeof(T).Name}.");
            return new T();
        }

        return Parse<T>(jsonFile.text, jsonFile.name);
    }

    // For JSON that does not come from a TextAsset (file on disk, StreamingAssets, network).
    public static T Parse<T>(string json, string sourceName = "JSON") where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            Debug.LogError($"[{nameof(JsonDataLoader)}] '{sourceName}' is empty.");
            return new T();
        }

        try
        {
            return JsonUtility.FromJson<T>(json) ?? new T();
        }
        catch (ArgumentException e)
        {
            Debug.LogError($"[{nameof(JsonDataLoader)}] Can't read '{sourceName}' as {typeof(T).Name}: {e.Message}");
            return new T();
        }
    }
}
