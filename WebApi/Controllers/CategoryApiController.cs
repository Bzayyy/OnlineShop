using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopEdmx;
using OnlineShopEdmx.Model;
using OnlineShopping.Repository;
using OnlineShopping.ViewModel;


namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private Populator populator = new Populator();
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDetail>>> Get()
        {

            var result = await Task.FromResult(new CategoryViewModelLst
            {
                dbModelLst = _unitOfWork.GetRepositoryInstance<CategoryDetail>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false).ToList()
            }) ;
            return result.dbModelLst.ToList();

        }

        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>            
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CategoryDetail> Create(CategoryDetail item)
        {
            _unitOfWork.GetRepositoryInstance<CategoryDetail>().Add(item);

            return CreatedAtRoute("GetProduct", new { id = item.CategoryId}, item);
        }


        // PUT: api/TodoItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, CategoryDetail todoItem)
        {
            eCommerceEntities _context = new eCommerceEntities();

            if (id != todoItem.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            eCommerceEntities _context = new eCommerceEntities();
            var todo = _context.Product.Find(id);

            if (todo == null)
            {
                return NotFound();
            }

            _context.Product.Remove(todo);
            _context.SaveChanges();

            return NoContent();
        }


        // GET: api/ProductItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDetail>> GetProduct(int id)
        {
            var productItems = _unitOfWork.GetRepositoryInstance<CategoryDetail>().GetFirstorDefault(id);
            if (productItems == null)
            {
                return NotFound();
            }
            return productItems;
        }

        private bool TodoItemExists(int id)
        {
            eCommerceEntities _context = new eCommerceEntities();

            return _context.Category.Any(e => e.CategoryId == id);
        }
    }
}