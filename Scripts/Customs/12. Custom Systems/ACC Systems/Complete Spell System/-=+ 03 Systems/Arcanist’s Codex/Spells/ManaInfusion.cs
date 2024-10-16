using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class ManaInfusion : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mana Infusion", "Elvior Rinas",
            //SpellCircle.Third,
            215, // Circle of the spell
            9409 // Spell icon
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public ManaInfusion(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
        }

        private class InternalTarget : Target
        {
            private ManaInfusion m_Owner;

            public InternalTarget(ManaInfusion owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (m_Owner.CheckSequence())
                    {
                        // Calculate the amount of mana to restore
                        double manaToRestore = Utility.RandomMinMax(20, 50) + (m_Owner.Caster.Skills[SkillName.Magery].Value / 5);

                        // Restore mana to the target
                        target.Mana += (int)manaToRestore;
                        if (target.Mana > target.ManaMax)
                            target.Mana = target.ManaMax;

                        // Visual and sound effects
                        Point3D loc = target.Location;
                        Map map = target.Map;

                        // Play sound effect
                        Effects.PlaySound(loc, map, 0x1F2); // Mana restore sound

                        // Play visual effect
                        Effects.SendLocationEffect(loc, map, 0x37C4, 30, 10); // Blue magic particles

                        // Inform the caster and target
                        m_Owner.Caster.SendMessage("You have infused mana into your target!");
                        target.SendMessage("You feel a surge of magical energy as mana is infused into you!");

                        // Optional: Create a message log or additional effects
                    }
                }
                m_Owner.FinishSequence(); // Call FinishSequence on m_Owner instance
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence(); // Call FinishSequence on m_Owner instance
            }
        }
    }
}
