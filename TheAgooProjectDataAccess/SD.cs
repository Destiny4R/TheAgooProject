namespace TheAgooProjectDataAccess
{
    public static class  SD
    {
        //public static string privileges
        public const string Role_Admin = "Admin";
        public const string Role_ExamOfficer = "Exam Officer";
        public const string Role_Accountant = "Accountant";

        //FEES PAYMENT STATUS
        public static string Fees_Status_Completed = "Completed";
        public static string Fees_Status_Part_Payment = "Part Payment";

        //Teachers status
        public static string Status_Active = "Active";
        public static string Status_InActive = "InActive";

        public static string IsStudent = "IsStudent";

        //Teachers status
        public static string First = "First";
        public static string Second = "Second";
        public static string Third = "Third";

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
        public static string Remark(double totalScores)
        {
            if (totalScores >= 75)
            {
                return "Distinction";
            }
            else if (totalScores >= 65)
            {
                return "Very Good";
            }
            else if (totalScores >= 55)
            {
                return "Good";
            }
            else if (totalScores >= 40)
            {
                return "Fair";
            }
            else
            {
                return "Poor";
            }
        }
        public static string GetGrade(double? sum)
        {
            if (sum < 40)
            {
                return "E";
            }
            else if (sum < 55)
            {
                return "D";
            }
            else if (sum < 65)
            {
                return "C";
            }
            else if (sum < 75)
            {
                return "B";
            }
            else
            {
                return "A";
            }
        }
        //ANNUAL Remark Calculator
        public static string GetRemark(double? sum)
        {
            if (sum < 40)
            {
                return "Poor";
            }
            else if (sum < 55)
            {
                return "Fair";
            }
            else if (sum < 65)
            {
                return "Good";
            }
            else if (sum < 75)
            {
                return "Very Good";
            }
            else
            {
                return "Distintction";
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
            double sum = (totalscore / (terms * 100)) * 100;
            if (sum < 50)
            {
                return "Poor";
            }
            else if (sum < 60)
            {
                return "Fair";
            }
            else if (sum < 70)
            {
                return "Good";
            }
            else if (sum < 80)
            {
                return "Very Good";
            }
            else
            {
                return "Distintction";
            }
        }
        //Grades Calculator
        public static string CalculateGrade(double sum)
        {
            if (sum < 50)
            {
                return "E";
            }
            else if (sum < 60)
            {
                return "D";
            }
            else if (sum < 70)
            {
                return "C";
            }
            else if (sum < 80)
            {
                return "B";
            }
            else
            {
                return "A";
            }
        }
        //Annual Grades Calculator
        public static string Annual_Calculate_Grade(double totalscore, int terms)
        {
            double sum = (totalscore / (terms * 100)) * 100;
            if (sum < 50)
            {
                return "E";
            }
            else if (sum < 60)
            {
                return "D";
            }
            else if (sum < 70)
            {
                return "C";
            }
            else if (sum < 80)
            {
                return "B";
            }
            else
            {
                return "A";
            }
        }
        public static string Annual_Calculate_Remark2(double totalscore, int terms)
        {
            double sum = (totalscore / (terms * 100)) * 100;
            if (sum < 50)
            {
                return "Poor";
            }
            else if (sum < 60)
            {
                return "Fair";
            }
            else if (sum < 70)
            {
                return "Good";
            }
            else if (sum < 80)
            {
                return "Very Good";
            }
            else
            {
                return "Distintction";
            }
        }
        //Annual Grades Calculator
        public static string Annual_Calculate_Grade2(double totalscore, int terms)
        {
            double sum = (totalscore / (terms * 100)) * 100;
            if (sum < 50)
            {
                return "E";
            }
            else if (sum < 60)
            {
                return "D";
            }
            else if (sum < 70)
            {
                return "C";
            }
            else if (sum < 80)
            {
                return "B";
            }
            else
            {
                return "A";
            }
        }
        public static string GetComment(double average)
        {
            if (average >= 75)
                return "Exceptional performance. Strong grasp of subject matter. Maintain excellent work.";
            else if (average >= 65)
                return "Good performance. Solid understanding. Improve with focused practice.";
            else if (average >= 55)
                return "Satisfactory performance. Significant improvement needed. But you can do better.";
            else if (average >= 40)
                return "Performance below expectations. Improve with extra study and practice.";
            else
                return "Unsatisfactory performance. Requires significant improvement. Seek immediate help.";
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
                default:
                    sup = "th";
                    break;
            }
            return sup;
        }
    }
}
