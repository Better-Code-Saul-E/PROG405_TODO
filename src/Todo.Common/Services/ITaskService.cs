using Todo.Common.Models;
using Todo.Common.Requests;

namespace Todo.Common.Services
{
    public interface ITaskService
    {
        Task<Result<string>> CreateTaskAsync(CreateTaskRequest request);
        Task<Result> UpdateTaskAsync(UpdateTaskRequest request);
    }

    public class TaskService : ITaskService
    {
        private readonly IFileDataService fileDataService;

        public TaskService(IFileDataService fileDataService)
        {
            this.fileDataService = fileDataService;
        }

        public async Task<Result<string>> CreateTaskAsync(CreateTaskRequest request)
        {
            var modelResult = TaskModel.CreateTask(request);
            if (modelResult.IsErr())
            {
                return Result<string>.Err(modelResult.GetErr());
            }

            var model = modelResult.GetVal();
            if (model is null)
            {
                return Result<string>.Err("No model created");
            }

            await this.fileDataService.SaveAsync(model);
            return Result<string>.Ok(model.Key);
        }
        public async Task<Result> UpdateTaskAsync(UpdateTaskRequest request)
        {
            var validationResult = request.IsValid();
            if (validationResult.IsErr())
            {
                return Result.Err(validationResult.GetErr());
            }

            var task = await this.fileDataService.GetAsync(request.Key);
            if (task == null)
            {
                return Result.Err($"There is no task with key ${request.Key}");
            }

            var updatedTask = TaskModel.Update(task, request.Task);

            await this.fileDataService.SaveAsync(task);
            return Result.Ok();
            
        }
    }
}