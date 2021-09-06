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

namespace DevReviews.API.Controllers
{
    [ApiController]
    [Route("api/{productId}/productreviews")]
    public class ProductReviewscontroller : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;        
        public ProductReviewscontroller(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/products/1/productreview/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int productId, int id){
            //Se não existir com o id especificado, retornar Notfound()
            var productReview = await _repository.GetReviewByIdAsync(id);

            if(productReview == null){
                return NotFound();
            }

            var productDetails = _mapper.Map<ProductReviewDetailsViewModel>(productReview);

            return Ok(productDetails);
        }
        
        //GET api/products/1/productreviews
        [HttpPost]
        public async Task<IActionResult> Post(int productId, AddProductReviewInputModel model){
            //Se tiver erros de validação, retornar BadRequest();
            var productReview = new ProductReview(model.Author, model.Rating, model.comments, productId);
            await _repository.AddReviewAsync(productReview);

            return CreatedAtAction(nameof(GetById), new {id = productReview.Id, productId = productId}, model);
        }

    }
}