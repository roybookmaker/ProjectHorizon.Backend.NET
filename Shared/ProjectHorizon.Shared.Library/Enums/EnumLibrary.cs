using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHorizon.Shared.Library.Enums
{
    public class EnumLibrary
    {
        //login process
        public const string LoginSuccessByPassword = "Login successful by password";
        public const string LoginSuccessByToken = "Login successful by token";
        public const string PasswordInvalid = "Invalid password";
        public const string PasswordEmpty = "Password is empty";
        public const string RecoveryEmailSent = "Recovery email sent";
        public const string RecoveryEmailFailed = "Recovery email failed";
        public const string RecoveryKeyNotFound = "Recovery key not found";
        public const string ResetPasswordSuccess = "Password reset successful";

        //user
        public const string UserNotFound = "User not found";
        public const string UserAlreadyExists = "User already exists";
        public const string UserCreated = "User created";
        public const string UserUpdated = "User updated";
        public const string UserRegistered = "User registered";

        //token
        public const string TokenExpired = "Token has expired";
    }
}
