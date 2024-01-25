using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GameBoard.GameBoard
{
    internal class Tile : Button
    {
        public int x {  get; set; }
        public int y { get; set; }
        public Team team = Team.Neutral;

        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
            Text = Team.Neutral.ToString();
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
