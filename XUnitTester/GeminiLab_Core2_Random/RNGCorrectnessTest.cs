using Xunit;

using GeminiLab.Core2.Random;
using GeminiLab.Core2.Random.RNG;

namespace XUnitTester.GeminiLab_Core2_Random {
    public class RNGCorrectnessTest {
        [Fact]
        public void LCGCorrectness() {
            var lcg = new LCG(123, 456, 1);

            Assert.Equal(579u, lcg.Next());
            Assert.Equal(71673u, lcg.Next());
            Assert.Equal(8816235u, lcg.Next());
            Assert.Equal(1084397361u, lcg.Next());
            Assert.Equal(236889683u, lcg.Next());
            Assert.Equal(3367627689u, lcg.Next());
            Assert.Equal(1901345787u, lcg.Next());
            Assert.Equal(1937298273u, lcg.Next());
            Assert.Equal(2064486755u, lcg.Next());
            Assert.Equal(528800857u, lcg.Next());
            Assert.Equal(617996427u, lcg.Next());
            Assert.Equal(2999116945u, lcg.Next());
            Assert.Equal(3819164531u, lcg.Next());
            Assert.Equal(1605802505u, lcg.Next());
            Assert.Equal(4240180251u, lcg.Next());
            Assert.Equal(1851128513u, lcg.Next());

            lcg.Seed(2);

            Assert.Equal(702u, lcg.Next());
            Assert.Equal(86802u, lcg.Next());
            Assert.Equal(10677102u, lcg.Next());
            Assert.Equal(1313284002u, lcg.Next());
            Assert.Equal(2620142750u, lcg.Next());
            Assert.Equal(155011506u, lcg.Next());
            Assert.Equal(1886546510u, lcg.Next());
            Assert.Equal(116987202u, lcg.Next());
            Assert.Equal(1504524414u, lcg.Next());
            Assert.Equal(372909650u, lcg.Next());
            Assert.Equal(2918214446u, lcg.Next());
            Assert.Equal(2458091746u, lcg.Next());
            Assert.Equal(1697574494u, lcg.Next());
            Assert.Equal(2643233010u, lcg.Next());
            Assert.Equal(2995113486u, lcg.Next());
            Assert.Equal(3326739074u, lcg.Next());
        }

        [Fact]
        public void LCG64Correctness() {
            var lcg = new LCG64(314159265358ul, 97932ul, 1);

            Assert.Equal(314159363290ul, lcg.Next());
            Assert.Equal(5993982177332860152ul, lcg.Next());
            Assert.Equal(369420633178190364ul, lcg.Next());
            Assert.Equal(17851908735567506196ul, lcg.Next());
            Assert.Equal(14998322079835412132ul, lcg.Next());
            Assert.Equal(5239223867101183108ul, lcg.Next());
            Assert.Equal(16808802568918681284ul, lcg.Next());
            Assert.Equal(2745174242918622788ul, lcg.Next());
            Assert.Equal(6801576654965309252ul, lcg.Next());
            Assert.Equal(1664223310916394308ul, lcg.Next());
            Assert.Equal(16395089757413359940ul, lcg.Next());
            Assert.Equal(700969116798794052ul, lcg.Next());
            Assert.Equal(8775323433165080900ul, lcg.Next());
            Assert.Equal(4578408129378986308ul, lcg.Next());
            Assert.Equal(6720761757790333252ul, lcg.Next());
            Assert.Equal(14641174864876988740ul, lcg.Next());
            Assert.Equal(11335382737800915268ul, lcg.Next());
            Assert.Equal(4053786157359950148ul, lcg.Next());
            Assert.Equal(18154670123740948804ul, lcg.Next());
            Assert.Equal(3128798039275333956ul, lcg.Next());

            lcg.Seed(1);

            Assert.Equal(314159363290ul, lcg.Next());
            Assert.Equal(5993982177332860152ul, lcg.Next());
            Assert.Equal(369420633178190364ul, lcg.Next());
            Assert.Equal(17851908735567506196ul, lcg.Next());
            Assert.Equal(14998322079835412132ul, lcg.Next());
            Assert.Equal(5239223867101183108ul, lcg.Next());
            Assert.Equal(16808802568918681284ul, lcg.Next());
            Assert.Equal(2745174242918622788ul, lcg.Next());
            Assert.Equal(6801576654965309252ul, lcg.Next());
            Assert.Equal(1664223310916394308ul, lcg.Next());
            Assert.Equal(16395089757413359940ul, lcg.Next());
            Assert.Equal(700969116798794052ul, lcg.Next());
            Assert.Equal(8775323433165080900ul, lcg.Next());
            Assert.Equal(4578408129378986308ul, lcg.Next());
            Assert.Equal(6720761757790333252ul, lcg.Next());
            Assert.Equal(14641174864876988740ul, lcg.Next());
            Assert.Equal(11335382737800915268ul, lcg.Next());
            Assert.Equal(4053786157359950148ul, lcg.Next());
            Assert.Equal(18154670123740948804ul, lcg.Next());
            Assert.Equal(3128798039275333956ul, lcg.Next());
        }

