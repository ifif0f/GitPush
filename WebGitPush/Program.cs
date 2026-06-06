using WebGitPush.Classes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddScoped<ConnectionApi>();

builder.Services.AddMvc(op => op.EnableEndpointRouting = false);

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.UseMvcWithDefaultRoute();
app.UseStatusCodePages();

app.Run();