using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevReviews.API.Entities;
using DevReviews.API.Models;
using DevReviews.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static DevReviews.API.Models.ProductDetailsViewModel;

namespace DevReviews.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Productscontroller : ControllerBase
    {
        private readonly DevReviewsDbContext _dbContext;
        private readonly IMapper _mapper;

        public Productscontroller(DevReviewsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }        

        [HttpGet]
        public IActionResult GetAll(){
            var products = _dbContext.Products;
            var productViewModel = _mapper.Map<List<ProductViewModel>>(products);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id){
            //Se não achar, retornar Notfound()
            var product = _dbContext.Products.SingleOrDefault(p => p.Id == id);

            if (product == null){
                return NotFound();
            } 

            var productDetails = _mapper.Map<ProductDetailsViewModel>(product);

            return Ok(productDetails);
        }
        
        //POST para api/products
        [HttpPost]
        public IActionResult Post(AddProductInputModel model){
            //Se tiver erros de validação, retornar BadRequest();
            var product = new Product(model.Title, model.Description, model.Price);
            _dbContext.Products.Add(product);

            return CreatedAtAction(nameof(GetById), new {id = product.Id}, model);
        }

        //PUT para apí/products/{id}
        [HttpPut("{id}")]
        public  IActionResult Put(int id,UpdateProductInputModel model){
            //Se tiver erros de validação, retornar BadRequest();
            //Se não existir produto com o id especificado, retornar Notfound()

            if(model.Description.Length > 50){
                return BadRequest();
            }
            var product = _dbContext.Products.SingleOrDefault(p => p.Id == id);

            if (product == null){
                return NotFound();
            }
            product.Update(model.Description, model.Price);
            
            return NoContent();
        }
    }
}