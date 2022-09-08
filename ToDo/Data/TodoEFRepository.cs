using Microsoft.EntityFrameworkCore;
using ToDo.Models;
using ToDo.Models.ViewModels;
using ToDoEntityFramework;

namespace ToDo.Data
{

    public class TodoEFRepository : ITodoRepository
    {
        private readonly TodoContext _context;
        public TodoEFRepository(TodoContext dbContext)
        {
            _context = dbContext;
        }

        public void Delete(int id)
        {
            var neededModel = _context.TodoItem.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(neededModel);
            _context.SaveChanges();
        }

        public TodoViewModel GetAllTodos()
        {

            var todos = _context.TodoItem.ToList();

            return new TodoViewModel
            {
                TodoList = todos.Select(x => new Models.TodoItem
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList(),
                Todo = new Models.TodoItem(),
            };

        }

        public Models.TodoItem GetById(int id)
        {
            var todo = _context.TodoItem.Where(x => x.Id == id).FirstOrDefault();

            return new Models.TodoItem
            {
                Name = todo.Name,
                Id = todo.Id
            };
        }

        public void Insert(Models.TodoItem todo)
        {
            var neededModel = new ToDoEntityFramework.TodoItem
            {
                Id = todo.Id,
                Name = todo.Name,
            };


            _context.TodoItem.Add(neededModel);
            _context.SaveChanges();

        }

        public void Update(Models.TodoItem todo)
        {
            var neededModel = _context.TodoItem.Where(x => x.Id == todo.Id).FirstOrDefault();
            neededModel.Name = todo.Name;
            _context.TodoItem.Update(neededModel);
            _context.SaveChanges();

        }
    }
}
