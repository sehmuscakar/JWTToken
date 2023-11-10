using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => //appsettings.json dosyas�nda da token yeri a� ordada propertylerinin baz�lar�n� berlirt
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = true,// izin verilen sitelerin denetlenip denetlenmeyece�i i�in bu propertyi
        ValidateIssuer = true, //hangi sitenin izin verece�ini denetlenip denetlenmeyece�i soruluyor.
        ValidateLifetime = true,// ya�am d�ng�s�n� aktif et yoksa �m�r  boyu olu�an token ile devam eder.
        ValidateIssuerSigningKey = true, // ilgili token�n bize ait olup olmas���n� kontrol ediliyor.
        ValidIssuer = builder.Configuration["Token:Issuer"], // burda token da tan�ml� olan� �a��r�yoruz
        ValidAudience = builder.Configuration["Token:Audience"],
        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        ClockSkew=TimeSpan.Zero // tooken�n ya�am d�ng�s� zaman�na ekleme yapma dedik zero diyerek 
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

app.UseAuthentication();//bunu bildirmen laz�m sisteme yani otantike olma i�lemini bunu token altyap�s�n� ekledi�in zaman eklemen laz�m 
app.UseAuthorization();

app.MapControllers();

app.Run();
