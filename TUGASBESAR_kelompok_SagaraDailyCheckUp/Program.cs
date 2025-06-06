﻿using TUGASBESAR_kelompok_SagaraDailyCheckUp.Model;
using TUGASBESAR_kelompok_SagaraDailyCheckUp;

var builder = WebApplication.CreateBuilder(args);

// Ambil konfigurasi dari appsettings.json
var config = builder.Configuration;
string appName = config["AppSettings:AppName"];
string version = config["AppSettings:Version"];
bool.TryParse(config["AppSettings:DebugMode"], out bool debugMode);

Console.WriteLine($"Nama Aplikasi: {appName}, Versi: {version}, Debug: {debugMode}");

// Tambahkan layanan
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// ✅ Tambahkan CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Konfigurasi pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ Aktifkan CORS
app.UseCors();

app.UseAuthorization();
app.MapControllers();

// 🔥 Jalankan Web API sebagai background task
var apiTask = Task.Run(() => app.RunAsync());

// 🔥 Jalankan CLI Menu setelah API dimulai
PilihMenu.PilihMenu1();

// Tunggu API task selesai jika user menutup menu CLI
await apiTask;
