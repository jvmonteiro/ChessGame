﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partida
{

	public Tabuleiro Tabuleiro { get; private set; }
	public Jogador Jogador1 { get; private set; }
	public Jogador Jogador2 { get; private set; }
	public IA jIA;
	public UITabuleiro UItab;
	public int Turno { get; private set; }
	public int TurnoDaUltimaCaptura { get; /*private*/ set; }
	public List<String> HistoricoDoTabuleiro { get; private set; }
	public bool fim { get; private set; }


	public Partida()
	{
		Tabuleiro = new Tabuleiro();
		Jogador1 = new Jogador('b', false);
		Jogador2 = new Jogador('p', true);
        Tabuleiro.partida = this;
        jIA = new IA(Jogador2);
		IniciarPartida(Jogador1, Jogador2);
		this.fim = false;
	}

	// TODO: avaliar necessidade desta função
    public int retornaTurno()
    {
        return Turno;
    }
    public void refazReferencias() {
        foreach (Peca p in Jogador1.conjuntoPecas)
        {
            p.CasaAtual.PecaAtual = p;
        }


        foreach (Peca p in Jogador2.conjuntoPecas)
        {
            p.CasaAtual.PecaAtual = p;
        }
    }
	public void PassarAVez()
	{
		if (VerificaEmpateObrigatorio())   
		{
			Debug.Log("Empate!");
			this.fim = true;
		}
			
		if (VerificaVitoria())
		{
			Debug.Log("Vitória!");
			this.fim = true;
		}
			
		if (VerificaEmpateOpcional())
			Debug.Log("Um empate pode ser pedido!");

		Turno++;
		Debug.Log("Passando turno...");
		//this.fim = true; usado nos testes
 // removendo temporariamente para teste de verificar vitoria
		/*
		if (JogadorDaVez() == Jogador2) // TODO: if (jogador da vez é IA)
		{
			//Tabuleiro.PrintaTabuleiro();
			Movimento m = jIA.minmax(3, -222222222, 2222222222, true, Tabuleiro).movimento;
			//Debug.Log(m.origem.PosX + "   " + m.origem.PosY);
			UItab.TryMove(m.origem.uiC, m.destino.uiC);
			RefazReferencias();
			//Tabuleiro.PrintaTabuleiro();
		}
*/
		// Debug.Log(Turno);

		RegistrarEstadoDoTabuleiro();
	}


	private Jogador JogadorDaVez()
	{
		if (Turno == 1) return Jogador1;
		else return Jogador2;
	}

	private void VerificaVitoria()
	{
		// Há movimentos possíveis?
		foreach (Peca peca in JogadorDaVez().inimigo.conjuntoPecas)
		{
			if (peca.ListaMovimentos(Tabuleiro, peca.CasaAtual).Count > 0)
			{
				// Sim
				return;
			}
		}

		// O rei está em xeque?
		if (JogadorDaVez().inimigo.EmXeque())
		{
			Debug.Log("Xeque-mate!");
		}

		Debug.Log("Fim");
	}

	void IniciarPartida(Jogador j1, Jogador j2)
	{
		j1.inimigo = j2;
		j2.inimigo = j1;
        if (j1.Cor == 'b')
        {
            Turno = 1;
        }
        else
        {
            Turno = 2;
        }
		Tabuleiro.InserePecasNaPosicaoInicial(this);
		Tabuleiro.PrintaTabuleiro();
	}
	
	public bool VerificaVitoria()
	{
		// para ocorrer a vitoria é preciso que o rei adversario esteja em uma posição em que seja impossivel escapar 
		// ou seja xeque mate:
		// ocorre quando: 
		// o rei não pode se movimentar para nenhuma casa sem que sofra um ataque(não há movimentos validos)
		// a peça(s) que esta(m) atacando não podem ser capturadas pelo rei
		//nenhuma peça pode proteger o rei (entrar na frente do atacante)
		int numameacas = 0;
		int numprote = 0;
		// O rei está em xeque?	
		if (JogadorDaVez().inimigo.EmXeque())
		{
			foreach (Peca peca in JogadorDaVez().inimigo.conjuntoPecas) // loop para achar o rei no conjunto de pecas inimigas
			{


				if (peca is Rei)
				{
					// agora deve-se verificar se o rei inimigo possui algum movimento valido (ou seja nenhum movimento o deixa em xeque ainda)
					if (peca.ListaMovimentos().Count > 0)
					{

						return false;
					}
					//agora deve-se verificar se as peças que ameaçam o rei estão a mais de uma casa de distancia dele(nao podem ser capturadas pelo rei)
					foreach (Peca pa in JogadorDaVez().conjuntoPecas)
					{
						if (pa.CasaAtual != null)
						{
							List<Movimento> movimentos = pa.ListaMovimentos(false);
							if (movimentos.Count > 0)
							{
								foreach (Movimento mov in movimentos)
								{
									if (mov.destino.PecaAtual is Rei && mov.tipo != Movimento.Tipo.SemCaptura) // se alguma das possibilidades de movimento envolve captura em direção ao rei
									{

										if ((peca.PosY == pa.PosY && Math.Abs(peca.PosX - pa.PosX) == 1) || (peca.PosX == pa.PosX && Math.Abs(peca.PosY - pa.PosY) == 1) || ((Math.Abs(peca.PosX - pa.PosX) == 1) && (Math.Abs(peca.PosY - pa.PosY) == 1)))
										{
											return false;
										}
										numameacas++;
									}
								}
							}
						}
					}
					// agora deve-se verificar se existe alguma peça para proteger o rei(pode ser movida para ao lado do rei)
					// (acho que deve ter alguma forma de deixar esse código melhor sem ter que chamar outro foreach aqui, mas depois eu faço isso) 
					//TODO*
					foreach (Peca prot in JogadorDaVez().inimigo.conjuntoPecas)
					{
						if (!(prot is Rei))
						{
							if (prot.CasaAtual != null)
							{
								List<Movimento> movimentosprot = prot.ListaMovimentos(false);
								foreach (Movimento mov in movimentosprot)
								{
									if ((peca.PosY == mov.destino.PosY && Math.Abs(peca.PosX - mov.destino.PosX) == 1) || (peca.PosX == mov.destino.PosX && Math.Abs(peca.PosY - mov.destino.PosY) == 1) || ((Math.Abs(peca.PosX - mov.destino.PosX) == 1) && (Math.Abs(peca.PosY - mov.destino.PosY) == 1)))
									{
										numprote++;
									}
								}
							}
						}
					}
					if (numprote == numameacas)
					{
						return false;
					}




					break; // evitar percorrer o resto do conjunto de pecas atoa

				}


			}



			return true;

		}


		return false;
	}

	// definem o final do jogo (TODO nos sonhos: verificar mais impossibilidade de xeque (é muito complicado de calcular, acho que podemos ignorar))
	private bool VerificaEmpateObrigatorio()
	{
		var pecasAliadas = JogadorDaVez().conjuntoPecas;
		var pecasInimigas = JogadorDaVez().inimigo.conjuntoPecas;

		// Afogamento
		bool semMovimentos = true;

		foreach (Peca peca in pecasInimigas)
		{
			if (peca.ListaMovimentos().Count > 0)
			{
				semMovimentos = false;
				break;
			}
		}

		if (semMovimentos && !JogadorDaVez().inimigo.EmXeque())
		{
			Debug.Log("Empate! Rei afogado.");
			return true;
		}


		// Impossibilidade de xeque por falta de peças (supõe que cada jogador possui um Rei)
		if (pecasInimigas.Count == 1)
		{
			if (pecasAliadas.Count == 1) // Apenas Reis
			{
				Debug.Log("Empate! Xeque impossível.");
				return true;
			}

			if (pecasAliadas.Count == 2)
			{
				foreach (var peca in pecasAliadas)
				{
					if (peca is Bispo || peca is Cavalo) // Apenas Rei e Bispo ou Rei e Cavalo
					{
						Debug.Log("Empate! Xeque impossível.");
						return true;
					}
				}
			}
		}
		else if (pecasInimigas.Count == 2 && pecasAliadas.Count == 2)
		{
			foreach (Bispo bispo in pecasAliadas)
			{
				foreach (Bispo bispoInimigo in pecasInimigas)
				{
					if (bispo.CasaAtual.cor == bispoInimigo.CasaAtual.cor) // Apenas Rei e Bispo dos dois lados, bispos em casa de mesma cor
					{
						Debug.Log("Empate! Xeque impossível.");
						return true;
					}
				}
			}
		}


		return false;
	}

	private bool VerificaEmpateOpcional()
	{
		// 50 ou mais turnos desde a última captura ou movimento de peão

		if (Turno >= 50)
		{
			if (TurnoDaUltimaCaptura + 50 <= Turno)
				return true;
			
			foreach (Peao peao in Jogador1.conjuntoPecas.Union(Jogador2.conjuntoPecas))
			{
				if (peao.UltimoTurnoMovido + 50 <= Turno)
					return true;
			}
		}


		// Repetição do mesmo estado do tabuleiro três vezes. Empate pode ser pedido apenas logo que a repetição acontece.

		String estado = EstadoAtual();

		int contagem = HistoricoDoTabuleiro.Where(x => x.Equals(estado)).Count();

		if (contagem >= 2) // TODO: dependendo da ordem de chamada desta função, a contagem deve ser >= 2 ou 3.
			return true;


		return false;
	}


	public Jogador JogadorDeCima()
	{
		if (Jogador1.jogadorCima && !Jogador2.jogadorCima)
			return Jogador1;
		else if (Jogador2.jogadorCima && !Jogador1.jogadorCima)
			return Jogador2;
		else
		{
			Debug.LogError("Exatamente um jogador deve ficar na parte de cima do tabuleiro.");
			return null;
		}
	}

	public Jogador JogadorDeBaixo()
	{
		Jogador oOutro = JogadorDeCima();
		if (oOutro == Jogador1)
			return Jogador2;
		else if (oOutro == Jogador2)
			return Jogador1;
		else
			return null;
	}


}