using System;
using static System.Formats.Asn1.AsnWriter;

namespace TheAgooProjectDataAccess
{
    public static class SD
    {
        //public static string privileges
        public const string Role_Admin = "Admin";
        public const string Role_ExamOfficer = "Exam Officer";
        public const string Role_Accountant = "Accountant";
        public const string IsStudent = "IsStudent";

        //FEES PAYMENT STATUS
        public static string Fees_Status_Completed = "Completed";
        public static string Fees_Status_Part_Payment = "Part Payment";

        //Teachers status
        public static string Status_Active = "Active";
        public static string Status_InActive = "InActive";

        

        //Teachers status
        public static string First = "First";
        public static string Second = "Second";
        public static string Third = "Third";
        public static string[] Terms = { "First", "Second", "Third" };
        public static string CookiesKey = "Doom-Faith-Love";

        //Remark Calculator
        public static double GetAverageScore(params double[] scores)
        {
            if (scores == null || scores.Length == 0)
            {
                return 0;
            }
            double totalScore = 0;
            foreach (double score in scores)
            {
                totalScore += score;
            }
            double average = totalScore / (scores.Length * 100);
            return Math.Round(average * 100, 2);
        }
        public static string Grade(double totalScores)
        {
            if (totalScores >= 75)
            {
                return "A";
            }
            else if (totalScores >= 65)
            {
                return "B";
            }
            else if (totalScores >= 55)
            {
                return "C";
            }
            else if (totalScores >= 40)
            {
                return "D";
            }
            else
            {
                return "E";
            }
        }
        public static string Remark(double score)
        {
            if(score >= 75)
            {
                return "Excellent";
            }
            else if (score >= 65 && score <= 74)
            {
                return "Very Good";
            }
            else if (score >= 55 && score <= 64)
            {
                return "Good";
            }
            else if (score >= 40 && score <= 54)
            {
                return "Fair";
            }
            else
            {
                return "Poor";
            }
        }
        public static string GetGrade(double? score)
        {
            if (score >= 75)
            {
                return "A";
            }
            else if (score >= 65 && score <= 74)
            {
                return "B";
            }
            else if (score >= 55 && score <= 64)
            {
                return "C";
            }
            else if (score >= 40 && score <= 54)
            {
                return "D";
            }
            else
            {
                return "E";
            }
        }
        //ANNUAL Remark Calculator
        public static string GetRemark(double? score)
        {
            if (score >= 75)
            {
                return "Excellent";
            }
            else if (score >= 65 && score <= 74)
            {
                return "Very Good";
            }
            else if (score >= 55 && score <= 64)
            {
                return "Good";
            }
            else if (score >= 40 && score <= 54)
            {
                return "Fair";
            }
            else
            {
                return "Poor";
            }
        }
        public static byte Rating(byte low, byte high)
        {
            Random random = new();
            return (byte)random.Next(low, high+1);
        }
        public static double Ttrem(int tterm, double scores)
        {
            double xterm = tterm * 100;
            return Math.Round((scores / xterm) * 100, 2);
        }
        public static byte LowRating(double average)
        {
            if (average >= 75)
            {
                return 4;
            }
            else if (average >= 65)
            {
                return 3;
            }
            else if (average >= 55)
            {
                return 3;
            }
            else if (average >= 40)
            {
                return 2;
            }
            else
            {
                return 2;
            }
        }

