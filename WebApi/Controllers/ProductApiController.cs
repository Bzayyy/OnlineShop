using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShopEdmx;
using OnlineShopEdmx.Model;
using OnlineShopping.Repository;
using OnlineShopping.ViewModel;


namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private Populator populator = new Populator();
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDetail>>> Get()
        {

            var result = await Task.FromResult(new ProductViewModelLst
            {
                dbModelLst = _unitOfWork.GetRepositoryInstance<ProductDetail>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false).ToList()
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
        public ActionResult<ProductDetail> Create(ProductDetail item)
        {
            _unitOfWork.GetRepositoryInstance<ProductDetail>().Add(item);

            return CreatedAtRoute("GetProduct", new { id = item.ProductId }, item);
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
        public async Task<ActionResult<ProductDetail>> GetProduct(int id)
        {
            var productItems = _unitOfWork.GetRepositoryInstance<ProductDetail>().GetFirstorDefault(id);
            if (productItems == null)
            {
                return NotFound();
            }
            return productItems;
        }

    }
}