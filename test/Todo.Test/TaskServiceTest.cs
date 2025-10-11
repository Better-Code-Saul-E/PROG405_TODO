using Todo.Common.Models;
using Todo.Common.Requests;
using Todo.Common.Services;

namespace Todo.Test;

public class ClassServiceTest
{
    private IFileDataService service;
    public ClassServiceTest()
    {
        this.service = new DummyFileDataService();
    }

    [Fact]
    public async Task CreateTaskSucceeds()
    {
        var taskService = new TaskService(this.service);


        var happyRequest = new CreateTaskRequest("Test Task", "Dummy Descritopn", DateTime.UtcNow.AddDays(3));

        var createTaskResult = await taskService.CreateTaskAsync(happyRequest);
        
    }
}

internal class DummyFileDataService : IFileDataService
{
    private readonly Dictionary<string, TaskModel> data = new Dictionary<string, TaskModel>();

    public void Seed(TaskModel taskModel)
    {
        this.data.Add(taskModel.Key, taskModel);
    }

    public void Seed(IEnumerable<TaskModel> taskModels)
    {
        foreach(var t in taskModels)
        {
            this.data.Add(t.Key, t);
        }
    }
        

    public async Task<TaskModel?> GetAsync(string key)
    {
        await Task.CompletedTask;

        if (data.ContainsKey(key))
        {
            return data[key];
        }
        else
        {
            return null;
        }
    }

    public async Task SaveAsync(TaskModel? obj)
    {
        await Task.CompletedTask;

        if (obj is null)
        {
            return;
        }
        if (data.ContainsKey(obj.Key))
        {
            data.Remove(obj.Key);
        }
        this.data.Add(obj.Key, obj);
    }
}