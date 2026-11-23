using System.Numerics;

namespace Conde.PosicoesDeGo.Testes;

public class PesadosTestes
{
    [Fact]
    [Trait("Category", "Pesado")]
    public void OOitoPorOito()
    {
        var g = Medidas.Grande(8);
        Assert.Equal(BigInteger.Parse("990966953618170260281935463385"), g.Legais);
        Assert.Equal(201975, g.Estados);
    }

    [Fact]
    [Trait("Category", "Pesado")]
    public void ONovePorNove()
    {
        var g = Medidas.Grande(9);
        Assert.Equal(BigInteger.Parse("103919148791293834318983090438798793469"), g.Legais);
        Assert.Equal(1007881, g.Estados);
    }

    [Fact]
    [Trait("Category", "Pesado")]
    public void OJuizNoQuatroPorCinco()
    {
        Assert.Equal(1840058693, Juiz.Contar(4, 5));
    }
}
