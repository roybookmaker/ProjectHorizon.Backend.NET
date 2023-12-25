using ProjectHorizon.Microservice.Account.Components.DTO;
using ProjectHorizon.Microservice.Account.Components.Models;
using ProjectHorizon.Shared.Library.Common;
using ProjectHorizon.Shared.Library.Enums;
using ProjectHorizon.Shared.Library.Helper;
using ProjectHorizon.Shared.Library.Service;

namespace ProjectHorizon.Microservice.Account.Components.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDatabaseHelper _databaseHelper;

        public AccountRepository(IDatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<QueryResult> LoginUser(LoginModel model)
        {
            try
            {
                var loginQuery = "SELECT id, username, salt, token, email, status, createddate, updateddate FROM account WHERE username = :username";
                var parameters = new Dapper.DynamicParameters();
                parameters.Add("username", model.Username);

                var user = await _databaseHelper.GetAsync<AccountDTO>(loginQuery, parameters);

                if (user == null)
                    return QueryResult.Error(EnumLibrary.UserNotFound);

                if (user.Token != null && user.Token == model.Token)
                {
                    if (user.UpdatedDate >= user.UpdatedDate.AddHours(2))
                    {
                        return QueryResult.Error(EnumLibrary.TokenExpired);
                    }
                    else
                    {
                        string newToken = UtilityHelper.GenerateRandomString();
                        await UpdateToken(model.Username, "", newToken);
                        return QueryResult.Ok(new { Token = newToken }, EnumLibrary.LoginSuccessByToken);
                    }
                }
                else
                {
                    if (model.Password != null)
                    {
                        byte[] hashedPassword = UtilityHelper.ConvertArgon2Hash(model.Password, user.Salt);

                        var loginWithPasswordQuery = "SELECT id, username, salt, token, email, status, createddate, updateddate FROM account WHERE username = :username AND password = :password";
                        var parametersLoginPassword = new Dapper.DynamicParameters();
                        parametersLoginPassword.Add("username", model.Username);
                        parametersLoginPassword.Add("password", hashedPassword);
                        var userWithPassword = await _databaseHelper.GetAsync<AccountDTO>(loginWithPasswordQuery, parametersLoginPassword);

                        if (userWithPassword != null)
                        {
                            string newToken = UtilityHelper.GenerateRandomString();
                            await UpdateToken(model.Username, model.Password, newToken);
                            return QueryResult.Ok(new { Token = newToken }, EnumLibrary.LoginSuccessByPassword);
                        }
                        else
                        {
                            return QueryResult.Error(EnumLibrary.PasswordInvalid);
                        }
                    }
                    else
                    {
                        return QueryResult.Error(EnumLibrary.PasswordEmpty);
                    }
                }
            }
            catch (Exception ex)
            {
                return QueryResult.Error(ex.Message);
            }
        }

        private async Task UpdateToken(string username, string password, string newToken)
        {
            try
            {
                if(password != "")
                {
                    var updateToken = "UPDATE account SET password = :password, salt = :salt, token = :token, updateddate = CURRENT_TIMESTAMP WHERE username = :username";
                    byte[][] newPassAndSalt = UtilityHelper.GenerateArgon2Hash(password);
                    var parameters = new Dapper.DynamicParameters();

                    parameters.Add("token", newToken);
                    parameters.Add("username", username);
                    parameters.Add("password", newPassAndSalt[0]);
                    parameters.Add("salt", newPassAndSalt[1]);

                    var process = await _databaseHelper.EditData(updateToken, parameters);
                }
                else
                {
                    var updateToken = "UPDATE account SET token = :token, updateddate = CURRENT_TIMESTAMP WHERE username = :username";
                    var parameters = new Dapper.DynamicParameters();

                    parameters.Add("token", newToken);
                    parameters.Add("username", username);

                    var process = await _databaseHelper.EditData(updateToken, parameters);
                }
                
            }
            catch
            {
                throw;
            }
        }

        public async Task<QueryResult> RegisterUser(RegisterModel model)
        {
            try
            {
                var selectQuery = "SELECT id FROM account WHERE username = :username OR email = :email";
                var parametersSelect = new Dapper.DynamicParameters();
                parametersSelect.Add("username", model.Username);
                parametersSelect.Add("email", model.Email);
                var processSelect = await _databaseHelper.GetAsync<AccountDTO>(selectQuery, parametersSelect);

                if (processSelect != null)
                {
                    return QueryResult.Error(EnumLibrary.UserAlreadyExists);
                }

                var query = "INSERT INTO account (fullname, username, password, salt, email) VALUES (:fullname, :username, :password, :salt, :email)";

                byte[][] newPassAndSalt = UtilityHelper.GenerateArgon2Hash(model.Password);

                var parameters = new Dapper.DynamicParameters();
                parameters.Add("fullname", model.Fullname);
                parameters.Add("username", model.Username);
                parameters.Add("password", newPassAndSalt[0]);
                parameters.Add("salt", newPassAndSalt[1]);
                parameters.Add("email", model.Email);

                var process = await _databaseHelper.EditData(query, parameters);
                if(process == 1)
                {
                    return QueryResult.Ok(EnumLibrary.UserCreated);
                } else
                {
                    return QueryResult.Error(EnumLibrary.UserAlreadyExists);
                }
            }
            catch (Exception ex)
            {
                return QueryResult.Error(ex.Message);
            }
        }

        public async Task<QueryResult> RecoveryUser(RecoveryModel model)
        {
            try
            {
                var query = "SELECT id, email FROM account WHERE email = :email";
                var parameters = new Dapper.DynamicParameters();
                parameters.Add("email", model.Email);

                var user = await _databaseHelper.GetAsync<AccountDTO>(query, parameters);

                if (user == null)
                    return QueryResult.Error(EnumLibrary.UserNotFound);

                var recoveryQuery = "INSERT INTO recoveryrequest (userid, recoverykey) VALUES (:userid, :recoverykey)";
                var recoveryParameters = new Dapper.DynamicParameters();
                recoveryParameters.Add("userid", user.Id);
                recoveryParameters.Add("recoverykey", UtilityHelper.GenerateRandomString(8));

                var process = await _databaseHelper.EditData(recoveryQuery, recoveryParameters);

                var sendingEmail = await EmailService.EmailServiceData("recovery", user.Email);
                if (sendingEmail.IsSuccessful)
                    return QueryResult.Ok(EnumLibrary.RecoveryEmailSent);
                else
                    return QueryResult.Error(EnumLibrary.RecoveryEmailFailed);
            }
            catch (Exception ex)
            {
                return QueryResult.Error(ex.Message);
            }
        }

        public async Task<QueryResult> ResetUserPass(ResetModel model)
        {
            try
            {
                var query = "SELECT * FROM recoveryrequest WHERE id = :id and recoverykey = :recoverykey ";
                var parameters = new Dapper.DynamicParameters();
                parameters.Add("id", model.Id);
                parameters.Add("recoverykey", model.RecoveryKey);

                var key = await _databaseHelper.GetAsync<RecoveryDTO>(query, parameters);

                if (key == null)
                {
                    return QueryResult.Error(EnumLibrary.RecoveryKeyNotFound);
                }
                else if (key.ExpiryDate < DateTime.Now || key.Used == true) //probably needed to fix this to follow the timezone of user, or probably not since backend sometimes in the same place as the database and probably using the same timezone
                {
                    return QueryResult.Error(EnumLibrary.TokenExpired);
                }

                string newToken = UtilityHelper.GenerateRandomString();

                var updateToken = "UPDATE account SET password = :password, salt = :salt, token = :token, updateddate = CURRENT_TIMESTAMP WHERE id = :id";
                byte[][] newPassAndSalt = UtilityHelper.GenerateArgon2Hash(model.NewPassword);
                var parameters2 = new Dapper.DynamicParameters();

                parameters2.Add("token", newToken);
                parameters2.Add("id", key.Userid);
                parameters2.Add("password", newPassAndSalt[0]);
                parameters2.Add("salt", newPassAndSalt[1]);
                var process = await _databaseHelper.EditData(updateToken, parameters2);

                var updateRecovery = "UPDATE recoveryrequest SET used = true WHERE id = :id and recoverykey = :recoverykey";
                var parameters3 = new Dapper.DynamicParameters();
                parameters3.Add("id", model.Id);
                parameters3.Add("recoverykey", model.RecoveryKey);
                var process2 = await _databaseHelper.EditData(updateRecovery, parameters3);

                return QueryResult.Ok(new { Token = newToken }, EnumLibrary.ResetPasswordSuccess);

            }
            catch (Exception ex)
            {
                return QueryResult.Error(ex.Message);
            }
        }
    }
}
