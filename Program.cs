using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppMono;
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
using ClassInjector = Il2CppInterop.Runtime.Injection.ClassInjector;
using Color = System.Drawing.Color;
using Console = System.Console;
using IEnumerator = System.Collections.IEnumerator;
using Object = Il2CppSystem.Object;

[assembly: MelonAdditionalDependencies("WildfrostModMiya")]
[assembly: MelonInfo(typeof(WildFrostCardPackMod), "WildFrost CardPack", "1", "Kopie_Miya")]
[assembly: MelonGame("Deadpan Games", "Wildfrost")]

namespace WildFrostCardPack;

public class WildFrostCardPackMod : MelonMod
{
    internal static WildFrostCardPackMod Instance;
    
    public static T CreateStatusEffectData<T>(string modName, string cardName) where T : StatusEffectData
    {
        T instance = ScriptableObject.CreateInstance(Il2CppType.From(typeof(T))).Cast<T>();
        instance.textKey = new LocalizedString();
        instance.name = cardName.StartsWith(modName) ? cardName : modName + "." + cardName;
        if (modName == "")
            instance.name = cardName;
        return instance;
    }
    public override void OnInitializeMelon()
    {
        Instance = this;
        StatusEffectAdder.OnAskForAddingStatusEffects+= delegate(int i)
        {
            {
                var data = StatusEffectAdder.CreateStatusEffectData<StatusEffectApplyXOnCardPlayed>("MiyasCardPack","OnCardPlayedAppySpiceToRandomUnit" );
                data.effectToApply = CardAdder.VanillaStatusEffects.Spice.StatusEffectData();
                data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomUnit;
                data.desc = "Apply <{0}><keyword=spice> to a random ally or enemy";
                data = data.SetText("Apply {0} to a random ally or enemy");
                data.textInsert="<{a}><keyword=spice>";
                data.RegisterStatusEffectInApi();
            }
            {
                var data =  StatusEffectAdder.CreateStatusEffectData<StatusEffectApplyXWhenDestroyed>("MiyasCardPack","WhenDestroyedGiveAttackToRandomAlly" );
                data.effectToApply = CardAdder.VanillaStatusEffects.IncreaseAttack.StatusEffectData();
                data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomAlly;
                data.desc = "When destroyed <{0}><keyword=attack> to a random ally when destroyed";
                data = data.SetText("When destroyed {0} to a random ally when destroyed");
                data.textInsert="<{a}><keyword=attack>";
                data.RegisterStatusEffectInApi();

            }
            {
                var data =  StatusEffectAdder.CreateStatusEffectData<StatusEffectApplyXOnKill>("MiyasCardPack","OnKillApplySpiceToCardsInHand" );
                data.effectToApply = CardAdder.VanillaStatusEffects.Spice.StatusEffectData();
                data.desc = "On kill <{0}><keyword=spice> to cards in hand";
                data = data.SetText("On kill {0}  to cards in hand");
                data.textInsert="<{a}><keyword=spice>";
                data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Hand;
                data.RegisterStatusEffectInApi();

            }
            {
           
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
                .SetStartWithEffects("MiyasCardPack.WhenDestroyedGiveAttackToRandomAlly".StatusEffectStack(3))
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
            CardAdder.CreateCardData("MiyasCardPack", "BlastChill")
                .SetTitle("Blast Chill")
                .SetSprites("MiyasCardPack\\Tootie", "MiyasCardPack\\TootieBackground")
                .SetIsItem()
                .SetDamage(0)
                .SetAttackEffects(CardAdder.VanillaStatusEffects.Frost.StatusEffectStack(6))
                .SetItemUses(3)
                .SetText("Can be used 3 times before consuming.")
                .SetTraits(CardAdder.VanillaTraits.Consume.TraitStack(1))
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