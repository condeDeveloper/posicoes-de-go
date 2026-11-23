using System.Numerics;

namespace Conde.PosicoesDeGo.Testes;

/// <summary>Os numeros do README, presos.</summary>
public class MedidasTestes
{
    [Fact]
    public void OsQuadradosAteSeteporSete()
    {
        Assert.Equal(new[]
        {
            BigInteger.Parse("1"),
            BigInteger.Parse("57"),
            BigInteger.Parse("12675"),
            BigInteger.Parse("24318165"),
            BigInteger.Parse("414295148741"),
            BigInteger.Parse("62567386502084877"),
            BigInteger.Parse("83677847847984287628595"),
        }, Medidas.Quadrados().Select(q => q.Legais));
    }

    [Fact]
    public void AFracaoLegalCaiDepoisDoDoisPorDois()
    {
        var f = Medidas.Quadrados().Select(q => Math.Round(100 * q.Fracao, 2)).ToArray();
        Assert.Equal(new[] { 33.33, 70.37, 64.40, 56.49, 48.90, 41.69, 34.97 }, f);
    }

    [Fact]
    public void AFaixaDeUmaLinha()
    {
        Assert.Equal(new long[] { 1, 5, 15, 41, 113, 313, 867 }, Medidas.Faixa().Select(x => (long)x));
    }

    [Fact]
    public void RetangulosQueOREADMECita()
    {
        Assert.Equal(1840058693, (long)Medidas.Ret(4, 5).Legais);
        Assert.Equal(BigInteger.Parse("21026744638200555"), Medidas.Ret(5, 7).Legais);
    }

    [Fact]
    public void OsEstadosDaFronteiraPorLargura()
    {
        Assert.Equal(new[] { 5, 27, 103, 415, 1859, 8645, 41227 },
            Enumerable.Range(1, 7).Select(c => Medidas.Ret(7, c).Estados));
    }
}
