using Todo.Common.Models;
using Todo.Common.Requests;

namespace Todo.Common.Services
{
    public interface ITaskService
    {
        Task<Result<string>> CreateTaskAsync(CreateTaskRequest request);
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
    }
}