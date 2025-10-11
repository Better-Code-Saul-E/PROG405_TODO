
using System.Text.Json;
using Todo.Common.Extensions;
using Todo.Common.Models;

namespace Todo.Common.Services
{
    public interface IDataService<T, TKey>
    {
        Task SaveAsync(T obj);
        Task<T> GetAsync(TKey id);
    }

    public interface IFileDataService : IDataService<TaskModel?, string> {}
    
    public class FileDataService : IFileDataService
    {
        private readonly string path;

        //TODO: configure ILogger
        public FileDataService(string path)
        {
            this.path = path;
        }

        public async Task<TaskModel?> GetAsync(string id)
        {
            try
            {
                string fileName = TaskModelExtensions.ToFileName(id);
                string combinedPath = Path.Combine(this.path, fileName);

                if (!File.Exists(combinedPath))
                {
                    Console.WriteLine($"File Does Not Exist At Path: {combinedPath}");
                    return null;
                }

                using StreamReader sr = new StreamReader(this.path);
                string text = await sr.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(text))
                {
                    Console.WriteLine($"Empty File At Path {combinedPath}");
                }

                return JsonSerializer.Deserialize<TaskModel>(text);
            }
            catch (IOException)
            {
                Console.WriteLine($"Getting File Failed For {id}");
                throw;
            }
            catch (JsonException)
            {
                Console.WriteLine($"Deserializing File Failed");
                throw;
            }
            catch (Exception)
            {
                Console.WriteLine($"RUT ROH RAGGY");
                throw;
            }
        }

        public async Task SaveAsync(TaskModel? obj)
        {
            if (obj is null)
            {
                return;
            }
            //TODO: Test if overwriting is silent
            try
            {
                string fileName = obj.ToFileName();
                string combinedPath = Path.Combine(this.path, fileName);
                using StreamWriter sw = new StreamWriter(combinedPath);
                string text = JsonSerializer.Serialize(obj);
                await sw.WriteAsync(text);
            }
            catch (IOException)
            {
                Console.WriteLine($"Failed Writing File For Task {obj.Key}");
                throw;
            }
            catch (JsonException)
            {
                Console.WriteLine($"Serializing File Failed");
                throw;
            }
            catch (Exception)
            {
                Console.WriteLine($"RUT ROH RAGGY");
                throw;
            }
        }
    }
}
