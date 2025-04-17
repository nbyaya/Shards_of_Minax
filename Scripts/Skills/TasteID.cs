using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.SkillHandlers
{
    public class TasteID
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.TasteID].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.Target = new InternalTarget();
            m.SendLocalizedMessage(502807); // What would you like to taste?
            return TimeSpan.FromSeconds(1.0);
        }

        private class InternalTarget : Target
        {
            public InternalTarget() : base(2, false, TargetFlags.None)
            {
                this.AllowNonlocal = true;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!(from is PlayerMobile player))
                    return;

                var profile = player.AcquireTalents();
                
                // Passive Bonuses
                int sensitivityBonus = profile.Talents[TalentID.TasteIDSensitivity].Points; // Poison Detection
                int analysisBonus = profile.Talents[TalentID.TasteIDAnalysis].Points; // Potion Analysis
                int refinementBonus = profile.Talents[TalentID.TasteIDRefinement].Points; // Special food effects

                if (targeted is Mobile)
                {
                    from.SendLocalizedMessage(502816); // You feel that such an action would be inappropriate.
                }
                else if (targeted is Food food)
                {
                    if (from.CheckTargetSkill(SkillName.TasteID, food, 0, 100))
                    {
                        if (food.Poison != null)
                        {
                            if (sensitivityBonus > 0) // Poison detection is improved by TasteIDSensitivity
                            {
                                from.SendMessage($"You detect a subtle poison in this food with your trained palate.");
                                from.SendMessage($"Poison Type: {food.Poison.ToString()}");
                            }
                            else
                            {
                                from.SendLocalizedMessage(1038284); // It appears to have poison smeared on it.
                            }
                        }
                        else
                        {
                            from.SendLocalizedMessage(1010600); // You detect nothing unusual about this substance.
                        }

                        if (refinementBonus > 0)
                        {
                            from.SendMessage($"Your refined taste allows you to identify rare ingredients in this food.");
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(502823); // You cannot discern anything about this substance.
                    }
                }
                else if (targeted is BasePotion potion)
                {
                    if (analysisBonus > 0) // Potion recognition improved by TasteIDAnalysis
                    {
                        from.SendMessage($"You recognize this potion instantly as: {potion.Name}");
                    }
                    else
                    {
                        potion.SendLocalizedMessageTo(from, 502813); // You already know what kind of potion that is.
                        potion.SendLocalizedMessageTo(from, potion.LabelNumber);
                    }
                }
                else if (targeted is PotionKeg keg)
                {
                    if (keg.Held <= 0)
                    {
                        keg.SendLocalizedMessageTo(from, 502228); // There is nothing in the keg to taste!
                    }
                    else
                    {
                        keg.SendLocalizedMessageTo(from, 502229); // You are already familiar with this keg's contents.
                        keg.SendLocalizedMessageTo(from, keg.LabelNumber);
                    }
                }
                else
                {
                    from.SendLocalizedMessage(502820); // That's not something you can taste.
                }
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                from.SendLocalizedMessage(502815); // You are too far away to taste that.
            }
        }
    }
}
