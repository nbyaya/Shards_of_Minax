using System;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.SpiritSpeakMagic
{
    public abstract class SpiritSpeakSpell : CSpell
    {
        public SpiritSpeakSpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
        }

        public abstract SpellCircle Circle { get; }

        public override SkillName CastSkill { get { return SkillName.SpiritSpeak; } }
        public override SkillName DamageSkill { get { return SkillName.SpiritSpeak; } }
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(1 * CastDelaySecondsPerTick); } }
        public override bool ClearHandsOnCast { get { return false; } }

        public override void GetCastSkills(out double min, out double max)
        {
            min = RequiredSkill;
            max = RequiredSkill;
        }

        public override int GetMana()
        {
            return RequiredMana;
        }

        public override bool ConsumeReagents()
        {
            return true;
        }
        
        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(CastDelay);
        }
    }
}
