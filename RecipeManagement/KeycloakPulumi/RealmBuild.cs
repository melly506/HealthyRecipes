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
        var realm = new Keycloak.Realm("DevRealm-realm", new Keycloak.RealmArgs
        {
            RealmName = "DevRealm",
            RegistrationAllowed = true,
            ResetPasswordAllowed = true,
            RememberMe = true,
            EditUsernameAllowed = true
        });
        var recipemanagementScope = ScopeFactory.CreateScope(realm.Id, "recipe_management");
        
        var recipeManagementSwaggerClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "recipe_management.swagger", 
            "974d6f71-d41b-4601-9a7a-a33081f80687", 
            "RecipeManagement Swagger",
            "https://localhost:5009",
            redirectUris: null,
            webOrigins: null
            );
        recipeManagementSwaggerClient.ExtendDefaultScopes("recipe_management");
        recipeManagementSwaggerClient.AddAudienceMapper("recipe_management");
        
        var recipeManagementPostmanCodeClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "recipe_management.postman.code", 
            "974d6f71-d41b-4601-9a7a-a33081f84680", 
            "RecipeManagement Postman Code",
            "https://oauth.pstmn.io",
            redirectUris: null,
            webOrigins: null
            );
        recipeManagementPostmanCodeClient.ExtendDefaultScopes("recipe_management");
        recipeManagementPostmanCodeClient.AddAudienceMapper("recipe_management");
        
        var recipeManagementWebClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "recipe_management.web", 
            "974d6f71-d41b-4601-9a7a-a33081f80987", 
            "RecipeManagement Web",
            "https://localhost:4378",
            redirectUris: new InputList<string>() 
                {
                "https://localhost:4378/signin-oidc",
                },
            webOrigins: new InputList<string>() 
                {
                "https://localhost:5009",
                "https://localhost:4378",
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