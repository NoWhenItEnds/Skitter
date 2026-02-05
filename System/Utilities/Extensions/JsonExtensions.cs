using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Godot;

namespace Skitter.Utilities.Extensions
{
    /// <summary> A helper class for working with Json serialisation. </summary>
    public static class JsonExtensions
    {
        public static T[] LoadData<T>(String relativeDirectoryPath) where T : class
        {
            String[] filepaths = FileExtensions.GetFilepaths(relativeDirectoryPath, [".json"]);
            HashSet<T> data = new HashSet<T>();

            foreach (String filepath in filepaths)
            {
                try
                {
                    GD.Print(ProjectSettings.GlobalizePath(filepath));
                    T newData = JsonSerializer.Deserialize<T>(filepath) ?? throw new JsonException($"Unable to deserialise file at '{filepath}' to type {typeof(T)}.");
                    data.Add(newData);
                }
                catch (JsonException exception)
                {
                    GD.PrintErr(exception.Message);
                }
            }

            return data.ToArray();
        }
    }
}
