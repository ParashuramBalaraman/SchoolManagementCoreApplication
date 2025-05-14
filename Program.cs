using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using SchoolManagementCoreApplication.Models;
using SchoolManagementCoreApplication.Service.Students.Interfaces;
using SchoolManagementCoreApplication.Service.Students;
using SchoolManagementCoreApplication.Service.Teachers.Interfaces;
using SchoolManagementCoreApplication.Service.Teachers;
using SchoolManagementCoreApplication.Service.Degrees.Interfaces;
using SchoolManagementCoreApplication.Service.Degrees;
using SchoolManagementCoreApplication.Service.Departments.Interfaces;
using SchoolManagementCoreApplication.Service.Departments;
using SchoolManagementCoreApplication.Service.Ethnicities.Interfaces;
using SchoolManagementCoreApplication.Service.Ethnicities;
using SchoolManagementCoreApplication.Service.Genders.Interfaces;
using SchoolManagementCoreApplication.Service.Genders;
using SchoolManagementCoreApplication.Service.StudentDegrees.Interfaces;
using SchoolManagementCoreApplication.Service.StudentDegrees;
using SchoolManagementCoreApplication.Service.StudentTeachers.Interfaces;
using SchoolManagementCoreApplication.Service.StudentTeachers;
using NLog.Extensions.Logging;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddDbContext<SchoolDatabaseContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Register the Service Interfaces and their implementations
    builder.Services.AddScoped<IStudentService, StudentService>();
    builder.Services.AddScoped<ITeacherService, TeacherService>();
    builder.Services.AddScoped<IDegreeService, DegreeService>();
    builder.Services.AddScoped<IDepartmentService, DepartmentService>();
    builder.Services.AddScoped<IEthnicityService, EthnicityService>();
    builder.Services.AddScoped<IGenderService, GenderService>();
    builder.Services.AddScoped<IStudentDegreeService, StudentDegreeService>();
    builder.Services.AddScoped<IStudentTeacherService, StudentTeacherService>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //configure logging
    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddNLog();
    });

    // Configure CORS
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("http://localhost:4200") 
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }
    else
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseCors();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    // NLog: catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
