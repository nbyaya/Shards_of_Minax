using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class InstigateConflict : ProvocationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Instigate Conflict", "Conflicto Instigare",
            21005,
            9500,
            false,
            Reagent.BlackPearl,
            Reagent.Bloodmoss,
            Reagent.SulfurousAsh
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
		public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 30; } }

        public InstigateConflict(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private InstigateConflict m_Owner;

            public InternalTarget(InstigateConflict owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                {
                    IPoint3D p = (IPoint3D)targeted;
                    SpellHelper.Turn(m_Owner.Caster, p);

                    if (m_Owner.CheckSequence())
                    {
                        List<Mobile> enemies = new List<Mobile>();
                        IPooledEnumerable mobilesInRange = m_Owner.Caster.GetMobilesInRange(5);

                        foreach (Mobile m in mobilesInRange)
                        {
                            if (m != m_Owner.Caster && m is BaseCreature && ((BaseCreature)m).Controlled == false && ((BaseCreature)m).Summoned == false)
                            {
                                enemies.Add(m);
                            }
                        }

                        mobilesInRange.Free();

                        if (enemies.Count >= 2)
                        {
                            // Randomly choose two groups of enemies
                            List<Mobile> group1 = new List<Mobile>();
                            List<Mobile> group2 = new List<Mobile>();

                            foreach (Mobile enemy in enemies)
                            {
                                if (Utility.RandomBool())
                                    group1.Add(enemy);
                                else
                                    group2.Add(enemy);
                            }

                            foreach (Mobile mob1 in group1)
                            {
                                mob1.Combatant = group2[Utility.Random(group2.Count)];
                                mob1.Say("*becomes enraged and attacks*");
                                mob1.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Visual effect
                                mob1.PlaySound(0x22C); // Sound effect
                            }

                            foreach (Mobile mob2 in group2)
                            {
                                mob2.Combatant = group1[Utility.Random(group1.Count)];
                                mob2.Say("*turns to fight the new enemy*");
                                mob2.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Visual effect
                                mob2.PlaySound(0x22C); // Sound effect
                            }
                        }

                        Effects.PlaySound(from.Location, from.Map, 0x29); // Main spell sound
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008); // Main spell effect
                    }
                }
                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
