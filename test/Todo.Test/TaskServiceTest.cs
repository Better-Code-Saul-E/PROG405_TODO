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

        var originalTaskResult = TaskModel.CreateTask(new CreateTaskRequest("Original Task", "Original Description", DateTime.UtcNow.AddDays(3)));
        Assert.True(originalTaskResult.IsOk());
        var originalTask = originalTaskResult.GetVal()!;
        await this.service.SaveAsync(originalTask);


        var taskKey = originalTask.Key;
        Assert.False(string.IsNullOrWhiteSpace(taskKey));

        var fetchFile = await this.service.GetAsync(taskKey);
        Assert.NotNull(fetchFile);
        Assert.Equal("Original Task", fetchFile!.Name);


        var updatedTaskResult = TaskModel.CreateTask(
            new CreateTaskRequest("Update Task","Update Description", DateTime.UtcNow.AddDays(6)
        ));
        Assert.True(updatedTaskResult.IsOk());
        var updatedTask = updatedTaskResult.GetVal()!;
        var updatedTaskRequest = new UpdateTaskRequest(taskKey, updatedTask);

        var updateTaskResult = await taskService.UpdateTaskAsync(updatedTaskRequest);
        Assert.True(updateTaskResult.IsOk());

        var updated = await this.service.GetAsync(taskKey);
        Assert.NotNull(updated);
        Assert.Equal("Update Task", updated!.Name);
        Assert.Equal("Update Description", updated.Description);
        Assert.Equal(taskKey, updated.Key);
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