using ToDo.Models;
using ToDo.Models.ViewModels;

namespace ToDo.Data
{
    public interface ITodoRepository
    {
        void Delete(int id);
        void Insert(TodoItem todo);
        void Update(TodoItem todo);
        TodoItem GetById(int id);
        TodoViewModel GetAllTodos();
    }
}