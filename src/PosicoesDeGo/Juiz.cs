namespace Conde.PosicoesDeGo;

/// <summary>
/// O juiz gera TODAS as 3^(linhas*colunas) posicoes e confere cada uma pela
/// definicao: cada grupo de pedras ligadas da mesma cor precisa tocar uma casa
/// vazia. Os grupos saem por inundacao em mapa de bits. Vai ate 20 casas.
/// </summary>
public static class Juiz
{
    public const int MaiorCasas = 20;

    public static long Contar(int linhas, int colunas)
    {
        int n = linhas * colunas;
        if (n > MaiorCasas)
            throw new ArgumentOutOfRangeException(nameof(linhas), $"o juiz para em {MaiorCasas} casas");
        int cheio = (1 << n) - 1;
        var viz = new int[n];
        for (int i = 0; i < n; i++)
        {
            int l = i / colunas, c = i % colunas;
            if (l > 0) viz[i] |= 1 << (i - colunas);
            if (l < linhas - 1) viz[i] |= 1 << (i + colunas);
            if (c > 0) viz[i] |= 1 << (i - 1);
            if (c < colunas - 1) viz[i] |= 1 << (i + 1);
        }

        int Vizinhos(int m)
        {
            int r = 0;
            while (m != 0)
            {
                int i = System.Numerics.BitOperations.TrailingZeroCount(m);
                m &= m - 1;
                r |= viz[i];
            }
            return r;
        }

        bool CorViva(int pedras, int vazias)
        {
            int resto = pedras;
            while (resto != 0)
            {
                int g = resto & -resto;
                while (true)
                {
                    int maior = (g | Vizinhos(g)) & pedras;
                    if (maior == g)
                        break;
                    g = maior;
                }
                if ((Vizinhos(g) & vazias) == 0)
                    return false;
                resto &= ~g;
            }
            return true;
        }

        long legais = 0;
        // pretas: todo subconjunto; brancas: todo subconjunto do resto
        for (int pretas = 0; pretas <= cheio; pretas++)
        {
            int resto = cheio & ~pretas;
            int brancas = resto;
            while (true)
            {
                int vazias = resto & ~brancas;
                if (CorViva(pretas, vazias) && CorViva(brancas, vazias))
                    legais++;
                if (brancas == 0)
                    break;
                brancas = (brancas - 1) & resto;
            }
        }
        return legais;
    }
}
