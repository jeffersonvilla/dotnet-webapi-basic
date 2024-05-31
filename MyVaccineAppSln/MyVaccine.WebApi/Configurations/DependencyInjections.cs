using MyFamilyGroup.WebApi.Repositories.Implementations;
using MyFamilyGroup.WebApi.Services.Implementations;
using MyUser.WebApi.Services.Contracts;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Repositories.Implementations;
using MyVaccine.WebApi.Services;
using MyVaccine.WebApi.Services.Contracts;
using MyVaccine.WebApi.Services.Implementations;

namespace MyVaccine.WebApi.Configurations;

public static class DependencyInjections
{
    public static IServiceCollection SetDependencyInjection(this IServiceCollection services)
    {
        #region Repositories Injection
        services.AddScoped<IUserRepository, UserRepository> ();
        services.AddScoped<IBaseRepository<Dependent>, BaseRepository<Dependent>> ();
        services.AddScoped<IVaccineCategoryRepository<VaccineCategory>, VaccineCategoryRepository>();
        services.AddScoped<IVaccineRepository<Vaccine>, VaccineRepository>();
        services.AddScoped<IFamilyGroupRepository<FamilyGroup>, FamilyGroupRepository>();
        services.AddScoped<IAllergyRepository<Allergy>, AllergyRepository>();
        services.AddScoped<IVaccineRecordRepository<VaccineRecord>, VaccineRecordRepository>();

        #endregion

        #region Services Injection

        services.AddScoped<IUserService, UserService> ();
        services.AddScoped<IDependentService, DependentService> ();
        services.AddScoped<IVaccineCategoryService, VaccineCategoryService>();
        services.AddScoped<IVaccineService, VaccineService>();
        services.AddScoped<IFamilyGroupService, FamilyGroupService>();
        services.AddScoped<IAllergyService, AllergyService>();
        services.AddScoped<IVaccineRecordService, VaccineRecordService>();
        #endregion

        #region Only for  testing propourses
        services.AddScoped<IGuidGeneratorScope, GuidServiceScope>();
        services.AddTransient<IGuidGeneratorTrasient, GuidServiceTransient>();
        services.AddSingleton<IGuidGeneratorSingleton, GuidServiceSingleton>();
        services.AddScoped<IGuidGeneratorDeep, GuidGeneratorDeep>();
        #endregion
        return services;
    }
}
