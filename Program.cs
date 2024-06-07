using Microsoft.EntityFrameworkCore;
using loja.Data;
using loja.Models;
using loja.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<ProductService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LojaDbContext>(options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36))));

var app = builder.Build();

// app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/Produtos", async (ProductService productService) =>
{

    var produtos = await productService.GetAllProductsAsync();
    return Results.Ok(produtos);
});

app.MapGet("/ProdutoId/{id}", async (int id, ProductService productService) =>
{
    var produtos = await productService.GetProductByIdAsync(id);

    if (produtos == null)
    {
        return Results.NotFound($"Product with ID {id} not found");
    }
    return Results.Ok(produtos);
});

app.MapPost("/createProduto", async (Produto produto, ProductService productService) =>
{
    productService.AddProductAsync(produto);
    return Results.Created($"/createProduto/{produto.Id}", produto);
});


app.MapPut("/updateProduto/{id}", async (int id, Produto updateProduto, ProductService productService) =>
{
    if (id != updateProduto.Id)
    {
        return Results.BadRequest("Product ID mismatch.");
    }
    await productService.UpdateProductAsync(updateProduto);
    return Results.Ok();
});

app.MapDelete("/deleteProdutos/{id}", async (int id, ProductService productService) =>
{
    await productService.DeleteProductAsync(id);
    return Results.Ok();
});

app.MapPost("/createCliente", async (LojaDbContext dbContext, Cliente newCliente) =>
{
    dbContext.Clientes.Add(newCliente);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/createCliente/{newCliente.Id}", newCliente);
});

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers();

app.Run();