        [Fact]
        public void Mt19937Correctness() {
            var mt = new Mt19937(1);

            Assert.Equal(1791095845u, mt.Next());
            Assert.Equal(4282876139u, mt.Next());
            Assert.Equal(3093770124u, mt.Next());
            Assert.Equal(4005303368u, mt.Next());
            Assert.Equal(491263u, mt.Next());
            Assert.Equal(550290313u, mt.Next());
            Assert.Equal(1298508491u, mt.Next());
            Assert.Equal(4290846341u, mt.Next());
            Assert.Equal(630311759u, mt.Next());
            Assert.Equal(1013994432u, mt.Next());
            Assert.Equal(396591248u, mt.Next());
            Assert.Equal(1703301249u, mt.Next());
            Assert.Equal(799981516u, mt.Next());
            Assert.Equal(1666063943u, mt.Next());
            Assert.Equal(1484172013u, mt.Next());
            Assert.Equal(2876537340u, mt.Next());

            mt.Seed(28);

            Assert.Equal(3131090177u, mt.Next());
            Assert.Equal(733679609u, mt.Next());
            Assert.Equal(2410505733u, mt.Next());
            Assert.Equal(1237120278u, mt.Next());
            Assert.Equal(536729588u, mt.Next());
            Assert.Equal(705436704u, mt.Next());
            Assert.Equal(1707646211u, mt.Next());
            Assert.Equal(1705394263u, mt.Next());
            Assert.Equal(3355693196u, mt.Next());
            Assert.Equal(1347680448u, mt.Next());
            Assert.Equal(2194698163u, mt.Next());
            Assert.Equal(92026263u, mt.Next());
            Assert.Equal(784661996u, mt.Next());
            Assert.Equal(2092773132u, mt.Next());
            Assert.Equal(3665809861u, mt.Next());
            Assert.Equal(4014284552u, mt.Next());
        }

        [Fact]
        public void Mt19937X64Correctness() {
            var mt = new Mt19937X64(159874236);

            Assert.Equal(1405254506197085464ul, mt.Next());
            Assert.Equal(16932505752461784433ul, mt.Next());
            Assert.Equal(13782451535796395450ul, mt.Next());
            Assert.Equal(6633361670665813930ul, mt.Next());
            Assert.Equal(865771470458265719ul, mt.Next());
            Assert.Equal(11437493971236514495ul, mt.Next());
            Assert.Equal(15272287304350458522ul, mt.Next());
            Assert.Equal(292022267716416239ul, mt.Next());
            Assert.Equal(13611972907223859775ul, mt.Next());
            Assert.Equal(17740997846040392661ul, mt.Next());
            Assert.Equal(3484281369859786888ul, mt.Next());
            Assert.Equal(2780545255184361367ul, mt.Next());
            Assert.Equal(15902985971886410388ul, mt.Next());
            Assert.Equal(8454127194480537068ul, mt.Next());
            Assert.Equal(17365409255493216969ul, mt.Next());
            Assert.Equal(1781705271995382934ul, mt.Next());

            mt.Seed(201905110010);

            Assert.Equal(13887965351299336683ul, mt.Next());
            Assert.Equal(6260977772888043112ul, mt.Next());
            Assert.Equal(17777210828109271680ul, mt.Next());
            Assert.Equal(12746483107584115753ul, mt.Next());
            Assert.Equal(14061982465212323104ul, mt.Next());
            Assert.Equal(10896062506565190268ul, mt.Next());
            Assert.Equal(3250480588076344384ul, mt.Next());
            Assert.Equal(13438093262240028680ul, mt.Next());
            Assert.Equal(3300645515724409725ul, mt.Next());
            Assert.Equal(10079906706463176284ul, mt.Next());
            Assert.Equal(4425874048456983770ul, mt.Next());
            Assert.Equal(10057518736645531232ul, mt.Next());
            Assert.Equal(1137767575129573759ul, mt.Next());
            Assert.Equal(3800149734625212216ul, mt.Next());
            Assert.Equal(14750666319745019266ul, mt.Next());
            Assert.Equal(10797686269382288876ul, mt.Next());
        }

        [Fact]
        public void DefaultRNGTests() {
            DefaultRNG.I32.Next();
            DefaultRNG.Coin.Next();
            DefaultRNG.Next();
            DefaultRNG.NextU64();
            DefaultRNG.NextDouble();
        }
    }
}
