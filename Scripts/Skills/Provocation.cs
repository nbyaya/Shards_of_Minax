using System;
using Server.Engines.XmlSpawner2;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Engines.Quests;

namespace Server.SkillHandlers
{
    public class Provocation
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Provocation].Callback = OnUse;
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.RevealingAction();
            BaseInstrument.PickInstrument(m, OnPickedInstrument);
            return TimeSpan.FromSeconds(1.0); // Base cooldown before another skill can be used
        }

        public static void OnPickedInstrument(Mobile from, BaseInstrument instrument)
        {
            from.RevealingAction();
            from.SendLocalizedMessage(501587); // Whom do you wish to incite?
            from.Target = new InternalFirstTarget(from, instrument);
        }

        public class InternalFirstTarget : Target
        {
            private readonly BaseInstrument m_Instrument;

            public InternalFirstTarget(Mobile from, BaseInstrument instrument)
                : base(GetProvokeRange(from), false, TargetFlags.None)
            {
                m_Instrument = instrument;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (targeted is BaseCreature && from.CanBeHarmful((Mobile)targeted, true))
                {
                    BaseCreature creature = (BaseCreature)targeted;

                    if (!m_Instrument.IsChildOf(from.Backpack))
                    {
                        from.SendLocalizedMessage(1062488); // The instrument you are trying to play is no longer in your backpack!
                    }
                    else if (from is PlayerMobile && creature.Controlled)
                    {
                        from.SendLocalizedMessage(501590); // They are too loyal to their master to be provoked.
                    }
                    else if (creature.IsParagon && BaseInstrument.GetBaseDifficulty(creature) >= 160.0)
                    {
                        from.SendLocalizedMessage(1049446); // You have no chance of provoking those creatures.
                    }
                    else
                    {
                        from.RevealingAction();
                        m_Instrument.PlayInstrumentWell(from);
                        from.SendLocalizedMessage(1008085);
                        from.Target = new InternalSecondTarget(from, m_Instrument, creature);
                    }
                }
                else
                {
                    from.SendLocalizedMessage(501589); // You can't incite that!
                }
            }
        }

        public class InternalSecondTarget : Target
        {
            private readonly BaseCreature m_Creature;
            private readonly BaseInstrument m_Instrument;

            public InternalSecondTarget(Mobile from, BaseInstrument instrument, BaseCreature creature)
                : base(GetProvokeRange(from), false, TargetFlags.None)
            {
                m_Instrument = instrument;
                m_Creature = creature;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (targeted is BaseCreature || (from is BaseCreature && ((BaseCreature)from).CanProvoke))
                {
                    BaseCreature creature = targeted as BaseCreature;
                    Mobile target = targeted as Mobile;
                    bool questTargets = QuestTargets(creature, from);

                    if (!m_Instrument.IsChildOf(from.Backpack))
                    {
                        from.SendLocalizedMessage(1062488);
                    }
                    else if (m_Creature.Unprovokable)
                    {
                        from.SendLocalizedMessage(1049446);
                    }
                    else if (creature != null && creature.Unprovokable && !(creature is DemonKnight) && !questTargets)
                    {
                        from.SendLocalizedMessage(1049446);
                    }
                    else if (m_Creature.Map != target.Map || !m_Creature.InRange(target, GetProvokeRange(from)))
                    {
                        from.SendLocalizedMessage(1049450);
                    }
                    else if (m_Creature != target)
                    {
                        from.NextSkillTime = Core.TickCount + GetCooldownReduction(from);

                        double diff = ((m_Instrument.GetDifficultyFor(m_Creature) + m_Instrument.GetDifficultyFor(target)) * 0.5) - 5.0;
                        double music = from.Skills[SkillName.Musicianship].Value;
                        int masteryBonus = 0;

                        if (from is PlayerMobile)
                            masteryBonus = Spells.SkillMasteries.BardSpell.GetMasteryBonus((PlayerMobile)from, SkillName.Provocation);

                        if (masteryBonus > 0)
                            diff -= (diff * ((double)masteryBonus / 100));

                        if (music > 100.0)
                            diff -= (music - 100.0) * 0.5;

                        // **Apply passive difficulty reduction from skill tree**
                        if (from is PlayerMobile player)
                        {
                            var profile = player.AcquireTalents();
                            int passiveBonus = profile.Talents[TalentID.ProvocationBonus].Points;
                            diff -= passiveBonus; // Each bonus point reduces required diff by 1
                        }

                        if (questTargets || (from.CanBeHarmful(m_Creature, true) && from.CanBeHarmful(target, true)))
                        {
                            if (from.Player && !BaseInstrument.CheckMusicianship(from))
                            {
                                from.SendLocalizedMessage(500612); // You play poorly, and there is no effect.
                                m_Instrument.PlayInstrumentBadly(from);
                                m_Instrument.ConsumeUse(from);
                            }
                            else
                            {
                                if (!from.CheckTargetSkill(SkillName.Provocation, target, diff - 25.0, diff + 25.0))
                                {
                                    from.SendLocalizedMessage(501599); // Your music fails to incite enough anger.
                                    m_Instrument.PlayInstrumentBadly(from);
                                    m_Instrument.ConsumeUse(from);
                                }
                                else
                                {
                                    from.SendLocalizedMessage(501602); // Your music succeeds, as you start a fight.
                                    m_Instrument.PlayInstrumentWell(from);
                                    m_Instrument.ConsumeUse(from);
                                    m_Creature.Provoke(from, target, true);
                                }
                            }
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(501593); // You can't tell someone to attack themselves!
                    }
                }
                else
                {
                    from.SendLocalizedMessage(501589); // You can't incite that!
                }
            }

            public bool QuestTargets(BaseCreature creature, Mobile from)
            {
                if (creature != null)
                {
                    Mobile master = creature.GetMaster();
                    if (master != null && master is PlayerMobile)
                        return false;

                    return from is PlayerMobile && (m_Creature is Rabbit || m_Creature is JackRabbit) && (creature is WanderingHealer || creature is EvilWanderingHealer);
                }
                return false;
            }
        }

        // **Passive Bonus: Extended Provocation Range**
        public static int GetProvokeRange(Mobile from)
        {
            int baseRange = 8; // Default bard skill range
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                int extraRange = profile.Talents[TalentID.ProvocationRange].Points;
                return baseRange + extraRange;
            }
            return baseRange;
        }

        // **Passive Bonus: Reduced Cooldown**
        public static int GetCooldownReduction(Mobile from)
        {
            int baseCooldown = 10000; // Default cooldown (10 seconds)
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                int reduction = profile.Talents[TalentID.ProvocationCooldownReduction].Points * 500; // Each point reduces cooldown by 0.5 sec
                return Math.Max(5000, baseCooldown - reduction); // Minimum 5 sec cooldown
            }
            return baseCooldown;
        }
    }
}
