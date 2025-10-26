using Todo.Common.Requests;

namespace Todo.Common.Models
{
    public class TaskModel
    {
        internal TaskModel()
        {
            //MUST:
            //Exist
            this.Key = string.Empty;
            //MUST:
            //Exist
            this.Name = string.Empty;
            //Optional
            this.Description = string.Empty;
            //Must:
            //Be In The Future
            //Exist
            this.DueDate = DateTime.MinValue;
        }

        public string Key { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; }

        public static Result<TaskModel> CreateTask(CreateTaskRequest request)
        {
            var validationResult = request.IsValid();
            if (validationResult.IsErr())
            {
                return Result<TaskModel>.Err(validationResult.GetErr());
            }

            return Result<TaskModel>.Ok(new TaskModel
            {
                Key = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                DueDate = request.DueDate
            });
        }
        public static TaskModel Update(TaskModel oldTask, string name, string description, DateTime dueDate){
            return new TaskModel
            {
                Key = oldTask.Key,
                Name = name,
                Description = description,
                DueDate = dueDate,
            };
        }
    }
}