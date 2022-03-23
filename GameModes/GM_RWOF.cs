using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InControl;
using ItemShops.Utils;
using ModdingUtils.AIMinion;
using Photon.Pun;
using Rounds_Rogelike.Cards;
using Rounds_Rogelike.Extentions;
using Rounds_Rogelike.Floors;
using UnboundLib;
using UnboundLib.Extensions;
using UnboundLib.GameModes;
using UnityEngine;

namespace Rounds_Rogelike.GameModes
{
    public class GM_RWOF : MonoBehaviour
    {
        public static GM_RWOF instance;

        protected int playersNeededToStart = 1;

        private void Awake()
        {
            GM_RWOF.instance = this;
        }

        public void PlayerDied(Player killedPlayer, int playersAlive)
        {
            if (killedPlayer.playerID == 0)
            {
                UnityEngine.Debug.Log("Died on Floor" + floorHandler.floor);
                UnityEngine.Debug.Log("Clossing game");
               Application.Quit();
                //Game Over
            }
            ItemShops.Extensions.PlayerExtension.GetAdditionalData(PlayerManager.instance.players.Find(p => p.playerID == 0))
                .bankAccount.Deposit(Main.Gold, Mathf.Clamp(new System.Random().Next(killedPlayer.data.stats.GetAditionalData().gold_drop_min,
                    killedPlayer.data.stats.GetAditionalData().gold_drop_max),0,int.MaxValue));


            PlayerManager.instance.RemovePlayer(killedPlayer);
            if (playersAlive == 1)
            {
                floorHandler.floor++;
                TimeHandler.instance.DoSlowDown();
                base.StartCoroutine(this.NextRound());
                //next event
            }
        }

        private IEnumerator NextRound()
        {
            yield return floorHandler.getNextFloor(this);
            yield return new WaitForSecondsRealtime(0.25f);
            UnityEngine.Debug.Log(ItemShops.Extensions.PlayerExtension.GetAdditionalData(PlayerManager.instance.players.Find(p => p.playerID == 0))
                .bankAccount.Money[Main.Gold]);
            GameManager.instance.battleOngoing = false;
            TimeHandler.instance.DoSlowDown();
            PlayerManager.instance.SetPlayersSimulated(false);
            PlayerManager.instance.InvokeMethod("SetPlayersVisible", false);
            UnityEngine.Debug.Log(floor);
            if (floor.isShop) {
                floor.Shop.Show(PlayerManager.instance.players.Find(p => p.playerID == 0));
                yield return new WaitForSecondsRealtime(0.25f);
                floor.Shop.Hide(); 
                yield return new WaitForSecondsRealtime(0.25f);
                floor.Shop.Show(PlayerManager.instance.players.Find(p => p.playerID == 0));
                yield return new WaitForSecondsRealtime(2f);
                yield return new WaitUntil(() => !ShopManager.instance.PlayerIsInShop(PlayerManager.instance.players.Find(p => p.playerID == 0)));
                floorHandler.floor++;
                TimeHandler.instance.DoSlowDown();
                yield return new WaitForSecondsRealtime(2f);
                base.StartCoroutine(this.NextRound());
            }
            else
            {
                GameManager.instance.battleOngoing = false;
                TimeHandler.instance.DoSlowDown();
                PlayerManager.instance.SetPlayersSimulated(false);
                PlayerManager.instance.InvokeMethod("SetPlayersVisible", false);
                MapManager.instance.LoadNextLevel(false, false);

                yield return new WaitForSecondsRealtime(2f);

                MapManager.instance.CallInNewMapAndMovePlayers(MapManager.instance.currentLevelID);

                yield return new WaitForSecondsRealtime(1f);
                TimeHandler.instance.DoSpeedUp();
                TimeHandler.instance.StartGame();
                GameManager.instance.battleOngoing = true;
                PlayerManager.instance.InvokeMethod("SetPlayersVisible", true);
                PlayerManager.instance.SetPlayersSimulated(true);

                yield return new WaitForSecondsRealtime(2f);
                foreach (CardInfo[] cards in floor.AI_to_spawn)
                    base.StartCoroutine(CreatePlayer(cards));
            }
        }

        public void StartGame()
        {
            /*if (GameManager.instance.isPlaying)
            {
                return;
            }
            GameManager.instance.isPlaying = true;*/
            this.StartCoroutine(this.DoStartGame());
        }

