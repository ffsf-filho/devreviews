using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevReviews.API.Entities;
using DevReviews.API.Models;
using DevReviews.API.Persistence;
using DevReviews.API.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static DevReviews.API.Models.ProductDetailsViewModel;

namespace DevReviews.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Productscontroller : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public Productscontroller(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }        

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var products = await _repository.GetAllAsync();
            var productViewModel = _mapper.Map<List<ProductViewModel>>(products);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id){
            //Se não achar, retornar Notfound()
            var product = await _repository.GetDetailsByIdAsync(id);

            if (product == null){
                return NotFound();
            } 

            var productDetails = _mapper.Map<ProductDetailsViewModel>(product);

            return Ok(productDetails);
        }
        
        //POST para api/products
        [HttpPost]
        public async Task<IActionResult> Post(AddProductInputModel model){
            //Se tiver erros de validação, retornar BadRequest();
            var product = new Product(model.Title, model.Description, model.Price);

            return CreatedAtAction(nameof(GetById), new {id = product.Id}, model);
        }

        //PUT para apí/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,UpdateProductInputModel model){
            //Se tiver erros de validação, retornar BadRequest();
            //Se não existir produto com o id especificado, retornar Notfound()

            if(model.Description.Length > 50){
                return BadRequest();
            }
            var product = await _repository.GetByIdAsync(id);

            if (product == null){
                return NotFound();
            }
            product.Update(model.Description, model.Price);
            await _repository.UpdateAsync(product);
            
            return NoContent();
        }
    }
}