using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Menambahkan controller dan layanan lainnya
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Mengonfigurasi pipeline API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Menjalankan API di background
var apiTask = Task.Run(() => app.Run());

// Menampilkan menu
await Menu.ShowMenu();

// Menunggu hingga API selesai
await apiTask;
