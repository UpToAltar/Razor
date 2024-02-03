using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Razor.Mail;
using Razor.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
// Add DBContext
services.AddDbContext<RazorDb>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("RazorDB");
    options.UseSqlServer(connectionString);
});
// Add Identity
// services.AddIdentity<AppUser, IdentityRole>()
//     .AddEntityFrameworkStores<RazorDb>()
//     .AddDefaultTokenProviders();

// Add Identity with UI
services.AddDefaultIdentity<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<RazorDb>()
    .AddDefaultTokenProviders();
    

// Truy cập IdentityOptions
services.Configure<IdentityOptions> (options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;         // Xác thực tài khoản
});

// Cấu hình Cookie
services.ConfigureApplicationCookie (options => {
    // options.Cookie.HttpOnly = true;
    // options.ExpireTimeSpan = TimeSpan.FromMinutes (5);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});
//Add Authentication google, facebook
services.AddAuthentication()
    .AddGoogle(options =>
    {
        IConfigurationSection googleAuthNSection =
            builder.Configuration.GetSection("Authentication:Google");

        options.ClientId = googleAuthNSection["ClientId"];
        options.ClientSecret = googleAuthNSection["ClientSecret"];
        options.CallbackPath = googleAuthNSection["CallbackPath"];
    })
    .AddFacebook(options =>
    {
        IConfigurationSection facebookAuthNSection =
            builder.Configuration.GetSection("Authentication:Facebook");

        options.AppId = facebookAuthNSection["AppId"];
        options.AppSecret = facebookAuthNSection["AppSecret"];
        options.CallbackPath = facebookAuthNSection["CallbackPath"];
    });
//Mail
services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
services.AddScoped<IEmailSender, SendMailService>();
// Razor
services.AddRazorPages();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();