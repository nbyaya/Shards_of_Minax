using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class ScentOfValor : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Scent of Valor", "Valor Scent!",
            //SpellCircle.Third,
            21017,
            9313
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public ScentOfValor(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Effects.PlaySound(Caster.Location, Caster.Map, 0x208); // Play casting sound
                Caster.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist); // Casting particles

                ArrayList followers = new ArrayList();
                
                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m is BaseCreature)
                    {
                        BaseCreature bc = (BaseCreature)m;
                        if (bc.Controlled && bc.ControlMaster == Caster)
                        {
                            followers.Add(bc);
                        }
                    }
                }

                if (followers.Count > 0)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(0.5), ApplyBuff, followers);
                }
            }

            FinishSequence();
        }

        private void ApplyBuff(ArrayList followers)
        {
            foreach (BaseCreature bc in followers)
            {
                bc.PlaySound(0x5C); // Play a buff sound
                bc.FixedParticles(0x375A, 10, 15, 5012, EffectLayer.Waist); // Buff particles
                
                int originalStr = bc.RawStr;
                int bonusStr = 10 + (int)(Caster.Skills[SkillName.Anatomy].Value / 10); // Strength bonus calculation
                bc.RawStr += bonusStr;

                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
                {
                    bc.RawStr = originalStr; // Revert the strength after 30 seconds
                    bc.PlaySound(0x1F2); // Play end of buff sound
                });
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
