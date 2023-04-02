using System;

namespace Core.CensusTracts {
    [Serializable]
    public struct CensusNumber {
        public int Number;
        public int SubNumber;
        public override string ToString() => SubNumber == 0 ? Number.ToString() : $"{Number}.{SubNumber:00}";
        public CensusNumber(string number) {
            int dotIndex = number.IndexOf('.');
            if (dotIndex == -1) {
                Number = int.Parse(number);
                SubNumber = 0;
            } else {
                Number = int.Parse(number[..dotIndex]);
                SubNumber = int.Parse(number[(dotIndex + 1)..]);
            }
        }
    }
}