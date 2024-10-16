using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class NecromancersWard : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Necromancer's Ward", "An Sanct Nox",
                                                        21004, // Gump ID for the spell
                                                        9300, // Effect ID
                                                        false,
                                                        Reagent.Bloodmoss,
                                                        Reagent.Nightshade
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 20; } }

        public NecromancersWard(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x5C3); // Play casting sound
                
                // Create and place the ward item
                InternalItem ward = new InternalItem(Caster);
                ward.MoveToWorld(Caster.Location, Caster.Map);

                // Show visual effects for the spell
                Effects.SendTargetParticles(Caster, 0x376A, 10, 15, 5018, 0, 0, EffectLayer.Waist, 0); // Added the missing parameter
                Caster.SendMessage("You create a protective barrier around yourself!");

                // Apply temporary stat modifier
                Caster.AddStatMod(new StatMod(StatType.Str, "NecromancerWard", 10, TimeSpan.FromSeconds(30.0))); // Using Str as a placeholder

                // Timer to remove the ward after 30 seconds
                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () => ward.Delete()); 
            }

            FinishSequence();
        }

        private class InternalItem : Item
        {
            private Mobile m_Caster;

            public InternalItem(Mobile caster) : base(0x1F13)
            {
                Movable = false;
                Visible = true;
                m_Caster = caster;

                Hue = 1109; // Color of the barrier
                Name = "Necromancer's Ward";

                Effects.PlaySound(m_Caster.Location, m_Caster.Map, 0x1F2); // Sound effect when barrier appears
                Effects.SendTargetParticles(m_Caster, 0x375A, 1, 15, 9909, 1153, 2, EffectLayer.Waist, 0); // Added the missing parameter
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

                if (m_Caster != null)
                {
                    m_Caster.SendMessage("The Necromancer's Ward has faded.");
                    m_Caster.PlaySound(0x1F1); // Sound effect when the barrier fades
                }
            }

            public InternalItem(Serial serial) : base(serial)
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

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
