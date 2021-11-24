using System.Collections.Generic;

namespace ConsoleApp1 {
    class ArithmeticCompression {
        private SortedDictionary<int, decimal> _probabilities = new();
        public ArithmeticCompression(SortedDictionary<int, decimal> probabities) {
            _probabilities = probabities;
        }

        public decimal Encode(List<int> uncompressed) {
            decimal leftLimit = 0, rightLimit = 1, codeRange;
            var rangesForEachSymbol = GetRangeForEachSymbol();

            for(int i = 0; i < uncompressed.Count; i++) {
                codeRange = rightLimit - leftLimit;
                rightLimit = leftLimit + codeRange * rangesForEachSymbol[uncompressed[i]].Item2;
                leftLimit += codeRange * rangesForEachSymbol[uncompressed[i]].Item1;
            }

            return leftLimit;
        }

        private Dictionary<int, (decimal, decimal)> GetRangeForEachSymbol() {
            var result = new Dictionary<int, (decimal, decimal)>();
            decimal rightRange = 0;
            decimal leftRange;
            foreach (KeyValuePair<int, decimal> elem in _probabilities) {
                leftRange = rightRange;
                rightRange += elem.Value;
                result.Add(elem.Key, (leftRange, rightRange));
            }

            return result;
        }

    }
}