        public static byte HighRating(double average)
        {
            if (average >= 75)
            {
                return 5;
            }
            else if (average >= 65)
            {
                return 5;
            }
            else if (average >= 55)
            {
                return 5;
            }
            else if (average >= 40)
            {
                return 4;
            }
            else
            {
                return 4;
            }
        }
        public static double AnnualTermAverage(int noInClass, double scores)
        {
            if (noInClass == 0)
            {
                return 0;
            }
            return Math.Round((scores / noInClass), 2);
        }
        public static string ToNaira(double money)
        {
            char naira = (char)8358;
            string Money;
            Money = money.ToString("c");
            return Money.Replace('$', naira);
        }
        //AnnualAverageSubject
        public static IEnumerable<U> Rank<T, TKey, U>(this IEnumerable<T> source, Func<T, TKey> keySelector, Func<T, int, U> selector)
        {
            if (!source.Any())
            {
                yield break;
            }

            int itemCount = 0;
            T[] ordered = source.OrderByDescending(keySelector).ToArray();
            TKey previous = keySelector(ordered[0]);
            int rank = 1;
            foreach (T t in ordered)
            {
                itemCount += 1;
                TKey current = keySelector(t);
                if (!current.Equals(previous))
                {
                    rank = itemCount;
                }
                yield return selector(t, rank);
                previous = current;
            }
        }
        public static double AnnualAverage(double scores, int noOfsubjects)
        {
            int generalScores = (noOfsubjects * 3) * 100;
            double average = (scores / generalScores) * 100;
            return Math.Round(average, 2);
        }
        public static bool Attendances(double days)
        {
            if (days > 0) {return true; }   
            else
                return false;
        }

        public static string Annual_Calculate_Remark(double totalscore, int terms)
        {
            if (terms <= 0)
            {
                return "Invalid terms count"; // or throw an ArgumentException
            }

            double score = Math.Round(totalscore); // Simplified calculation
            if (score >= 75)
                return "Excellent";
            if (score >= 65)
                return "Very Good";
            if (score >= 55)
                return "Good";
            if (score >= 40)
                return "Fair";

            return "Poor";
        }
        //Grades Calculator
        public static string CalculateGrade(double score)
        {
            if (score >= 75)
            {
                return "A";
            }
            else if (score >= 65 && score <= 74)
            {
                return "B";
            }
            else if (score >= 55 && score <= 64)
            {
                return "C";
            }
            else if (score >= 40 && score <= 54)
            {
                return "D";
            }
            else
            {
                return "E";
            }
        }
        //Annual Grades Calculator
        public static string Annual_Calculate_Grade(double totalscore, int terms)
        {
            double score = (totalscore / (terms * 100)) * 100;
            if (score >= 75)
            {
                return "A";
            }
            else if (score >= 65 && score <= 74)
            {
                return "B";
            }
            else if (score >= 55 && score <= 64)
            {
                return "C";
            }
            else if (score >= 40 && score <= 54)
            {
                return "D";
            }
            else
            {
                return "E";
            }
        }
        public static string Annual_Calculate_Remark2(double totalscore, int terms)
        {
            double score = (totalscore / (terms * 100)) * 100;

            if (score >= 75)
            {
                return "Excellent";
            }
            else if (score >= 65 && score <= 74)
            {
                return "Very Good";
            }
            else if (score >= 55 && score <= 64)
            {
                return "Good";
            }
            else if (score >= 40 && score <= 54)
            {
                return "Fair";
            }
            else
            {
                return "Poor";
            }
        }
        //Annual Grades Calculator
        

        public static string Annual_Calculate_Grade2(double totalscore, int terms)
        {
            double score = (totalscore / (terms * 100)) * 100;
            if (score >= 75)
            {
                return "A";
            }
            else if (score >= 65 && score <= 74)
            {
                return "B";
            }
            else if (score >= 55 && score <= 64)
            {
                return "C";
            }
            else if (score >= 40 && score <= 54)
            {
                return "D";
            }
            else
            {
                return "E";
            }
        }
        public static string GetComment(double average)
        {
            if (average >= 75)
                return "Exceptional performance. Maintain excellent work.";
            else if (average >= 65)
                return "Very Good performance. Solid understanding. Improve with focused practice.";
            else if (average >= 55)
                return "Satisfactory performance. Significant improvement needed. But you can do better.";
            else if (average >= 40)
                return "Performance below expectations. Improve with extra study and practice.";
            else
                return "Unsatisfactory performance. Requires significant improvement. Seek immediate help.";
        }

