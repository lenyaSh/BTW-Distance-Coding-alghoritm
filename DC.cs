using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1 {
    class DC {
        private readonly byte[] _sourceSequence;

        public DC(byte[] sourceSequence) {
            _sourceSequence = sourceSequence;
        }

        private Dictionary<byte, int> GetUniqueSymbolsIndex() {
            List<byte> temp = new();
            Dictionary<byte, int> result = new();

            for (int i = 0; i < _sourceSequence.Length; i++) {
                if (!temp.Contains(_sourceSequence[i])) {
                    temp.Add(_sourceSequence[i]);
                    result.Add(_sourceSequence[i], i);
                }
            }

            return result;
        }

        public (List<int>, Dictionary<byte, int>) Encode() {
            List<int> result = new();
            Dictionary<byte, int> IndexOfSymbols = GetUniqueSymbolsIndex();
            List<int> indexOfFindedSymbols = IndexOfSymbols.Values.ToList();

            int endIndexOfSeries;
            int indexOfNewJoin;
            int countFindedSymbolsBetweenEndAndStartIndex;
            int i = 0;
            while(i < _sourceSequence.Length) {
                endIndexOfSeries = SkipRepeatedSymbols(i, indexOfFindedSymbols);

                if(endIndexOfSeries == _sourceSequence.Length) {
                    result.Add(0);
                    return (result, IndexOfSymbols);
                }
                else {
                    indexOfNewJoin = Array.IndexOf(_sourceSequence[endIndexOfSeries..], _sourceSequence[endIndexOfSeries - 1]);

                    if (indexOfNewJoin == -1) {

                        if (indexOfFindedSymbols.Count == _sourceSequence.Length)
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

            return (result, IndexOfSymbols);
        }

        private int SkipRepeatedSymbols(int index, List<int> FindedSymbols) {

            while (index < _sourceSequence.Length - 1 && _sourceSequence[index + 1] == _sourceSequence[index]) {
                FindedSymbols.Add(index);
                index++;
            }

            return index + 1;
        }

        private static int GetCountWithoutFindedSymbols(int startIndex, int endIndex, List<int> findedIndex) {
            int counter = 0;

            for (int i = startIndex; i < endIndex; i++) {
                if (findedIndex.Contains(i)) {
                    counter++;
                }
            }
            
            return counter == 0 ? 0 : counter;
        }


        public byte[] Decode(List<int> distance, Dictionary<byte, int> indexOfSymbols, int fileSize) {
            int[] result = new int[fileSize];
            for(int i = 0; i < result.Length; i++) {
                result[i] = -1;
            }

            foreach(KeyValuePair<byte, int> elem in indexOfSymbols) {
                result[elem.Value] = elem.Key;
            }

            int j = 0;
            for(int i = 0, counter = 0;  i < result.Length - 1; i++) {

                while (i < result.Length - 1 && result[i + 1] == -1) {
                    i++;
                    result[i] = result[i-1];
                }

                if(counter < distance.Count && distance[counter] != 0 ) {
                    j = i + 1;

                    while(distance[counter] > 0) {
                        if(result[j] == -1) {
                            distance[counter]--;
                        }

                        j++;
                    }

                    result[j - 1] = result[i];
                }

                counter++;
            }

            return result.Select(elem => (byte)elem).ToArray();
        }


        // TODO: добавить запись и чтение из файла, расчет энтропии, показать размер 
        // TODO: потестить разные файлы на вход и посмотреть энтропию и как сжимает
    }
}
