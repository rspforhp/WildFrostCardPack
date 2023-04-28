using HarmonyLib;
using Il2Cpp;
using Il2CppSystem.Collections;
using Il2CppSystem.IO;
using Il2CppSystem.Reflection;
using MelonLoader;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using UniverseLib;
using WildFrostCardPack;
using WildfrostModMiya;
using Color = System.Drawing.Color;
using IEnumerator = System.Collections.IEnumerator;
using Object = Il2CppSystem.Object;

[assembly: MelonAdditionalDependencies("WildfrostModMiya")]
[assembly: MelonInfo(typeof(WildFrostCardPackMod), "WildFrost CardPack", "1", "Kopie_Miya")]
[assembly: MelonGame("Deadpan Games", "Wildfrost")]

namespace WildFrostCardPack;

public class WildFrostCardPackMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        StatusEffectAdder.OnAskForAddingStatusEffects+= delegate(int i)
        {
            //SpiceEffectWorkAround
            {
                var data = CardAdder.VanillaStatusEffects.OnCardPlayedApplyBlockToRandomUnit.StatusEffectData().Instantiate().Cast<StatusEffectApplyXOnCardPlayed>();
                data.name = "MiyasCardPack.OnCardPlayedAppySpiceToRandomUnit";
                data.effectToApply = CardAdder.VanillaStatusEffects.Spice.StatusEffectData();
                data.textInsert="<{a}><keyword=spice>";
                data.RegisterStatusEffectInApi();
            }
            {
                var data = CardAdder.VanillaStatusEffects.WhenDestroyedApplyFrenzyToRandomAlly.StatusEffectData().Instantiate().Cast<StatusEffectApplyXWhenDestroyed>();
                data.name = "MiyasCardPack.WhenDestroyedGiveAttackToRandomAlly";
                data.effectToApply = CardAdder.VanillaStatusEffects.IncreaseAttack.StatusEffectData();
                data.textInsert="<{a}><keyword=attack>";
                data.RegisterStatusEffectInApi();

            }
            {
                var data = CardAdder.VanillaStatusEffects.OnKillApplyAttackToSelf.StatusEffectData().Instantiate().Cast<StatusEffectApplyXOnKill>();
                data.name = "MiyasCardPack.OnKillApplySpiceToCardsInHand";
                data.effectToApply = CardAdder.VanillaStatusEffects.Spice.StatusEffectData();
                data.textInsert="<{a}><keyword=spice>";
                data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Hand;
                data.RegisterStatusEffectInApi();

            }
        };
        CardAdder.OnAskForAddingCards += delegate(int i)
        {
            
         
            CardAdder.CreateCardData("MiyasCardPack", "SpiceDice")
                .SetTitle("Spice dice")
                .SetSprites("MiyasCardPack\\SpiceDice", "MiyasCardPack\\SpiceDiceBackground")
                .SetIsItem()
                .SetTargetMode(CardAdder.VanillaTargetModes.TargetModeAll)
                .SetStartWithEffects("MiyasCardPack.OnCardPlayedAppySpiceToRandomUnit".StatusEffectStack(5), CardAdder.VanillaStatusEffects.MultiHit.StatusEffectStack(1))
                .AddToPool(CardAdder.VanillaRewardPools.GeneralItemPool)
                .RegisterCardInApi()
                ;


            CardAdder.CreateCardData("MiyasCardPack", "Tootie")
                .SetTitle("Tootie")
                .SetSprites("MiyasCardPack\\Tootie", "MiyasCardPack\\TootieBackground")
                .SetIsUnit()
                .SetStats(1, 1, 3)
                .SetStartWithEffects("MiyasCardPack.WhenDestroyedGiveAttackToCardsInHand".StatusEffectStack(3))
                .AddToPool(CardAdder.VanillaRewardPools.GeneralUnitPool)
                .RegisterCardInApi()
                ;

            CardAdder.CreateCardData("MiyasCardPack", "Diska")
                .SetTitle("Diska")
                .SetSprites("MiyasCardPack\\Tootie", "MiyasCardPack\\TootieBackground")
                .SetIsUnit()
                .SetStats(5, 4, 6)
                .SetStartWithEffects("MiyasCardPack.OnKillApplySpiceToCardsInHand".StatusEffectStack(1))
                .AddToPool(CardAdder.VanillaRewardPools.GeneralUnitPool)
                .RegisterCardInApi()
                ;

            /*
             how about a new clunker 1/0 Apply 1 Snow Trigger against target when an ally in your row attacks
             */
        };
        base.OnInitializeMelon();
    }
}