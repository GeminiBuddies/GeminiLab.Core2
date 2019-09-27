namespace GeminiLab.Core2 {
    public static class GMath {
        public static ulong Ceil2(ulong v) {
            unchecked {
                if ((v & (v - 1)) == 0) return v;

                while ((v & (v - 1)) != 0) v &= (v - 1);
                return v << 1;
            }
        }
    }
}
