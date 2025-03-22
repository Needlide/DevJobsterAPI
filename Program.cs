using DevJobsterAPI.Models.RequestModels.Recruiter.Validators;
using FluentValidation;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<RecruiterUpdateValidator>();

var app = builder.Build();



app.Run();