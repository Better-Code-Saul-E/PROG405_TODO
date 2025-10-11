using Todo.Common.Models;

namespace Todo.Common.Extensions
{
    public static class TaskModelExtensions
    {
        public static string ToFileName(this TaskModel model)
            => $"{model.Key}.json";

        public static string ToFileName(string key)
            => $"{key}.json";
    }    
}