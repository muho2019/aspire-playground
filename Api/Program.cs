using Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//1. ServiceDefaults 추가 (Aspire 기본 설정: 텔레메트리, 헬스체크 등)
builder.AddServiceDefaults();

builder.Services.AddControllers();

// 3. MSSQL DbContext 등록
// "appdata"는 AppHost에서 .AddDatabase("appdata")로 지정한 이름과 같아야 합니다.
builder.AddSqlServerDbContext<AppDbContext>("appdata");

// 4. Redis 캐시 등록
// "cache"는 AppHost에서 .AddRedis("cache")로 지정한 이름과 같아야 합니다.
builder.AddRedisClient("cache");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints(); // 헬스체크 등 엔드포인트 매핑

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Starting migration...");
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // DB가 없으면 생성하고, 마이그레이션 적용
        await db.Database.MigrateAsync();
    }
}

app.Run();
