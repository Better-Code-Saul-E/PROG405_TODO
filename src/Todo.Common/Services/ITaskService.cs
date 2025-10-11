using Todo.Common.Models;
using Todo.Common.Requests;

namespace Todo.Common.Services
{
    public interface ITaskService
    {
        Task<Result> CreateTaskAsync(CreateTaskRequest request);
    }

    public class TaskService : ITaskService
    {
        private readonly IFileDataService fileDataService;

        public TaskService(IFileDataService fileDataService)
        {
            this.fileDataService = fileDataService;
        }

// adding <string> to both 
        public async Task<Result> CreateTaskAsync(CreateTaskRequest request)
        {
            var modelResult = TaskModel.CreateTask(request);
            if (modelResult.IsErr())
            {
                return Result.Err(modelResult.GetErr());
            }
            var model = modelResult.GetVal();
            if (model is null)
            {
                //return Result<string>.Err("No Models");
            }
            await this.fileDataService.SaveAsync(modelResult.GetVal());
            return Result.Ok();
        }
    }
}