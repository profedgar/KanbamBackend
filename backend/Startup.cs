using backend.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace backend
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //10 -Deve ser usada alguma forma de persistência, no C# pode-se usar o Entity Framework (in-memory)

            services.AddDbContext<MeuDbContext>(opt => opt.UseInMemoryDatabase("kanbam"));
            services.ResolveDependencies();
            services.AddMvc();

            var key = Encoding.ASCII.GetBytes(_configuration["environments:JwtKey"]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddCors(options =>
                options.AddPolicy("MyPolicy",
                 builder =>
                 {
                     builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                 }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();



            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            var scopeeee = app.ApplicationServices.CreateScope();

            MeuDbContext context = scopeeee.ServiceProvider.GetRequiredService<MeuDbContext>();

           
            AdicionarDadosTeste(context);
        }

         private void AdicionarDadosTeste(MeuDbContext context)
        {

            string login = _configuration["environments:login"];
            string senha = _configuration["environments:senha"];

            var Usuario1 = new Usuarios 
            {
               
                login=login,
                senha=senha
            };
            context.usuarios.Add(Usuario1);
       
            context.SaveChanges();
        }
    }
}
