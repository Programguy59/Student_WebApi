using Microsoft.EntityFrameworkCore;
using Contracts;
using Repository;

using Entities;
using LoggerService;
using ServicesContracts;
using Services;
using System.Reflection;

namespace Student_WebApi.Extensions
{
  public static class ServiceExtensions
  {
    public static void ConfigureCors(this IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed((host) => true));
      });
    }

    public static void ConfigureIISIntegration(this IServiceCollection services)
    {
      services.Configure<IISOptions>(options =>
      {

      });
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
      services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureMsSqlContext(this IServiceCollection services, IConfiguration config)
    {
      var connectionString = config["ConnectionStrings:Student_WebApiDBConnectionString"];

      services.AddDbContext<DatabaseContext>(o => o.UseSqlServer(connectionString, x => x.MigrationsAssembly("Entities")));
    }

    public static void ConfigureRepositoryWrapper(this IServiceCollection services)
    {
      services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
    }

    public static void ConfigureServiceLayerWrappers(this IServiceCollection services)
    {
      //services.AddScoped<ICityService, CityService>();
      //services.AddScoped<ICityLanguageService, CityLanguageService>();
      //services.AddScoped<IPointOfInterestService, PointOfInterestService>();
    }

    // The function below has been made to handle Update problem when using Mapster.
    public static bool CloneData<T>(this T target, object source, bool MakeTarget = false)
    {
      var targetHere = (T)Activator.CreateInstance(typeof(T));

      if (!MakeTarget)
      {
        targetHere = target;
      }
      else
      {
        target = targetHere;
      }

      Type objTypeBase = source.GetType();
      Type objTypeTarget = targetHere.GetType();

      PropertyInfo _propinfo = null;
      var propInfos = objTypeBase.GetProperties(BindingFlags.Instance | BindingFlags.Public);
      foreach (var propInfo in propInfos)
      {
        try
        {
          _propinfo = objTypeTarget.GetProperty(propInfo.Name, BindingFlags.Instance | BindingFlags.Public);
          if (_propinfo != null)
          {
            _propinfo.SetValue(targetHere, propInfo.GetValue(source));
          }
        }
        catch (ArgumentException ex)
        {
          if (!string.IsNullOrEmpty(ex.Message))
            continue;
        }
        catch (Exception ex)
        {
          if (!string.IsNullOrEmpty(ex.Message))
            //return default(T); 
            return (false);
        }
      }

      return (true);
    }
  }
}
