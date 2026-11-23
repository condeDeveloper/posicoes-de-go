using System.Numerics;

namespace Conde.PosicoesDeGo;

/// <summary>
/// Conta as posicoes legais de Go num tabuleiro de linhas x colunas sem gerar
/// posicao nenhuma: anda casa por casa em ordem de leitura carregando so a
/// FRONTEIRA, as ultimas `colunas` casas postas. Para cada casa da fronteira o
/// estado guarda a cor, a que grupo ela pertence (entre os grupos que tocam a
/// fronteira) e, por grupo, se ele ja tem liberdade.
///
/// Um grupo sem liberdade que sai da fronteira nao ganha liberdade nunca mais:
/// o estado morre ali. No fim, todo grupo que sobrou tem que ter liberdade.
/// </summary>
public sealed class Fronteira
{
    public const int MaiorLargura = 9;
    private const int Fora = 3;

    private readonly int _colunas;

    /// <summary>O maior numero de estados que a fronteira carregou de uma casa para a outra.</summary>
    public int MaiorEstados { get; private set; }

    public Fronteira(int colunas)
    {
        if (colunas < 1 || colunas > MaiorLargura)
            throw new ArgumentOutOfRangeException(nameof(colunas));
        _colunas = colunas;
    }

    public BigInteger Contar(int linhas)
    {
        int n = _colunas;
        var cor = new int[n];
        var grupo = new int[n];
        var lib = new bool[n + 1];
        Array.Fill(cor, Fora);
        var estados = new Dictionary<ulong, BigInteger> { [Codificar(cor, grupo, lib)] = BigInteger.One };
        MaiorEstados = 1;

        for (int r = 0; r < linhas; r++)
            for (int c = 0; c < n; c++)
            {
                var novos = new Dictionary<ulong, BigInteger>();
                foreach (var (chave, quantos) in estados)
                    for (int x = 0; x < 3; x++)
                    {
                        Decodificar(chave, cor, grupo, lib);
                        if (Por(c, x, cor, grupo, lib))
                        {
                            ulong k = Codificar(cor, grupo, lib);
                            novos[k] = novos.TryGetValue(k, out var v) ? v + quantos : quantos;
                        }
                    }
                estados = novos;
                MaiorEstados = Math.Max(MaiorEstados, estados.Count);
            }

        BigInteger total = BigInteger.Zero;
        foreach (var (chave, quantos) in estados)
        {
            Decodificar(chave, cor, grupo, lib);
            bool ok = true;
            for (int j = 0; j < n; j++)
                if (cor[j] is 1 or 2 && !lib[grupo[j]])
                    ok = false;
            if (ok)
                total += quantos;
        }
        return total;
    }

    /// <summary>Poe a cor x (0 vazio, 1 preta, 2 branca) na coluna c. Falso se um grupo morreu.</summary>
    private bool Por(int c, int x, int[] cor, int[] grupo, bool[] lib)
    {
        int n = _colunas;
        const int novo = MaiorLargura; // rotulo provisorio do grupo da pedra nova
        int esq = c - 1;
        bool temEsq = c > 0;
        int corCima = cor[c], grupoCima = grupo[c];

        bool libNovo = false;
        if (x == 0)
        {
            if (corCima is 1 or 2)
                lib[grupoCima] = true;
            if (temEsq && cor[esq] is 1 or 2)
                lib[grupo[esq]] = true;
        }
        else
        {
            foreach (int viz in temEsq ? new[] { c, esq } : new[] { c })
            {
                if (cor[viz] == 0)
                    libNovo = true;
                else if (cor[viz] == x)
                {
                    int g = grupo[viz];
                    if (g == novo)
                        continue;
                    libNovo |= lib[g];
                    for (int j = 0; j < n; j++)
                        if (cor[j] == x && grupo[j] == g)
                            grupo[j] = novo;
                }
            }
        }

        // a casa de cima sai da fronteira: se era pedra de um grupo sem
        // liberdade que nao fica em mais nenhuma casa, esse grupo morreu
        if (corCima is 1 or 2 && !(x == corCima))
        {
            bool fica = false;
            for (int j = 0; j < n; j++)
                if (j != c && cor[j] == corCima && grupo[j] == grupoCima)
                    fica = true;
            if (!fica && !lib[grupoCima])
                return false;
        }

        cor[c] = x;
        if (x != 0)
            grupo[c] = novo;

        // renomeia os grupos na ordem em que aparecem
        var mapa = new int[MaiorLargura + 1];
        Array.Fill(mapa, -1);
        var libNova = new bool[n + 1];
        int prox = 0;
        for (int j = 0; j < n; j++)
        {
            if (cor[j] is not (1 or 2))
            {
                grupo[j] = 0;
                continue;
            }
            int g = grupo[j];
            if (mapa[g] < 0)
            {
                mapa[g] = prox++;
                libNova[mapa[g]] = g == novo ? libNovo : lib[g];
            }
            grupo[j] = mapa[g];
        }
        Array.Clear(lib);
        Array.Copy(libNova, lib, n + 1);
        return true;
    }

    private ulong Codificar(int[] cor, int[] grupo, bool[] lib)
    {
        ulong k = 0;
        for (int j = 0; j < _colunas; j++)
            k |= (ulong)(cor[j] | grupo[j] << 2) << (6 * j);
        for (int g = 0; g < _colunas; g++)
            if (lib[g])
                k |= 1UL << (6 * MaiorLargura + g);
        return k;
    }

    private void Decodificar(ulong k, int[] cor, int[] grupo, bool[] lib)
    {
        for (int j = 0; j < _colunas; j++)
        {
            int v = (int)(k >> (6 * j) & 63);
            cor[j] = v & 3;
            grupo[j] = v >> 2;
        }
        Array.Clear(lib);
        for (int g = 0; g < _colunas; g++)
            lib[g] = (k >> (6 * MaiorLargura + g) & 1) == 1;
    }
}
