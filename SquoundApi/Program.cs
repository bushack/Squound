using Microsoft.EntityFrameworkCore;

using SquoundApi.Data;
using SquoundApi.Factories;
using SquoundApi.Interfaces;
using SquoundApi.Models;
using SquoundApi.Services;


var builder = WebApplication.CreateBuilder(args);

// Allow external access to API from Android device via Kestrel.
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.ListenAnyIP(5050); // http
//    serverOptions.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps()); // https
//});

// Add SQL database to the project.
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ProductDb;Trusted_Connection=True;"));

// Add services to the container.
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IDtoFactory, DtoFactory>();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed the database with sample data.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

    if (db.Categories.Any() == false)
    {
        db.Categories.AddRange(
            new CategoryModel { Name = "None" },
            new CategoryModel { Name = "Lighting" });
        db.SaveChanges();
    }

    if (db.Products.Any() == false)
    {
        var CatNone = db.Categories.First(c => c.Name == "None").Id;
        var CatLighting = db.Categories.First(c => c.Name == "Lighting").Id;

        db.Products.AddRange(
            new ProductModel
            {
                CategoryId = CatNone,
                Name = "1970s Teak Cabinet",
                Manufacturer = "Abbess",
                Description = "Teak sliding doors with inset aluminium circular pulls, robust metal legs and adjustable internal shelf. Versatile and charming.",
                Quantity = 1,
                Price = 1234.56m,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_1_.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            },
            new ProductModel
            {
                CategoryId = CatNone,
                Name = "Mid-Century Tallboy",
                Manufacturer = "Wrighton Furniture",
                Description = "Elegant styling, circular pulls, sculptural leg detailing and beautiful timbers make this an especially unusual & desirable piece.",
                Quantity = 1,
                Price = 2345.67m,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/mid_century_tallboy_wrighton.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            },
            new ProductModel
            {
                CategoryId = CatNone,
                Name = "1960s Tallboy",
                Manufacturer = "Austinsuite",
                Description = "Designed by Frank Guille for top end British maker Austinsuite. Elegant lines, sculpted full length pulls, tapered legs and differing drawer sizes combine with high quality timbers and beautiful craftmanship making this one of the most desirable British mid century tallboys you can find.",
                Quantity = 1,
                Price = 3456.78m,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/tallboy_austinsuite.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            },
            new ProductModel
            {
                CategoryId = CatLighting,
                Name = "Mid-Century Lamp",
                Manufacturer = "",
                Description = "Feast your eyes on this magnificent, sculptural 1960s solid teak & brass floor lamp which we’ve paired with this eyegasmic, vibrant handmade Swedish dog fabric drum shade.",
                Quantity = 1,
                Price = 4567.89m,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/mid_century_lamp_a.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            },
            new ProductModel
            {
                CategoryId = CatNone,
                Name = "Mid-Century Sideboard",
                Manufacturer = "",
                Description = "Cast your gaze over this truly outstanding Scandinavian teak sideboard. Sliding doors, circular pulls, tapered legs and an incredible teak grain make this one of our favourites in a good while. Hugely desirable and stylish.",
                Quantity = 1,
                Price = 9876.54m,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/mid_century_sideboard_a.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            },
            new ProductModel
            {
                CategoryId = CatLighting,
                Name = "Mid-Century Lamp",
                Manufacturer = "",
                Description = "Feast your eyes on this magnificent, sculptural 1960s solid teak floor lamp which we’ve paired with this eyegasmic, vibrant handmade Swedish cat fabric drum shade.",
                Quantity = 1,
                Price = 8765.43m,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/mid_century_lamp_b.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            },
            new ProductModel
            {
                CategoryId = CatNone,
                Name = "Mid-Century Tallboy",
                Manufacturer = "Homeworthy Furniture",
                Description = "Quality craftmanship, curvaceous pulls, overhanging top and afromosia detailing; classic mid century styling & super desirable.",
                Quantity = 1,
                Price = 7654.32m,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/tallboy_homeworthy.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            },
            new ProductModel
            {
                CategoryId = CatNone,
                Name = "Mid-Century Tallboy",
                Manufacturer = "Avalon Furniture",
                Description = "Beautifully grained, rich honey coloured timbers, sculpted solid afromosia pulls and shapely flaring solid beech legs; a fabulous, classic design from Avalon.",
                Quantity = 1,
                Price = 6543.21m,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/tallboy_avalon.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            },
            new ProductModel
            {
                CategoryId = CatNone,
                Name = "Mid-Century Sideboard",
                Manufacturer = "",
                Description = "",
                Quantity = 1,
                Price = 1010.10m,
                Image0 = "https://raw.githubusercontent.com/bushack/images/main/mid_century_sideboard_b.jpg",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            },
            new ProductModel
            {
                CategoryId = CatNone,
                Name = "Mid-Century Table",
                Manufacturer = "",
                Description = "",
                Quantity = 1,
                Price = 2848.46m,
                Image0 = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRuP0X2tY9on2gHhGKTjTwfJQts2GsbCT8QxA&s",
                Image1 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_2_.jpg",
                Image2 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_3_.jpg",
                Image3 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_4_.jpg",
                Image4 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_5_.jpg",
                Image5 = "https://raw.githubusercontent.com/bushack/images/main/cabinet_abbess_6_.jpg"
            });

        db.SaveChanges();
    }
}

app.Run();
