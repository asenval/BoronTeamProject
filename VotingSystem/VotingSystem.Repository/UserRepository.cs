using VotingSystem.Model;
using VotingSystem.Data;
using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text;

namespace VotingSystem.Repository
{
    public class UserRepository: EntityRepository<User>
    {
        private const string SessionKeyChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int SessionKeyLen = 50;

        private const int Sha1CodeLength = 40;
        private const string ValidUsernameChars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM_1234567890";
        private const string ValidNicknameChars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM_1234567890 -";
        private const int MinUsernameNicknameChars = 6;
        private const int MaxUsernameNicknameChars = 30;
        protected static Random rand = new Random();

        public UserRepository(IDbContextFactory<DbContext> contextFactory)
            : base(contextFactory)
        {
        }

        public User GetUserBySessionKey(string sessionKey)
        {
            var context = contextFactory.Create();
            var result = context.Set<User>().Where(x => x.SessionKey == sessionKey).FirstOrDefault();
            return result;
        }

        public void CreateUser(string username, string dispayName, string authCode)
        {
            ValidateUsername(username);
            ValidateNickname(dispayName);
            ValidateAuthCode(authCode);
            var context = contextFactory.Create();

            var usernameToLower = username.ToLower();
            var displayNameToLower = dispayName.ToLower();

            var dbUser = context.Set<User>().FirstOrDefault(u => u.Username == usernameToLower || u.DisplayName.ToLower() == displayNameToLower);

            if (dbUser != null)
            {
                if (dbUser.Username.ToLower() == usernameToLower)
                {
                    throw new InvalidOperationException("Username already exists");
                }
                else
                {
                    throw new InvalidOperationException("Nickname already exists");
                }
            }

            dbUser = new User()
            {
                Username = usernameToLower,
                DisplayName = dispayName,
                AuthCode = authCode
            };
            context.Set<User>().Add(dbUser);
            context.SaveChanges();
        }

        public string LoginUser(string username, string authCode, out string displayName)
        {
            ValidateUsername(username);
            ValidateAuthCode(authCode);
            var context = contextFactory.Create();
            var usernameToLower = username.ToLower();
            var user = context.Set<User>().FirstOrDefault(u => u.Username == usernameToLower && u.AuthCode == authCode);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid username or password");
            }

            var sessionKey = GenerateSessionKey((int)user.Id);
            user.SessionKey = sessionKey;
            displayName = user.DisplayName;
            context.SaveChanges();
            return sessionKey;
        }

        public int LoginUser(string sessionKey)
        {
            ValidateSessionKey(sessionKey);
            var context = contextFactory.Create();
            using (context)
            {
                var user = context.Set<User>().FirstOrDefault(u => u.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid user authentication");
                }
                return (int)user.Id;
            }
        }

        public void LogoutUser(string sessionKey)
        {
            ValidateSessionKey(sessionKey);
            var context = contextFactory.Create();
            var user = context.Set<User>().FirstOrDefault(u => u.SessionKey == sessionKey);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user authentication");
            }
            user.SessionKey = null;
            context.SaveChanges();
        }

        private void ValidateSessionKey(string sessionKey)
        {
            if (sessionKey.Length != SessionKeyLen || sessionKey.Any(ch => !SessionKeyChars.Contains(ch)))
            {
                throw new InvalidOperationException("Invalid Password");
            }
        }

        private static string GenerateSessionKey(int userId)
        {
            StringBuilder keyChars = new StringBuilder(50);
            keyChars.Append(userId.ToString());
            while (keyChars.Length < SessionKeyLen)
            {
                int randomCharNum;
                lock (rand)
                {
                    randomCharNum = rand.Next(SessionKeyChars.Length);
                }
                char randomKeyChar = SessionKeyChars[randomCharNum];
                keyChars.Append(randomKeyChar);
            }
            string sessionKey = keyChars.ToString();
            return sessionKey;
        }

        private static void ValidateUsername(string username)
        {
            if (username == null || username.Length < MinUsernameNicknameChars || username.Length > MaxUsernameNicknameChars)
            {
                throw new InvalidOperationException(string.Format("Username should be between {0} and {1} symbols long", MinUsernameNicknameChars, MaxUsernameNicknameChars));
            }
            else if (username.Any(ch => !ValidUsernameChars.Contains(ch)))
            {
                throw new InvalidOperationException("Username contains invalid characters");
            }
        }

        private static void ValidateNickname(string nickname)
        {
            if (nickname == null || nickname.Length < MinUsernameNicknameChars || nickname.Length > MaxUsernameNicknameChars)
            {
                throw new ServerErrorException(string.Format("Nickname should be between {0} and {1} symbols long", MinUsernameNicknameChars, MaxUsernameNicknameChars), "INV_NICK_LEN");
            }
            else if (nickname.Any(ch => !ValidNicknameChars.Contains(ch)))
            {
                throw new ServerErrorException("Nickname contains invalid characters", "INV_NICK_CHARS");
            }
        }

        private static void ValidateAuthCode(string authCode)
        {
            if (authCode.Length != Sha1CodeLength)
            {
                throw new InvalidOperationException("Invalid user authentication");
            }
        }
    }
}
