using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class CommandingPresence : TacticsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Commanding Presence", "Domine Ut Bellum",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public CommandingPresence(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();
                
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                // Play sound and effect at the caster's location
                Effects.PlaySound(Caster.Location, Caster.Map, 0x2D6);
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5042);

                // Apply debuff to all enemies in range
                List<Mobile> targets = new List<Mobile>();
                foreach (Mobile m in Caster.GetMobilesInRange(8)) // 8 tile range
                {
                    if (m != Caster && m.Alive && Caster.CanBeHarmful(m, false))
                    {
                        if (m is BaseCreature && ((BaseCreature)m).Controlled) // Only affect summoned or controlled creatures
                            continue;

                        if (m is PlayerMobile || (m is BaseCreature && !((BaseCreature)m).Controlled))
                        {
                            targets.Add(m);
                        }
                    }
                }

                if (targets.Count > 0)
                {
                    foreach (Mobile target in targets)
                    {
                        Caster.DoHarmful(target);
                        target.SendMessage("You feel compelled to focus on " + Caster.Name + "!");
                        target.Combatant = Caster;

                        // Apply debuff
                        target.SendMessage("You feel your combat effectiveness waning under the commanding presence!");
                        target.AddStatMod(new StatMod(StatType.Str, "CommandingPresenceDebuff", -10, TimeSpan.FromSeconds(30))); // Reduce strength
                        target.AddStatMod(new StatMod(StatType.Dex, "CommandingPresenceDebuff", -10, TimeSpan.FromSeconds(30))); // Reduce dexterity

                        // Visual effect on each affected target
                        Effects.SendTargetParticles(target, 0x374A, 10, 15, 5030, EffectLayer.Head);
                    }
                }

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private CommandingPresence m_Owner;

            public InternalTarget(CommandingPresence owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
