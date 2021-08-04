using System;
using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Common.Utilities
{
    public class AttendanceDateAttribute : RangeAttribute
    {
        public AttendanceDateAttribute()
        : base(typeof(DateTime), DateTime.Now.AddDays(-7).ToShortDateString(), DateTime.Now.ToShortDateString()) { }
    }
}
