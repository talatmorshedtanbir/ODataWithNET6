using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ODataWithNET6.Contexts.DBContexts;
using ODataWithNET6.DataAccess.Abstract;
using ODataWithNET6.DataAccess.Concrete;
using ODataWithNET6.Entities;
using ODataWithNET6.Services.Abstract;
using ODataWithNET6.Services.Concrete;

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<Note>("Notes");
    return builder.GetEdmModel();
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddOData(opt => opt
    .AddRouteComponents("v1", GetEdmModel())
    .Filter()
    .Select()
    .Expand());

// Add services to the container.
builder.Services.AddDbContext<NoteAppContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddTransient<INotesRepository, NotesRepository>();
builder.Services.AddTransient<INotesService, NotesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.Run();
