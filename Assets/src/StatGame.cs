using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class StatGame
{
	// class to store the statistics of a game
	
	public Stopwatch chronometre;
	public Dictionary <string, int> score =  new Dictionary <string, int> ();
	public static List <StatPerso> listPerso=  new List <StatPerso>();
	public string css="<style type=\"text/css\"><!-- body {font-family: Verdana, Helvetica, Arial, sans-serif;font-size: 14px;color:#C0C0C0;background-color: #000000;margin-left:10%;margin-right:10%;padding: 20px;Line-Height: 2;}h1,h2{background-color: #0C0C0D;color:#FFFFFF;text-align: center;}img{vertical-align:middle;}--></style>";
	public string img1="<img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAACXBIWXMAACNvAAAjbwE1/Af7AAAAB3RJTUUH3wMcEAQ3siRV1gAAAUpJREFUOMutVLFuwyAQPcAiC13IYGWLMmX1aP+B//8vrC6tVSUh4V0HY5caSFMlT2IB7nF373HCe88UIISgZ1ERESml6BUAMBHmwMx3g0vVVH8R7fd7N47jl1IKRPQ2DIOO7yXEAJiIeLrzA631bd7v+37kiWFeiO/O+wC4ymUWXl0aq7XmtGKxxDDzkqlcl7zZbPyjIiilkkYnhM459ahAAMRdwsPh4HKB3peT3m63t6woobmfUeMTsYgImfPzLAwAXpec1GatfY/FyFm2mGFd15f49d1u97G2xnoZY65xhgIASykTo0op2Xsvwp6PbZQTTAhBAFKVQ5+ImUWwBkpkOSSEXdedY7MCkKVga+0t56VEzePxeJJSotQ3IoK19pr7er8I1/+5bdtLyOJMRCdjzLVpGpf7x4ko/xldpRFWnIfPTO5qZn4VvgGUvDNUjlPwkQAAAABJRU5ErkJggg==\">";
	public string img2="<img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAACXBIWXMAACNvAAAjbwE1/Af7AAAAB3RJTUUH3wMcEBAToIlmUgAAAd1JREFUOMutlLFvEzEUhz8/22lLm4tKkWiCmBjaCVGmLmGAgQ2JoQP/CAsMIDEjBmbUAYmlGwsDG2NZ24GBoUKVqFBpIqGUNGcz2HfnpNeCGixZd35+73u/Z787lee5Jw6lFNMOA6BXn14o+PO7N9xa+Y4IeEDNOWQaNctLvxieaLyHoraLAUXBjKE/mMWj8D7d+tu5GYFZC80ZWLwEV5vQyaCdsbW9xjC35F4omMo552XlSRXc0GA02GJKWDeS98JuNXvPHzN0huutH9iFUbgUrsyDNdFJKlhtgmiP0JtvX/Hy7iarJ/usLxQKN14HoDkD1pDTqsfgQYx71I0K21misCjNBJCeKLdUnEJj0qIP6bTKjRefNrjdHXD5muLDVsazh+/Hg0v1NapLYLtVlnjnwYDeARx89bTmh9Bp1pY3ZjMy/qWEksPmzias3fd82dYc931Ilp6bnNVmKi05K8+l17f0jnKWbzi+7SlYnEsC/DkN66s+vOcFJVP/F/joXVB4uAtiwlQGRIOyoIt18hTzD3+bw5364BRSJEmTSrHWMdFSBP7crRyVCa2nbOVYqrcRbCbscdKNwN9H59SgEoit1Io9LaAEpnc4eZfKgxuBGoE7nrzTGn/nnOc/jj8GVHNLuih6TwAAAABJRU5ErkJggg==\">";

	// DATA :
	public string map;
	public string duration;
	public int nbPlayers, nbAppleBox, nbWeaponBox;


	public StatGame(int n)
	{
		nbPlayers = n;
		initStatPerso (n);
		map = Game.translateNameMapsRevert[Application.loadedLevelName];
		// args : string m, int nbPlayers
	}

	public void startGame()
	{
		chronometre = new Stopwatch();
		chronometre.Start();
	}

	public void pauseGame(bool activ)
	{
		if (activ)
			chronometre.Stop ();
		else
			chronometre.Start();
	}

	public void endGame()
	{
		chronometre.Stop();
		TimeSpan t = new TimeSpan ();
		t = chronometre.Elapsed;
		if(t.Hours == 0)
			duration = String.Format("{0:00}min, {1:00}s", t.Minutes, t.Seconds);
		else
			duration = String.Format("{0:00}H, {1:00}min, {2:00}s", t.Hours, t.Minutes, t.Seconds);

	}

	public void initStatPerso(int nbPlayers)
	{
		for(int i=0;i<nbPlayers;i++)
		{
			listPerso.Add(new StatPerso(i+1));
		}
	}

	public StatPerso getStatPerso(int numeroPlayer)
	{
		foreach(StatPerso p in listPerso)
		{
			if(p.numeroPlayer==numeroPlayer)
				return p;
		}
		UnityEngine.Debug.Log ("erreur: il n'y a pas de StatPerso numero " + numeroPlayer);
		return new StatPerso(-1);
	}

	public string[] getReport(bool export)
	{
		List <string> lines=  new List <string>();

		if(export) lines.Add("<html><head>"+css+"<meta charset=\"UTF-8\"><title>CRASH TEAM RACING II</title></head><body><center><h1>");
		lines.Add("CRASH TEAM RACING II");
		if(export) lines.Add("</h1>");
		lines.Add("Partie jouée le "+DateTime.Now.ToString("dd MMMM yyyy à H:mm"));
		if(export) lines.Add("</center></br>");
		lines.Add("");
		if(export) lines.Add("</br>"+img1);
		lines.Add("Durée : "+duration);
		if(export) lines.Add("</br>"+img2);
		lines.Add("Map : "+map);
		if(export) lines.Add("</br>");
		lines.Add("Nombre de Joueurs : "+nbPlayers);
		if(export) lines.Add("</br>");
		lines.Add("Caisses d'Armes : "+nbWeaponBox+" caisse cassées");
		if(export) lines.Add("</br>");
		lines.Add("Caisses de Pommes : "+nbAppleBox+" caisse cassées");
		if(export) lines.Add("</br>");
		lines.Add("Scores :");
		if(export) lines.Add("</br>");
		foreach (string key in score.Keys)
		{
			if(export) lines.Add("&nbsp;&nbsp;&nbsp;");
			lines.Add("\t"+key+" : "+score[key]+" Pts");
			if(export) lines.Add("</br>");
		}
		if(export) lines.Add("</br>");
		foreach (StatPerso p in listPerso)
		{
			lines.Add("");
			if(export) lines.Add("</br><h2>");
			lines.Add("JOUEUR "+p.numeroPlayer);
			if(export) lines.Add("</h2></br>");
			lines.Add("\t Score : "+p.score+" Pts");
			if(export) lines.Add("</br>");
			lines.Add("\t Points Marqués : "+p.PtsMarques.Count+" Pts");
			if(export) lines.Add("</br>");
			List<int> traites = new List<int>();
			foreach(int j in p.PtsMarques)
			{
				if(j != p.numeroPlayer && !traites.Contains(j))
				{
					if(export) lines.Add("&nbsp;&nbsp;&nbsp;");
					lines.Add("\t\t Contre Joueur "+j+" : "+compte(p.PtsMarques,j)+" Pts");
					if(export) lines.Add("</br>");
					traites.Add(j);
				}
			}
			lines.Add("\t Points Perdus (suicide) : "+p.nbSuicides+" Pts");
			if(export) lines.Add("</br>");
			if(p.PtsDonnes.Count==0)
			{
				lines.Add("\t Points Donnés (mort) : "+p.PtsDonnes.Count+" Pts");
				if(export) lines.Add("</br>");
			}
			else
			{
				lines.Add("\t Points Donnés (mort) : "+p.PtsDonnes.Count+" Pts (pire ennemi : Joueur "+most(p.PtsDonnes)+")");
				if(export) lines.Add("</br>");
			}
			if (p.getTotalWeapons()>0)
			{
				lines.Add("\t Détail des Armes obtenues : ("+p.getTotalWeapons()+" au total)");
				if(export) lines.Add("</br>");
				p.printWeapons(lines,export);
			}
			if(export) lines.Add("</br>");
		}
		if(export) lines.Add("</body></html>");
		string[] linesArray = lines.ToArray();
		if(export)
		{
			string date = DateTime.Now.ToString("ddMMyyHmm");
			System.IO.File.WriteAllLines(@"C:\Users\Jonathan\Desktop\CTR2\CTR2_"+date+".htm", linesArray);
		}
		return linesArray;
	}

	public int compte(List <int> liste, int element)
	{
		int occurence=0;
		foreach(int e in liste)
		{
			if(e==element)
				occurence++;
		}
		return occurence;
	}

	public int most(List <int> liste)
	{
		int deathByJ1 = compte (liste,1);
		int deathByJ2 = compte (liste,2);
		int deathByJ3 = compte (liste,3);
		int deathByJ4 = compte (liste,4);
		int maxDeath = System.Math.Max (deathByJ1, System.Math.Max(deathByJ2, System.Math.Max(deathByJ3, deathByJ4)));
		if (maxDeath == deathByJ1)
			return 1;
		else if (maxDeath == deathByJ2)
			return 2;
		else if (maxDeath == deathByJ3)
			return 3;
		else
			return 4;
	}

}



