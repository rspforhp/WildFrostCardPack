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
           CardAdder.CreateCardData("ExampleMod", "ExampleCard").SetTitle("Example card").SetStats(1, 1, 3).SetIsUnit().SetCanPlay(CardAdder.CanPlay.CanPlayOnBoard)
               .SetAttackEffects(CardAdder.VanillaStatusEffects.Demonize.StatusEffectStack(1)).RegisterCardInApi();
       };
       /// QWQ idk why but this way it crashes sometimes idk whats wrong with that, but usually it shouldnt crash i believe
       StatusEffectData data=null;
       StatusEffectAdder.OnAskForAddingStatusEffects+= delegate(int i)
       {
           data=
               StatusEffectAdder.CreateStatusEffectData<StatusEffectApplyXEveryTurn>("ExampleMod", "ExampleEffect").SetText("Apply <keyword=haze> every turn to opposing unit").ModifyFields(
               delegate(StatusEffectApplyXEveryTurn turn)
               {
                   turn.applyToFlags = StatusEffectApplyX.ApplyToFlags.FrontEnemy;
                   turn.effectToApply = CardAdder.VanillaStatusEffects.Haze.StatusEffectData();
                   return turn;
               }).RegisterStatusEffectInApi();
       };
       CardAdder.OnAskForAddingCards+= delegate(int i)
       {
           // OR  CardAdder.CreateCardData("ExampleMod", "ExampleCardWithEffectGotBeforeRegistration").SetStats(1, 1, 3)
           //                    .SetAttackEffects( "ExampleMod.ExampleEffect". StatusEffectStack(1)).RegisterCardInApi();
           // BUT it might fail, so this way its safer to get the effects u made yourself :3
           CardAdder.CreateCardData("ExampleMod", "ExampleCardWithEffectGotBeforeRegistration").SetTitle("Example Card 2").SetIsUnit().SetCanPlay(CardAdder.CanPlay.CanPlayOnBoard).SetStats(1, 1, 3)
               .SetStartWithEffects(new CardData.StatusEffectStacks(data,1)).RegisterCardInApi();
       };
        
    }
}