namespace Conde.PosicoesDeGo.Testes;

public class FronteiraTestes
{
    [Fact]
    public void UmaCasaSoSoPodeFicarVazia()
    {
        Assert.Equal(1, (int)new Fronteira(1).Contar(1));
    }

    [Fact]
    public void DuasCasasTemCincoPosicoes()
    {
        // vazio-vazio, preta-vazio, vazio-preta, branca-vazio, vazio-branca
        Assert.Equal(5, (int)new Fronteira(2).Contar(1));
        Assert.Equal(5, (int)new Fronteira(1).Contar(2));
    }

    [Theory]
    [InlineData(2, 2)]
    [InlineData(2, 5)]
    [InlineData(3, 3)]
    [InlineData(3, 4)]
    [InlineData(1, 7)]
    public void BateComOJuiz(int l, int c)
    {
        Assert.Equal(Juiz.Contar(l, c), (long)new Fronteira(c).Contar(l));
        Assert.Equal(Juiz.Contar(c, l), (long)new Fronteira(l).Contar(c));
    }

    [Fact]
    public void DeitadoEEmPeDaoOMesmoNumero()
    {
        for (int l = 1; l <= Medidas.MaiorLado; l++)
            for (int c = 1; c <= Medidas.MaiorLado; c++)
                Assert.Equal(Medidas.Ret(l, c).Legais, Medidas.Ret(c, l).Legais);
    }

    [Fact]
    public void LarguraForaDaFaixaNaoMonta()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Fronteira(10));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Fronteira(0));
    }
}
