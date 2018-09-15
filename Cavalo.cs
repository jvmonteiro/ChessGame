﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cavalo : Peca
{
	public Cavalo(bool jogadorCima, Jogador j) : base(jogadorCima, j)
	{

	}


    public override List<Movimento> ListaMovimentos(Casa[,] tab, int x, int y)
    {
        List<Movimento> movimentos = new List<Movimento>();
        //int quantMovimentos;

        //verifica cada lugar que o cavalo pode ir
        //verifica se pode mover para jogadorCima 2x e esquerda 1x
        if (x - 2 >= 0 && y - 1 >= 0)
        {
            movimentos.Add(new Movimento(tab[x - 2, y - 1], tab[x, y]));
        }
        //verifica se pode mover para jogadorCima 2x e direita 1x
        if (x - 2 >= 0 && y + 1 < tamTabuleiro)
        {
            movimentos.Add(new Movimento(tab[x - 2, y + 1], tab[x, y]));
        }
        //verifica se pode mover para jogadorCima 1x e esquerda 2x
        if (x - 1 >= 0 && y - 2 >= 0)
        {
            movimentos.Add(new Movimento(tab[x - 1, y - 2], tab[x, y]));
        }
        //verifica se pode mover para jogadorCima 1x e direita 2x
        if (x - 1 >= 0 && y + 2 < tamTabuleiro)
        {
            movimentos.Add(new Movimento(tab[x - 1, y + 2], tab[x, y]));
        }
        //verifica se pode mover para baixo 2x e esquerda 1x
        if (x + 2 < tamTabuleiro && y - 1 >= 0)
        {
            movimentos.Add(new Movimento(tab[x + 2, y - 1], tab[x, y]));
        }
        //verifica se pode mover para baixo 2x e direita 1x
        if (x + 2 < tamTabuleiro && y + 1 < tamTabuleiro)
        {
            movimentos.Add(new Movimento(tab[x + 2, y + 1], tab[x, y]));
        }
        //verifica se pode mover para baixo 1x e esquerda 2x
        if (x + 1 < tamTabuleiro && y - 2 >= 0)
        {
            movimentos.Add(new Movimento(tab[x + 1, y - 2], tab[x, y]));
        }
        //verifica se pode mover para baixo 1x e direita 2x
        if (x + 1 < tamTabuleiro && y + 2 < tamTabuleiro)
        {
            movimentos.Add(new Movimento(tab[x + 1, y - 2], tab[x, y]));
        }

        return movimentos;
    }

    /*public override GenuListaMovimentos(Tabuleiro tabuleiro, Casa origem)
	{
		var movimentos = new List<Movimento>();

		movimentos.AddRange(Movimento.SeguindoDirecao(tabuleiro, origem, -2, -1, passos = 1));
		movimentos.AddRange(Movimento.SeguindoDirecao(tabuleiro, origem, -2, +1, passos = 1));
		movimentos.AddRange(Movimento.SeguindoDirecao(tabuleiro, origem, -1, -2, passos = 1));
		movimentos.AddRange(Movimento.SeguindoDirecao(tabuleiro, origem, -1, +2, passos = 1));
		movimentos.AddRange(Movimento.SeguindoDirecao(tabuleiro, origem, +2, -1, passos = 1));
		movimentos.AddRange(Movimento.SeguindoDirecao(tabuleiro, origem, +2, +1, passos = 1));
		movimentos.AddRange(Movimento.SeguindoDirecao(tabuleiro, origem, +1, -2, passos = 1));
		movimentos.AddRange(Movimento.SeguindoDirecao(tabuleiro, origem, +1, +2, passos = 1));

		return movimentos;
	}*/
}
