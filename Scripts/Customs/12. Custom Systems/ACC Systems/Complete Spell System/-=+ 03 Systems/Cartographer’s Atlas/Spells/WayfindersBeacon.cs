using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class WayfindersBeacon : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Wayfinder's Beacon", "In Ex Rune",
            //SpellCircle.Eighth,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public override double CastDelay { get { return 3.0; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 50; } }

        public WayfindersBeacon(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                Map map = Caster.Map;

                // Create glowing rune effect
                Effects.SendLocationEffect(loc, map, 0x3728, 30, 10, 0, 0);
                Effects.PlaySound(loc, map, 0x1F8); // Play sound effect

                // Create the permanent rune
                GlowingRune glowingRune = new GlowingRune();
                glowingRune.MoveToWorld(loc, map);
                glowingRune.Name = "Wayfinder's Beacon";

                // Create recall rune and give it to the caster
                RecallRune recallRune = new RecallRune();
                recallRune.Target = loc;
                recallRune.TargetMap = map;
                recallRune.House = null; // No house restriction
                recallRune.Marked = true;
                recallRune.Name = "Wayfinder's Recall Rune";
                recallRune.MoveToWorld(Caster.Location, Caster.Map);

                Caster.AddToBackpack(recallRune);

                // Set the rune to last 10 minutes
                Timer.DelayCall(TimeSpan.FromMinutes(10), () => glowingRune.Delete());
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private WayfindersBeacon m_Owner;

            public InternalTarget(WayfindersBeacon owner) : base(12, true, TargetFlags.None)
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

        private class GlowingRune : Item
        {
            public GlowingRune() : base(0x1F14) // Rune stone item ID
            {
                Movable = false;
                Hue = 1153; // Set the hue to give a glowing effect
            }

            public GlowingRune(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }
        }
    }
}
