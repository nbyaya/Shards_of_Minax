#region References
using System;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
#endregion

namespace Server.SkillHandlers
{
    public class Begging
    {
        public static void Initialize()
        {
            // Set up the Begging skill callback.
            SkillInfo.Table[(int)SkillName.Begging].Callback = OnUse;
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.RevealingAction();
            m.SendLocalizedMessage(500397); // "To whom do you wish to grovel?"
            Timer.DelayCall(() => m.Target = new InternalTarget());
            return TimeSpan.FromHours(1.0);
        }

        private class InternalTarget : Target
        {
            private bool m_SetSkillTime = true;

            public InternalTarget() : base(12, false, TargetFlags.None)
            { }

            protected override void OnTargetFinish(Mobile from)
            {
                if (m_SetSkillTime)
                    from.NextSkillTime = Core.TickCount;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                int number = -1;

                if (targeted is Mobile)
                {
                    Mobile targ = (Mobile)targeted;

                    // Cannot beg from players.
                    if (targ.Player)
                        number = 500398; // "Perhaps just asking would work better."
                    else if (!targ.Body.IsHuman)
                        number = 500399; // "There is little chance of getting money from that!"
                    else if (!from.InRange(targ, 2))
                        number = targ.Female ? 500402 : 500401; // "You are too far away..."
                    else if (!Core.ML && from.Mounted)
                        number = 500404; // "They seem unwilling to give you any money."
                    else
                    {
                        // Face each other and animate a bow.
                        from.Direction = from.GetDirectionTo(targ);
                        targ.Direction = targ.GetDirectionTo(from);
                        from.Animate(32, 5, 1, true, false, 0);

                        // Adjust recruitment chance with the BeggingPersuasion talent bonus.
                        double recruitBonus = 0.0;
                        if (from is PlayerMobile pm)
                        {
                            var profile = pm.AcquireTalents();
                            recruitBonus = profile.Talents[TalentID.BeggingPersuasion].Points * 0.02;
                        }

                        // Approximately 5% chance to attempt recruiting (modified by bonus)
                        if (Utility.RandomDouble() >= (0.05 - recruitBonus))
                        {
                            new InternalBeggingTimer(from, targ).Start();
                        }
                        else
                        {
                            AttemptRecruit(from, targ);
                        }
                        m_SetSkillTime = false;
                    }
                }
                else
                {
                    number = 500399;
                }

                if (number != -1)
                    from.SendLocalizedMessage(number);
            }

            private void AttemptRecruit(Mobile from, Mobile targ)
            {
                if (targ is BaseCreature)
                {
                    BaseCreature target = (BaseCreature)targ;
                    if (!target.Controlled)
                    {
                        double recruitChance = from.Skills[SkillName.Begging].Value / 100.0;
                        // Increase chance with BeggingPersuasion bonus.
                        if (from is PlayerMobile pm)
                        {
                            var profile = pm.AcquireTalents();
                            recruitChance += profile.Talents[TalentID.BeggingPersuasion].Points * 0.02;
                        }

                        if (Utility.RandomDouble() <= recruitChance)
                        {
                            if (from.Followers + 1 > from.FollowersMax)
                                from.SendMessage("You have too many followers to recruit this NPC.");
                            else
                            {
                                target.Controlled = true;
                                target.ControlMaster = from;
                                target.ControlTarget = from;
                                target.ControlOrder = OrderType.Follow;
                                target.BondingBegin = DateTime.Now;
                                target.OwnerAbandonTime = DateTime.Now + TimeSpan.FromDays(1.0);
                                from.SendMessage("You have successfully recruited the adventurer.");
                                from.NextSkillTime = Core.TickCount;
                            }
                        }
                        else
                        {
                            from.SendMessage("You failed to recruit the adventurer.");
                        }
                    }
                    else
                    {
                        from.SendMessage("You can only recruit uncontrolled human NPCs.");
                    }
                }
                else
                {
                    from.SendMessage("You can only recruit human NPCs.");
                }
            }

            private class InternalBeggingTimer : Timer
            {
                private readonly Mobile m_From;
                private readonly Mobile m_Target;

                public InternalBeggingTimer(Mobile from, Mobile target)
                    : base(TimeSpan.FromSeconds(2.0))
                {
                    m_From = from;
                    m_Target = target;
                    Priority = TimerPriority.TwoFiftyMS;
                }

