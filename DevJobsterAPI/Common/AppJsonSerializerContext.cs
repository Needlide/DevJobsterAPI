using System.Text.Json.Serialization;
using DevJobsterAPI.ApiModels;
using DevJobsterAPI.DatabaseModels.Admin;
using DevJobsterAPI.DatabaseModels.Chat;
using DevJobsterAPI.DatabaseModels.Recruiter;
using DevJobsterAPI.DatabaseModels.RequestModels.Admin;
using DevJobsterAPI.DatabaseModels.RequestModels.Chat;
using DevJobsterAPI.DatabaseModels.RequestModels.Recruiter;
using DevJobsterAPI.DatabaseModels.RequestModels.Security;
using DevJobsterAPI.DatabaseModels.RequestModels.User;
using DevJobsterAPI.DatabaseModels.RequestModels.Vacancy;
using DevJobsterAPI.DatabaseModels.Security;
using DevJobsterAPI.DatabaseModels.User;
using DevJobsterAPI.DatabaseModels.Vacancy;
using Microsoft.AspNetCore.Mvc;

namespace DevJobsterAPI.Common;

[JsonSerializable(typeof(ApiResponse<Admin>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<Admin>>))]
[JsonSerializable(typeof(ApiResponse<Report>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<Report>>))]
[JsonSerializable(typeof(ApiResponse<Log>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<Log>>))]
[JsonSerializable(typeof(ApiResponse<RegisteredAccount>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<RegisteredAccount>>))]
[JsonSerializable(typeof(ApiResponse<Chat>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<Chat>>))]
[JsonSerializable(typeof(ApiResponse<Message>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<Message>>))]
[JsonSerializable(typeof(ApiResponse<Recruiter>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<Recruiter>>))]
[JsonSerializable(typeof(ApiResponse<User>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<User>>))]
[JsonSerializable(typeof(ApiResponse<Application>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<Application>>))]
[JsonSerializable(typeof(ApiResponse<Vacancy>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<Vacancy>>))]
[JsonSerializable(typeof(ApiResponse<AdminRegisteredAccountView>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<AdminRegisteredAccountView>>))]
[JsonSerializable(typeof(ApiResponse<AdminReportView>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<AdminReportView>>))]
[JsonSerializable(typeof(ApiResponse<AdminView>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<AdminView>>))]
[JsonSerializable(typeof(ApiResponse<MessageView>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<MessageView>>))]
[JsonSerializable(typeof(ApiResponse<RecruiterRegistration>))]
[JsonSerializable(typeof(ApiResponse<RecruiterUpdate>))]
[JsonSerializable(typeof(ApiResponse<RecruiterView>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<RecruiterView>>))]
[JsonSerializable(typeof(ApiResponse<LoginRegisterModel>))]
[JsonSerializable(typeof(ApiResponse<LogView>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<LogView>>))]
[JsonSerializable(typeof(ApiResponse<RegisteredAccountShortView>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<RegisteredAccountShortView>>))]
[JsonSerializable(typeof(ApiResponse<RegisteredAccountUpdatedStatus>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<RegisteredAccountUpdatedStatus>>))]
[JsonSerializable(typeof(ApiResponse<UserApplicationView>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<UserApplicationView>>))]
[JsonSerializable(typeof(ApiResponse<List<UserApplicationView>>))]
[JsonSerializable(typeof(ApiResponse<UserProfileView>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<UserProfileView>>))]
[JsonSerializable(typeof(ApiResponse<UserRegistration>))]
[JsonSerializable(typeof(ApiResponse<UserUpdate>))]
[JsonSerializable(typeof(ApiResponse<UpdateVacancy>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<UpdateVacancy>>))]
[JsonSerializable(typeof(ApiResponse<VacancyView>))]
[JsonSerializable(typeof(ApiResponse<IEnumerable<VacancyView>>))]
[JsonSerializable(typeof(ApiResponse<List<VacancyView>>))]
[JsonSerializable(typeof(ApiResponse<object>))]
[JsonSerializable(typeof(ApiResponse<AddVacancy>))]
[JsonSerializable(typeof(ApiResponse<TokenResponse>))]
[JsonSerializable(typeof(ApiResponse<AddApplication>))]
[JsonSerializable(typeof(ApiResponse<AddChat>))]
[JsonSerializable(typeof(ApiResponse<AddMessage>))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(ValidationProblemDetails))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}