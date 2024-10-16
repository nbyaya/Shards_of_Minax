using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class CrushingBlowSpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Crushing Blow", "Haymaker",
            212,
            9041,
            Reagent.BlackPearl,
            Reagent.Ginseng
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 20; } }

        private static Dictionary<Mobile, DateTime> m_Table = new Dictionary<Mobile, DateTime>();

        public CrushingBlowSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendLocalizedMessage(1060090); // You prepare to deliver a crushing blow!
                Caster.PlaySound(0x249);
                Caster.FixedEffect(0x37C4, 10, 19, 1684, 0);

                TimeSpan duration = TimeSpan.FromSeconds(10.0);
                m_Table[Caster] = DateTime.Now + duration;

                Timer.DelayCall(duration, new TimerStateCallback(RemoveEffect), Caster);
            }

            FinishSequence();
        }

        public static bool HasEffect(Mobile m)
        {
            return m_Table.ContainsKey(m) && m_Table[m] > DateTime.Now;
        }

        public static void RemoveEffect(object state)
        {
            Mobile m = (Mobile)state;

            if (m_Table.ContainsKey(m))
            {
                m_Table.Remove(m);
                m.SendLocalizedMessage(1060091); // The crushing blow effect has worn off.
            }
        }

        public static void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (!HasEffect(attacker))
                return;

            RemoveEffect(attacker);

            attacker.SendLocalizedMessage(1060090); // You have delivered a crushing blow!
            defender.SendLocalizedMessage(1060091); // You take extra damage from the crushing attack!

            defender.PlaySound(0x1E1);
            defender.FixedParticles(0, 1, 0, 9946, EffectLayer.Head);

            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(defender.X, defender.Y, defender.Z + 50), defender.Map), new Entity(Serial.Zero, new Point3D(defender.X, defender.Y, defender.Z + 20), defender.Map), 0xFB4, 1, 0, false, false, 0, 3, 9501, 1, 0, EffectLayer.Head, 0x100);

            int extraDamage = (int)(damage * 0.5);
            AOS.Damage(defender, attacker, extraDamage, 100, 0, 0, 0, 0);
        }
    }
}