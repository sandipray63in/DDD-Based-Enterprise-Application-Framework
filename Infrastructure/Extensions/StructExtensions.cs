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
                int structValueInt = structValue.ConvertToType<int>();
                int otherStructValueInt = otherStructValue.ConvertToType<int>();
                int finalStructValueInt = structValueInt + otherStructValueInt;
                return finalStructValueInt.ConvertToType<TStruct>();
            }
            else if (typeof(TStruct) == typeof(uint))
            {
                uint structValueUInt = structValue.ConvertToType<uint>();
                uint otherStructValueUInt = otherStructValue.ConvertToType<uint>();
                uint finalStructValueUInt = structValueUInt + otherStructValueUInt;
                return finalStructValueUInt.ConvertToType<TStruct>();
            }
            else if (typeof(TStruct) == typeof(long))
            {
                long structValueLong = structValue.ConvertToType<long>();
                long otherStructValueLong = otherStructValue.ConvertToType<long>();
                long finalStructValueLong = structValueLong + otherStructValueLong;
                return finalStructValueLong.ConvertToType<TStruct>();
            }
            else if (typeof(TStruct) == typeof(ulong))
            {
                ulong structValueULong = structValue.ConvertToType<ulong>();
                ulong otherStructValueULong = otherStructValue.ConvertToType<ulong>();
                ulong finalStructValueULong = structValueULong + otherStructValueULong;
                return finalStructValueULong.ConvertToType<TStruct>();
            }
            else throw new NotSupportedException(string.Format("Type {0} is not supported currently!", typeof(TStruct)));
        }

        public static bool IsEqualTo<TStruct>(this TStruct structValue, TStruct otherStructValue)
            where TStruct : struct, IComparable<TStruct>
        {
            if (typeof(TStruct) == typeof(int))
            {
                int structValueInt = structValue.ConvertToType<int>();
                int otherStructValueInt = otherStructValue.ConvertToType<int>();
                return structValueInt == otherStructValueInt;
            }
            else if (typeof(TStruct) == typeof(uint))
            {
                uint structValueUInt = structValue.ConvertToType<uint>();
                uint otherStructValueUInt = otherStructValue.ConvertToType<uint>();
                return structValueUInt == otherStructValueUInt;
            }
            else if (typeof(TStruct) == typeof(long))
            {
                long structValueLong = structValue.ConvertToType<long>();
                long otherStructValueLong = otherStructValue.ConvertToType<long>();
                return structValueLong == otherStructValueLong;
            }
            else if (typeof(TStruct) == typeof(ulong))
            {
                ulong structValueULong = structValue.ConvertToType<ulong>();
                ulong otherStructValueULong = otherStructValue.ConvertToType<ulong>();
                return structValueULong == otherStructValueULong;
            }
            else throw new NotSupportedException(string.Format("Type {0} is not supported currently!", typeof(TStruct)));
        }

        public static bool IsGreaterThan<TStruct>(this TStruct structValue, TStruct otherStructValue)
            where TStruct : struct, IComparable<TStruct>
        {
            if (typeof(TStruct) == typeof(int))
            {
                int structValueInt = structValue.ConvertToType<int>();
                int otherStructValueInt = otherStructValue.ConvertToType<int>();
                return structValueInt > otherStructValueInt;
            }
            else if (typeof(TStruct) == typeof(uint))
            {
                uint structValueUInt = structValue.ConvertToType<uint>();
                uint otherStructValueUInt = otherStructValue.ConvertToType<uint>();
                return structValueUInt > otherStructValueUInt;
            }
            else if (typeof(TStruct) == typeof(long))
            {
                long structValueLong = structValue.ConvertToType<long>();
                long otherStructValueLong = otherStructValue.ConvertToType<long>();
                return structValueLong > otherStructValueLong;
            }
            else if (typeof(TStruct) == typeof(ulong))
            {
                ulong structValueULong = structValue.ConvertToType<ulong>();
                ulong otherStructValueULong = otherStructValue.ConvertToType<ulong>();
                return structValueULong > otherStructValueULong;
            }
            else throw new NotSupportedException(string.Format("Type {0} is not supported currently!", typeof(TStruct)));
        }

        public static bool IsLesserThan<TStruct>(this TStruct structValue, TStruct otherStructValue)
            where TStruct : struct, IComparable<TStruct>
        {
            return !structValue.IsGreaterThan(otherStructValue) && !structValue.IsEqualTo(otherStructValue);
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

        public static TStruct ConvertToType<TStruct>(this object value)
            where TStruct : struct, IComparable<TStruct>
        {
            return (TStruct)Convert.ChangeType(value, typeof(TStruct));
        }
    }
}
