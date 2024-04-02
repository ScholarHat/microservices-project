using Ocelot.DependencyInjection;

namespace ApiGatewayOcelot
{
    public class Route
    {
        public string DownstreamPathTemplate { get; set; }
        public string DownstreamScheme { get; set; }
        public List<HostAndPort> DownstreamHostAndPorts { get; set; }
        public string UpstreamPathTemplate { get; set; }
        public List<string> UpstreamHttpMethod { get; set; }
        public string Key { get; set; }
        public AuthenticationOptions AuthenticationOptions { get; set; }
        public RouteClaimsRequirement RouteClaimsRequirement { get; set; }
    }

    public class HostAndPort
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class AuthenticationOptions
    {
        public string AuthenticationProviderKey { get; set; }
    }

    public class RouteClaimsRequirement
    {
        public string Roles { get; set; }
    }

    public class Aggregate
    {
        public string UpstreamPathTemplate { get; set; }
        public List<string> RouteKeys { get; set; }
    }

    public class GlobalConfiguration
    {
        public string BaseUrl { get; set; }
    }

    public class OcelotConfiguration
    {
        public List<Route> Routes { get; set; }
        public List<Aggregate> Aggregates { get; set; }
        public GlobalConfiguration GlobalConfiguration { get; set; }
    }

    public class OcelotConfig
    {
        public static void ConfigureOcelotServices(IServiceCollection services, IConfiguration configuration)
        {
            var ocelotConfig = new OcelotConfiguration
            {
                Routes = new List<Route>
                {
                    new Route
                    {
                        DownstreamPathTemplate = "/api/auth/{everything}",
                        DownstreamScheme = "https",
                        DownstreamHostAndPorts = new List<HostAndPort>
                        {
                            new HostAndPort { Host = "localhost", Port = 7134 }
                        },
                        UpstreamPathTemplate = "/auth/{everything}",
                        UpstreamHttpMethod = new List<string> { "Get", "Post" }
                    },
                    // Protected APIs
                    new Route
                    {
                        DownstreamPathTemplate = "/api/product/{everything}",
                        DownstreamScheme = "https",
                        DownstreamHostAndPorts = new List<HostAndPort>
                        {
                            new HostAndPort { Host = "localhost", Port = 7295 }
                        },
                        AuthenticationOptions = new AuthenticationOptions
                        {
                            AuthenticationProviderKey = configuration["Keys:CatalogService"]
                        },
                        RouteClaimsRequirement = new RouteClaimsRequirement
                        {
                            Roles = "Admin"
                        },
                        UpstreamPathTemplate = "/product/{everything}",
                        UpstreamHttpMethod = new List<string> { "Get", "Post", "Put" }
                    }
                    // Add additional routes here as needed
                },
                GlobalConfiguration = new GlobalConfiguration
                {
                    BaseUrl = "https://localhost:7019"
                }
            };

            services.AddOcelot();
            services.AddSingleton(ocelotConfig);
        }
    }
}
