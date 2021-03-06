﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebFeatures.Application.Constants;
using WebFeatures.Application.Features.Products.CreateProduct;
using WebFeatures.Application.Features.Products.DeleteProduct;
using WebFeatures.Application.Features.Products.GetProduct;
using WebFeatures.Application.Features.Products.GetProductComments;
using WebFeatures.Application.Features.Products.GetProductList;
using WebFeatures.Application.Features.Products.GetProductReviews;
using WebFeatures.Application.Features.Products.UpdateProduct;
using WebFeatures.Application.Infrastructure.Results;
using WebFeatures.WebApi.Attributes;
using WebFeatures.WebApi.Controllers.Base;

namespace WebFeatures.WebApi.Controllers
{
    /// <summary>
    /// Товары
    /// </summary>
    public class ProductsController : BaseController
    {
        /// <summary>
        /// Получить товар
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Товар</returns>
        /// <response code="200" cref="ProductInfoDto">Успех</response>
        /// <response code="400" cref="ValidationError">Товар отсутствует</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProductInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.SendAsync(new GetProductQuery() { Id = id }));
        }

        /// <summary>
        /// Получить список товаров
        /// </summary>
        /// <returns>Список</returns>
        /// <response code="200" cref="IEnumerable{ProductInfoDto}">Успех</response>
        [HttpGet("list")]
        [ProducesResponseType(typeof(IEnumerable<ProductListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList()
        {
            return Ok(await Mediator.SendAsync(new GetProductListQuery()));
        }

        /// <summary>
        /// Получить обзоры на товар
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Обзоры</returns>
        /// <response code="200" cref="IEnumerable{ProductReviewInfoDto}">Успех</response>
        [HttpGet("{id:guid}/reviews")]
        [ProducesResponseType(typeof(IEnumerable<ProductReviewInfoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviews(Guid id)
        {
            return Ok(await Mediator.SendAsync(new GetProductReviewsQuery() { Id = id }));
        }

        /// <summary>
        /// Получить комментарии к товару
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Комментарии</returns>
        /// <response code="200" cref="IEnumerable{ProductCommentInfoDto}">Успех</response>
        [HttpGet("{id:guid}/comments")]
        [ProducesResponseType(typeof(IEnumerable<ProductCommentInfoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetComments(Guid id)
        {
            return Ok(await Mediator.SendAsync(new GetProductCommentsQuery() { Id = id }));
        }

        /// <summary>
        /// Создать товар
        /// </summary>
        /// <returns>Идентификатор созданного товара</returns>
        /// <response code="201" cref="Guid">Успех</response>
        /// <response code="400" cref="ValidationError">Ошибка валидации</response>
        /// <response code="403">Доступ запрещен</response>
        [HttpPost]
        [Authorize]
        [AuthorizePermission(PermissionConstants.Products.Create)]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromForm][Required] CreateProductCommand request)
        {
            return Created(await Mediator.SendAsync(request));
        }

        /// <summary>
        /// Редактировать товар
        /// </summary>
        /// <response code="200">Успех</response>
        /// <response code="400" cref="ValidationError">Ошибка валидации</response>
        /// <response code="403">Доступ запрещен</response>
        [HttpPut("{id:guid}")]
        [Authorize]
        [AuthorizePermission(PermissionConstants.Products.Update)]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(Guid id, [FromForm][Required] UpdateProductCommand request)
        {
            await Mediator.SendAsync(request);

            return Ok();
        }

        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <response code="204">Успех</response>
        /// <response code="400" cref="ValidationError">Товар отсутствует</response>
        /// <response code="403">Доступ запрещен</response>
        [HttpDelete("{id:guid}")]
        [Authorize]
        [AuthorizePermission(PermissionConstants.Products.Delete)]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.SendAsync(new DeleteProductCommand() { Id = id });

            return NoContent();
        }
    }
}