        public virtual IEnumerator DoStartGame()
        {
            CardBarHandler.instance.Rebuild();
            PlayerAssigner.instance.InvokeMethod("SetPlayersCanJoin", false);
            UIHandler.instance.InvokeMethod("SetNumberOfRounds", 0);
            ArtHandler.instance.NextArt();

            //yield return GameModeManager.TriggerHook(GameModeHooks.HookGameStart);

            GameManager.instance.battleOngoing = false;

            UIHandler.instance.ShowJoinGameText("LETS GOO!", PlayerSkinBank.GetPlayerSkinColors(1).winText);
            yield return new WaitForSecondsRealtime(0.25f);
            UIHandler.instance.HideJoinGameText();


            UnityEngine.Debug.Log((GM_Test.instance && GM_Test.instance.gameObject.activeSelf));
            PlayerManager.instance.SetPlayersSimulated(false);
            PlayerManager.instance.InvokeMethod("SetPlayersVisible", false);
            MapManager.instance.LoadNextLevel(false, false);
            TimeHandler.instance.DoSpeedUp();

            yield return new WaitForSecondsRealtime(1f);
            /*
            yield return GameModeManager.TriggerHook(GameModeHooks.HookPickStart);
            Player player = PlayerManager.instance.players.ToArray()[0];

            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickStart);

            CardChoiceVisuals.instance.Show(player.playerID, true);
            yield return CardChoice.instance.DoPick(1, player.playerID, PickerType.Player);
            
            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickEnd);

            yield return new WaitForSecondsRealtime(0.1f);

            CardChoiceVisuals.instance.Hide();
           
            yield return GameModeManager.TriggerHook(GameModeHooks.HookPickEnd);
            */
            yield return new WaitForSecondsRealtime(0.1f);

            MapManager.instance.LoadLevelFromID(0, false, true);
            PlayerManager.instance.RPCA_MovePlayers();

            TimeHandler.instance.DoSpeedUp();
            TimeHandler.instance.StartGame();
            GameManager.instance.battleOngoing = true;
            UIHandler.instance.ShowRoundCounterSmall(0,0,0,0);
            PlayerManager.instance.InvokeMethod("SetPlayersVisible", true);


            PlayerManager.instance.SetPlayersSimulated(true);
            if(PlayerManager.instance.players.Count< 2)
               base.StartCoroutine(CreatePlayer(Grub.cardInfo));
            floorHandler = new FloorHandler();

        }
        public virtual IEnumerator DoRoundStart()
        {
            while (PlayerManager.instance.players.ToList().Any(p => !(bool)p.data.isPlaying))
            {
                yield return null;
            }
            PlayerManager.instance.SetPlayersSimulated(true);
            UnityEngine.Debug.Log(Application.isEditor);
        }

        public void PlayerJoined(Player player)
        {
            if (player.playerID != 0) return;
            player.data.isPlaying = false;
            
            MainMenuHandler.instance.Close();
            this.StartGame();
        }


        public void Start()
        {
            this.StartCoroutine(this.Init());
        }
        protected virtual IEnumerator Init()
        {
            yield return GameModeManager.TriggerHook(GameModeHooks.HookInitStart);

            PlayerManager.instance.SetPlayersSimulated(false);
            

            yield return GameModeManager.TriggerHook(GameModeHooks.HookInitEnd);
        }




        public IEnumerator CreatePlayer(params CardInfo[] cards)
        {
            int playerIDToSet = PlayerManager.instance.players.Count;
            int teamIDToSet =  1;
            List<SpawnPoint> spawnPoints = MapManager.instance.currentMap.Map.GetComponentsInChildren<SpawnPoint>().Where(s => s.localStartPos.x > 0).ToList();
            spawnPoints.Shuffle();
            Vector3 vector;
            if(spawnPoints.Count > 0)
                vector = spawnPoints.First().localStartPos;
            else
                vector = Vector3.zero;
            CharacterData component = PhotonNetwork.Instantiate(PlayerAssigner.instance.playerPrefab.name, vector, Quaternion.identity, (byte)0, (object[])null).GetComponent<CharacterData>();
            
            GameObject original = PlayerAssigner.instance.player2AI;

            component.GetComponent<CharacterData>().SetAI();
            UnityEngine.Object.Instantiate(original, component.transform.position, component.transform.rotation, component.transform);


            PlayerAssigner.instance.players.Add(component);
            PlayerAssigner.instance.InvokeMethod("RegisterPlayer",component, teamIDToSet, playerIDToSet);
            ModdingUtils.Utils.Cards.instance.AddCardsToPlayer(component.player, cards, false, addToCardBar:false);
            yield break;
        }

        public FloorHandler floorHandler;
        public Floor floor;
    }
}
