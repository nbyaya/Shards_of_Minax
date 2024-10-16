using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class Decryption : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Decryption", "Ex Por Cypx",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public Decryption(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Decryption m_Owner;

            public InternalTarget(Decryption owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is RecallRune rune)
                {
                    if (m_Owner.CheckSequence())
                    {
                        Point3D loc = rune.Target;
                        Map map = rune.TargetMap;

                        if (loc != Point3D.Zero && map != null)
                        {
                            // Display the coordinates to the caster
                            from.SendMessage($"The rune is marked for coordinates: X: {loc.X}, Y: {loc.Y}, Z: {loc.Z} on {map}.");

                            // Create a visual and sound effect at the caster's location
                            Effects.SendLocationEffect(from.Location, from.Map, 0x376A, 20, 10, 0, 0);
                            Effects.PlaySound(from.Location, from.Map, 0x1ED);

                            // More flashy effects
                            Effects.SendTargetParticles(from, 0x36BD, 1, 17, 1149, 7, 9903, EffectLayer.Waist, 1);
                            Effects.SendTargetParticles(rune, 0x36BD, 1, 17, 1149, 7, 9903, EffectLayer.Waist, 1);
                        }
                        else
                        {
                            from.SendMessage("This rune is not marked for any location.");
                        }
                    }

                    m_Owner.FinishSequence();
                }
                else
                {
                    from.SendMessage("You must target a recall rune.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
