namespace GeminiLab.Core2.Yielder.Yielders {
    internal class NYielder : IYielder<int> {
        private int _now = 0;

        public int Next() => _now++;
    }
}
