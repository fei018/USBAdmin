using Microsoft.Extensions.DependencyInjection;
using USBModel;
using LoginUserManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Unicode;
using Microsoft.Extensions.Configuration;
using System.Text.Encodings.Web;
using System.IO;
using System;

namespace USBAdminWebMVC
{
    public static class StartupExtension
    {
        public static void AddCustomService(this IServiceCollection services, IConfiguration configuration)
        {
            #region USBAdminHelp
            USBAdminHelp.WebHttpUrlPrefix = configuration.GetSection("WebHttpUrlPrefix").Value;
            USBAdminHelp.InitMenuName = configuration.GetSection("InitMenuName").Value;

            USBAdminHelp.AgentHttpKey = configuration.GetSection("AgentHttpKey").Value;
            if (string.IsNullOrWhiteSpace(USBAdminHelp.AgentHttpKey))
            {
                throw new Exception("USBAdminHelp.AgentHttpKey is empty.");
            }

            USBAdminHelp.AgentUpdateFilePath = configuration.GetSection("Path").GetSection("AgentUpdateFile").Value;
            if (string.IsNullOrWhiteSpace(USBAdminHelp.AgentUpdateFilePath))
            {
                USBAdminHelp.AgentUpdateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Update\\Agent", "Release.zip");
            }
            #endregion

            // cookie
            #region cookie
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = new PathString("/account/login");
                options.LogoutPath = new PathString("/account/login");
                options.AccessDeniedPath = new PathString("/AccessDenied.html");

                // cookie 路徑設為 "/" 路徑, 防止 IIS虛擬目錄站點 http 請求時 不帶 cookie 導致 response code 401 錯誤
                options.Cookie.Path = "/";
            });

            services.AddAntiforgery(optiong =>
            {
                optiong.Cookie.Path = "/";
            });
            #endregion

            services.AddHttpContextAccessor();

            // http response html 拉丁中文不编码
            services.AddSingleton(HtmlEncoder.Create(new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs }));

            services.AddUSBDB(configuration.GetConnectionString("USBAdmin"));
        }

        #region + public static void AddUSBDB(this IServiceCollection services, string connstring)
        public static void AddUSBDB(this IServiceCollection services, string connstring)
        {
            services.AddScoped(x => new LoginUserDb(connstring));
            services.AddScoped<LoginUserService>();

            services.AddScoped(x => new USBAdminDatabaseHelp(connstring));

            services.AddScoped<EmailHelp>();
        }
        #endregion

    }
}
