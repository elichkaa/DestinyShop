using EzBuy.Data;
using EzBuy.Models;
using EzBuy.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EzBuy.Services
{
    public class PaymentService:IPaymentService
    {
        private readonly EzBuyContext context;
        public PaymentService()
        {
            context = new EzBuyContext();
        }

        public bool AddTransaction(Dictionary<string, string> request, string payRequestId)
        {
            try
            {
                Transaction transaction = new Transaction
                {
                    DATE = DateTime.Now,
                    PAY_REQUEST_ID = payRequestId,
                    REFERENCE = request["REFERENCE"],
                    AMOUNT = int.Parse(request["AMOUNT"]),
                    CUSTOMER_EMAIL_ADDRESS = request["EMAIL"]
                };
                context.Transactions.Add(transaction);
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public string GetMd5Hash(Dictionary<string, string> data, string encryptionKey)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                StringBuilder input = new StringBuilder();
                foreach (string value in data.Values)
                {
                    input.Append(value);
                }

                input.Append(encryptionKey);

                byte[] hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input.ToString()));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    sBuilder.Append(hash[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public Transaction GetTransaction(string payRequestId)
        {
            Transaction transaction = context.Transactions.FirstOrDefault(p => p.PAY_REQUEST_ID == payRequestId);
            if (transaction == null)
            {
                return new Transaction();
            }

            return transaction;
        }

        public User GetUser()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> ToDictionary(string response)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string[] valuePairs = response.Split('&');
            foreach (string valuePair in valuePairs)
            {
                string[] values=valuePair.Split('=');
                result.Add(values[0],HttpUtility.UrlDecode(values[1]));
            }
            return result;
        }

        public string ToUrlEncodedString(Dictionary<string, string> request)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string key in request.Keys)
            {
                builder.Append("&");
                builder.Append(key);
                builder.Append("=");
                builder.Append(HttpUtility.UrlEncode(request[key]));
            }

            string result = builder.ToString().TrimStart('&');

            return result;
        }

        public bool UpdateTransaction(Dictionary<string, string> request, string PayrequestId)
        {
            bool IsUpdated = false;

            Transaction transaction = GetTransaction(PayrequestId);
            if (transaction == null)
                return IsUpdated;

            transaction.TRANSACTION_STATUS = request["TRANSACTION_STATUS"];
            transaction.RESULT_DESC = request["RESULT_DESC"];
            transaction.RESULT_CODE = (ResultCodes)int.Parse(request["RESULT_CODE"]);
            try
            {
                context.SaveChanges();
                IsUpdated = true;
            }
            catch (Exception e)
            {
            }
            return IsUpdated;
        }

        public bool VerifyMd5Hash(Dictionary<string, string> data, string encryptionKey, string hash)
        {
            Dictionary<string, string> hashDict = new Dictionary<string, string>();

            foreach (string key in data.Keys)
            {
                if (key != "CHECKSUM")
                {
                    hashDict.Add(key, data[key]);
                }
            }

            string hashOfInput = GetMd5Hash(hashDict, encryptionKey);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

