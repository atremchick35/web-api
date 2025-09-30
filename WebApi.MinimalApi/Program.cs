using AutoMapper;
using Microsoft.AspNetCore.Mvc.Formatters;
using WebApi.MinimalApi.Domain;
using WebApi.MinimalApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");
builder.Services
    .AddControllers(options =>
    {
        options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
        options.ReturnHttpNotAcceptable = true;
        options.RespectBrowserAcceptHeader = true;
    })
    .ConfigureApiBehaviorOptions(options => {
        options.SuppressModelStateInvalidFilter = true;
        options.SuppressMapClientErrors = true;
    });

builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddAutoMapper(x =>
{
    x.CreateMap<UserEntity, UserDto>()
        .ForMember(destinationMember => destinationMember.FullName,
            options => options
                .MapFrom(src => $"{src.LastName} {src.FirstName}"));
    x.CreateMap<CreateUserDto, UserEntity>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.CurrentGameId, opt => opt.Ignore())
        .ForMember(dest => dest.GamesPlayed, opt => opt.Ignore());

});

var app = builder.Build();

app.MapControllers();

app.Run();