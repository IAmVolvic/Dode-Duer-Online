namespace API;

public class OLDProgram
{
    //builder.AddPgContainer();
    // var options = builder.Configuration.GetSection(nameof(AppOptions)).Get<AppOptions>()!;

    //DB CONTEXT INIT
    /*
    builder.Services.AddDbContext<>(config =>
    {
        config.UseNpgsql(options.DbConnectionString);
        config.EnableSensitiveDataLogging();
    });
    */
    //builder.Services.AddScoped<IHospitalService, HospitalService>();

    
    
    #region Security
    //RASMUS SECIURITY SECTION 
        
    /*
    builder
        .Services.AddIdentityApiEndpoints<User>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<HospitalContext>();
    builder
        .Services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o => { o.TokenValidationParameters = JwtTokenClaimService.ValidationParameters(options); });
    builder.Services.AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .Build();

    });
    builder.Services.AddScoped<ITokenClaimsService, JwtTokenClaimService>();
    */
    #endregion
    
    
    
    // I DONT KNOW WHAT ABOUT THIS PART, SOMETHING WITH SECURITY
        
    /*
    builder.Services.AddOpenApiDocument(configuration =>
    {
        {
            configuration.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Scheme = "Bearer ",
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });
            //configuration.AddTypeToSwagger<T>(); //If you need to add some type T to the Swagger known types
            configuration.DocumentProcessors.Add(new MakeAllPropertiesRequiredProcessor());

            configuration.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        }
    });
    */
    
    
    
    //CREATING SCOPES 
    /*
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        if (!roleManager.RoleExistsAsync(Role.Reader).GetAwaiter().GetResult())
        {
             roleManager.CreateAsync(new IdentityRole(Role.Reader)).GetAwaiter().GetResult();
        }            File.WriteAllText("current_db.sql", context.Database.GenerateCreateScript());
    }
    */
}