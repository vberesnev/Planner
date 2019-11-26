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

        public DateValue(int key, string value)
        {
            Key = key;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            DateValue date = obj as DateValue;
            if (date == null) return false;
            return Key == date.Key && Value == date.Value;
        }


    }
}