                protected override void OnTick()
                {
                    Container theirPack = m_Target.Backpack;
                    double badKarmaChance = 0.5 - ((double)m_From.Karma / 8570);

                    if (theirPack == null && m_Target.Race != Race.Elf)
                    {
                        m_From.SendLocalizedMessage(500404);
                    }
                    else if (m_From.Karma < 0 && badKarmaChance > Utility.RandomDouble())
                    {
                        m_Target.PublicOverheadMessage(MessageType.Regular, m_Target.SpeechHue, 500406);
                    }
                    else if (m_From.CheckTargetSkill(SkillName.Begging, m_Target, 0.0, 100.0))
                    {
                        // Retrieve passive bonuses from the talent system.
                        int bonusGold = 0;
                        int bonusKarmaReduction = 0;
                        if (m_From is PlayerMobile pm)
                        {
                            var profile = pm.AcquireTalents();
                            // Each point in BeggingLuck gives extra gold.
                            bonusGold = profile.Talents[TalentID.BeggingLuck].Points;
                            // Each point in BeggingKarma reduces karma loss by 2.
                            bonusKarmaReduction = profile.Talents[TalentID.BeggingKarma].Points * 2;
                        }

                        if (m_Target.Race != Race.Elf)
                        {
                            int toConsume = theirPack.GetAmount(typeof(Gold)) / 10;
                            int max = 10 + (m_From.Fame / 2500);
                            if (max > 14)
                                max = 14;
                            else if (max < 10)
                                max = 10;
                            if (toConsume > max)
                                toConsume = max;

                            if (toConsume > 0)
                            {
                                int consumed = theirPack.ConsumeUpTo(typeof(Gold), toConsume);
                                if (consumed > 0)
                                {
                                    m_Target.PublicOverheadMessage(MessageType.Regular, m_Target.SpeechHue, 500405);
                                    // Award gold plus any bonus from BeggingLuck.
                                    Gold gold = new Gold(consumed + bonusGold);
                                    m_From.AddToBackpack(gold);
                                    m_From.PlaySound(gold.GetDropSound());

                                    if (m_From.Karma > -3000)
                                    {
                                        int toLose = m_From.Karma + 3000;
                                        if (toLose > 40)
                                            toLose = 40;
                                        // Reduce karma loss using BeggingKarma bonus.
                                        toLose -= bonusKarmaReduction;
                                        if (toLose < 0)
                                            toLose = 0;
                                        Titles.AwardKarma(m_From, -toLose, true);
                                    }
                                }
                                else
                                {
                                    m_Target.PublicOverheadMessage(MessageType.Regular, m_Target.SpeechHue, 500407);
                                }
                            }
                            else
                            {
                                m_Target.PublicOverheadMessage(MessageType.Regular, m_Target.SpeechHue, 500407);
                            }
                        }
                        else // Special rewards when targeting an Elf.
                        {
                            double chance = Utility.RandomDouble();
                            Item reward = new Gold(1);
                            string rewardName = "";
                            if (chance >= .99)
                            {
                                int rand = Utility.Random(9);
                                switch (rand)
                                {
                                    case 0: reward = new BegBedRoll(); rewardName = "a bedroll"; break;
                                    case 1: reward = new BegCookies(); rewardName = "a plate of cookies"; break;
                                    case 2: reward = new BegFishSteak(); rewardName = "a fish steak"; break;
                                    case 3: reward = new BegFishingPole(); rewardName = "a fishing pole"; break;
                                    case 4: reward = new BegFlowerGarland(); rewardName = "a flower garland"; break;
                                    case 5: reward = new BegSake(); rewardName = "a bottle of Sake"; break;
                                    case 6: reward = new BegTurnip(); rewardName = "a turnip"; break;
                                    case 7: reward = new BegWine(); rewardName = "a bottle of wine"; break;
                                    case 8: reward = new BegWinePitcher(); rewardName = "a pitcher of wine"; break;
                                }
                            }
                            else if (chance >= .76)
                            {
                                int rand = Utility.Random(7);
                                switch (rand)
                                {
                                    case 0: reward = new BegStew(); rewardName = "a bowl of stew"; break;
                                    case 1: reward = new BegCheeseWedge(); rewardName = "a wedge of cheese"; break;
                                    case 2: reward = new BegDates(); rewardName = "a bunch of dates"; break;
                                    case 3: reward = new BegLantern(); rewardName = "a lantern"; break;
                                    case 4: reward = new BegLiquorPitcher(); rewardName = "a pitcher of liquor"; break;
                                    case 5: reward = new BegPizza(); rewardName = "pizza"; break;
                                    case 6: reward = new BegShirt(); rewardName = "a shirt"; break;
                                }
                            }
                            else if (chance >= .25)
                            {
                                int rand = Utility.Random(2);
                                if (rand == 0)
                                {
                                    reward = new BegFrenchBread();
                                    rewardName = "french bread";
                                }
                                else
                                {
                                    reward = new BegWaterPitcher();
                                    rewardName = "a pitcher of water";
                                }
                            }

                            m_Target.Say(1074854); // "Here, take this..."
                            m_From.AddToBackpack(reward);
                            m_From.SendLocalizedMessage(1074853, rewardName); // You have been given ~1_name~
                            
                            if (m_From.Karma > -3000)
                            {
                                int toLose = m_From.Karma + 3000;
                                if (toLose > 40)
                                    toLose = 40;
                                toLose -= bonusKarmaReduction;
                                if (toLose < 0)
                                    toLose = 0;
                                Titles.AwardKarma(m_From, -toLose, true);
                            }
                        }
                    }
                    else
                    {
                        m_Target.SendLocalizedMessage(500404); // They seem unwilling to give you any money.
                    }
                    m_From.NextSkillTime = Core.TickCount + 10;
                }
            }
        }
    }
}
