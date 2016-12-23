using System;
using System.Collections.Generic;
using System.Data;
using amxDatenImport.Exceptions;

namespace amxDatenImport
{
    public static class AmxDatenImport
    {
        public static List<T> Transform<T>(DataSet excelDataSet,int tableindex, IReadOnlyDictionary<int, string> matchdict, int rowDataOffset = 0, int columnDataOffset = 0, bool ignoreMatchExceptions = true) where T : new()
        {
            try
            {
                if ((rowDataOffset > excelDataSet.Tables[tableindex].Rows.Count) || (rowDataOffset < 0))
                {
                    throw new AmxInvalidRowOffsetException(rowDataOffset, excelDataSet.Tables[tableindex].Rows.Count,
                        "Der Rowoffset Wert ist ungültig");
                }
                if ((columnDataOffset > excelDataSet.Tables[tableindex].Columns.Count) || (rowDataOffset < 0))
                {
                    throw new AmxInvalidColumnOffsetException(columnDataOffset,
                        excelDataSet.Tables[tableindex].Columns.Count, "Der Columnoffset Wert ist ungültig");
                }
                if ((tableindex > excelDataSet.Tables.Count) || (tableindex < 0))
                {
                    throw new AmxInvalidTableIndexException(columnDataOffset,
                        excelDataSet.Tables[tableindex].Columns.Count, "Der TableIndex Wert ist ungültig");
                }
                var resultlist = new List<T>();
                foreach (DataRow row in excelDataSet.Tables[0].Rows)
                {
                    var khkObject = new T();
                    if (excelDataSet.Tables[tableindex].Rows.IndexOf(row) < rowDataOffset) continue; // Überspringe alle Rows die über dem RowOffset liegen.
                    for (var i = 0; i < row.ItemArray.Length - columnDataOffset; i++)
                    {
                        var propertyname = "";
                        try
                        {
                            if (!matchdict.TryGetValue(i + columnDataOffset, out propertyname) && !ignoreMatchExceptions)
                            {
                                throw new AmxInvalidMatchDictonaryEntryException(i + columnDataOffset,
                                    "Der Wert für Row: " + (i + columnDataOffset) +
                                    " im MatchDict wurde nicht gefunden oder ist Ungültig");
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        var value = row[i + columnDataOffset]; // Rows ab dem columnDataOffset
                        if (propertyname != null) FillProperty(typeof (T), khkObject, propertyname, value);
                    }
                    resultlist.Add(khkObject);
                }
                return resultlist;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private static void FillProperty(Type type, object instance,string propertyname, object value)
        {
            try
            {
                if(type.GetProperty(propertyname) == null)
                {
                    throw new AmxInvalidTypePropertyException(propertyname,type,"Die Property: " + propertyname + " für den Typen: " + type +" wurde nicht gefunden.");
                }
                if (type.GetProperty(propertyname).PropertyType != value.GetType())
                {
                    try
                    {
                        value = TryConvert(value, type.GetProperty(propertyname).PropertyType);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    //throw new AmxInvalidValueForPropertyTypeException(propertyname, value, type.GetProperty(propertyname).PropertyType, value.GetType(), "Der Typ der zuzuweisenden Property entspricht nicht dem Value Property-Typen");
                }
                type.GetProperty(propertyname).SetValue(instance, value);
            }
            catch (FormatException ex)
            {
                throw new AmxInvalidValueForPropertyTypeException(propertyname, value,
                    type.GetProperty(propertyname).PropertyType, value.GetType(),
                    "Der Typ der zuzuweisenden Property entspricht nicht dem Value Property-Typen");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private static object TryConvert(object value, Type targetType)
        {
            try
            {
                if (targetType == typeof (int) || targetType == typeof (int?))
                {
                    return Convert.ToInt32(value);
                }
                if (targetType == typeof (string))
                {
                    return Convert.ToString(value);
                }
                if (targetType == typeof (decimal) || targetType == typeof (decimal?))
                {
                    return Convert.ToDecimal(value);
                }
                if (targetType == typeof (DateTime) || targetType == typeof (DateTime?))
                {
                    return Convert.ToDecimal(value);
                }
                if (targetType == typeof (short) || targetType == typeof (short?))
                {
                    return Convert.ToInt16(value);
                }
                else return null;
            }
            
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
