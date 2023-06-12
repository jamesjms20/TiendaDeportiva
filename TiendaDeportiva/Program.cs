var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("ApiProduct", config =>
{
    config.BaseAddress = new Uri(builder.Configuration["ServicesUrl:Product"]);
});

builder.Services.AddHttpClient("ApiPerson", config =>
{
    config.BaseAddress = new Uri(builder.Configuration["ServicesUrl:Order"]);
});
builder.Services.AddHttpClient("ApiCategory", config =>
{
    config.BaseAddress = new Uri(builder.Configuration["ServicesUrl:Category"]);
});
builder.Services.AddHttpClient("ApiPerson", config =>
{
    config.BaseAddress = new Uri(builder.Configuration["ServicesUrl:Person"]);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



builder.Configuration.AddJsonFile("appsettings.json");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
