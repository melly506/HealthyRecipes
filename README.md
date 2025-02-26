# HealthyRecipes

## Getting Started
1. Run `docker-compose up --build` from your `.sln` directory to spin up your database(s) and other supporting 
infrastructure depending on your configuration (e.g. RabbitMQ, Keycloak, Jaeger, etc.).
2. If using a Keycloak auth server, you'll need to configure it manually (new realm, client, etc) or use the scaffolded Pulumi setup.
    1. [Install the pulumi CLI](https://www.pulumi.com/docs/get-started/) 
    1. `cd` to your scaffolded Pulumi project
    1. Run `pulumi login --local` to use Pulumi locally
    1. Run `pulumi up` to start the scaffolding process
        > Note: The stack name must match the extension on your yaml config file (e.g. `Pulumi.dev.yaml`) would have a stack of `dev`.
    1. Select yes to apply the configuration to your local Keycloak instance.
        > If you want to reset your pulumi configuration, run `pulumi destroy` to remove all the resources and then `pulumi up` again to start fresh.

### Running Your Project(s)
Once you have your database(s) running, you can run your API(s), BFF, and Auth Servers by using 
the `dotnet run` command or running your project(s) from your IDE of choice.   

### Migrations
Migrations should be applied for you automatically on startup, but if you have any any issues, you can do the following:
    1. Make sure you have a migrations in your boundary project (there should be a `Migrations` directory in the project directory). 
    If there isn't see [Running Migrations](#running-migrations) below.
    2. Confirm your environment (`ASPNETCORE_ENVIRONMENT`) is set to `Development` using 
    `$Env:ASPNETCORE_ENVIRONMENT = "Development"` for powershell or `export ASPNETCORE_ENVIRONMENT=Development` for bash.
    3. `cd` to the boundary project root (e.g. `cd RecipeManagement/src/RecipeManagement`)
    4. Run your project and your migrations should be applied automatically. Alternatively, you can run `dotnet ef database update` to apply your migrations manually.

    > You can also stay in the `sln` root and run something like `dotnet ef database update --project RecipeManagement/src/RecipeManagement`


## Running Migrations
To create a new migration, make sure your environment is set to `Development`:

### Powershell
```powershell
$Env:ASPNETCORE_ENVIRONMENT = "Development"
```

### Bash
```bash
export ASPNETCORE_ENVIRONMENT=Development
```

Then run the following:

```shell
cd .\RecipeManagement\src\RecipeManagement\
dotnet ef migrations add "InitialMigration"
```

To apply your migrations to your local db, make sure your database is running in docker run the following:

```bash
cd YourBoundedContextName/src/YourBoundedContextName
dotnet ef database update
```
