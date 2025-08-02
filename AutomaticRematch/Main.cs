using Il2CppRUMBLE.Interactions.InteractionBase;
using Il2CppRUMBLE.Networking.MatchFlow;
using MelonLoader;
using RumbleModdingAPI;
using RumbleModUI;
using System.Collections;
using UnityEngine;

namespace AutomaticRematch
{
    public class Main : MelonMod
    {
        private string currentScene = "Loader";
        private Mod AutomaticRematch = new Mod();
        private bool enabled = true;
        private float timeToWait = 0f;
        private int buttonToUse = 0;

        private void Log(string msg)
        {
            MelonLogger.Msg(msg);
        }

        public static class BuildInfo
        {
            public const string ModName = "AutomaticRematch";
            public const string ModVersion = "1.1.1";
            public const string Description = "Automatically Presses Rematch Button if On";
            public const string Author = "UlvakSkillz";
            public const string Company = "";
        }

        public override void OnLateInitializeMelon()
        {
            Calls.onMatchEnded += matchEnded;
            UI.instance.UI_Initialized += UIInit;
        }

        private void UIInit()
        {
            AutomaticRematch.ModName = BuildInfo.ModName;
            AutomaticRematch.ModVersion = BuildInfo.ModVersion;
            AutomaticRematch.SetFolder("AutomaticRematch");
            AutomaticRematch.AddToList("Auto Rematch", true, 0, "Automatically Presses Rematch Button if On", new Tags { });
            AutomaticRematch.AddToList("Time before Pressing", 0f, "Time to wait before it Presses Rematch", new Tags { });
            AutomaticRematch.AddToList("Which Button To Use", 0, $"Sets What Button it Watches to Prevent Automatic Rematching{Environment.NewLine}0 : Left Controller Trigger{Environment.NewLine}1 : Left Controller Primary{Environment.NewLine}2 : Left Controller Secondary{Environment.NewLine}3 : Right Controller Trigger{Environment.NewLine}4 : Right Controller Primary{Environment.NewLine}5 : Right Controller Secondary", new Tags { });
            AutomaticRematch.GetFromFile();
            AutomaticRematch.ModSaved += Save;
            Save();
            UI.instance.AddMod(AutomaticRematch);
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            currentScene = sceneName;
        }

        private void Save()
        {
            enabled = (bool)AutomaticRematch.Settings[0].SavedValue;
            timeToWait = (float)AutomaticRematch.Settings[1].SavedValue;
            buttonToUse = (int)AutomaticRematch.Settings[2].SavedValue;
        }

        private void matchEnded()
        {
            if (enabled)
            {
                MelonCoroutines.Start(MatchEnded());
            }
        }

        private IEnumerator MatchEnded()
        {
            GameObject myButton;
            if (Calls.Players.IsHost())
            {
                if (currentScene == "Map0")
                {
                    myButton = Calls.GameObjects.Map0.Logic.MatchSlabOne.MatchSlab.SlabBuddyMatchVariant.MatchForm.RePlayButton.InteractionButton.Button.GetGameObject();
                }
                else
                {
                    myButton = Calls.GameObjects.Map1.Logic.MatchSlabOne.MatchSlab.SlabBuddyMatchVariant.MatchForm.RePlayButton.InteractionButton.Button.GetGameObject();
                }
            }
            else
            {
                if (currentScene == "Map0")
                {
                    myButton = Calls.GameObjects.Map0.Logic.MatchSlabTwo.MatchSlab.SlabBuddyMatchVariant.MatchForm.RePlayButton.InteractionButton.Button.GetGameObject();
                }
                else
                {
                    myButton = Calls.GameObjects.Map1.Logic.MatchSlabTwo.MatchSlab.SlabBuddyMatchVariant.MatchForm.RePlayButton.InteractionButton.Button.GetGameObject();
                }
            }
            yield return new WaitForSeconds(timeToWait);
            float buttonValue = 0;
            switch (buttonToUse)
            {
                case 0:
                    buttonValue = Calls.ControllerMap.LeftController.GetTrigger();
                    break;
                case 1:
                    buttonValue = Calls.ControllerMap.LeftController.GetPrimary();
                    break;
                case 2:
                    buttonValue = Calls.ControllerMap.LeftController.GetSecondary();
                    break;
                case 3:
                    buttonValue = Calls.ControllerMap.RightController.GetTrigger();
                    break;
                case 4:
                    buttonValue = Calls.ControllerMap.RightController.GetPrimary();
                    break;
                case 5:
                    buttonValue = Calls.ControllerMap.RightController.GetSecondary();
                    break;
                default:
                    break;
            }
            if (buttonValue < 0.5f)
            {
                myButton.GetComponent<RematchButton>().RPC_OnToggleStateChanged(true);
            }
            yield break;
        }
    }
}
