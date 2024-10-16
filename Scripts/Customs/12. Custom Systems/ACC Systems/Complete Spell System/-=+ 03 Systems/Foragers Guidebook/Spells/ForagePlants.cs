using System;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ForagersGuidebook
{
    public class ForagersForagePlantsSpell : ForagersGuidebookSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Forage Plants", "Gathering",
            212,
            9041,
            CReagent.Kindling
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double CastDelay { get { return 3.0; } }
        public override double RequiredSkill { get { return 10; } }
        public override int RequiredMana { get { return 10; } }

        private static Dictionary<Mobile, Timer> m_Timers = new Dictionary<Mobile, Timer>();

        public ForagersForagePlantsSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            return base.CheckCast();
        }

        public override void OnCast()
        {
            if (Caster.CanBeginAction(typeof(ForagersForagePlantsSpell)) && CheckSequence())
            {
                Caster.BeginAction(typeof(ForagersForagePlantsSpell));
                Caster.SendMessage("You begin to forage for plants...");
                Caster.Frozen = true;
                Caster.Animate(32, 5, 1, true, false, 0); // Start the animation

                Timer animationTimer = new AnimationTimer(Caster);
                animationTimer.Start();

                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x373A, 10, 15, 5013);

                Timer timer = new InternalTimer(Caster, TimeSpan.FromSeconds(5), animationTimer);
                timer.Start();
                m_Timers[Caster] = timer;
            }
            else
            {
                Caster.SendMessage("You cannot forage for plants in that state!");
            }
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Caster;
            private DateTime m_End;
            private Timer m_AnimationTimer;

            public InternalTimer(Mobile caster, TimeSpan duration, Timer animationTimer)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(1))
            {
                m_Caster = caster;
                m_End = DateTime.Now + duration;
                m_AnimationTimer = animationTimer;
            }

            protected override void OnTick()
            {
                if (DateTime.Now >= m_End)
                {
                    m_Caster.Frozen = false;
                    m_Caster.EndAction(typeof(ForagersForagePlantsSpell));
                    m_AnimationTimer.Stop(); // Stop the animation timer

                    double skillValue = m_Caster.Skills[SkillName.Camping].Value;
                    double successChance = skillValue / 100.0;

                    if (successChance >= Utility.RandomDouble())
                    {
                        List<Item> foragedItems = GetForagedItems(skillValue);
                        foreach (Item item in foragedItems)
                        {
                            m_Caster.AddToBackpack(item);
                        }
                        m_Caster.SendMessage("You successfully foraged and found: " + string.Join(", ", foragedItems.ConvertAll(item => item.Name)) + "!");
                    }
                    else
                    {
                        m_Caster.SendMessage("You failed to find anything while foraging.");
                    }

                    m_Timers.Remove(m_Caster);
                    Stop();
                }
            }

            private List<Item> GetForagedItems(double skillValue)
            {
                List<Item> items = new List<Item>();

                // Always get the lowest tier
                items.Add(GetRandomItemFromTier(1));

                if (skillValue >= 70)
                {
                    items.Add(GetRandomItemFromTier(2));
                }
                if (skillValue >= 80)
                {
                    items.Add(GetRandomItemFromTier(3));
                }
                if (skillValue >= 90)
                {
                    items.Add(GetRandomItemFromTier(4));
                }

                return items;
            }

            private Item GetRandomItemFromTier(int tier)
            {
                switch (tier)
                {
                    case 1:
                        switch (Utility.Random(3))
                        {
                            case 0: return new Apple();
                            case 1: return new Pear();
                            default: return new Banana();
                        }
                    case 2:
                        switch (Utility.Random(3))
                        {
                            case 0: return new Peach();
                            case 1: return new Lime();
                            default: return new Lemon();
                        }
                    case 3:
                        switch (Utility.Random(3))
                        {
                            case 0: return new Grapes();
                            case 1: return new Coconut();
                            default: return new Watermelon();
                        }
                    case 4:
                        switch (Utility.Random(12))
                        {
                            case 0: return new AppleSeed();
                            case 1: return new BananaTreeSeed();
							case 2: return new CantaloupeVineSeed();
							case 3: return new CoconutTreeSeed();
							case 4: return new DateTreeSeed();
							case 5: return new GrapeSeed();
							case 6: return new LemonSeed();
							case 7: return new LimeSeed();
							case 8: return new PeachSeed();
							case 9: return new PearSeed();
							case 10: return new SquashVineSeed();
                            default: return new WatermelonVineSeed();
                        }
                    default:
                        return new Cabbage();
                }
            }
        }

        private class AnimationTimer : Timer
        {
            private Mobile m_Caster;

            public AnimationTimer(Mobile caster)
                : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
            {
                m_Caster = caster;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Caster != null && !m_Caster.Deleted)
                {
                    m_Caster.Animate(32, 5, 1, true, false, 0); // Loop the animation
                }
                else
                {
                    Stop();
                }
            }
        }
    }
}
