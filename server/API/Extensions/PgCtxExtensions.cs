
using PgCtx;
using Service;

namespace API.Extensions;

public static class PgCtxExtensions
{
    //CONFIGURE LATER WITH OUR OWN CONTEXT
    /*
    public static WebApplicationBuilder AddPgContainer(this WebApplicationBuilder builder)
    {
        var appOptions = builder.Configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();
        if (appOptions.RunInTestContainer)
        {
            var pg = new PgCtxSetup<HospitalContext>();
            builder.Configuration[nameof(AppOptions) + ":" + nameof(AppOptions.DbConnectionString)] =
                pg._postgres.GetConnectionString();
        }

        return builder;
    }
    */
}