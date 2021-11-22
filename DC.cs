using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1 {
    class DC {
        private string _sourceString;

        public DC(string sourceString) {
            _sourceString = sourceString;
        }

        private List<int> GetUniqueSymbolsIndex() {
            string temp = "";
            List<int> result = new();

            for (int i = 0; i < _sourceString.Length; i++) {
                if (!temp.Contains(_sourceString[i])) {
                    temp += _sourceString[i];
                    result.Add(i);

                }
            }

            return result;
        }

        // TODO: припилить арифметическое сжатие + переписать на байты
        public List<int> Encode() {
            List<int> result = new();
            List<int> indexOfFindedSymbols = GetUniqueSymbolsIndex();

            int endIndexOfSeries = 0;
            int indexOfNewJoin = 0;
            int countFindedSymbolsBetweenEndAndStartIndex;
            int i = 0;
            while(i < _sourceString.Length) {
                endIndexOfSeries = SkipRepeatedSymbols(i, indexOfFindedSymbols);

                if(endIndexOfSeries == _sourceString.Length) {
                    result.Add(0);
                    return result;
                }
                else {
                    indexOfNewJoin = _sourceString[endIndexOfSeries..].IndexOf(_sourceString[endIndexOfSeries - 1]);

                    if (indexOfNewJoin == -1) {

                        if (indexOfFindedSymbols.Count == _sourceString.Length)
                            break;

                        result.Add(0);

                        if (!indexOfFindedSymbols.Contains(i)) {
                            indexOfFindedSymbols.Add(i);
                        }
                    }
                    else {
                        indexOfNewJoin += endIndexOfSeries;
                        countFindedSymbolsBetweenEndAndStartIndex = GetCountWithoutFindedSymbols(endIndexOfSeries, indexOfNewJoin, indexOfFindedSymbols);
                        result.Add(indexOfNewJoin - endIndexOfSeries + 1 - countFindedSymbolsBetweenEndAndStartIndex);

                        if (!indexOfFindedSymbols.Contains(indexOfNewJoin))
                            indexOfFindedSymbols.Add(indexOfNewJoin);
                    }
                }

                i = endIndexOfSeries;
            }

            return result;
        }

        private bool CheckOfAllFindedSymbols(List<int> FindedSymbols, int index) {
            FindedSymbols.Sort();
            for(int i = index; i < FindedSymbols.Count; i++) {
                if (FindedSymbols[i] - FindedSymbols[i - 1] > 1)
                    return false;
            }
            return true;
        }

        private int SkipRepeatedSymbols(int index, List<int> FindedSymbols) {

            while (index < _sourceString.Length - 1 && _sourceString[index + 1] == _sourceString[index]) {
                FindedSymbols.Add(index);
                index++;
            }

            return index + 1;
        }

        private int GetCountWithoutFindedSymbols(int startIndex, int endIndex, List<int> findedIndex) {
            int counter = 0;

            for (int i = startIndex; i < endIndex; i++) {
                if (findedIndex.Contains(i)) {
                    counter++;
                }
            }
            
            return counter == 0 ? 0 : counter;
        }
    }
}
