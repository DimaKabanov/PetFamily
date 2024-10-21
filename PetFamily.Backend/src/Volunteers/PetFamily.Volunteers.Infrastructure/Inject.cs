namespace PetFamily.Volunteers.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddVolunteersInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<IReadDbContext, ReadDbContext>();
        
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();

        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));

        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                ?? throw new ApplicationException("Empty minio configuration");
            
            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(false);
        });

        services.AddScoped<IPhotoProvider, MinioProvider>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IMessageQueue<IEnumerable<PhotoInfo>>, InMemoryMessageQueue<IEnumerable<PhotoInfo>>>();

        return services;
    }
}