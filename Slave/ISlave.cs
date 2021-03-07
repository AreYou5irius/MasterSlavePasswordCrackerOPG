using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Slave.model;
using Slave.utilities;

namespace Slave
{
    public interface ISlave
    {
        //private readonly HashAlgorithm _messageDigest;

        //public Cracking()
        //{
        //    _messageDigest = new SHA1CryptoServiceProvider();

        //}

        private static List<UserInfoClearText> result = new List<UserInfoClearText>(); //might be empty

        private IEnumerable<UserInfoClearText> CheckWordWithVariationsPartialResult(String dictionaryEntry, List<UserInfo> userInfos)
        {
            String possiblePassword = dictionaryEntry;
            IEnumerable<UserInfoClearText> partialResult = CheckSingleWord(userInfos, possiblePassword);
            result.AddRange(partialResult);
            return result;
        }


        private IEnumerable<UserInfoClearText> CheckWordWithVariationsPartialResultUpperCase(String dictionaryEntry, List<UserInfo> userInfos)
        {
            String possiblePasswordUpperCase = dictionaryEntry.ToUpper();
            IEnumerable<UserInfoClearText> partialResultUpperCase = CheckSingleWord(userInfos, possiblePasswordUpperCase);
            result.AddRange(partialResultUpperCase);

            return result;
        }


        private IEnumerable<UserInfoClearText> CheckWordWithVariationsPartialResultCapitalized(String dictionaryEntry, List<UserInfo> userInfos)
        {
            String possiblePasswordCapitalized = StringUtilities.Capitalize(dictionaryEntry);
            IEnumerable<UserInfoClearText> partialResultCapitalized = CheckSingleWord(userInfos, possiblePasswordCapitalized);
            result.AddRange(partialResultCapitalized);

            return result;
        }

        private IEnumerable<UserInfoClearText> CheckWordWithVariationsPartialResultReverse(String dictionaryEntry, List<UserInfo> userInfos)
        {
            String possiblePasswordReverse = StringUtilities.Reverse(dictionaryEntry);
            IEnumerable<UserInfoClearText> partialResultReverse = CheckSingleWord(userInfos, possiblePasswordReverse);
            result.AddRange(partialResultReverse);

            return result;
        }

        private IEnumerable<UserInfoClearText> CheckWordWithVariationsPartialResultEndDigit(String dictionaryEntry, List<UserInfo> userInfos)
        {
            for (int i = 0; i < 100; i++)
            {
                String possiblePasswordEndDigit = dictionaryEntry + i;
                IEnumerable<UserInfoClearText> partialResultEndDigit = CheckSingleWord(userInfos, possiblePasswordEndDigit);
                result.AddRange(partialResultEndDigit);
            }
            return result;
        }

        private IEnumerable<UserInfoClearText> CheckWordWithVariationsPartialResultStartDigit(String dictionaryEntry, List<UserInfo> userInfos)
        {
            for (int i = 0; i < 100; i++)
            {
                String possiblePasswordStartDigit = i + dictionaryEntry;
                IEnumerable<UserInfoClearText> partialResultStartDigit = CheckSingleWord(userInfos, possiblePasswordStartDigit);
                result.AddRange(partialResultStartDigit);
            }

            return result;
        }
        private IEnumerable<UserInfoClearText> CheckWordWithVariationsPartialResultStartEndDigit(String dictionaryEntry, List<UserInfo> userInfos)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    String possiblePasswordStartEndDigit = i + dictionaryEntry + j;
                    IEnumerable<UserInfoClearText> partialResultStartEndDigit = CheckSingleWord(userInfos, possiblePasswordStartEndDigit);
                    result.AddRange(partialResultStartEndDigit);
                }
            }

            return result;
        }

        private IEnumerable<UserInfoClearText> CheckSingleWord(IEnumerable<UserInfo> userInfos, String possiblePassword)
        {

            char[] charArray = possiblePassword.ToCharArray();
            byte[] passwordAsBytes = Array.ConvertAll(charArray, PasswordFileHandler.GetConverter());

            // byte[] encryptedPassword = _messageDigest.ComputeHash(passwordAsBytes); ------------------------indkommenter
            //string encryptedPasswordBase64 = System.Convert.ToBase64String(encryptedPassword);

            List<UserInfoClearText> results = new List<UserInfoClearText>();

            foreach (UserInfo userInfo in userInfos)
            {
                // if (CompareBytes(userInfo.EntryptedPassword, encryptedPassword))  //compares byte arrays  ------------------------indkommenter
                {
                    results.Add(new UserInfoClearText(userInfo.Username, possiblePassword));
                    Console.WriteLine(userInfo.Username + " " + possiblePassword);
                }
            }
            return results;
        }

        private static bool CompareBytes(IList<byte> firstArray, IList<byte> secondArray)
        {
            if (firstArray.Count != secondArray.Count)
            {
                return false;
            }
            for (int i = 0; i < firstArray.Count; i++)
            {
                if (firstArray[i] != secondArray[i])
                    return false;
            }
            return true;
        }
    }
}
