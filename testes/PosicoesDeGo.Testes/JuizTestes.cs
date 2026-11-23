namespace Conde.PosicoesDeGo.Testes;

public class JuizTestes
{
    [Fact]
    public void NumTabuleiroDeUmaCasaSoOVazioVale()
    {
        Assert.Equal(1, Juiz.Contar(1, 1));
    }

    [Fact]
    public void ODoisPorDois()
    {
        Assert.Equal(57, Juiz.Contar(2, 2));
    }

    [Fact]
    public void OJuizConfereTodoRetanguloDeAte16Casas()
    {
        for (int l = 1; l <= 4; l++)
            for (int c = l; c <= 7; c++)
                if (l * c <= 16)
                    Assert.Equal((long)Medidas.Ret(l, c).Legais, Juiz.Contar(l, c));
    }

    [Fact]
    public void OJuizParaEmVinteCasas()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Juiz.Contar(3, 7));
    }
}
