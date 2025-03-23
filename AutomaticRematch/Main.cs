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

        private void Log(string msg)
        {
            MelonLogger.Msg(msg);
        }

        public static class BuildInfo
        {
            public const string ModName = "AutomaticRematch";
            public const string ModVersion = "1.0.0";
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
            AutomaticRematch.ModSaved += Save;
            UI.instance.AddMod(AutomaticRematch);
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            currentScene = sceneName;
            //if ((currentScene == "Map0") || (currentScene == "Map1")) { MelonCoroutines.Start(Test()); }
        }

        /*private IEnumerator Test()
        {
            yield return new WaitForSeconds(5);
            MelonCoroutines.Start(MatchEnded());
        }*/

        private void Save()
        {
            enabled = (bool)AutomaticRematch.Settings[0].SavedValue;
            timeToWait = (float)AutomaticRematch.Settings[1].SavedValue;
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
            myButton.GetComponent<RematchButton>().RPC_OnToggleStateChanged(true);
            yield break;
        }
    }
}
