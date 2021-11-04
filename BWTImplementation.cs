using System;

namespace ConsoleApp1 {
    class BWTImplementation {
        public void Bwt_encode(byte[] buf_in, byte[] buf_out, int size, ref int primary_index) {
            int[] indices = new int[size];
            for (int i = 0; i < size; i++)
                indices[i] = i;

            Array.Sort(indices, 0, size, new BWTComparator(buf_in, size));

            for (int i = 0; i < size; i++)
                buf_out[i] = buf_in[(indices[i] + size - 1) % size];

            for (int i = 0; i < size; i++) {
                if (indices[i] == 1) {
                    primary_index = i;
                    return;
                }
            }
        }

        public void Bwt_decode(byte[] buf_encoded, byte[] buf_decoded, int size, int primary_index) {
            byte[] F = new byte[size];
            int[] buckets = new int[0x100];
            int[] indices = new int[size];

            for (int i = 0; i < 0x100; i++)
                buckets[i] = 0;

            for (int i = 0; i < size; i++)
                buckets[buf_encoded[i]]++;

            for (int i = 0, k = 0; i < 0x100; i++) {
                for (int j = 0; j < buckets[i]; j++) {
                    F[k++] = (byte)i;
                }
            }

            for (int i = 0, j = 0; i < 0x100; i++) {
                while (i > F[j] && j < size - 1) {
                    j++;
                }
                buckets[i] = j;
            }

            for (int i = 0; i < size; i++)
                indices[buckets[buf_encoded[i]]++] = i;

            for (int i = 0, j = primary_index; i < size; i++) {
                buf_decoded[i] = buf_encoded[j];
                j = indices[j];
            }
        }
    }
}
