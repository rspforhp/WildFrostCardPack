using BepInEx;
using BepInEx.Unity.IL2CPP;
using WildfrostModMiya;

namespace WildFrostCardPack;
[BepInPlugin("WildFrost.Miya.ExampleMod", "ExampleMod", "0.1.0.0")]
[BepInDependency("WildFrost.Miya.WildfrostAPI")]
public class WildFrostCardPackMod : BasePlugin
{
    public override void Load()
    {
       CardAdder.OnAskForAddingCards+= delegate(int i)
       {
           CardAdder.CreateCardData("ExampleMod", "ExampleCard").SetStats(1, 1, 3).SetIsUnit().SetCanPlay(CardAdder.CanPlay.CanPlayOnBoard)
               .SetAttackEffects(CardAdder.VanillaStatusEffects.Demonize.StatusEffectStack(1)).RegisterCardInApi();
       };
       //Isnt a actually working effect but u get the hang of it
       var newCustomEffect =
           StatusEffectAdder.CreateStatusEffectData<StatusEffectApplyXEveryTurn>("ExampleMod", "ExampleEffect").SetText("Apply <keyword=haze> every turn to opposing unit");
       StatusEffectAdder.OnAskForAddingStatusEffects+= delegate(int i)
       {
           newCustomEffect = newCustomEffect.ModifyFields(
               delegate(StatusEffectApplyXEveryTurn turn)
               {
                   turn.applyToFlags = StatusEffectApplyX.ApplyToFlags.FrontEnemy;
                   turn.effectToApply = CardAdder.VanillaStatusEffects.Haze.StatusEffectData();
                   return turn;
               });
           newCustomEffect.RegisterStatusEffectInApi();
       };
       CardAdder.OnAskForAddingCards+= delegate(int i)
       {
           // OR  CardAdder.CreateCardData("ExampleMod", "ExampleCardWithEffectGotBeforeRegistration").SetStats(1, 1, 3)
           //                    .SetAttackEffects( "ExampleMod.ExampleEffect". StatusEffectStack(1)).RegisterCardInApi();
           // BUT it might fail, so this way its safer to get the effects u made yourself :3
           CardAdder.CreateCardData("ExampleMod", "ExampleCardWithEffectGotBeforeRegistration").SetIsUnit().SetCanPlay(CardAdder.CanPlay.CanPlayOnBoard).SetStats(1, 1, 3)
               .SetAttackEffects(new CardData.StatusEffectStacks(newCustomEffect,1)).RegisterCardInApi();
       };
        
    }
}