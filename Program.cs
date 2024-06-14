using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using loja.Data;
using loja.Models;
using loja.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("abc"))
        };
    });

// Add services to the container.

// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<FornecedorService>();
builder.Services.AddScoped<UsuarioService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LojaDbContext>(options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36))));

private readonly LojaDbContext _lojaBbContex;

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
    await productService.AddProductAsync(produto);
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

//**************************************************************

app.MapGet("/Fornecedores", async (FornecedorService fornecedorService) =>
{

    var fornecedores = await fornecedorService.GetAllFornecedoresAsync();
    return Results.Ok(fornecedores);
});

app.MapGet("/FornecedorId/{id}", async (int id, FornecedorService fornecedorService) =>
{
    var fornecedor = await fornecedorService.GetFornecedorByIdAsync(id);

    if (fornecedor == null)
    {
        return Results.NotFound($"Fornecedor with ID {id} not found");
    }
    return Results.Ok(fornecedor);
});

app.MapPost("/createFornecedor", async (Fornecedor fornecedor, FornecedorService fornecedorService) =>
{
    await fornecedorService.AddFornecedorAsync(fornecedor);
    return Results.Created($"/createFornecedor/{fornecedor.Id}", fornecedor);
});


app.MapPut("/updateFornecedor/{id}", async (int id, Fornecedor updateFornecedor, FornecedorService fornecedorService) =>
{
    if (id != updateFornecedor.Id)
    {
        return Results.BadRequest("Fornecedor ID mismatch.");
    }
    await fornecedorService.UpdateFornecedorAsync(updateFornecedor);
    return Results.Ok();
});

app.MapDelete("/deleteFornecedor/{id}", async (int id, FornecedorService fornecedorService) =>
{
    await fornecedorService.DeleteFornecedorAsync(id);
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

app.MapPost("/login", async (HttpContext context) =>
{
  using var reader = new StreamReader(context.Request.Body);
  var body = await reader.ReadToEndAsync();

  var json = JsonDocument.Parse(body);
  var username = json.RootElement.GetProperty("username").GetString();
  var email = json.RootElement.GetProperty("email").GetString();
  var senha = json.RootElement.GetProperty("senha").GetString();

  var token = "";
  if (senha == )
  {
    token = GenerateToken(email);
  }
  //return token;
  context.Response.WriteAsync(token);
});

string GenerateToken(string data)
{
  var tokenHandler = new JwtSecurityTokenHandler();
  var secretKey = Encoding.ASCII.GetBytes("abcabcabcabcabcabcabcabcabcabcabcabc"); //Esta chave sera gravada em uma variavel de ambiente
  var tokenDecriptor = new SecurityTokenDescriptor
  {
    Expires = DateTime.UtcNow.AddHours(1),
    SigningCredentials = new SigningCredentials(
          new SymmetricSecurityKey(secretKey),
          SecurityAlgorithms.HmacSha256Signature
      )
  };

  var token = tokenHandler.CreateToken(tokenDecriptor);

  return tokenHandler.WriteToken(token); //Converte para string
}

app.Run();
