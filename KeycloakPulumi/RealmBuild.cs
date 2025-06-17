namespace KeycloakPulumi;

using KeycloakPulumi.Extensions;
using KeycloakPulumi.Factories;
using Pulumi;
using Pulumi.Keycloak.Inputs;
using Keycloak = Pulumi.Keycloak;

class RealmBuild : Stack
{
    public RealmBuild()
    {
        var realmName = Environment.GetEnvironmentVariable("KEYCLOAK_REALM_NAME") ?? "DevRealm";
        var baseUrl = Environment.GetEnvironmentVariable("APP_BASE_URL") ?? "https://localhost";
        var basePort = Environment.GetEnvironmentVariable("APP_PORT") ?? "5009";
        var webAppPort = Environment.GetEnvironmentVariable("WEB_APP_PORT") ?? "4378";
        var swaggerClientSecret = Environment.GetEnvironmentVariable("SWAGGER_CLIENT_SECRET") ?? "974d6f71-d41b-4601-9a7a-a33081f80687";
        var postmanClientSecret = Environment.GetEnvironmentVariable("POSTMAN_CLIENT_SECRET") ?? "974d6f71-d41b-4601-9a7a-a33081f84680";
        var webClientSecret = Environment.GetEnvironmentVariable("WEB_CLIENT_SECRET") ?? "974d6f71-d41b-4601-9a7a-a33081f80987";

        var apiBaseUrl = $"{baseUrl}:{basePort}";
        var webAppBaseUrl = $"{baseUrl}:{webAppPort}";

        var realm = new Keycloak.Realm($"{realmName}-realm", new Keycloak.RealmArgs
        {
            RealmName = realmName,
            RegistrationAllowed = true,
            ResetPasswordAllowed = false,
            RememberMe = true,
            EditUsernameAllowed = true
        });
        var recipemanagementScope = ScopeFactory.CreateScope(realm.Id, "recipe_management");

        var recipeManagementSwaggerClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "recipe_management.swagger",
            swaggerClientSecret,
            "RecipeManagement Swagger",
            apiBaseUrl,
            redirectUris: null,
            webOrigins: null
            );
        recipeManagementSwaggerClient.ExtendDefaultScopes("recipe_management");
        recipeManagementSwaggerClient.AddAudienceMapper("recipe_management");
        
        var recipeManagementPostmanCodeClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "recipe_management.postman.code", 
            postmanClientSecret, 
            "RecipeManagement Postman Code",
            "https://oauth.pstmn.io",
            redirectUris: null,
            webOrigins: null
            );
        recipeManagementPostmanCodeClient.ExtendDefaultScopes("recipe_management");
        recipeManagementPostmanCodeClient.AddAudienceMapper("recipe_management");

        var recipeManagementWebClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "recipe_management.web",
            webClientSecret,
            "RecipeManagement Web",
            webAppBaseUrl,
            redirectUris: new InputList<string>()
                {
                $"{webAppBaseUrl}/signin-oidc",
                },
            webOrigins: new InputList<string>()
                {
                apiBaseUrl,
                webAppBaseUrl,
                }
            );
        recipeManagementWebClient.ExtendDefaultScopes("recipe_management");
        recipeManagementWebClient.AddAudienceMapper("recipe_management");
        
        var bob = new Keycloak.User("bob", new Keycloak.UserArgs
        {
            RealmId = realm.Id,
            Username = "bob",
            Enabled = true,
            Email = "bob@domain.com",
            FirstName = "Smith",
            LastName = "Bobson",
            InitialPassword = new UserInitialPasswordArgs
            {
                Value = "bob",
                Temporary = true,
            },
        });

        var alice = new Keycloak.User("alice", new Keycloak.UserArgs
        {
            RealmId = realm.Id,
            Username = "alice",
            Enabled = true,
            Email = "alice@domain.com",
            FirstName = "Alice",
            LastName = "Smith",
            InitialPassword = new UserInitialPasswordArgs
            {
                Value = "alice",
                Temporary = true,
            },
        });
    }
}