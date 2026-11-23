using System.Diagnostics;
using Conde.PosicoesDeGo;

if (args.Length == 0)
{
    Console.WriteLine("medidor <tabela|retangulos|juiz|fronteira|grandes|tudo>");
    return 1;
}

switch (args[0])
{
    case "tabela": Tabela(); break;
    case "retangulos": Retangulos(); break;
    case "juiz": JuizContraFronteira(); break;
    case "fronteira": FronteiraTabela(); break;
    case "grandes": Grandes(); break;
    case "tudo": Tabela(); Retangulos(); JuizContraFronteira(); FronteiraTabela(); break;
    default:
        Console.WriteLine($"nao conheco '{args[0]}'");
        return 1;
}
return 0;

void Tabela()
{
    Console.WriteLine("lado   posicoes legais                       de todas as 3^(n*n)   fracao");
    foreach (var q in Medidas.Quadrados())
        Console.WriteLine($"{q.Linhas}x{q.Colunas,-4} {q.Legais,-37} {q.Todas,-21} {100 * q.Fracao:F2}%");
    Console.WriteLine();
}

void Retangulos()
{
    Console.WriteLine("linhas x colunas (linhas <= colunas); a fronteira conta pelas duas larguras");
    int iguais = 0, pares = 0;
    for (int l = 1; l <= Medidas.MaiorLado; l++)
        for (int c = l; c <= Medidas.MaiorLado; c++)
        {
            var a = Medidas.Ret(l, c);
            var b = Medidas.Ret(c, l);
            pares++;
            if (a.Legais == b.Legais)
                iguais++;
            Console.WriteLine($"{l}x{c,-4} {a.Legais}{(a.Legais == b.Legais ? "" : $"  DIVERGE do {c}x{l}: {b.Legais}")}");
        }
    Console.WriteLine($"deitado e em pe dao o mesmo numero em {iguais} de {pares} retangulos");
    Console.WriteLine("a faixa de uma linha: " + string.Join(", ", Medidas.Faixa()));
    Console.WriteLine();
}

void JuizContraFronteira()
{
    Console.WriteLine($"o juiz gera as 3^(casas) posicoes, ate {16} casas");
    Console.WriteLine("retangulo   juiz         fronteira    tempo do juiz");
    for (int l = 1; l <= Medidas.MaiorLado; l++)
        for (int c = l; c <= Medidas.MaiorLado; c++)
        {
            if (l * c > 16)
                continue;
            var sw = Stopwatch.StartNew();
            long j = Juiz.Contar(l, c);
            var f = Medidas.Ret(l, c).Legais;
            Console.WriteLine($"{l}x{c,-9} {j,-12} {f,-12} {sw.Elapsed.TotalSeconds:F1} s{(j == f ? "" : "  DIVERGE")}");
        }
    Console.WriteLine();
}

void FronteiraTabela()
{
    Console.WriteLine("largura   maior numero de estados na fronteira (contando 7 linhas)");
    for (int c = 1; c <= Medidas.MaiorLado; c++)
        Console.WriteLine($"{c,-9} {Medidas.Ret(Medidas.MaiorLado, c).Estados}");
    Console.WriteLine();
}

void Grandes()
{
    foreach (int n in new[] { 8, 9 })
    {
        var sw = Stopwatch.StartNew();
        var g = Medidas.Grande(n);
        Console.WriteLine($"{n}x{n}: {g.Legais} posicoes legais ({100 * g.Fracao:F2}% de 3^{n * n}), {g.Estados} estados, {sw.Elapsed.TotalSeconds:F0} s");
    }
}
