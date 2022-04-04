using System;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace PasswordValidat
{
    public class UserNormal
    {
        [Theory]
        [InlineData("Parola",false)]
        [InlineData("password",true)]
        public void PasswordHasAtLeast8Chars(string input,bool output)
        {
            Assert.Equal(input.Length >= 8,output);
        }
        [Theory]
        [InlineData("Parola12",true)]
        [InlineData("Pa1rola2",true)]
        [InlineData("Parola1",false)]
        public void PasswordHasAtLeast2Numbers(string input,bool output)
        {
            //var result = Regex.Match(input, @"\d+");
            //Assert.Equal(result.Length >= 2, output);
            var user = new User(input);
            Assert.Equal(user.NumberOfDigits()>=2, output);
        }
        [Theory]
        [InlineData("User","UserMihai",true)]
        [InlineData("parola","PaRolaCeva",true)]
        [InlineData("User","UseMihair",false)]
        public void UsernameIsNotInPassword(string user,string password,bool output)
        {
            Assert.Equal(password.IndexOf(user,StringComparison.OrdinalIgnoreCase)!=-1,output);
        }
        //[Theory]
        //[InlineData("Parola", "Parola",true)]
        //[InlineData("Parola","ParOLA",false)]
        //public void PasswordIsCaseSensitive(string password1, string password2,bool output)
        //{
        //    Assert.Equal(password2==password1,output);
        //}

    }
    public class Admin:UserNormal
    {
        [Theory]
        [InlineData("ParolA",true)]
        [InlineData("paRsOord",true)]
        [InlineData("Parola",false)]
        [InlineData("parola",false)]
        public void PasswordHasAtLeast2CappitalLetters(string input,bool output)
        {
            var user = new User(input);
            Assert.Equal(user.NumberOfCapitalLetters()>=2, output);
        }
        [Theory]
        [InlineData("paR", true)]
        [InlineData("pAR", true)]
        [InlineData("PAR", false)]
        public void PasswordHasAtLeastOneLowercase(string input, bool output)
        {
            var user = new User(input);
            Assert.Equal(user.NumberOfLowercase()>=1, output);
        }
        [Theory]
        [InlineData("12345678910", true)]
        [InlineData("123456", false)]
        [InlineData("12132132321312312", true)]
        public void PasswordHasAtLeast10Characters(string input, bool output)
        {
            var user = new User(input);
            Assert.Equal(user.NumberOfCharacters() >= 10, output);
        }
        [Theory]
        [InlineData("123&AD", true)]
        [InlineData("ADS**", true)]
        [InlineData("ADNO", false)]
        public void PasswordHasAtLeastOneSpecialCharacter(string input,bool output)
        {
            var user = new User(input);
            Assert.Equal(user.NumberOfSpecialChars() >= 1, output);
        }
        [Theory]
        [InlineData("123321", true)]
        [InlineData("ADS**", false)]
        [InlineData("ADNDA", true)]
        public void PasswordIsPalindrome(string input, bool output)
        {
            var user = new User(input);
            Assert.Equal(user.PasswordIsPalindrome(), output);
        }
    }
}
public class User
{
    private string password;
    private string username;
    public User (string password,string username="")
    {
        this.password = password;
        this.username = username;
    }

    public int NumberOfCapitalLetters()
    {
        return password.Count(c => char.IsUpper(c));
    }
    public int NumberOfLowercase()
    {
        return password.Count(c => char.IsLower(c));
    }
    public int NumberOfCharacters()
    {
        return password.Length;
    }
    public int NumberOfSpecialChars()
    {
        return password.Count(c=>(!char.IsLetterOrDigit(c)));
    }
    public bool PasswordIsPalindrome()
    {
        //if (password.Length % 2 == 0)
        //    return (password.Substring(0, password.Length / 2 - 1) == password.Substring(password.Length / 2, password.Length - 1));
        //else
        //    return (password.Substring(0, password.Length / 2 - 1) == password.Substring(password.Length / 2 + 1, password.Length - 1));
        return password.SequenceEqual(password.Reverse());
    }
    public int NumberOfDigits()
    {
        return password.Count(c=>char.IsDigit(c));  
    }
}