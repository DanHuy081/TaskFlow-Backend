//using Microsoft.EntityFrameworkCore;
//using TaskFlowBE.Data;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Microsoft.OpenApi.Models;
//using Microsoft.Extensions.DependencyInjection;
//using LogicBusiness.Service;
//using LogicBusiness.UseCase;
//using LogicBusiness.Repository;
//using SqlServer;
//using SqlServer.Mapping;
//using System.Text.Json.Serialization;
//using CoreEntities.Mapping;



//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options =>
//{
//    var jwtSecurityScheme = new OpenApiSecurityScheme
//    {
//        BearerFormat = "JWT",
//        Name = "Authorrization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.Http,
//        Scheme = JwtBearerDefaults.AuthenticationScheme,
//        Description = "Enter Your Acces Token",
//        Reference = new OpenApiReference
//        {
//            Id = JwtBearerDefaults.AuthenticationScheme,
//            Type = ReferenceType.SecurityScheme, 
//        }
//    };

//    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {jwtSecurityScheme, Array.Empty<string>() }
//    });

//});
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// 2️⃣ Cấu hình JWT
//var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(key)
//        };
//    });

//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//builder.Services.AddAutoMapper(typeof(TaskProfile));
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddScoped<ITaskService, TaskService>();
//builder.Services.AddScoped<ITaskRepository, TaskRepository>();
//builder.Services.AddScoped<ICommentRepository, CommentRepository>();
//builder.Services.AddScoped<ICommentService, CommentService>();
//builder.Services.AddScoped<ITagRepository, TagRepository>();
//builder.Services.AddScoped<ITagService, TagService>();
//builder.Services.AddScoped<ITaskTagRepository, TaskTagRepository>();
//builder.Services.AddScoped<ITaskTagService, TaskTagService>();
//builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
//builder.Services.AddScoped<IAttachmentService, AttachmentService>();
//builder.Services.AddScoped<IListRepository, ListRepository>();
//builder.Services.AddScoped<IListService, ListService>();
//builder.Services.AddScoped<IFolderRepository, FolderRepository>();
//builder.Services.AddScoped<IFolderService, FolderService>();
//builder.Services.AddScoped<ISpaceRepository, SpaceRepository>();
//builder.Services.AddScoped<ISpaceService, SpaceService>();
//builder.Services.AddScoped<ITeamRepository, TeamRepository>();
//builder.Services.AddScoped<ITeamService, TeamService>();
//builder.Services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
//builder.Services.AddScoped<ITeamMemberService, TeamMemberService>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<ITaskAssigneeRepository, TaskAssigneeRepository>();
//builder.Services.AddScoped<ITaskAssigneeService, TaskAssigneeService>();
//builder.Services.AddScoped<IGoalRepository, GoalRepository>();
//builder.Services.AddScoped<IGoalService, GoalService>();
//builder.Services.AddScoped<IChecklistRepository, ChecklistRepository>();
//builder.Services.AddScoped<IChecklistService, ChecklistService>();
//builder.Services.AddScoped<IChecklistItemRepository, ChecklistItemRepository>();
//builder.Services.AddScoped<IChecklistItemService, ChecklistItemService>();
//builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();
//builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();
//builder.Services.AddScoped<ICustomFieldRepository, CustomFieldRepository>();
//builder.Services.AddScoped<ICustomFieldService, CustomFieldService>();
//builder.Services.AddScoped<ITaskCustomFieldValueRepository, TaskCustomFieldValueRepository>();
//builder.Services.AddScoped<ITaskCustomFieldValueService, TaskCustomFieldValueService>();
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddAutoMapper(typeof(Program).Assembly);

//builder.Services.AddControllers()
//    .AddJsonOptions(opt =>
//    {
//        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//    });

////builder.Services.AddAutoMapper(typeof(MappingProfile));

//var app = builder.Build();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy
//            .AllowAnyOrigin()
//            .AllowAnyMethod()
//            .AllowAnyHeader();
//    });
//});


//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//    app.UseDeveloperExceptionPage();
//}

//app.UseHttpsRedirection();
//app.UseCors("AllowAll");

//app.UseAuthorization();
//// 3️⃣ Kích hoạt Authentication
//app.UseAuthentication();


//app.MapControllers();

//app.Run();

using Microsoft.EntityFrameworkCore;
using TaskFlowBE.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using LogicBusiness.Service;
using LogicBusiness.UseCase;
using LogicBusiness.Repository;
using SqlServer;
using SqlServer.Mapping;
using System.Text.Json.Serialization;
using CoreEntities.Mapping;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// 1. Add Services
// -------------------------

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

// Swagger + JWT
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter JWT Bearer token only",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// JWT Authentication
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            RoleClaimType = ClaimTypes.Role
        };
    });

// CORS — MUST be before Build()
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()  // Cho phép mọi nguồn (bao gồm cả link figma lạ kia)
                   .AllowAnyMethod()  // Cho phép GET, POST, PUT, DELETE...
                   .AllowAnyHeader(); // Cho phép mọi Header
        });
});

// -------------------------
// 2. Dependency Injection
// -------------------------
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ITaskTagRepository, TaskTagRepository>();
builder.Services.AddScoped<ITaskTagService, TaskTagService>();
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddScoped<IListRepository, ListRepository>();
builder.Services.AddScoped<IListService, ListService>();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();
builder.Services.AddScoped<IFolderService, FolderService>();
builder.Services.AddScoped<ISpaceRepository, SpaceRepository>();
builder.Services.AddScoped<ISpaceService, SpaceService>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
builder.Services.AddScoped<ITeamMemberService, TeamMemberService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskAssigneeRepository, TaskAssigneeRepository>();
builder.Services.AddScoped<ITaskAssigneeService, TaskAssigneeService>();
builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IChecklistRepository, ChecklistRepository>();
builder.Services.AddScoped<IChecklistService, ChecklistService>();
builder.Services.AddScoped<IChecklistItemRepository, ChecklistItemRepository>();
builder.Services.AddScoped<IChecklistItemService, ChecklistItemService>();
builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();
builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();
builder.Services.AddScoped<ICustomFieldRepository, CustomFieldRepository>();
builder.Services.AddScoped<ICustomFieldService, CustomFieldService>();
builder.Services.AddScoped<ITaskCustomFieldValueRepository, TaskCustomFieldValueRepository>();
builder.Services.AddScoped<ITaskCustomFieldValueService, TaskCustomFieldValueService>();

// -------------------------
// 3. Build App
// -------------------------

var app = builder.Build();

// -------------------------
// 4. Middlewares
// -------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Authentication phải trước Authorization!!
app.UseAuthentication();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();

