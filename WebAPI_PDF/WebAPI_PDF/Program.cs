using WebAPI_PDF.Helpers;
using WebAPI_PDF.Templates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<RazorRenderer>();
builder.Services.AddScoped<PdfGenerator>();

// Agregar servicios necesarios para las sesiones
builder.Services.AddDistributedMemoryCache(); // Almacenamiento en memoria para la sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de vida de la sesión
    options.Cookie.HttpOnly = true; // Seguridad del cookie de sesión
    options.Cookie.IsEssential = true; // Necesario para cumplir con GDPR
});

// Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(Program)); // Escanea todos los perfiles en el ensamblado

builder.Services.AddControllers();

var app = builder.Build();

// Configurar el middleware para sesiones
app.UseSession();
app.UseAuthorization();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("GenerateInvoicePdf", async (RazorRenderer renderer, PdfGenerator pdfGenerator) =>
{

    // Generamos datos ficticios en un Modelo
    InvoiceTemplateModel model = new InvoiceTemplateModel
    {
        CustomerName = "John Doe",
        Items = [
                        new InvoiceItemModel
                        {
                            Description = "Item 1",
                            Quantity = 2,
                            UnitPrice = 10.0,

                        },
                        new InvoiceItemModel
                        {
                            Description = "Item 2",
                            Quantity = 1,
                            UnitPrice = 35.0,
                        },
                        new InvoiceItemModel
                        {
                            Description = "Item 3",
                            Quantity = 2,
                            UnitPrice = 10.0,
                        },
                        new InvoiceItemModel
                        {
                            Description = "Item 4",
                            Quantity = 3,
                            UnitPrice = 3.0,
                        },
        ]
    };

    // Renderizamos el componente de Razor a un String pasando los datos del Modelo
    var html = await renderer.RenderComponentAsync<InvoiceTemplate>(new Dictionary<string, object?>
            {
                { "Model", model }
            });

    // Ese mismo String, que contiene todo el HTML, lo convertimos en el documento PDFs
    var pdfBytes = await pdfGenerator.GeneratePdfAsync(html);

    // Guardamos el archivo PDF de forma local
    // System.IO.File.WriteAllBytes("invoice.pdf", pdfBytes);

    // La api descarga el PDF en el navegador
    return Results.File(pdfBytes, "application/pdf", "invoice.pdf");
});


app.Run();