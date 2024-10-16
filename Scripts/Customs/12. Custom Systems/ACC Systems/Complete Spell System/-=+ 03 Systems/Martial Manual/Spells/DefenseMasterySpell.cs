using System;
using System.Collections;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class DefenseMasterySpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Defense Mastery", "Defendas",
            212,
            9041
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        private static readonly Hashtable m_Table = new Hashtable();

        public DefenseMasterySpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x375A, 1, 17, 0x7F2, 0x3E8, 0x3, EffectLayer.Waist);
                Caster.SendLocalizedMessage(1063353); // You perform a masterful defense!

                int modifier = (int)(30.0 * ((Math.Max(Caster.Skills[SkillName.Bushido].Value, Caster.Skills[SkillName.Ninjitsu].Value) - 50.0) / 70.0));

                DefenseMasteryInfo info = m_Table[Caster] as DefenseMasteryInfo;

                if (info != null)
                    EndDefense(info);

                ResistanceMod mod = new ResistanceMod(ResistanceType.Physical, 50 + modifier);
                Caster.AddResistanceMod(mod);

                info = new DefenseMasteryInfo(Caster, 80 - modifier, mod);
                info.m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(3.0), new TimerStateCallback(EndDefense), info);

                m_Table[Caster] = info;

                Caster.Delta(MobileDelta.WeaponDamage);
            }

            FinishSequence();
        }

        private static void EndDefense(object state)
        {
            DefenseMasteryInfo info = (DefenseMasteryInfo)state;

            if (info.m_Mod != null)
                info.m_From.RemoveResistanceMod(info.m_Mod);

            if (info.m_Timer != null)
                info.m_Timer.Stop();

            // No message is sent to the player.

            m_Table.Remove(info.m_From);

            info.m_From.Delta(MobileDelta.WeaponDamage);
        }

        private class DefenseMasteryInfo
        {
            public readonly Mobile m_From;
            public readonly int m_DamageMalus;
            public readonly ResistanceMod m_Mod;
            public Timer m_Timer;

            public DefenseMasteryInfo(Mobile from, int damageMalus, ResistanceMod mod)
            {
                m_From = from;
                m_DamageMalus = damageMalus;
                m_Mod = mod;
            }
        }
    }
}
