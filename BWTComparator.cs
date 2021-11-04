using System.Collections.Generic;

namespace ConsoleApp1 {
    class BWTComparator : IComparer<int> {
        private byte[] rotlexcmp_buf = null;
        private int rottexcmp_bufsize = 0;

        public BWTComparator(byte[] array, int size) {
            rotlexcmp_buf = array;
            rottexcmp_bufsize = size;
        }

        public int Compare(int li, int ri) {
            int ac = rottexcmp_bufsize;
            while (rotlexcmp_buf[li] == rotlexcmp_buf[ri]) {
                if (++li == rottexcmp_bufsize)
                    li = 0;
                if (++ri == rottexcmp_bufsize)
                    ri = 0;
                if (--ac <= 0)
                    return 0;
            }
            if (rotlexcmp_buf[li] > rotlexcmp_buf[ri])
                return 1;

            return -1;
        }
    }
}