        public static string GetPerformanceComment(double averageScore)
        {
            if (averageScore >= 75)
            {
                return "Excellent! Truly outstanding work. You've mastered the material. Keep shining!";
            }
            else if (averageScore >= 65)
            {
                return "Very Good! Solid progress and a strong understanding. Keep building and you'll reach excellence!";
            }
            else if (averageScore >= 55)
            {
                return "Satisfactory! Good effort, you're on track. Focus on improvement for even better results.";
            }
            else if (averageScore >= 40)
            {
                return "Passed! A step forward. Let's focus on growth and understanding to improve further.";
            }
            else
            {
                return "Starting Point! Don't worry, it's a chance to learn and grow. We can turn this around with effort!";
            }
        }
        public static string colorful(int v)
        {
            string color = "";
            switch (v)
            {
                case 1:
                    color = "bg-warning text-dark bg-opacity-25";
                    break;
                case 2:
                    color = "bg-info text-dark bg-opacity-25";
                    break;
                case 3:
                    color = "bg-primary text-dark bg-opacity-50";
                    break;
                case 4:
                    color = "bg-success text-dark bg-opacity-25";
                    break;
                case 5:
                    color = "bg-info text-dark bg-opacity-25";
                    break;
                case 6:
                    color = "bg-primary text-dark bg-opacity-50";
                    break;
                case 7:
                    color = "bg-warning text-dark bg-opacity-25";
                    break;
                case 8:
                    color = "bg-info text-dark bg-opacity-25";
                    break;
                case 9:
                    color = "bg-primary text-dark bg-opacity-50";
                    break;
                case 10:
                    color = "bg-secondary text-dark bg-opacity-25";
                    break;
                case 11:
                    color = "bg-info text-dark bg-opacity-50";
                    break;
                case 12:
                    color = "bg-light text-dark bg-opacity-50";
                    break;
                case 13:
                    color = "bg-warning";
                    break;
                case 14:
                    color = "bg-info";
                    break;
                case 15:
                    color = "bg-light";
                    break;
                case 16:
                    color = "bg-secondary   ";
                    break;
                case 17:
                    color = "bg-info";
                    break;
                case 18:
                    color = "bg-light";
                    break;
                case 19:
                    color = "bg-warning";
                    break;
                case 20:
                    color = "bg-light";
                    break;
                case 21:
                    color = "bg-secondary";
                    break;
                case 22:
                    color = "bg-info";
                    break;
                case 23:
                    color = "bg-light";
                    break;
                case 24:
                    color = "bg-warning";
                    break;
                case 25:
                    color = "bg-info";
                    break;
                case 26:
                    color = "bg-light";
                    break;
                case 27:
                    color = "bg-secondary   ";
                    break;
                case 28:
                    color = "bg-warning";
                    break;
                case 29:
                    color = "bg-light";
                    break;
                case 30:
                    color = "bg-secondary";
                    break;
                case 31:
                    color = "bg-info";
                    break;
                case 32:
                    color = "bg-light";
                    break;
                case 33:
                    color = "bg-secondary";
                    break;
                case 34:
                    color = "bg-info";
                    break;
                case 35:
                    color = "bg-light";
                    break;
                case 36:
                    color = "bg-secondary";
                    break;
                case 37:
                    color = "bg-info";
                    break;
                case 38:
                    color = "bg-light";
                    break;
                case 39:
                    color = "bg-info";
                    break;
                case 40:
                    color = "bg-light";
                    break;
                case 41:
                    color = "bg-secondary";
                    break;
                case 42:
                    color = "bg-info";
                    break;
                case 43:
                    color = "bg-light";
                    break;
                case 44:
                    color = "bg-secondary";
                    break;
                case 45:
                    color = "bg-info";
                    break;
                case 46:
                    color = "bg-light";
                    break;
                case 47:
                    color = "bg-secondary";
                    break;
                case 48:
                    color = "bg-info";
                    break;
                case 49:
                    color = "bg-light";
                    break;
                case 50:
                    color = "bg-secondary";
                    break;
                default:
                    color = "bg-success";
                    break;
            }
            return color;
        }
        public static string specialNumber(int sub)
        {
            string sup = "";
            switch (sub)
            {
                case 1:
                    sup = "st";
                    break;
                case 2:
                    sup = "nd";
                    break;
                case 3:
                    sup = "rd";
                    break;
                case 21:
                    sup = "st";
                    break;
                case 22:
                    sup = "nd";
                    break;
                case 23:
                    sup = "rd";
                    break;
                case 31:
                    sup = "st";
                    break;
                case 32:
                    sup = "nd";
                    break;
                case 33:
                    sup = "rd";
                    break;
                case 41:
                    sup = "st";
                    break;
                case 42:
                    sup = "nd";
                    break;
                case 43:
                    sup = "rd";
                    break;
                case 51:
                    sup = "st";
                    break;
                case 52:
                    sup = "nd";
                    break;
                case 53:
                    sup = "rd";
                    break;
                case 61:
                    sup = "st";
                    break;
                case 62:
                    sup = "nd";
                    break;
                case 63:
                    sup = "rd";
                    break;
                case 71:
                    sup = "st";
                    break;
                case 72:
                    sup = "nd";
                    break;
                case 73:
                    sup = "rd";
                    break;
                case 81:
                    sup = "st";
                    break;
                case 82:
                    sup = "nd";
                    break;
                case 83:
                    sup = "rd";
                    break;
                case 91:
                    sup = "st";
                    break;
                case 92:
                    sup = "nd";
                    break;
                case 93:
                    sup = "rd";
                    break;
                default:
                    sup = "th";
                    break;
            }
            return sup;
        }

