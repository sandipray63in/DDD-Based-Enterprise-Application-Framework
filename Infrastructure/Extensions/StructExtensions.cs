using System;

namespace Infrastructure.Extensions
{
    public static class StructExtensions
    {
        public static TStruct Add<TStruct>(this TStruct structValue,TStruct otherStructValue) 
            where TStruct : struct,IComparable<TStruct>
        {
            if (typeof(TStruct) == typeof(int))
            {
                int structValueInt = ConvertToType<int>(structValue);
                int otherStructValueInt = ConvertToType<int>(otherStructValue);
                int finalStructValueInt = structValueInt + otherStructValueInt;
                return ConvertToType<TStruct>(finalStructValueInt);
            }
            else if (typeof(TStruct) == typeof(uint))
            {
                uint structValueUInt = ConvertToType<uint>(structValue);
                uint otherStructValueUInt = ConvertToType<uint>(otherStructValue);
                uint finalStructValueUInt = structValueUInt + otherStructValueUInt;
                return ConvertToType<TStruct>(finalStructValueUInt);
            }
            else if (typeof(TStruct) == typeof(long))
            {
                long structValueLong = ConvertToType<long>(structValue);
                long otherStructValueLong = ConvertToType<long>(otherStructValue);
                long finalStructValueLong = structValueLong + otherStructValueLong;
                return ConvertToType<TStruct>(finalStructValueLong);
            }
            else if (typeof(TStruct) == typeof(ulong))
            {
                ulong structValueULong = ConvertToType<ulong>(structValue);
                ulong otherStructValueULong = ConvertToType<ulong>(otherStructValue);
                ulong finalStructValueULong = structValueULong + otherStructValueULong;
                return ConvertToType<TStruct>(finalStructValueULong);
            }
            else throw new NotSupportedException(string.Format("Type {0} is not supported currently!", typeof(TStruct)));
        }

        public static bool IsEqualTo<TStruct>(this TStruct structValue, TStruct otherStructValue)
            where TStruct : struct, IComparable<TStruct>
        {
            if (typeof(TStruct) == typeof(int))
            {
                int structValueInt = ConvertToType<int>(structValue);
                int otherStructValueInt = ConvertToType<int>(otherStructValue);
                return structValueInt == otherStructValueInt;
            }
            else if (typeof(TStruct) == typeof(uint))
            {
                uint structValueUInt = ConvertToType<uint>(structValue);
                uint otherStructValueUInt = ConvertToType<uint>(otherStructValue);
                return structValueUInt == otherStructValueUInt;
            }
            else if (typeof(TStruct) == typeof(long))
            {
                long structValueLong = ConvertToType<long>(structValue);
                long otherStructValueLong = ConvertToType<long>(otherStructValue);
                return structValueLong == otherStructValueLong;
            }
            else if (typeof(TStruct) == typeof(ulong))
            {
                ulong structValueULong = ConvertToType<ulong>(structValue);
                ulong otherStructValueULong = ConvertToType<ulong>(otherStructValue);
                return structValueULong == otherStructValueULong;
            }
            else throw new NotSupportedException(string.Format("Type {0} is not supported currently!", typeof(TStruct)));
        }

        public static bool IsGreaterThan<TStruct>(this TStruct structValue, TStruct otherStructValue)
            where TStruct : struct, IComparable<TStruct>
        {
            if (typeof(TStruct) == typeof(int))
            {
                int structValueInt = ConvertToType<int>(structValue);
                int otherStructValueInt = ConvertToType<int>(otherStructValue);
                return structValueInt > otherStructValueInt;
            }
            else if (typeof(TStruct) == typeof(uint))
            {
                uint structValueUInt = ConvertToType<uint>(structValue);
                uint otherStructValueUInt = ConvertToType<uint>(otherStructValue);
                return structValueUInt > otherStructValueUInt;
            }
            else if (typeof(TStruct) == typeof(long))
            {
                long structValueLong = ConvertToType<long>(structValue);
                long otherStructValueLong = ConvertToType<long>(otherStructValue);
                return structValueLong > otherStructValueLong;
            }
            else if (typeof(TStruct) == typeof(ulong))
            {
                ulong structValueULong = ConvertToType<ulong>(structValue);
                ulong otherStructValueULong = ConvertToType<ulong>(otherStructValue);
                return structValueULong > otherStructValueULong;
            }
            else throw new NotSupportedException(string.Format("Type {0} is not supported currently!", typeof(TStruct)));
        }

        public static bool IsLesserThan<TStruct>(this TStruct structValue, TStruct otherStructValue)
            where TStruct : struct, IComparable<TStruct>
        {
            return !structValue.IsGreaterThan(otherStructValue);
        }

        public static bool IsGreaterThanOrEqualTo<TStruct>(this TStruct structValue, TStruct otherStructValue)
            where TStruct : struct, IComparable<TStruct>
        {
            return structValue.IsEqualTo(otherStructValue) || structValue.IsGreaterThan(otherStructValue);
        }

        public static bool IsLesserThanOrEqualTo<TStruct>(this TStruct structValue, TStruct otherStructValue)
            where TStruct : struct, IComparable<TStruct>
        {
            return structValue.IsEqualTo(otherStructValue) || structValue.IsLesserThan(otherStructValue);
        }

        private static TStruct ConvertToType<TStruct>(object value) where TStruct : struct
        {
            return (TStruct)Convert.ChangeType(value, typeof(TStruct));
        }
    }
}
