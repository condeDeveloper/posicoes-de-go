# Posicoes de Go

Num tabuleiro de Go cada ponto fica vazio, com pedra preta ou com pedra
branca. Uma posicao e **legal** quando todo grupo de pedras ligadas da mesma
cor toca pelo menos um ponto vazio: um grupo sem liberdade ja teria sido
capturado. Das `3^(n*n)` posicoes de um tabuleiro n por n, quantas sao
legais?

```
1x2: vazio-vazio, preta-vazio, vazio-preta, branca-vazio, vazio-branca  -> 5
     (preta-preta, preta-branca... nao: algum grupo fica sem liberdade)
```

Este repositorio conta todas, ate o 9x9, por um caminho que nao gera
posicao nenhuma, e confere com outro que gera todas.

## Como rodar

```bash
dotnet test testes/PosicoesDeGo.Testes/PosicoesDeGo.Testes.csproj -c Release --filter "Category!=Pesado"
dotnet test testes/PosicoesDeGo.Testes/PosicoesDeGo.Testes.csproj -c Release --filter "Category=Pesado"
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- tabela
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- retangulos
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- juiz
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- fronteira
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- grandes
```

Os testes pesados (8x8, 9x9 e o juiz no 4x5) levam uns quinze minutos e rodam
num job proprio do CI.

## A fronteira e o juiz

- **A fronteira** anda casa por casa, em ordem de leitura, carregando so as
  ultimas `colunas` casas postas. Para cada uma o estado guarda a cor, a que
  grupo ela pertence (entre os grupos que ainda tocam a fronteira) e, por
  grupo, se ele ja tem liberdade. Um grupo sem liberdade que sai da fronteira
  nunca mais ganha uma, e o estado morre ali. Estados iguais se juntam somando
  quantas posicoes levam a eles, em inteiro grande.
- **O juiz** gera as `3^(casas)` posicoes, as pretas como todo subconjunto e
  as brancas como todo subconjunto do resto, acha cada grupo por inundacao em
  mapa de bits e confere a liberdade pela definicao. Ele bate com a fronteira
  em todo retangulo de ate 16 casas, e no 4x5 (20 casas, 3,5 bilhoes de
  posicoes) no teste pesado.

Ha uma terceira conferencia de graca: o retangulo de l linhas por c colunas e
contado com uma fronteira de largura c, e o de c por l com uma de largura l.
Os estados nao tem nada a ver um com o outro, e os dois numeros tem que ser
iguais. Sao iguais nos 28 retangulos ate 7x7.

## O que isto mediu

**O 9x9 tem 103.919.148.791.293.834.318.983.090.438.798.793.469 posicoes
legais, 23,44% das 3^81.** A fronteira chega a 1.007.881 estados e leva uns
sete minutos; o 8x8 tem 990.966.953.618.170.260.281.935.463.385 (28,86%) e
leva uns 40 segundos.

```
lado   posicoes legais                             fracao de 3^(n*n)
1x1    1                                           33.33%
2x2    57                                          70.37%
3x3    12675                                       64.40%
4x4    24318165                                    56.49%
5x5    414295148741                                48.90%
6x6    62567386502084877                           41.69%
7x7    83677847847984287628595                     34.97%
8x8    990966953618170260281935463385              28.86%
9x9    103919148791293834318983090438798793469     23.44%
```

**A fracao legal sobe do 1x1 para o 2x2 e depois so cai, uns 7 pontos por
lado.** No 1x1 a unica pedra nao tem vizinho e morre: so o vazio vale. No 2x2
70% das posicoes valem, e dai em diante cada lado a mais tira entre 5 e 8
pontos.

**A fronteira cresce quase 5 vezes a cada coluna, e e isso que para a tabela
no 9x9.** O maior numero de estados ao longo de 7 linhas, por largura:

```
largura   estados
1         5
2         27
3         103
4         415
5         1859
6         8645
7         41227
8         201975   (no 8x8)
9         1007881  (no 9x9)
```

De 7 para 8 colunas os estados multiplicam por 4,90, e de 8 para 9 por 4,99.

**Numa linha so, as posicoes legais vao 1, 5, 15, 41, 113, 313, 867**, e a
razao entre vizinhos ja esta em 2,77 na setima casa: cada casa a mais um
pouco menos que triplica as posicoes.

## O que as medidas me corrigiram

**Alarguei a fronteira de 7 para 9 colunas e rodei 8x8, 9x9 e mais tres
retangulos de uma vez.** O 9x9 sozinho levou 409 segundos e a rodada passou
dos dez minutos; os retangulos de 9 colunas que faltavam (7x9 contra 9x7)
ficaram de fora, e a conferencia deitado contra em pe vai ate 7x7.

## Os arquivos

```
src/PosicoesDeGo/Fronteira.cs   cor, grupo e liberdade das ultimas casas, somando estados iguais
src/PosicoesDeGo/Juiz.cs        todas as 3^(casas) posicoes, grupos por inundacao em bits
src/PosicoesDeGo/Medidas.cs     as tabelas deste README, guardadas
ferramentas/Medidor/            o executavel que imprime as tabelas
testes/                         21 testes, 3 deles pesados
```

21 testes, 3 deles pesados.
