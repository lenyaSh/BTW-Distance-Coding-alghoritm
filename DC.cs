using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1 {
    class DC {
        private string _sourceString;
        private string _alphabet;

        public DC(string sourceString) {
            _sourceString = sourceString;
        }

        private string GetAlhabetFromText() {
            string result = "";
            foreach(char elem in _sourceString) {
                if(!result.Contains(elem)) {
                    result += elem;
                }
            }

            return string.Concat(result.OrderBy(char.ToLower));
        }

        public List<int> Encode() {
            _alphabet = GetAlhabetFromText();
            string temp = _alphabet + _sourceString;
            List<int> result = new();
            List<int> indexOfFindedSymbols = new() { 4, 3, 2, 1, 0};

            int indexOfSearchSymbol;
            int countWithoutFindedSymbols = 0;
            int indexSearcedSymbolOnSubsrting = 0;

            for(int i = 0; i < temp.Length - 1; i++) {
                indexSearcedSymbolOnSubsrting = temp[(i + 1)..].IndexOf(temp[i]);

                if(indexSearcedSymbolOnSubsrting != -1) {
                    indexOfSearchSymbol = indexSearcedSymbolOnSubsrting + i + 1;
                    countWithoutFindedSymbols = GetCountWithoutFindedSymbols(i, indexOfSearchSymbol, indexOfFindedSymbols);
                }
                else {
                    countWithoutFindedSymbols = GetCountWithoutFindedSymbols(i, temp.Length, indexOfFindedSymbols, true);
                }

                if (countWithoutFindedSymbols != -1)
                    result.Add(countWithoutFindedSymbols);
            }
            return result;
        }
       
        private int GetCountWithoutFindedSymbols(int startIndex, int endIndex, List<int> findedIndex, bool isEndOfText = false) {
            int counter = 0;

            for(int i = startIndex + 1; i < endIndex; i++) {
                if (findedIndex.Contains(i)) {
                    counter++;
                }
            }
            if (!isEndOfText)
                findedIndex.Add(endIndex);
            else
                findedIndex.Add(startIndex);

            if (counter == 0) {
                return -1;
            }

            return endIndex - startIndex - counter - 1;
        }
    }
}