public class StatPerso
{
	// class to store the statistics of a character

	public int numeroPlayer, score, nbSuicides;

	public int nbBomb, nbMissile, nbTNT, nbNitro, nbFlacon, nbAku, nbAccelerator, nbShield;

	// Cette liste représente les points marqués par le joueur.
	// les nombres qu'elle contient sont les numéro des énemis tués.
	// ex [2,2,2,3] signifie : 3 points contre J2, 1 point contre J3
	public List <int> PtsMarques=  new List <int>();

	// Cette liste représente les points donnés par le joueur (points marqués par les énemis contre lui).
	// les nombres qu'elle contient sont les numéro des énemis qui l'ont tué.
	// ex [2,3,3] signifie : J2 l'a tué 1 fois, J3 deux fois.
	public List <int> PtsDonnes=  new List <int>();

	public StatPerso(int n)
	{
		numeroPlayer = n;
	}

	public void printWeapons(List <string> l, bool export)
	{
		if(nbBomb>0)
		{
			if(export) l.Add("&nbsp;&nbsp;&nbsp;");
			l.Add("\t\tBombes : "+ nbBomb);
			if(export) l.Add("</br>");
		}
		if(nbMissile>0)
		{
			if(export) l.Add("&nbsp;&nbsp;&nbsp;");
			l.Add("\t\tMissiles : "+ nbMissile);
			if(export) l.Add("</br>");
		}
		if(nbTNT>0)
		{
			if(export) l.Add("&nbsp;&nbsp;&nbsp;");
			l.Add("\t\tTNT : "+ nbTNT);
			if(export) l.Add("</br>");
		}
		if(nbNitro>0)
		{
			if(export) l.Add("&nbsp;&nbsp;&nbsp;");
			l.Add("\t\tNitros : "+ nbNitro);
			if(export) l.Add("</br>");
		}
		if(nbFlacon>0)
		{
			if(export) l.Add("&nbsp;&nbsp;&nbsp;");
			l.Add("\t\tFlacons : "+ nbFlacon);
			if(export) l.Add("</br>");
		}
		if(nbAku>0)
		{
			if(export) l.Add("&nbsp;&nbsp;&nbsp;");
			l.Add("\t\tAku-Aku : "+ nbAku);
			if(export) l.Add("</br>");
		}
		if(nbAccelerator>0)
		{
			if(export) l.Add("&nbsp;&nbsp;&nbsp;");
			l.Add("\t\tTurbos : "+ nbAccelerator);
			if(export) l.Add("</br>");
		}
		if(nbShield>0)
		{
			if(export) l.Add("&nbsp;&nbsp;&nbsp;");
			l.Add("\t\tBoucliers : "+ nbShield);
			if(export) l.Add("</br>");
		}
	}

	public int getTotalWeapons()
	{
		return nbShield+nbAccelerator+nbAku+nbFlacon+nbNitro+nbTNT+nbMissile+nbBomb;
	}

	public void addWeapon(string w)
	{
		switch (w)
		{
		case "greenShield":
			nbShield++;
			break;
		case "blueShield":
			nbShield++;
			break;
		case "greenBeaker":
			nbFlacon++;
			break;
		case "redBeaker":
			nbFlacon++;
			break;
		case "bomb":
			nbBomb++;
			break;
		case "triple_bomb":
			nbBomb+=3;
			break;
		case "missile":
			nbMissile++;
			break;
		case "triple_missile":
			nbMissile+=3;
			break;
		case "Aku-Aku":
			nbAku++;
			break;
		case "TNT":
			nbTNT++;
			break;
		case "nitro":
			nbNitro++;
			break;
		case "turbo":
			nbAccelerator++;
			break;
		default:
			break;
		}
	}

}



