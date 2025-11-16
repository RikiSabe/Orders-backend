using Microsoft.AspNetCore.Builder;
using App;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("App.Web.csproj"); 
await builder.RunAbpModuleAsync<AppWebTestModule>(applicationName: "App.Web");

public partial class Program
{
}
