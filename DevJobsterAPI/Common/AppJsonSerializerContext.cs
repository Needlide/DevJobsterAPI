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

namespace DevJobsterAPI.Common;

[JsonSerializable(typeof(Admin))]
[JsonSerializable(typeof(IEnumerable<Admin>))]
[JsonSerializable(typeof(Report))]
[JsonSerializable(typeof(IEnumerable<Report>))]
[JsonSerializable(typeof(Log))]
[JsonSerializable(typeof(IEnumerable<Log>))]
[JsonSerializable(typeof(RegisteredAccount))]
[JsonSerializable(typeof(IEnumerable<RegisteredAccount>))]
[JsonSerializable(typeof(Chat))]
[JsonSerializable(typeof(IEnumerable<Chat>))]
[JsonSerializable(typeof(Message))]
[JsonSerializable(typeof(IEnumerable<Message>))]
[JsonSerializable(typeof(Recruiter))]
[JsonSerializable(typeof(IEnumerable<Recruiter>))]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(IEnumerable<User>))]
[JsonSerializable(typeof(Application))]
[JsonSerializable(typeof(IEnumerable<Application>))]
[JsonSerializable(typeof(Vacancy))]
[JsonSerializable(typeof(IEnumerable<Vacancy>))]
[JsonSerializable(typeof(AdminRegisteredAccountView))]
[JsonSerializable(typeof(IEnumerable<AdminRegisteredAccountView>))]
[JsonSerializable(typeof(AdminReportView))]
[JsonSerializable(typeof(IEnumerable<AdminReportView>))]
[JsonSerializable(typeof(AdminView))]
[JsonSerializable(typeof(IEnumerable<AdminView>))]
[JsonSerializable(typeof(MessageView))]
[JsonSerializable(typeof(IEnumerable<MessageView>))]
[JsonSerializable(typeof(RecruiterRegistration))]
[JsonSerializable(typeof(RecruiterUpdate))]
[JsonSerializable(typeof(RecruiterView))]
[JsonSerializable(typeof(IEnumerable<RecruiterView>))]
[JsonSerializable(typeof(LoginRegisterModel))]
[JsonSerializable(typeof(LogView))]
[JsonSerializable(typeof(IEnumerable<LogView>))]
[JsonSerializable(typeof(RegisteredAccountShortView))]
[JsonSerializable(typeof(IEnumerable<RegisteredAccountShortView>))]
[JsonSerializable(typeof(RegisteredAccountUpdatedStatus))]
[JsonSerializable(typeof(IEnumerable<RegisteredAccountUpdatedStatus>))]
[JsonSerializable(typeof(UserApplicationView))]
[JsonSerializable(typeof(IEnumerable<UserApplicationView>))]
[JsonSerializable(typeof(UserProfileView))]
[JsonSerializable(typeof(IEnumerable<UserProfileView>))]
[JsonSerializable(typeof(UserRegistration))]
[JsonSerializable(typeof(UserUpdate))]
[JsonSerializable(typeof(UpdateVacancy))]
[JsonSerializable(typeof(IEnumerable<UpdateVacancy>))]
[JsonSerializable(typeof(VacancyView))]
[JsonSerializable(typeof(IEnumerable<VacancyView>))]
[JsonSerializable(typeof(ApiResponse<object>))]
[JsonSerializable(typeof(AddVacancy))]
[JsonSerializable(typeof(TokenResponse))]
[JsonSerializable(typeof(AddApplication))]
[JsonSerializable(typeof(AddChat))]
[JsonSerializable(typeof(AddMessage))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}