using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class ScrollOfRecall : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Scroll of Recall", "Vas Rel Por",
                                                        21004,
                                                        9300,
                                                        false,
                                                        Reagent.MandrakeRoot,
                                                        Reagent.BlackPearl
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 40; } }

        public ScrollOfRecall(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ScrollOfRecall m_Owner;

            public InternalTarget(ScrollOfRecall owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is RecallRune rune)
                {
                    if (!rune.Marked)
                    {
                        from.SendLocalizedMessage(501805); // That rune is not yet marked.
                    }
                    else if (rune.House != null && !rune.House.IsFriend(from))
                    {
                        from.SendLocalizedMessage(501807); // You are not a friend of the house that rune is marked to.
                    }
                    else if (SpellHelper.CheckTravel(from.Map, from.Location, TravelCheckType.RecallFrom)) // Corrected here
                    {
                        if (SpellHelper.CheckTravel(rune.Map, rune.Target, TravelCheckType.RecallTo)) // Corrected to use rune.Map
                        {
                            if (from.Mana >= m_Owner.RequiredMana)
                            {
                                from.Mana -= m_Owner.RequiredMana;
                                m_Owner.Scroll?.Consume();

                                Effects.SendLocationParticles(
                                    EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration),
                                    0x3728, 10, 10, 2023);

                                from.PlaySound(0x1FC);
                                SpellHelper.Turn(from, rune.Target); // Corrected to use Point3D directly

                                Effects.SendLocationParticles(
                                    EffectItem.Create(rune.Target, rune.Map, EffectItem.DefaultDuration),
                                    0x3728, 10, 10, 5023);

                                from.PlaySound(0x1FC);
                                from.MoveToWorld(rune.Target, rune.Map); // Corrected to use rune.Map

                                from.SendMessage("You have successfully recalled to the marked location.");
                            }
                            else
                            {
                                from.SendMessage("You do not have enough mana to cast this spell.");
                            }
                        }
                        else
                        {
                            from.SendLocalizedMessage(501802); // That location is blocked.
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(501802); // That location is blocked.
                    }
                }
                else
                {
                    from.SendLocalizedMessage(501804); // Target is not a marked rune.
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }

    public abstract class ScribesCompendiumSpell : Spell
    {
        public ScribesCompendiumSpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
        }

        public abstract double CastDelay { get; }
        public abstract double RequiredSkill { get; }
        public abstract int RequiredMana { get; }
    }
}
