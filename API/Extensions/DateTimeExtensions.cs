using System;

namespace API.Extensions
{
    public  static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var Age = today.Year - dob.Year;
            if(dob.Date > today.AddYears(-Age)) Age--;
            return Age;  
        }
    }
}