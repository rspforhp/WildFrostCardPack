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
using UniverseLib.Runtime.Il2Cpp;
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


    public override void OnInitializeMelon()
    {
        Instance = this;
        StatusEffectAdder.OnAskForAddingStatusEffects += delegate(int i)
        {
            /* OLD STUFF
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
                var data =StatusEffectAdder.CreateStatusEffectData<StatusEffectApplyXWhenHit>("MiyasCardPack","TestAgain" );
                data.desc = "log stuff when hit";
                data = data.SetText( "log stuff when hit");
                data.RegisterStatusEffectInApi();
    
          

            }
            */
            StatusEffectAdder.CreateStatusEffectData<StatusEffectApplyXOnKill>("MiyasCardPack", "JolitoEffect").ModifyFields(delegate(StatusEffectApplyXOnKill kill)
                {
                    kill.desc = "On kill count down <keyword=counter> of allies by <{0}>";
                    kill = kill.SetText("On kill count down <keyword=counter> of allies by {0}");
                    kill.textInsert="<{a}>";
                    kill.effectToApply = CardAdder.VanillaStatusEffects.ReduceCounter.StatusEffectData();
                    kill.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    return kill;
                }).RegisterStatusEffectInApi();
        };
        CardAdder.OnAskForAddingCards += delegate(int i)
        {
            /* OLD STUFF 
CardAdder.CreateCardData("MiyasCardPack", "TestCardAgain")
.SetTitle("Test")
.SetSprites("MiyasCardPack\\SpiceDice", "MiyasCardPack\\SpiceDiceBackground")
.SetIsUnit()
.SetTargetMode(CardAdder.VanillaTargetModes.TargetModeAll)
.SetStartWithEffects("MiyasCardPack.TestAgain".StatusEffectStack(1))
.AddToPool(CardAdder.VanillaRewardPools.GeneralUnitPool)
.SetStats(10,1,1)
.AddToPets()
.RegisterCardInApi()
;


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
.AddToPool(CardAdder.VanillaRewardPools.GeneralItemPool)
.RegisterCardInApi()
;
CardAdder.CreateCardData("MiyasCardPack", "ChillyBot")
.SetTitle("Chilly bot")
.SetSprites("MiyasCardPack\\Tootie", "MiyasCardPack\\TootieBackground")
.SetIsUnit()
.SetCardType(CardAdder.VanillaCardTypes.Clunker)
.SetStats(null,null,0)
.SetStartWithEffects(CardAdder.VanillaStatusEffects.Scrap.StatusEffectStack(1), CardAdder.VanillaStatusEffects.TriggerWhenAllyInRowAttacks.StatusEffectStack(1))
.SetAttackEffects(CardAdder.VanillaStatusEffects.Snow.StatusEffectStack(1))
.AddToPool(CardAdder.VanillaRewardPools.GeneralUnitPool)
.RegisterCardInApi()
;
CardAdder.CreateCardData("MiyasCardPack", "Panacea")
.SetTitle("Panacea")
.SetSprites("MiyasCardPack\\Tootie", "MiyasCardPack\\TootieBackground")
.SetIsItem()
.SetAttackEffects(CardAdder.VanillaStatusEffects.Cleanse.StatusEffectStack(1))
.SetText("Remove all effects from a card")
.SetCanPlay(CardAdder.CanPlay.CanPlayOnBoard| CardAdder.CanPlay.CanPlayOnEnemy | CardAdder.CanPlay.CanPlayOnFriendly | CardAdder.CanPlay.CanPlayOnHand)
.AddToPool(CardAdder.VanillaRewardPools.GeneralItemPool)
.RegisterCardInApi()
;
*/

            CardAdder.CreateCardData("MiyasCardPack", "jolito").SetStats(6,2,5)
                .SetIsUnit()
                .SetStartWithEffects("MiyasCardPack.JolitoEffect".StatusEffectStack(2)).SetSprites(CardAdder.LoadSpriteFromCardPortraits("MiyasCardPack\\Jolito"),CardAdder.LoadSpriteFromCardPortraits("MiyasCardPack\\TootieBackground"))
                .RegisterCardInApi();
        };
        base.OnInitializeMelon();
    }
}