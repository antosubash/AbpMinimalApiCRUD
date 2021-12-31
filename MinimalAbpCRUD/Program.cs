using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddAppSettingsSecretsJson().UseAutofac();
builder.Services.ReplaceConfiguration(builder.Configuration);
builder.Services.AddApplication<MinimalModule>();
var app = builder.Build();

app.MapGet("/book", async ([FromServices] IRepository<Book, Guid> repository) =>
{
    return await repository.GetListAsync();
});

app.MapPost("/book", async (string name, [FromServices] IRepository<Book, Guid> repository) =>
{
    var newBook = await repository.InsertAsync(new Book(Guid.NewGuid(),name));
    return Results.Created($"/book/{newBook.Id}", newBook);
});

app.MapPut("/book/{id}", async (Guid id, string name, [FromServices] IRepository<Book, Guid> repository) =>
{
    var book = await repository.GetAsync(id);
    book.Name = name;
    return await repository.UpdateAsync(book);
});

app.MapDelete("/book/{id}", async (Guid id, [FromServices] IRepository<Book, Guid> repository) =>
{
    var book = await repository.GetAsync(id);
    await repository.DeleteAsync(id);
});

app.InitializeApplication();
app.Run();

public class Book : AggregateRoot<Guid>
{
    public Book(Guid id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
    public string Name { get; set; }
}

public class MyDbContext : AbpDbContext<MyDbContext>
{
    public DbSet<Book> Books => Set<Book>();

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Book>(b =>
        {
            b.ToTable("Books");
            b.ConfigureByConvention();
            b.HasData(new Book(Guid.NewGuid(),"My Book"));
        });
    }
}

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
)]
public class MinimalModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {  
        context.Services.AddEndpointsApiExplorer();
        context.Services.AddSwaggerGen();   
        context.Services.AddAbpDbContext<MyDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
        }); 
        Configure<AbpDbContextOptions>(options =>
        {
            options.UseSqlite();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
    }
}