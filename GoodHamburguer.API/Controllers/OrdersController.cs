using GoodHamburguer.Application.DTOs;
using GoodHamburguer.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order is null)
            return NotFound(new ErrorResponse { Message = "Pedido não encontrado", Errors = [$"Nenhum pedido encontrado com o id {id}."] });

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        if (request is null)
            return BadRequest(new ErrorResponse { Message = "Requisição inválida.", Errors = ["O corpo da requisição não pode ser vazio."] });

        var (result, error) = await _orderService.CreateOrderAsync(request);
        if (error is not null)
            return BadRequest(error);

        return CreatedAtAction(nameof(GetById), new { id = result!.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderRequest request)
    {
        if (request is null)
            return BadRequest(new ErrorResponse { Message = "Requisição inválida.", Errors = ["O corpo da requisição não pode ser vazio."] });

        var (result, error) = await _orderService.UpdateOrderAsync(id, request);

        if (error is not null)
        {
            if (error.Message == "Pedido não encontrado")
                return NotFound(error);

            return BadRequest(error);
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _orderService.DeleteOrderAsync(id);
        if (!deleted)
            return NotFound(new ErrorResponse { Message = "Pedido não encontrado", Errors = [$"Nenhum pedido encontrado com o id {id}."] });

        return NoContent();
    }
}

