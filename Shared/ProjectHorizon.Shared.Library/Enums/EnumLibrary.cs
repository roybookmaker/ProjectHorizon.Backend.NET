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
        public const int LoginSuccessByPassword = 1;
        public const int LoginSuccessByToken = 2;
        public const int PasswordInvalid = 3;
        public const int PasswordEmpty = 9;
        public const int RecoveryEmailSent = 11;
        public const int RecoveryEmailFailed = 12;
        public const int ResetPasswordSuccess = 13;

        //user
        public const int UserNotFound = 4;
        public const int UserAlreadyExists = 5;
        public const int UserCreated = 6;
        public const int UserUpdated = 7;
        public const int UserRegistered = 10;

        //token
        public const int TokenExpired = 8;
    }
}
