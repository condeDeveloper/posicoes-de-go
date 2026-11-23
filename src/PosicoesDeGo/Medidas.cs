using System.Numerics;

namespace Conde.PosicoesDeGo;

public sealed record Retangulo(int Linhas, int Colunas, BigInteger Legais, int Estados)
{
    public BigInteger Todas => BigInteger.Pow(3, Linhas * Colunas);
    public double Fracao => Math.Exp(BigInteger.Log(Legais) - BigInteger.Log(Todas));
}

/// <summary>As tabelas do README, calculadas uma vez e guardadas.</summary>
public static class Medidas
{
    public const int MaiorLado = 7;

    private static readonly Lazy<Retangulo[,]> _ret = new(() =>
    {
        var r = new Retangulo[MaiorLado + 1, MaiorLado + 1];
        for (int c = 1; c <= MaiorLado; c++)
            for (int l = 1; l <= MaiorLado; l++)
            {
                var f = new Fronteira(c);
                var v = f.Contar(l);
                r[l, c] = new Retangulo(l, c, v, f.MaiorEstados);
            }
        return r;
    });

    /// <summary>Linhas x colunas, contado com a fronteira da largura das colunas.</summary>
    public static Retangulo Ret(int linhas, int colunas) => _ret.Value[linhas, colunas];

    public static IEnumerable<Retangulo> Quadrados() => Enumerable.Range(1, MaiorLado).Select(n => Ret(n, n));

    /// <summary>A faixa de uma linha so: 1, 5, 15, 41, ...</summary>
    public static IEnumerable<BigInteger> Faixa() => Enumerable.Range(1, MaiorLado).Select(n => Ret(1, n).Legais);

    /// <summary>Os maiores: 8x8 e 9x9, minutos de fronteira.</summary>
    public static Retangulo Grande(int n)
    {
        var f = new Fronteira(n);
        var v = f.Contar(n);
        return new Retangulo(n, n, v, f.MaiorEstados);
    }
}
