using ToDo.Models;

namespace ToDo.Data
{
    public interface ITodoRepository
    {
        void Delete(int id);
        void Insert(TodoItem todo);
        void Update(TodoItem todo);
    }
}