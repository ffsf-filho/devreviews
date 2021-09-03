using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DevReviews.API.Models;
using DevReviews.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevReviews.API.Controllers
{
    [ApiController]
    [Route("api/{productId}/productreviews")]
    public class ProductReviewscontroller : ControllerBase
    {

        //GET api/products/1/productreview/5
        [HttpGet("{id}")]
        public IActionResult GetById(int productId, int id){
            //Se não existir com o id especificado, retornar Notfound()
            return Ok();
        }
        
        //GET api/products/1/productreviews
        [HttpPost]
        public IActionResult Post(int productId, AddProductReviewInputModel model){
            //Se tiver erros de validação, retornar BadRequest();
            return CreatedAtAction(nameof(GetById), new {id = 1, productId = 2}, model);
        }

    }
}