using App.Localization;
using App.Orders;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Data;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace App;

[DependsOn(
    typeof(AppApplicationContractsModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpAspNetCoreMvcModule)
)]
public class AppHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AppHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var connectionString = BuildConnectionString(configuration);

        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = connectionString;
        });

        ConfigureLocalization();
        ConfigureConventionalControllers();
        ConfigureCors(context, configuration);
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AppResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers
                .Create(typeof(IOrderAppService).Assembly, opts =>
                {
                    opts.RootPath = "app";
                });
        });
    }

    private string BuildConnectionString(IConfiguration configuration)
    {
        return $"Host={configuration["DB_HOST"]};" +
           $"Port={configuration["DB_PORT"]};" +
           $"Database={configuration["DB_NAME"]};" +
           $"Username={configuration["DB_USERNAME"]};" +
           $"Password={configuration["DB_PASSWORD"]}";
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                var corsOrigins = configuration["CORS_ORIGINS"]?
                    .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    ?? new[] { "http://localhost:3000" };

                builder
                    .WithOrigins(corsOrigins)
                    .WithAbpExposedHeaders()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();

                //builder
                //    .AllowAnyOrigin()
                //    .AllowAnyHeader()
                //    .AllowAnyMethod();
            });
        });
    }
}