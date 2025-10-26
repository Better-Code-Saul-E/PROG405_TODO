using Todo.Common.Models;

namespace Todo.Common.Requests
{
    public class UpdateTaskRequest
    {
        public UpdateTaskRequest(string key, TaskModel task)
        {
            this.Key = key;
            this.Task = task;

        }
        public string Key { get; }
        public TaskModel Task { get; }

        public Result IsValid()
        {
            if (string.IsNullOrWhiteSpace(this.Key))
            {
                return Result.Err("Key required for task update");
            }
            if (this.Task == null)
            {
                return Result.Err("task update cannot be null");
            }
            if (string.IsNullOrWhiteSpace(this.Task.Name))
            {
                return Result.Err("Name Required");
            }

            if (this.Task.DueDate <= DateTime.UtcNow)
            {
                return Result.Err("Due Date Must Be In Future");
            }

            return Result.Ok();
        }
    }
}