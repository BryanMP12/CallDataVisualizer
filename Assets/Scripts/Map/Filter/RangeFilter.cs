// using System;
//
// namespace Map.Filter {
//     public readonly struct RangeFilter<T> where T : IComparable, IComparable<T> {
//         readonly T Min;
//         readonly T Max;
//         readonly RangeType Type;
//         public RangeFilter(T mi, T ma, RangeType type) => (Min, Max, Type) = (mi, ma, type);
//         public bool InRange(T val) =>
//             Type switch {
//                 RangeType.NONE    => true,
//                 RangeType.MIN     => Min.IsLessThan(val),
//                 RangeType.MAX     => val.IsLessThan(Max),
//                 RangeType.BETWEEN => Min.IsLessThan(val) && val.IsLessThan(Max),
//                 RangeType.EQUAL   => Min.IsEqual(val),
//                 _                 => false
//             };
//     }
//     public enum RangeType { NONE, MIN, MAX, BETWEEN, EQUAL }
//     public static class FilterExtensions {
//         public static bool IsLessThan<T>(this T actual, T comp) where T : IComparable<T> => actual.CompareTo(comp) < 0;
//         public static bool IsEqual<T>(this T actual, T comp) where T : IComparable<T> => actual.CompareTo(comp) == 0;
//     }
// }