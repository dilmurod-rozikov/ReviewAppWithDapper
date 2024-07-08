using ReviewApp.DataAccess;
using ReviewApp.Interfaces;
using ReviewApp.Repository;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
        builder.Services.AddScoped<ICountryRepository, CountryRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
        builder.Services.AddScoped<IReviewerRepository, ReviewerRepository>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

        builder.Services.AddSingleton<SqlDataAccess>();

        builder.Services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.IncludeFields = true);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}