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
        Assert.True(createTaskResult.IsOk());

        var taskKey = createTaskResult.GetVal();
        Assert.False(string.IsNullOrWhiteSpace(taskKey));

        var fetchFile = await this.service.GetAsync(taskKey!);
        Assert.NotNull(fetchFile);
        Assert.Equal("Test Task", fetchFile!.Name);
    }

    [Fact]
    public async Task UpdateTaskSuceeds()
    {
        var taskService = new TaskService(this.service);

        var originalRequest = new CreateTaskAsync("Original Task", "Original Description", DateTime.UtcNow.AddDays(3));
        var createRequest = await taskService.CreateTaskAsync(originalRequest);
        Assert.True(createRequest.IsOk());

        var taskKey = createRequest.GetVal();
        Asset.False(string.IsNullOrWhiteSpace(taskKey));

        var fetchFile = await this.service.GetAsync(taskKey!);
        Assert.NotNull(fetchFile);
        Assert.Equal("Original Task", fetchFile!.Name);


        var updateRequest = new UpdateTaskRequest
        {
            Key = taskKey!,
            Task = new TaskModel
            {
                Name = "Update Task",
                Description = "Update Description",
                DueDate = DateTime.UtcNow.AddDays(6)
            }
        };
        var updateResult = await taskService.UpdateTaskAsync(updateRequest);
        Assert.True(updateResult.IsOk());

        var updatedTask = await this.service.GetAsync(taskKey!);
        Assert.NotNull(updatedTask);
        Assert.Equal("Update Task", updatedTask!.Name);
        Assert.Equal("Update Description", updatedTask.Description);
        Assert.Equal(taskKey, updatedTask.Key);

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
        foreach (var t in taskModels)
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