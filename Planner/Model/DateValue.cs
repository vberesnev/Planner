using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model
{
    public class DateValue
    {
        public int Key { get; private set; }
        public string Value { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public DateValue(int key)
        {
            Key = key;
        }

        public DateValue(int key, string value)
        {
            Key = key;
            Value = value;
        }

        public DateValue(int key, string value, DateTime start, DateTime end)
        {
            Key = key;
            Value = value;
            Start = start;
            End = end;
        }

        public override bool Equals(object obj)
        {
            DateValue date = obj as DateValue;
            if (date == null) return false;
            return Key == date.Key || Value == null;
        }


    }
}
