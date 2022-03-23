using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.GameModes;

namespace Rounds_Rogelike.GameModes
{
    public class RWOFHandler : GameModeHandler<GM_RWOF>
    {
        public RWOFHandler() : base("Adventure")
        {
            Settings = new GameSettings()
            {
                { "pointsToWinRound", 1},
                { "roundsToWinGame", 1},
                { "playersRequiredToStartGame", 1 },
                { "SinglePlayerGame", true},
            };
            
        }

        public override GameSettings Settings { get; protected set; }

        public override string Name { get { return "Adventure"; } }

        public override bool AllowTeams => false;

        public override TeamScore GetTeamScore(int teamID)
        {
            return new TeamScore(0,0);
        }

        public override void PlayerDied(Player killedPlayer, int playersAlive)
        {
            GameMode.PlayerDied(killedPlayer, playersAlive);
        }

        public override void PlayerJoined(Player player)
        {
            UnityEngine.Debug.Log("Joined");
            GameMode.PlayerJoined(player);
        }

        public override void ResetGame()
        {
            throw new NotImplementedException();
        }

        public override void SetActive(bool active)
        {
            GameMode.gameObject.SetActive(active);
        }

        public override void SetTeamScore(int teamID, TeamScore score)
        {

        }

        public override void StartGame()
        {
            GameMode.StartGame();
        }
    }
}
