using Domain.Data.Repository;
using Domain.DbContex;
using Domain.Services.Accounts;
using Domain.Services.Inventory;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Domain.Interface;
using Domain.Services.Shared;
using Microsoft.AspNetCore.Http;
using Domain.Services.Settings;


public static class ServiceRegistrationExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<DbConnectionDapper>();

        services.AddScoped<ProductRepository>();
        //services.AddScoped<TaskService>();
        //services.AddScoped<OrderService>();
        //services.AddScoped<OrderServiceWithSp>();
        services.AddScoped<ColorService>();
        services.AddScoped<CustomerService>();
        services.AddScoped<UserService>();
        services.AddScoped<CompanyService>();
        services.AddScoped<InvoiceService>();
        services.AddScoped<InvoiceItemService>();
        services.AddScoped<ProductSpecificationService>();
        services.AddScoped<ProductSkuService>();
        services.AddScoped<WarehouseService>();
        services.AddScoped<NotificationByService>();
        services.AddScoped<CountryServiceV2>();
        services.AddScoped<ShippingByService>();
        services.AddScoped<UnitService>();
        services.AddScoped<BusinessTypesService>();
        services.AddScoped<RoleService>();
        services.AddScoped<InvoiceTypeService>();
        services.AddScoped<LocationService>();
        services.AddScoped<ProductOrCupponCodeService>();
        services.AddScoped<EmailSetupService>();
        services.AddScoped<SmsSettinsService>();
        services.AddScoped<ProductService>();
        services.AddScoped<BasicColumnPermissionService>();
        services.AddScoped<PageDetailsService>();
        services.AddScoped<ReviewService>();
        services.AddScoped<SupplierService>();
        services.AddScoped<LanguageService>();
        services.AddScoped<ProductCategoryService>();
        services.AddScoped<AccHeadService>();
        services.AddScoped<StatusSettingService>();
        services.AddScoped<CurrencyService>();
        services.AddScoped<ProductSubCategoryService>();
        services.AddScoped<BrandService>();
        services.AddScoped<ProductSizeService>();
        services.AddScoped<CompanyBranceService>();
        services.AddScoped<CustomerPaymentDtlsService>();
        services.AddScoped<ProductSerialNumbersService>();
        services.AddScoped<AccountsDailyExpanseService>();
        services.AddScoped<PaymentTypesService>();
        services.AddScoped<BillingPlanService>();
        services.AddScoped<AccTypeServivce>();
        services.AddScoped<ProductMediaService>();
        //services.AddSingleton<ProductRepositoyWithSp>();
        //services.AddSingleton<TaskRepository>();
        //services.AddSingleton<FileUploadService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IViewRenderService, ViewRenderService>();



        services.AddScoped<BodyPartService>();
        services.AddScoped<ProductVariantService>();
        services.AddScoped<ItemCardService>();

        services.AddScoped<UserAddressBookService>();
        services.AddScoped<InvoiceItemSerialsService>();
    }
}
