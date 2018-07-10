using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http.Headers;

#region TodoController
namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;
        private static readonly HttpClient _client = new HttpClient();
private static readonly string _remoteUrl = "https://backendappname.azurewebsites.net";
        #endregion

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        #region snippet_GetAll
        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        var data = _client.GetStringAsync($"{GET}/api/Todo").Result;
return JsonConvert.DeserializeObject<List<TodoItem>>(data);
        {
            return _context.TodoItems.ToList();
        }

        #region snippet_GetByID
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
        var data = _client.GetStringAsync($"{GET}/api/Todo/{id}").Result;
return Content(data, "application/json");
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
        #endregion
        #endregion
        #region snippet_Create
        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
        var response = _client.PostAsJsonAsync($"{POST}/api/Todo", item).Result;
var data = response.Content.ReadAsStringAsync().Result;
return Content(data, "application/json");
            if (item == null)
            {
                return BadRequest();
            }

            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }
        #endregion

        #region snippet_Update
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
        var res = _client.PutAsJsonAsync($"{PUT}/api/Todo/{id}", item).Result;
return new NoContentResult()
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }
        #endregion

        #region snippet_Delete
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
        var res = _client.DeleteAsync($"{DELETE}/api/Todo/{id}").Result;
return new NoContentResult();
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }
        #endregion
    }
}

