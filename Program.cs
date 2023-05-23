using BepInEx;
using BepInEx.Unity.IL2CPP;
using WildfrostModMiya;

namespace WildFrostCardPack;
[BepInPlugin("WildFrost.Miya.AlternateCardPack", "AlternateCardPack", "0.1.0.0")]
[BepInDependency("WildFrost.Miya.WildfrostAPI")]
public class WildFrostCardPackMod : BasePlugin
{
    internal static WildFrostCardPackMod Instance;

    private static void AddTaintedCards()
    {
        HandleTaintedCard("Blunky", (originalData, createdData) => createdData.SetStats(1,1,2).SetStartWithEffects(CardAdder.VanillaStatusEffects.Block.StatusEffectStack(4)).SetTraits(CardAdder.VanillaTraits.Pigheaded.TraitStack(1)));
        HandleTaintedCard("Snoffel", (originalData, createdData) => createdData.SetStats(4,1,5).SetStartWithEffects().SetTraits().SetAttackEffects(CardAdder.VanillaStatusEffects.Snow.StatusEffectStack(1)).SetTargetMode(CardAdder.VanillaTargetModes.TargetModeAll).SetText("Hits all foes"));
        HandleTaintedCard("BigBerry", (originalData, createdData) => createdData.SetStats(5,0,5).SetStartWithEffects(CardAdder.VanillaStatusEffects.DamageEqualToHealth.StatusEffectStack(1)).SetTraits());
       // This is laggy, dont wanna deal with the bugs it causes
       // HandleTaintedCard("Jagzag", (data, cardData) => cardData.SetStats(8, null, 2).SetStartWithEffects(CardAdder.VanillaStatusEffects.Teeth.StatusEffectStack(1)).SetAttackEffects("TaintedCards.GainTeeth".StatusEffectStack(1)).SetTraits());
        HandleTaintedCard("Flash", (originalData, createdData) => createdData.SetStats(8,0,4).SetAttackEffects(CardAdder.VanillaStatusEffects.Overload.StatusEffectStack(1)).SetTargetMode(CardAdder.VanillaTargetModes.TargetModeAll).SetText("Hits all foes"));
        HandleTaintedCard("Shelly", (originalData, createdData) => createdData.SetStats(4,6,4).SetStartWithEffects().SetAttackEffects(CardAdder.VanillaStatusEffects.Shell.StatusEffectStack(5)));

    }

    private static void HandleTaintedCard(string name, Func<CardData, CardData, CardData> extraAction)
    {
        var originalCardData = AddressableLoader.groups["CardData"].lookup[name].Cast<CardData>();
        var newCardData =originalCardData.InstantiateKeepName();
        newCardData = extraAction(originalCardData, newCardData);
        AddressableLoader.groups["CardData"].lookup[name] = newCardData;
        AddressableLoader.groups["CardData"].list.Remove(originalCardData);
        AddressableLoader.groups["CardData"].list.Add(newCardData);
    }



    public override void Load()
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
            /* Jolito effect example
            StatusEffectAdder.CreateStatusEffectData<StatusEffectApplyXOnKill>("MiyasCardPack", "JolitoEffect").ModifyFields(delegate(StatusEffectApplyXOnKill kill)
                {
                    kill.desc = "On kill count down <keyword=counter> of allies by <{0}>";
                    kill = kill.SetText("On kill count down <keyword=counter> of allies by {0}");
                    kill.textInsert="<{a}>";
                    kill.effectToApply = CardAdder.VanillaStatusEffects.ReduceCounter.StatusEffectData();
                    kill.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    return kill;
                }).RegisterStatusEffectInApi();
            */
            
             StatusEffectAdder.CreateStatusEffectData<StatusEffectApplyXInstant>("TaintedCards", "GainTeeth").ModifyFields(
                delegate(StatusEffectApplyX x)
                {
                    x.desc = "Apply <{0}> <keyword=teeth> to self";
                    x = x.SetText("Apply {0} <keyword=teeth> to self");
                    x.textInsert="<{a}>";
                    x.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    x.effectToApply = CardAdder.VanillaStatusEffects.Teeth.StatusEffectData();
                    return x;
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
            /* Jolito card example
            CardAdder.CreateCardData("MiyasCardPack", "jolito").SetStats(6,2,5)
                .SetIsUnit()
                .SetStartWithEffects("MiyasCardPack.JolitoEffect".StatusEffectStack(2)).SetSprites(CardAdder.LoadSpriteFromCardPortraits("MiyasCardPack\\Jolito"),CardAdder.LoadSpriteFromCardPortraits("MiyasCardPack\\TootieBackground"))
                .RegisterCardInApi();
            */
            AddTaintedCards();
        };
    }
}