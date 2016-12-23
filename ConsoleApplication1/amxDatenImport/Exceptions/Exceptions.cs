using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace amxDatenImport.Exceptions
{
    public class AmxInvalidDataTypeException : Exception
    {
    }

    public class AmxInvalidRowOffsetException : Exception
    {
        public int RowOffset { get; set; }
        public int RowCount { get; set; }

        public AmxInvalidRowOffsetException(int rowoffset,int rowcount, string message) : base (message)
        {
            RowOffset = rowoffset;
            RowCount = rowcount;
        }
    }
    public class AmxInvalidColumnOffsetException : Exception
    {
        public int ColumnOffset { get; set; }
        public int ColumnCount { get; set; }

        public AmxInvalidColumnOffsetException(int columnoffset, int columncount, string message) : base(message)
        {
            ColumnOffset = columnoffset;
            ColumnCount = columncount;
        }
    }

    public class AmxInvalidTableIndexException : Exception
    {
        public int TableIndex { get; set; }
        public int TableCount { get; set; }

        public AmxInvalidTableIndexException(int tableindex, int tablecount, string message) : base(message)
        {
            TableIndex = tableindex;
            TableCount = tablecount;
        }
    }

    public class AmxInvalidTypePropertyException : Exception
    {
        public string PropertyName { get; set; }
        public Type TargetType { get; set; }

        public AmxInvalidTypePropertyException(string propertyName, Type targetType, string message) : base(message)
        {
            PropertyName = propertyName;
            TargetType = targetType;
        }
    }

    public class AmxInvalidValueForPropertyTypeException : Exception
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public Type PropertyType { get; set; }
        public Type ValueType { get; set; }

        public int Row { get; set; }
        public int Collumn { get; set; }

        public AmxInvalidValueForPropertyTypeException(string propertyName,object value,Type propertyType, Type valueType, string message) : base(message)
        {
            PropertyName = propertyName;
            Value = value;
            PropertyType = propertyType;
            ValueType = valueType;
        }
    }

    public class AmxInvalidMatchDictonaryEntryException : Exception
    {
        public AmxInvalidMatchDictonaryEntryException(int key, string message) : base(message)
        {
        }
    }
}
