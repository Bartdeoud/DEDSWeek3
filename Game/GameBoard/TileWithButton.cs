using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Game.GameBoard.GameBoard
{
    internal interface ITile
    {
        public int x { get; set; }
        public int y { get; set; }
        public Team team { get; set; }

        public ITile CreateTile(int x, int y, Team team = Team.Neutral);

        public void SetTeam(Team team);

        public void InfectWithTeam(Team team);
    }

    internal class TileToCalculate : ITile
    {
        public int x { get; set; }
        public int y { get; set; }
        public Team team { get; set; }

        public void SetTeam(Team team) { }
        public void InfectWithTeam(Team team) { }

        public ITile CreateTile(int x, int y, Team team = Team.Neutral)
        {
            this.x = x;
            this.y = y;
            this.team = team;
            return this;
        }
    }

    internal class TileWithButton : Button, ITile
    {
        public int x {  get; set; }
        public int y { get; set; }
        public Team team { get; set; }

        public ITile CreateTile(int x, int y, Team team = Team.Neutral)
        {
            this.x = x;
            this.y = y;
            this.team = team;
            Text = team.ToString();
            return this;
        }

        public void SetTeam(Team team)
        {
            switch (team)
            {
                case Team.Neutral:
                    BackColor = Color.White;
                    break;
                case Team.BaggySweater:
                    BackColor = Color.Green;
                    break;
                case Team.Hoodie:
                    BackColor = Color.Orange;
                    break;
            }
            Text = team.ToString();
            this.team = team;
        }

        public void InfectWithTeam(Team team)
        {
            if (this.team == Team.Neutral) return;
            SetTeam(team);
        }
    }

    public enum Team
    {
        Neutral = 0,
        Hoodie = 1,
        BaggySweater = 2
    }
}
