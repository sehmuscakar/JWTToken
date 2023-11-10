using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => //appsettings.json dosyasýnda da token yeri aç ordada propertylerinin bazýlarýný berlirt
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = true,// izin verilen sitelerin denetlenip denetlenmeyeceði için bu propertyi
        ValidateIssuer = true, //hangi sitenin izin vereceðini denetlenip denetlenmeyeceði soruluyor.
        ValidateLifetime = true,// yaþam döngüsünü aktif et yoksa ömür  boyu oluþan token ile devam eder.
        ValidateIssuerSigningKey = true, // ilgili tokenýn bize ait olup olmasýðýný kontrol ediliyor.
        ValidIssuer = builder.Configuration["Token:Issuer"], // burda token da tanýmlý olaný çaðýrýyoruz
        ValidAudience = builder.Configuration["Token:Audience"],
        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        ClockSkew=TimeSpan.Zero // tookenýn yaþam döngüsü zamanýna ekleme yapma dedik zero diyerek 
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();//bunu bildirmen lazým sisteme yani otantike olma iþlemini bunu token altyapýsýný eklediðin zaman eklemen lazým 
app.UseAuthorization();

app.MapControllers();

app.Run();
