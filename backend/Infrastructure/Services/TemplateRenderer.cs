using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class TemplateRenderer : ITemplateRenderer
{
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public TemplateRenderer(IWebHostEnvironment environment, IConfiguration configuration)
    {
        _environment = environment;
        _configuration = configuration;
    }

    public async Task<string> RenderAsync(
        string templateName,
        Dictionary<string, string> values)
    {
        var templatePath = Path.Combine(
            _environment.ContentRootPath,
            "Assets",
            "Templates",
            $"{templateName}.html");

        var layoutPath = Path.Combine(
            _environment.ContentRootPath,
            "Assets",
            "Templates",
            "Layout.html");


        var layout = await File.ReadAllTextAsync(layoutPath);
        var template = await File.ReadAllTextAsync(templatePath);

        foreach (var item in values)
        {
            template = template.Replace(
                $"{{{{{item.Key}}}}}",
                item.Value);
        }

        layout = layout.Replace("{{title}}", values.GetValueOrDefault("title", "Notificación"));
        layout = layout.Replace("{{content}}", template);
        layout = layout.Replace("{{logo}}", _configuration["Frontend:BaseUrl"] + "/Assets/logo.png");

        return layout;
    }
}
