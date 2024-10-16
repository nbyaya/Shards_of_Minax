using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class GlyphOfInsight : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Glyph of Insight", "In Wis Quas",
            21004, 9300,
            Reagent.BlackPearl, Reagent.Nightshade
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public GlyphOfInsight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private GlyphOfInsight m_Spell;

            public InternalTarget(GlyphOfInsight spell) : base(12, true, TargetFlags.None)
            {
                m_Spell = spell;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point)
                    m_Spell.Target(point);
                else
                    from.SendMessage("You must target a location.");
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Spell.FinishSequence();
            }
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                Point3D loc = new Point3D(p);
                Map map = Caster.Map;

                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5023);
                Effects.PlaySound(loc, map, 0x1ED);

                foreach (Mobile m in map.GetMobilesInRange(loc, 8)) // Radius of 8 tiles
                {
                    if (m.Hidden && m.AccessLevel == AccessLevel.Player) // Reveal players only
                    {
                        m.RevealingAction();
                        m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Head);
                        m.PlaySound(0x1FB);
                        Caster.SendMessage($"You have revealed {m.Name}.");
                    }
                }

                foreach (Item item in map.GetItemsInRange(loc, 8)) // Radius of 8 tiles
                {
                    // Note: There's no 'Hidden' property for items; this code is now removed.
                    if (item is ILockable && ((ILockable)item).Locked)
                    {
                        // If you need to manage locked items, you can perform actions here.
                        Effects.SendLocationEffect(item.Location, item.Map, 0x375A, 10, 10, 1153, 0);
                        Caster.SendMessage("You have revealed a hidden object.");
                    }
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
