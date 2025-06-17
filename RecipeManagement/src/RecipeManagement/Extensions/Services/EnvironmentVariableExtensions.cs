using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.RegularExpressions;

namespace RecipeManagement.Extensions.Services
{
    public static class EnvironmentVariableExtensions
    {
        public static void AddEnvironmentVariableProcessing(this IServiceCollection services, IConfiguration configuration)
        {
            var regex = new Regex(@"\${([^}:]+)(?::[-]([^}]*))?}");

            // Process all configuration sections in RecipeManagement
            ProcessSection(configuration, "RecipeManagement:ConnectionStrings:RecipeManagement", regex);
            ProcessSection(configuration, "RecipeManagement:Auth:Audience", regex);
            ProcessSection(configuration, "RecipeManagement:Auth:Authority", regex);
            ProcessSection(configuration, "RecipeManagement:Auth:AuthorizationUrl", regex);
            ProcessSection(configuration, "RecipeManagement:Auth:TokenUrl", regex);
            ProcessSection(configuration, "RecipeManagement:Auth:ClientId", regex);
            ProcessSection(configuration, "RecipeManagement:Auth:ClientSecret", regex);
            ProcessSection(configuration, "RecipeManagement:RabbitMq:Host", regex);
            ProcessSection(configuration, "RecipeManagement:RabbitMq:VirtualHost", regex);
            ProcessSection(configuration, "RecipeManagement:RabbitMq:Username", regex);
            ProcessSection(configuration, "RecipeManagement:RabbitMq:Password", regex);
            ProcessSection(configuration, "RecipeManagement:RabbitMq:Port", regex);
            ProcessSection(configuration, "RecipeManagement:JaegerHost", regex);
            ProcessSection(configuration, "RecipeManagement:CloudinaryUrl", regex);
        }

        private static void ProcessSection(IConfiguration configuration, string key, Regex regex)
        {
            var value = configuration[key];
            if (string.IsNullOrEmpty(value))
                return;

            var processed = ProcessEnvVars(value, regex);
            configuration[key] = processed;
        }

        private static string ProcessEnvVars(string value, Regex regex)
        {
            return regex.Replace(value, match =>
            {
                var envVarName = match.Groups[1].Value;
                var defaultValue = match.Groups[2].Success ? match.Groups[2].Value : null;

                var envValue = Environment.GetEnvironmentVariable(envVarName);
                return envValue ?? defaultValue ?? string.Empty;
            });
        }
    }
}