        public static string GenerateUniqueNumber()
        {
            string dateTimeString = DateTime.Now.ToString("yyyyMMddHHmmss");
            Random random = new Random();
            string randomAlphabets = new string(Enumerable.Range(0, 4).Select(_ => (char)random.Next('A', 'Z' + 1)).ToArray());
            return dateTimeString + randomAlphabets;
        }

		private static string[] strings = { "First", "Second", "Third" };

		public static string GenerateHtmlOptions()
		{
			var options = new System.Text.StringBuilder();
			foreach (var str in strings)
			{
				options.AppendLine($"<option value=\"{str}\">{str}</option>");
			}
			return options.ToString();
		}
        public static string GetValueAtIndex(string[] array, int index)
        {
            try
            {
                if (array == null || array.Length == 0)
                {
                    return "0";
                }
                if (index >= 0 || index < array.Length)
                {
                    return array[index];
                }
                else
                    return "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public static string TimeAgo(this DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("{0} minutes ago", timeSpan.Minutes) :
                    "a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("{0} hours ago", timeSpan.Hours) :
                    "an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("{0} days ago", timeSpan.Days) :
                    "yesterday";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("{0} months ago", timeSpan.Days / 30) :
                    "a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("{0} years ago", timeSpan.Days / 365) :
                    "a year ago";
            }

            return result;
        }

        public static string MoneyToWords(double value)
        {
            decimal money = Math.Round((decimal)value, 2);
            int number = (int)money;
            int decimalValue = 0;
            string doller = string.Empty;
            string cents = string.Empty;
            doller = NumberToWords(number);
            if (money.ToString().Contains("."))
            {
                decimalValue = int.Parse(money.ToString().Split('.')[1]);
                cents = NumberToWords(decimalValue);
            }
            string result = !string.IsNullOrEmpty(cents) ? (decimalValue == 1 ? string.Format("{0} Naira and {1} kobo Only.", doller, cents) : string.Format("{0} Naira and {1} kobo Only.", doller, cents)) : string.Format("{0} Naira Only.", doller);
            return result;
        }

        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[(number) / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[(number) % 10];
                }
            }
            return words;
        }

        //public static string GenerateUniqueNumber()
        //{
        //    var guid = Guid.NewGuid().ToString("N"); // Remove hyphens
        //    var numberPart = guid.Substring(0, 10);
        //    return numberPart.ToUpper();
        //}
    }
}
